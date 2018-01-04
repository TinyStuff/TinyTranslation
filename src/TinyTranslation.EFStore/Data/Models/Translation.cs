using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TinyTranslation.EFStore.Data.Models
{
    public class Translation
    {
        [Key]
        public int TranslationID { get; set; }

        public int LocalID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
