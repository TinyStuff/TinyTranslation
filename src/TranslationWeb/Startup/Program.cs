using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TinyTranslation.EFStore;
using TinyTranslation.EFStore.Data;
using TinyTranslation.Interfaces;

namespace TranslationWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var dbcontext = services.GetRequiredService<TranslationDbContext>();
                var dbstore = services.GetRequiredService<ITranslationStorage>() as DbStorage;
                if (dbstore != null)
                    dbstore.SetContext(dbcontext);

            }
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
