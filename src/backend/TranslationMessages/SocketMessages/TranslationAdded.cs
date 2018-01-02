using System;
using Newtonsoft.Json;
using TinyWebSockets;

namespace backend.SocketMessages
{
    [Message("added")]
    public class TranslationAdded : BaseMessage
    {
        [JsonProperty("word")]
        public string Word
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
