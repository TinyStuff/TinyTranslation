using System.IO;
using TinyTranslation.Interfaces;

namespace TinyTranslation.Storage
{
    public class FileStorage : ITranslationStorage
    {
        public string PostFix { get; set; } = ".csv";
        public string BasePath { get; set; } = "translations";

        private readonly string[] separator = new string[] { "##" };

        public bool Populate(TranslationDictionary dict)
        {
            var ret = false;
            var filePath = Path.Combine(BasePath, dict.Locale) + PostFix;
            if (File.Exists(filePath))
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            var kv = line.Split(separator, System.StringSplitOptions.None);
                            dict.Add(kv[0], kv[1]);
                            ret = true;
                        }
                    }
                }
            }
            return ret;
        }

        public void Store(TranslationDictionary dict)
        {
            var filePath = Path.Combine(BasePath, dict.Locale) + PostFix;
            FileMode fm = File.Exists(filePath) ? FileMode.Create : FileMode.CreateNew;

            using (var fileStream = new FileStream(filePath, fm))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    foreach (var newword in dict.GetAllTranslations())
                    {
                        writer.Write(newword.Key);
                        writer.Write("##");
                        writer.WriteLine(newword.Value);
                    }
                }
            }

        }
    }
}
