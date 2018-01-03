using System.Collections.Generic;
using TinyTranslation;

namespace TinyTranslation.Interfaces
{
    public interface ITranslationStorage
    {
        bool Populate(TranslationDictionary dict);
        void Store(TranslationDictionary dict);
    }
}