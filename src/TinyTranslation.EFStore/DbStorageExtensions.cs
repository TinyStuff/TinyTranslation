using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TinyTranslation.EFStore.Data;
using TinyTranslation.Interfaces;

namespace TinyTranslation.EFStore
{
    public static class DbStorageExtensions
    {
        public static IWebHost PopulateTranslationDb(this IWebHost host)
        {
            // Populate database translations to cache
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;


                var dbcontext = services.GetService<TranslationDbContext>();
                var dbstore = services.GetService<ITranslationStorage>() as DbStorage;
                if (dbstore != null)
                    dbstore.SetContext(dbcontext);

            }
            return host;
        }
    }
}
