﻿using System.Collections.Generic;
using System.Linq;

namespace TinyTranslation
{
    public class TranslationTable
    {
        public TranslationTable(IList<string> keys, IEnumerable<TranslationDictionary> dicts)
        {
            Locales = dicts.Select(d => d.Locale).ToList();
            Values = new Dictionary<string, IList<string>>();
            foreach(var key in keys) {
                List<string> values = new List<string>();
                Values.Add(key,values);
                foreach(var dict in dicts) {
                    values.Add(dict[key]);
                }
            }
        }

        public IList<string> Locales { get; set; }

        public Dictionary<string, IList<string>> Values { get; set; }
    }
}
