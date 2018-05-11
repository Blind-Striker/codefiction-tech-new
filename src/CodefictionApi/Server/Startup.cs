using System;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CodefictionApi.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Swashbuckle.AspNetCore.Swagger;

namespace Codefiction.CodefictionTech.CodefictionApi.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        public static IContainer Container { get; private set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddNodeServices();
            services.AddMemoryCache();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "Angular 5.0 Universal & ASP.NET Core advanced starter-kit web API",
                        Version = "v1"
                    });
            });

            AutoMapperConfig.Init();

            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.RegisterModule<ApiCoreModule>();

            Container = builder.Build();

            return new AutofacServiceProvider(Container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            File.ReadAllTextAsync("database.json");

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = c =>
                {
                    //Do not add cache to json files. We need to have new versions when we add new translations.

                    if (!c.Context.Request.Path.Value.Contains(".json"))
                    {
                        c.Context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
                        {
                            MaxAge = TimeSpan.FromDays(30) // Cache everything except json for 30 days
                        };
                    }
                    else
                    {
                        c.Context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
                        {
                            MaxAge = TimeSpan.FromMinutes(15) // Cache json for 15 minutes
                        };
                    }
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

                // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.

                app.MapWhen(x => !x.Request.Path.Value.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase),
                    builder =>
                    {
                        builder.UseMvc(routes =>
                        {
                            routes.MapSpaFallbackRoute(
                                name: "spa-fallback",
                                defaults: new { controller = "Home", action = "Index" });
                        });
                    });
            }
            else
            {
                app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");

                    routes.MapRoute(
                        "Sitemap",
                        "sitemap.xml",
                        new { controller = "Home", action = "SitemapXml" });

                    routes.MapSpaFallbackRoute(
                        name: "spa-fallback",
                        defaults: new { controller = "Home", action = "Index" });

                });
                app.UseExceptionHandler("/Home/Error");
            }
        }
    }
}


