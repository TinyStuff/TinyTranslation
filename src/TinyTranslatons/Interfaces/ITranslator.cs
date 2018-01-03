using System.Threading.Tasks;

namespace TinyTranslation.Interfaces
{
    public interface ITranslator
    {
        Task<string> Translate(string fromLocale, string toLocale, string word);
    }
}