using CustomMVC.App.Common;
using CustomMVC.App.Common.Abstractions;
using CustomMVC.App.Core.Middleware.Extensions;
using CustomMVC.App.Hosting.Application;
using CustomMVC.App.Hosting.Host;
using CustomMVC.App.MVC.Extensions;
using Execute.Samples;
using Mediator.Extensions;
using Mediator.Interfaces;
using System.Reflection;
using MyORMLibrary.Extensions;

var builder = WebApplication.CreateBuilder();

var configuration = builder.Services.GetService<IConfiguration>();

builder.Services.UseMediator(opt =>
{
    opt.Assemblies = new[] { Assembly.GetExecutingAssembly() };
});

builder.Services.AddORMContext(configuration.Get("Database", "ConnectionString")?.ToString() ?? "");

var app = builder.Build();

app.UseDefaultExceptionHandler();

app.UseStaticFiles();

app.UseControllersWithViews();

app.MapControllerRoute(
    "default",
    "{controller=Home}");

app.MapControllerRoute(
    "users",
    "{controller=User}/{action=GetAllUsers}");

app.MapControllerRoute(
    "users",
    "{controller=User}/{action=PostUser}");

app.MapControllerRoute(
    "users",
    "{controller=User}/{action=GetById}");

app.MapControllerRoute(
    "users",
    "{controller=User}/{action=UpdatePost}");

app.MapControllerRoute(
    "users",
    "{controller=User}/{action=DeletePost}");

//Нужно переделать маршрутизацию на попроще...

app.Run();