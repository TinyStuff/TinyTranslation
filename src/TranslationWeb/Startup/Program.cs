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
            BuildWebHost(args)
                .PopulateTranslationDb()
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
