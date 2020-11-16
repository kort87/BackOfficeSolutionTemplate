using Core.Data.Base;
using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Api
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
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TemplateBackEnd Api",
                    Description = "Bibliothèque Api du projet back-end template",
                    TermsOfService = null,
                    Contact = new OpenApiContact() { Name = "Template BackEnd Api", Email = "youremail@gmail.com" }
                });
            });

            // Configuration des différentes couches du projet
            this.AddBusinessDataObject(services);
            this.AddBusinessDomainObject(services);
            this.AddBusinessServiceObject(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName.ToUpper().Equals("DEVELOPMENT"))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Template BackEnd V1");
            });

            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        /// <summary>
        /// Méthode de chargement du paramétrage de la base de données, et de l'injection du UnitOfWork.
        /// </summary>
        /// <param name="services"></param>
        private void AddBusinessDataObject(IServiceCollection services)
        {
            //DBSettings.SetConnectionString(Configuration["Data:DBDEMO:Name"], Configuration["Data:DBDEMO:ConnectionString"], Configuration["Data:DBDEMO:Provider"]);
            //LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;

            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        /// <summary>
        /// Méthode de chargement des dtos et de leurs factories par injections de dépendances.
        /// </summary>
        /// <param name="services"></param>
        private void AddBusinessDomainObject(IServiceCollection services)
        {
            // Factories
            //services.AddSingleton<INTERFACEDTOBASEFACTORY, IMPLDTOBASEFACTORY>();
            //services.AddSingleton<INTERFACEDTOFACTORY, IMPLDTOFACTORY>();
            

            // Dtos
            //services.AddTransient<INTERFACEDTOBASE, IMPLDTOBASE>();
            //services.AddTransient<INTERFACEDTO, IMPLDTO>();
        }

        /// <summary>
        /// Méthode de chargement des services des APIs internes de l'application par injections de dépendances.
        /// </summary>
        /// <param name="services"></param>
        private void AddBusinessServiceObject(IServiceCollection services)
        {
            //services.AddScoped<INTERFACESERVICE, IMPLEMENTATIONSERVICE>();

        }
    }
}
