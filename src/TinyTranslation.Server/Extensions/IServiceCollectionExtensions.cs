using System;
using Microsoft.Extensions.DependencyInjection;
using TinyTranslation;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TranslationServiceExtensions
    {
        public static void AddTranslationService(this IServiceCollection services, Action<TranslationOptions> setupAction)
        {
            var settings = new TranslationOptions();
            setupAction.Invoke(settings);
            var trans = new TranslationService(settings);
            services.AddTransient<TranslationService>((arg) =>
            {
                return trans;
            });
        }
    }
}
