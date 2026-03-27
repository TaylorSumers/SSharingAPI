using Application.Common.Behaviours;
using AspNetCore.Yandex.ObjectStorage.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddYandexObjectStorage(options =>
            {
                options.BucketName = configuration["S3BucketName"];
                options.AccessKey = configuration["S3AccessKey"];
                options.SecretKey = configuration["S3SecretKey"];
            });
            services.AddValidatorsFromAssembly(assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddMediatR(options => options.RegisterServicesFromAssembly(assembly));
            return services;
        }
    }
}
