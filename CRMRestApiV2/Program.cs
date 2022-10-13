
using System.Diagnostics.CodeAnalysis;
internal class Program
{
    [ExcludeFromCodeCoverage]
    private static void Main(string[] args)
    {
        
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        //temporary
        app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(x => x.SerializeAsV2 = true);
            app.UseSwaggerUI();
            
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}