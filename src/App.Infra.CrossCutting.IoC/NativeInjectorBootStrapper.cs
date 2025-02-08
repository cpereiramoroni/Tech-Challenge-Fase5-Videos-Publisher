using App.Application.Interfaces;
using App.Domain.Interfaces;
using App.Domain.Models.External;
using App.Infra.Data.Context;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace App.Infra.CrossCutting.IoC
{
    public static class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration config)
        {
            ///     variables
            ///     
            services.AddSingleton<AwsConfig>(_ =>
               new AwsConfig
               {
                   S3Access = config["AWS_ACCESS_KEY_ID"],
                   S3Secret = config["AWS_SECRET_ACCESS_KEY"]
               });

            services.AddSingleton<RabbitMQConfig>(_ =>
            new RabbitMQConfig
            {
                Uri = new Uri(config["UrlRabbit"]),

            });


            ////=======================================================================
            ///
            ///  INSTACIAS DE SERVICES
            /// 
            ///

            services.AddScoped<IVideosService, VideosService>();
            services.AddScoped<IExternalService, ExternalService>();
            ////=======================================================================
            ///
            ///  INSTACIAS DE REPOSITORY
            /// 
            ///
            services.AddScoped<IVideosRepository, VideosRepository>();

            ////=======================================================================
            ///
            ///  INSTACIAS DE CONTEXTO
            /// 
            ///
            services.AddDbContext<MySQLContext>(options =>
              options.UseMySql(
                  config["ConnectionVideos"],
                  ServerVersion.AutoDetect(config["ConnectionVideos"])
              ));
            services.AddScoped<MySQLContext>();




        }
    }
}
