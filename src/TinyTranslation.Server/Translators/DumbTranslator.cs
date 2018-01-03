using System;
using System.Threading.Tasks;
using TinyTranslation.Interfaces;

namespace TinyTranslation.Translators
{
    public class DumbTranslator : ITranslator
    {
        public async Task<string> Translate(string fromLocale, string toLocale, string word)
        {
            return word;
        }
    }
}
