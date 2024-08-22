﻿using MediaBase.Models.ConfigModels;
using MediaBase.Services;

namespace MediaBase
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(
                options => options.AddPolicy("AllowCors",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                        .WithMethods("GET")
                        .AllowAnyHeader();
                    }
                )
            );

            services.AddControllers();
            services.AddControllersWithViews();

            services.Configure<MediaConfigs>
                (options => Configuration.GetSection("MediaConfigs").Bind(options));
            services.Configure<ConversionConfig>
                (options => Configuration.GetSection("ConversionConfig").Bind(options));

            services.AddScoped<MoviePathProvider>();
            services.AddScoped<SeriesPathProvider>();

            services.AddScoped<MovieRequestManager>();
            services.AddScoped<SeriesRequestManager>();

            services.AddScoped<MediaConverter>();

            services.AddHostedService<AutomaticConversionManager>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowCors");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}