using System;
using BannerWebAPI.Mappers;
using BannerWebAPI.Models;
using BannerWebAPI.Repositories;
using BannerWebAPI.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace BannerWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services
                .AddMvc(options => { options.EnableEndpointRouting = false; })
                .AddJsonOptions(options => { options.JsonSerializerOptions.IgnoreNullValues = true; });

            var dummyBanner = new Banner
            {
                Id = 1,
                Created = DateTime.UtcNow,
                Html = "<h1>My title</h1><p>hello<span style=\"color:red\">!</span></p>"
            };

            var bannerRepository = new BannerRepository();
            bannerRepository.TrySave(dummyBanner);

            services.AddSingleton<IBannerRepository>(provider => bannerRepository);
            services.AddSingleton<IMapper, Mapper>();
            services.AddSingleton<IValidate, ValidateHtml>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "BannerWebAPI", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors();
            app.UseMvc();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
