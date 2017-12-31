using System.Threading.Tasks;

namespace backend.Interfaces
{
    public interface ITranslator
    {
        Task<string> Translate(string fromLocale, string toLocale, string word);
    }
}