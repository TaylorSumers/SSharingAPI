using AspNetCore.Yandex.ObjectStorage.Extensions;
using AspNetCore.Yandex.ObjectStorage.Object;
using SecretsSharingAPI.Controllers;

namespace SecretsSharingAPI
{
    public class Program
    {

        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                var basePath = AppContext.BaseDirectory;

                var xmlPath = Path.Combine(basePath, "SecretsSharingAPI.xml");
                options.IncludeXmlComments(xmlPath);
            });
            builder.Services.AddControllers();
            builder.Services.AddYandexObjectStorage(options =>
            {
                options.BucketName = builder.Configuration.GetValue<string>("S3BucketName");
                options.AccessKey = builder.Configuration.GetValue<string>("S3AccessKey");
                options.SecretKey = builder.Configuration.GetValue<string>("S3SecretKey");
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}