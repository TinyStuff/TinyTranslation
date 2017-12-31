using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Interfaces;
using backend.Storage;
using backend.Translators;

namespace backend.Services
{
    public static class Helper
    {
        private static FileStorage storage;
        private static TranslationService translationService;

        public static ITranslationStorage Storage
        {
            get
            {
                return storage ?? (storage = new FileStorage());
            }
        }

        public static TranslationService TranslationsService
        {
            get
            {
                if (translationService==null) {
                    translationService = new TranslationService(Storage, "en");
                    translationService.SetAllowedLocales("en", "sv", "no");
                    translationService.SetTranslator(new BingTranslator());
                }  
                return translationService;
            }
        }
    }
}
