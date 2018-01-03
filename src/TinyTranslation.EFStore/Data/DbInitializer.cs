using System.Linq;
using TinyTranslation.EFStore.Data.Models;

namespace TinyTranslation.EFStore.Data
{
    public static class DbInitializer
    {
        public static void Initialize(TranslationDbContext ctx)
        {
            ctx.Database.EnsureCreated();
            if (ctx.Translations.Any())
            {
                return;   // DB has been seeded
            }
            ctx.Locales.Add(new Locale()
            {
                Key = "en"
            });
            ctx.Translations.Add(new Translation()
            {
                Key = "first",
                LocalID = 1,
                Value = "1'st"
            });
            ctx.SaveChanges();
        }
    }
}
