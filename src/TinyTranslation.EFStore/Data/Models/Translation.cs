using System;
namespace TinyTranslation.EFStore.Data.Models
{
    public class Translation
    {
        public int TranslationID { get; set; }
        public int LocalID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
