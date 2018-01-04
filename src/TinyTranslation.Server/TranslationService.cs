using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyTranslation.Interfaces;
using TinyTranslation.Translators;

namespace TinyTranslation
{
    public class TranslationService
    {
        private ITranslationStorage storage;
        private TranslationDictionary defaultLanguage;
        private ITranslator translator = new DumbTranslator();
        private List<string> AllowedLocales = new List<string>();

        public bool AutoLoadAllowedLocales { get; set; } = true;
        public string DefaultLocale { get; internal set; }
        public bool AutoTranslateNewLocales { get; set; } = true;

        public Dictionary<string, TranslationDictionary> Locales { get; set; } = new Dictionary<string, TranslationDictionary>();

        public TranslationService(ITranslationStorage storage, string defaultLocale)
        {
            this.storage = storage;
            defaultLanguage = AddLocale(defaultLocale, false);
        }

        public TranslationService(TranslationOptions settings, ITranslationStorage storage)
        {
            this.storage = storage;
            this.translator = settings.Translator;
            defaultLanguage = AddLocale(settings.DefaultLocale, false);
            if (settings.AllowedLocales.Any())
                SetAllowedLocales(settings.AllowedLocales.ToArray());
            AutoLoadAllowedLocales = settings.AutoLoadAllowedLocales;
            AutoTranslateNewLocales = settings.AutoTranslateNewLocales;
        }

        public ITranslationStorage GetStorage()
        {
            return storage;
        }

        public async Task<string> AutoTranslate(string key, string to)
        {
            var word = defaultLanguage[key];
            var fromLocale = defaultLanguage.Locale;
            return await translator.Translate(fromLocale, to, word);
        }

        public TranslationTable GetTableData()
        {
            return new TranslationTable(defaultLanguage.Keys.ToList(), Locales.Values);
        }

        public void SetAllowedLocales(params string[] locale)
        {
            AllowedLocales.Clear();
            AllowedLocales.AddRange(locale);
            if (AutoLoadAllowedLocales)
            {
                foreach (var loc in AllowedLocales)
                {
                    GetLanguage(loc);
                }
            }
        }

        public void Delete(string key)
        {
            foreach (var t in Locales.Values)
            {
                t.Remove(key);
            }
        }

        private Dictionary<string, Task> saveTasks = new Dictionary<string, Task>();

        void TriggerSave(object sender, KeyValuePair<string, string> e)
        {
            var origin = sender as TranslationDictionary;
            var start = (!saveTasks.ContainsKey(origin.Locale));
            if (start)
            {
                var tsk = Task.Delay(1000).ContinueWith((arg) =>
                {
                    SaveTranslations(origin);
                    saveTasks.Remove(origin.Locale);
                });
                saveTasks.Add(origin.Locale, tsk);
            }
        }

        public bool IsAllowedLocale(string locale)
        {
            return AllowedLocales.Contains(locale);
        }

        Action<TranslationDictionary> SaveTranslations => (dict) =>
        {
            storage.Store(dict);
        };

        private TranslationDictionary AddLocale(string locale, bool copyFromDefault)
        {
            var ret = new TranslationDictionary(locale);
            storage.Populate(ret);
            Locales.Add(locale, ret);
            ret.OnAdd += KeyAddedHandler;
            ret.OnAdd += TriggerSave;
            ret.OnUpdate += TriggerSave;
            if (copyFromDefault)
            {
                MatchFromDefault(ret);
            }
            else
                DefaultLocale = locale;
            if (storage is ITranslationMonitor monitor)
            {
                monitor.StartMonitoring(ret);
            }
            ret.IsPrimaryLanguage = !copyFromDefault;
            return ret;
        }

        public void SetTranslator(ITranslator translator)
        {
            this.translator = translator;
        }

        private string currentKeyHandled;
        private TranslationOptions settings;

        void KeyAddedHandler(object sender, KeyValuePair<string, string> e)
        {
            if (e.Key != currentKeyHandled)
            {
                currentKeyHandled = e.Key;

                var origin = sender as TranslationDictionary;
                foreach (var other in Locales.Values.Where(d => d != origin))
                {
                    if (!other.ContainsKey(e.Key))
                    {
                        Task.Run(async () =>
                        {
                            var newWord = await translator.Translate(origin.Locale, other.Locale, e.Value);
                            other.Add(e.Key, newWord);

                        });
                    }
                }
            };
        }

        public string Get(string locale, string key)
        {
            return GetLanguage(locale)[key];
        }

        public void Update(string locale, string key, string value)
        {
            var lang = GetLanguage(locale);
            lang.Add(key, value);
        }

        private void MatchFromDefault(TranslationDictionary ret)
        {
            foreach (var key in defaultLanguage.Keys)
            {
                if (!ret.ContainsKey(key))
                {
                    if (AutoTranslateNewLocales)
                    {
                        Task.Run(async () =>
                        {
                            var newWord = await translator.Translate(defaultLanguage.Locale, ret.Locale, defaultLanguage[key]);
                            ret.Add(key, newWord);

                        });
                    }
                    else
                        ret.Add(key, defaultLanguage[key]);
                }
            }
        }

        public TranslationDictionary GetLanguage(string locale)
        {
            if (locale.Equals("default"))
            {
                return defaultLanguage;
            }
            if (Locales.ContainsKey(locale))
            {
                return Locales[locale];
            }
            else
            {
                if (AllowedLocales.Contains(locale))
                    return AddLocale(locale, true);
                else
                    throw new Exceptions.NotAllowedLocaleException(locale);
            }
        }

        public Dictionary<string, string> GetTranslations(string locale)
        {
            return GetLanguage(locale).GetAllTranslations();
        }
    }
}
