using Newtonsoft.Json;
using TinyWebSockets;

namespace TranslationMessages.SocketMessages
{
    [Message("changeLanguage")]
    public class LanguageChange : BaseMessage
    {
        [JsonProperty("locale")]
        public string Locale
        {
            get;
            set;
        }
    }
}
