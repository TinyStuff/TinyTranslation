using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TinyTranslation.Translators;
using Swashbuckle.AspNetCore.Swagger;
using TinyTranslation.EFStore.Data;
using Microsoft.EntityFrameworkCore;
using TinyTranslation.Interfaces;
using System;

namespace TranslationWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TranslationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"), (b) => {
                    b.MigrationsAssembly("TranslationWeb");
                }));

            services.AddScoped<Func<TranslationDbContext>>((arg) => {
                return new Func<TranslationDbContext>(() =>
                {
                    return arg.GetRequiredService<TranslationDbContext>();
                });
            });

            services.AddSingleton<ITranslationStorage, TinyTranslation.EFStore.DbStorage>();
            services.AddTranslationService(options =>
            {
                options.AllowedLocales.Add("es");
                //options.Storage = new TinyTranslation.EFStore.DbStorage()
                options.Translator = new BingTranslator(Configuration["BingTranslator:Key"]);
            });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Translation API", Version = "v1" });
            });

            services.AddMvc();
                
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSwagger();  
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
            app.UseStaticFiles();
        }
    }
}
