using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyTranslation.EFStore.Data;
using TinyTranslation.EFStore.Data.Models;

namespace TinyTranslation.EFStore
{
    public class DbStorage : Interfaces.ITranslationStorage, Interfaces.ITranslationMonitor
    {

        Dictionary<string, LanguageMonitor> monitors = new Dictionary<string, LanguageMonitor>();
        Dictionary<string, Locale> locales = new Dictionary<string, Locale>();

        public DbStorage() //TranslationDbContext ctx
        {

        }

        public void SetContext(TranslationDbContext ctx)
        {
            context = ctx;
            foreach (var locale in ctx.Locales)
            {
                var old = locales.Values.FirstOrDefault(d => d.Key == locale.Key);
                if (old != null)
                {
                    old.LocalID = locale.LocalID;
                    monitors[locale.Key].Populate(ctx.Translations.Where(d => d.LocalID == locale.LocalID), locale.LocalID);
                }
                else
                {
                    locales.Add(locale.Key, locale);
                    monitors.Add(locale.Key, new LanguageMonitor(locale.LocalID, ctx.Translations.Where(d => d.LocalID == locale.LocalID)));
                }
            }
        }

        private TranslationDbContext context;


        private Locale GetLocale(string locale)
        {
            if (locales.ContainsKey(locale))
            {
                return locales[locale];
            }
            else
            {
                var lang = new Locale()
                {
                    Key = locale
                };
                locales.Add(locale, lang);
                monitors.Add(locale, new LanguageMonitor(0, new List<Translation>()));
                return lang;
            }
        }

        public bool Populate(TranslationDictionary dict)
        {
            var hasKeys = false;
            var locale = GetLocale(dict.Locale);
            var mon = monitors[dict.Locale];
            mon.SetDictionary(dict);
            foreach (var trans in mon.Translations.Values)
            {
                dict.Add(trans.Key, trans.Value);
                hasKeys = true;
            }
            return hasKeys;
        }


        public void Store(TranslationDictionary dict)
        {
            var i = 32;
            //if (monitors.ContainsKey(dict.Locale))
            //monitors[dict.Locale].TriggerNeedsSave();
        }

        public void SaveWithContext(TranslationDbContext context)
        {
            var localeAdded = false;
            foreach (var locale in locales)
            {
                if (locale.Value.LocalID == 0)
                {
                    localeAdded = true;
                    context.Locales.Add(locale.Value);
                }
            }
            if (localeAdded)
                context.SaveChanges();
            foreach (var m in monitors.Where(d => d.Value.NeedSaving))
            {
                if (m.Value.LangId==0) {
                    var l = locales.FirstOrDefault(d => d.Key == m.Key);
                    if (l.Value.LocalID>0)
                        m.Value.LangId = l.Value.LocalID;
                }
                MatchLocale(m.Value, context);
            }

        }

        private void MatchLocale(LanguageMonitor m, TranslationDbContext ctx)
        {
            
            foreach (var trans in m.Translations.Values)
            {
                if (trans.LocalID==0) {
                    trans.LocalID = m.LangId;
                }
                if (trans.TranslationID == 0)
                {
                    trans.LocalID = m.LangId;
                    ctx.Translations.Add(trans);
                }
            }
            ctx.SaveChanges();
            m.NeedSaving = false;
        }

        void TranslationAdded(object sender, KeyValuePair<string, string> e)
        {
            var dict = sender as TranslationDictionary;
            if (monitors.ContainsKey(dict.Locale))
                monitors[dict.Locale].AddOrUpdate(e);
        }

        void TranslationUpdated(object sender, KeyValuePair<string, string> e)
        {
            var dict = sender as TranslationDictionary;
            if (monitors.ContainsKey(dict.Locale))
                monitors[dict.Locale].AddOrUpdate(e);
        }

        public void StartMonitoring(TranslationDictionary dict)
        {
            //if (!monitors.ContainsKey(dict.Locale))
            {
                dict.OnAdd += TranslationAdded;
                dict.OnUpdate += TranslationUpdated;
                //var lang = GetLocale(dict.Locale);
                //monitors.Add(dict.Locale, new LanguageMonitor(lang.LocalID, context.Translations.Where(d => d.LocalID == lang.LocalID)));
            }
        }

        public void StopMonitoring(TranslationDictionary dict)
        {
            //if (!monitors.ContainsKey(dict.Locale))
            {
                dict.OnAdd -= TranslationAdded;
                dict.OnUpdate -= TranslationUpdated;
                //monitors.Remove(dict.Locale);
            }
        }
    }

    internal class LanguageMonitor
    {
        public Dictionary<string, Translation> Translations { get; internal set; } = new Dictionary<string, Translation>();

        public int LangId { get; set; }
        public bool NeedSaving { get; internal set; }
        private TranslationDictionary dict { get; set; }

        public void SetDictionary(TranslationDictionary dict)
        {
            this.dict = dict;
            if (Translations.Any() && !dict.Any())
            {
                foreach (var k in Translations.Values)
                {
                    dict.Add(k.Key, k.Value);
                }
            }
        }

        public LanguageMonitor(int langId, IEnumerable<Translation> translations)
        {
            Populate(translations, langId);
        }

        public void AddOrUpdate(KeyValuePair<string, string> kv)
        {
            NeedSaving = true;
            if (Translations.ContainsKey(kv.Key))
            {
                if (Translations[kv.Key].Value != kv.Value)
                {
                    Translations[kv.Key].Value = kv.Value;
                }
            }
            else
            {
                var trans = new Translation()
                {
                    TranslationID = 0,
                    LocalID = LangId,
                    Key = kv.Key,
                    Value = kv.Value
                };
                Translations.Add(kv.Key, trans);
            }
        }

        internal void Populate(IEnumerable<Translation> translations, int langId)
        {
            LangId = langId;
            foreach (var trans in translations)
            {
                Translations.Add(trans.Key, trans);
            }
        }
    }
}
