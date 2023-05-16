using Microsoft.EntityFrameworkCore;
using NewNLogWebApi.Models;
using NewNLogWebApi.Service;
using NLog.Web;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Logging.ClearProviders();
        builder.WebHost.UseNLog();
        var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        try
        {
            builder.Services.AddControllers().AddNewtonsoftJson(); //Обязательно нужно добавить для того чтобы иметь доступ к контролеру
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<UsersContext>(
            options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            //builder.Services.AddDbContext<UsersContext>(x =>
            //{
            //    x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            //});
            builder.Services.AddTransient<IUserService, UserService>(); //Нужен для имплементации -> Dependency injection


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
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        
    }
}