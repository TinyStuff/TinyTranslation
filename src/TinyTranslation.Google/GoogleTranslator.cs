using System;
using System.Threading.Tasks;
using TinyTranslation.Interfaces;
using Google.Cloud.Translation.V2;

namespace TinyTranslation.Translators
{
    public class GoogleTranslator : ITranslator
    {
        private Google.Apis.Auth.OAuth2.GoogleCredential credentials;

        public GoogleTranslator(string credentialsJsonString)
        {
            credentials = Google.Apis.Auth.OAuth2.GoogleCredential.FromJson(credentialsJsonString);
        }

        public GoogleTranslator(Google.Apis.Auth.OAuth2.GoogleCredential credentials)
        {
            this.credentials = credentials;
        }

        private Google.Cloud.Translation.V2.TranslationClient _client;

        private Google.Cloud.Translation.V2.TranslationClient client
        {
            get
            {
                return _client ?? (_client = Google.Cloud.Translation.V2.TranslationClient.Create(credentials));
            }
        }

        public async Task<string> Translate(string fromLocale, string toLocale, string word)
        {
            var ret = await client.TranslateTextAsync(word, toLocale, fromLocale);
            return ret.TranslatedText;
        }
    }
}
