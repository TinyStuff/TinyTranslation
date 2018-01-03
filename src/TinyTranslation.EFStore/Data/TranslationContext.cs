using System;
using Microsoft.EntityFrameworkCore;
using TinyTranslation.EFStore.Data.Models;

namespace TinyTranslation.EFStore.Data
{
    public class TranslationContext : DbContext
    {
        public TranslationContext(DbContextOptions<TranslationContext> options) : base(options)
        {
            
        }

        public DbSet<Locale> Locales { get; set; }
        public DbSet<Translation> Translations { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{

        //    modelBuilder.Entity<Locale>().ToTable("Locale");
        //    modelBuilder.Entity<Translation>().ToTable("Translation");
        
        //}
    }
}
