using Microsoft.EntityFrameworkCore;
using NewNLogWebApi.Models;
using NLog.Web;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers(); //Обязательно нужно добавить для того чтобы иметь доступ к контролеру
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<UsersContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Logging.ClearProviders();
        builder.WebHost.UseNLog();
        var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}