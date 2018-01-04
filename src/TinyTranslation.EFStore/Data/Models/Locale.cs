using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TinyTranslation.EFStore.Data.Models
{
    public class Locale
    {
        [Key]
        public int LocalID { get; set; }
        public string Key { get; set; }

        //public ICollection<Translation> Translations { get; set; }
    }
}
