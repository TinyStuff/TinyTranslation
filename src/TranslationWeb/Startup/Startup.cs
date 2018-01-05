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
using TinyTranslation.Storage;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using TranslationWeb.Data;
using Microsoft.AspNetCore.Identity;
using TranslationWeb.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using TranslationWeb.Services;

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
            ConfigureStorage(services);
            ConfigureAuthentication(services);

            services.AddTranslationService(options =>
            {
                options.AllowedLocales.Add("es");
                //options.Storage = new TinyTranslation.EFStore.DbStorage()
                options.Translator = new BingTranslator(Configuration["BingTranslator:Key"]);
            });

            services.AddAuthorization();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Translation API", Version = "v1" });
            });

            services.AddMvc();

        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddSingleton<TokenService>();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("IdentityDB")));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Jwt";
                options.DefaultChallengeScheme = "Jwt";
            }).AddJwtBearer("Jwt", jwtBearerOptions =>
                 {
                     jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["AuthToken"])),

                         ValidateIssuer = false,
                         ValidIssuer = "TinyTranslator",

                         ValidateAudience = false,
                         ValidAudience = "The name of the audience",

                         ValidateLifetime = false, //validate the expiration and not before values in the token

                         ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
                     };
                 });

        }

        private void ConfigureStorage(IServiceCollection services)
        {
            // Use database storage instead
            services.AddDbContext<TranslationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("TranslationDB"), (b) =>
                {
                    b.MigrationsAssembly("TranslationWeb");
                }));
            //services.AddSingleton<ITranslationStorage, TinyTranslation.EFStore.DbStorage>();

            services.AddSingleton<ITranslationStorage, FileStorage>();
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

            app.UseAuthentication();

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
