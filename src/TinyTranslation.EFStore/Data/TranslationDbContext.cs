using System;
using Microsoft.EntityFrameworkCore;
using TinyTranslation.EFStore.Data.Models;

namespace TinyTranslation.EFStore.Data
{
    public class TranslationDbContext : DbContext
    {
        public TranslationDbContext(DbContextOptions<TranslationDbContext> options) : base(options)
        {

        }

        public DbSet<Locale> Locales { get; set; }
        public DbSet<Translation> Translations { get; set; }

    }
}
