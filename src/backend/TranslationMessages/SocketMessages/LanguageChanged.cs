using System.Collections.Generic;
using Newtonsoft.Json;
using TinyWebSockets;

namespace TranslationMessages.SocketMessages
{
    [Message("languageChanged")]
    public class LanguageChanged : BaseMessage
    {
        [JsonProperty("locale")]
        public string Locale
        {
            get;
            set;
        }

        [JsonProperty("data")]
        public IDictionary<string, string> Data
        {
            get;
            set;
        }
    }
}
