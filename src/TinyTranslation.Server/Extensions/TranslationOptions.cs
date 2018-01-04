using System.Collections.Generic;
using TinyTranslation.Interfaces;
using TinyTranslation.Storage;
using TinyTranslation.Translators;

namespace TinyTranslation
{
    public class TranslationOptions
    {
        public IList<string> AllowedLocales
        {
            get;
            set;
        } = new List<string>() { "en", "sv" };

        public string DefaultLocale
        {
            get;
            set;
        } = "en";

        public bool AutoLoadAllowedLocales
        {
            get;
            set;
        } = true;

        public bool AutoTranslateNewLocales
        {
            get;
            set;
        } = true;

        public ITranslator Translator { get; set; } = new DumbTranslator();

    }
}
