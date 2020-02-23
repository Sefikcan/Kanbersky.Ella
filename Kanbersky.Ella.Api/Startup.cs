using Kanbersky.Ella.Business.Abstract;
using Kanbersky.Ella.Business.Concrete;
using Kanbersky.Ella.Business.Helpers;
using Kanbersky.Ella.DAL.Concrete.EntityFramework.Context;
using Kanbersky.Ella.DAL.Concrete.EntityFramework.GenericRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Kanbersky.Ella.Api
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.Configure<ElasticConnectionSettings>(_configuration.GetSection("ElasticConnectionSettings"));
            services.AddSingleton(typeof(ElasticClientProvider));

            services.AddDbContext<KanberContext>(opt =>
            {
                opt.UseSqlServer(_configuration["ConnectionStrings:DefaultConnection"]);
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IAutoCompleteService<>), typeof(AutoCompleteService<>));

            services.AddScoped<IProductService, ProductService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Kanbersky.Search Microservice",
                    Version = "v1"
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kanbersky Search");
            });
        }
    }
}
