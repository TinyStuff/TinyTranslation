using System;
using System.Threading.Tasks;
using backend.Interfaces;
using Google.Cloud.Translation.V2;

namespace backend.Translators
{
    public class GoogleTranslator : ITranslator
    {
        private TranslationClient _client;

        private TranslationClient client
        {
            get
            {
                var cred = Google.Apis.Auth.OAuth2.GoogleCredential.FromJson(System.IO.File.ReadAllText("mytest-3c1eddbcfc04.json"));
                return _client ?? (_client = TranslationClient.Create(cred));
            }
        }

        public async Task<string> Translate(string fromLocale, string toLocale, string word)
        {
            try
            {
                var ret = await client.TranslateTextAsync(word, toLocale, fromLocale);
                return ret.TranslatedText;
            }
            catch (Exception ex)
            {
                var i = 34;
            }
            return word;
        }
    }
}
