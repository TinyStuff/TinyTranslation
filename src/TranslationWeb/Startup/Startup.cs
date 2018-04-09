using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TinyTranslation.Translators;
using Swashbuckle.AspNetCore.Swagger;
using TinyTranslation.EFStore.Data;
using Microsoft.EntityFrameworkCore;
using TinyTranslation.Interfaces;
using System;
using TranslationWeb.Data;
using Microsoft.AspNetCore.Identity;
using TranslationWeb.Data.Models;
using TranslationWeb.Services;
using System.Linq;
using System.Security.Principal;
using System.Net;

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

        private static void ApiKeyMiddlewear(IApplicationBuilder app, IConfiguration config)
        {
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.StartsWithSegments(new PathString("/api")))
                {
                    // Let's check if this is an API Call
                    if (context.Request.Headers["apikey"].Any())
                    {
                        // validate the supplied API key
                        // Validate it
                        var headerKey = context.Request.Headers["apikey"].FirstOrDefault();

                        var keys = config["ApiKey"];
                        var valid = keys.Equals(headerKey);

                        if (!valid)
                        {
                            
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            await context.Response.WriteAsync("Invalid API Key");
                            return;
                        }
                        else
                        {
                            var identity = new GenericIdentity("API");
                            identity.AddClaim(new System.Security.Claims.Claim("Origin", "Api"));
                            var principal = new GenericPrincipal(identity, new[] { "Admin", "ApiUser" });
                            context.User = principal;
                            await next();
                        }
                    }
                    else
                    {
                        await next();
                    }
                }
                else
                {
                    await next();
                }
            });
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddSingleton<TokenService>();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IdentityDB")));
            services.AddIdentity<ApplicationUser, ApplicationRole>()
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
                options.UseSqlServer(Configuration.GetConnectionString("TranslationDB"), (b) =>
                {
                    b.MigrationsAssembly("TranslationWeb");
                }));
            services.AddSingleton<ITranslationStorage, TinyTranslation.EFStore.DbStorage>();

            //services.AddSingleton<ITranslationStorage, FileStorage>();
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
            ApiKeyMiddlewear(app, Configuration);
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
