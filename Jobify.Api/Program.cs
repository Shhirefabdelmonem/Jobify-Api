
using HealthChecks.UI.Client;
using Jobify.Infrastructure.Extention;
using Jobify.Services.Extension;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Scalar.AspNetCore;

namespace Jobify.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add infrastructure services (DB context, repositories, health checks, identity, JWT)
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();

            // Dependency Injection for application services

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
                app.UseSwaggerUI(options => options.SwaggerEndpoint(url: "/openapi/v1.json", name: "v1"));
            }

            app.MapHealthChecks("health", new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
            app.UseHttpsRedirection();
            app.UseCors("AllowAngularApp");

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
