using System;
using System.Threading.Tasks;
using TinyTranslation.Interfaces;

namespace TinyTranslation.Translators
{
    public class BingTranslator : ITranslator
    {
        private TranslatorService.TranslatorServiceClient _client;
        private string key;

        public BingTranslator(string key)
        {
            this.key = key;
        }

        private TranslatorService.TranslatorServiceClient client
        {
            get
            {
                return _client ?? (_client = new TranslatorService.TranslatorServiceClient(key));
            }
        }

        public async Task<string> Translate(string fromLocale, string toLocale, string word)
        {
            return await client.TranslateAsync(word, fromLocale, toLocale);

        }
    }
}
