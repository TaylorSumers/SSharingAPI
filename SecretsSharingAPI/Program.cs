using Application.Common.Mappings;
using Application.Interfaces;
using AspNetCore.Yandex.ObjectStorage.Extensions;
using Persistence;
using System.Reflection;
using Application;
using SecretsSharingAPI.Middleware;

namespace SecretsSharingAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAutoMapper(config =>
            {
                config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
                config.AddProfile(new AssemblyMappingProfile(typeof(ISecretsDbContext).Assembly));
            });
            builder.Services.AddApplication(builder.Configuration);
            builder.Services.AddPersistence(builder.Configuration);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "SecretsSharingAPI.xml");
                options.IncludeXmlComments(xmlPath);
            });
            builder.Services.AddControllers();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var context = serviceProvider.GetRequiredService<SecretsDbContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    // TODO: catch ex
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
            }
            app.UseCustomExceptionHandler();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}