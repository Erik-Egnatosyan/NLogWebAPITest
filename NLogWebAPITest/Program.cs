using FluentAssertions.Common;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using NLogWebAPITest.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSqlServer<NLogDBContext>(builder.Configuration.GetConnectionString("DefaultConnection"));
        
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

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


