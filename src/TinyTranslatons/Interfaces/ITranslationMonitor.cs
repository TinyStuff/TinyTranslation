namespace TinyTranslation.Interfaces
{
    public interface ITranslationMonitor
    {
        void StartMonitoring(TranslationDictionary dict);
        void StopMonitoring(TranslationDictionary dict);
    }
}