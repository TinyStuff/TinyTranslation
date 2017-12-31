using System.Collections.Generic;

namespace backend.Interfaces
{
    public interface ITranslationStorage
    {
        bool Populate(TranslationDictionary dict);
        void Store(TranslationDictionary dict);
    }
}