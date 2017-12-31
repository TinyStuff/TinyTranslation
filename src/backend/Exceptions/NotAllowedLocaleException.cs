using System;
using System.Runtime.Serialization;

namespace backend.Exceptions
{
    [Serializable]
    internal class NotAllowedLocaleException : Exception
    {
        public NotAllowedLocaleException()
        {
        }

        public NotAllowedLocaleException(string message) : base(message)
        {
        }

        public NotAllowedLocaleException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotAllowedLocaleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}