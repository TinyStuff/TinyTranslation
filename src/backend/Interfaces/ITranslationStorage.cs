using System.Collections.Generic;
using TinyTranslations;

namespace backend.Interfaces
{
    public interface ITranslationStorage
    {
        bool Populate(TranslationDictionary dict);
        void Store(TranslationDictionary dict);
    }
}