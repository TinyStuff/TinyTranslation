using Newtonsoft.Json;
using TinyWebSockets;

namespace TranslationMessages.SocketMessages
{
    [Message("added")]
    public class TranslationUpdated : BaseMessage
    {
        [JsonProperty("word")]
        public string Word
        {
            get;
            set;
        }

        [JsonProperty("old")]
        public string OldWord
        {
            get;
            set;
        }

        [JsonProperty("key")]
        public string Key
        {
            get;
            set;
        }
    }
}
