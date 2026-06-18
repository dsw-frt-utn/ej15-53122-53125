
using Dsw2026Ej15.Data;

namespace Dsw2026Ej15.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

    

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

         
            builder.Services.AddSwaggerGen();

     
            builder.Services.AddSingleton<IPersistence, PersistenceInMemory>();

            var app = builder.Build();

            app.UseMiddleware<Dsw2026Ej15.Api.Middlewares.ExceptionMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Clinica API v1");
                options.RoutePrefix = "swagger"; 
            });

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}