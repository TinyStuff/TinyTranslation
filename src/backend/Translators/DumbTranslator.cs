using System;
using System.Threading.Tasks;
using backend.Interfaces;

namespace backend.Translators
{
    public class DumbTranslator : ITranslator
    {
        public async Task<string> Translate(string fromLocale, string toLocale, string word)
        {
            return word;
        }
    }
}
