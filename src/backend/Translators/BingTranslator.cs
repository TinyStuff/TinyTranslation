using System;
using System.Threading.Tasks;
using backend.Interfaces;

namespace backend.Translators
{
    public class BingTranslator : ITranslator
    {
        private TranslatorService.TranslatorServiceClient _client;
        private TranslatorService.TranslatorServiceClient client
        {
            get
            {
                return _client ?? (_client = new TranslatorService.TranslatorServiceClient("4d00ae933d5d4b7f92ec581d3d0facb8"));
            }
        }
        public async Task<string> Translate(string fromLocale, string toLocale, string word)
        {
            return await client.TranslateAsync(word, fromLocale, toLocale);

        }
    }
}
