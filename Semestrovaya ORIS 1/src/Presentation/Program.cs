using CustomMVC.App.Common.Abstractions;
using CustomMVC.App.Core.Middleware.Extensions;
using CustomMVC.App.Hosting.Application;
using CustomMVC.App.MVC.Extensions;
using CustomMVC.App.Hosting.Application.Extensions;
using Data.Extensions;
using Application.Extensions;
using Microsoft.Identity.Client;
using CustomMVC.App.MVC.Controllers.Common.Entities;
using Presentation.Controllers;
using System.Net;
using WebFramework.MVC.Controllers.Abstractions.Attributes;
using CustomMVC.App.MVC.Controllers.Routing;
using Presentation.Services.Interfaces;
using Presentation.Services;

var builder = WebApplication.CreateBuilder();

var config = builder.Services.GetService<IConfiguration>();

builder.Services.AddDAL(config.Get("Database", "ConnectionString")!.ToString()!);
builder.Services.AddApplicationLayer();
builder.Services.AddSingleton<IImageUploader, ImageService>();

var app = builder.Build();

app.UseDefaultExceptionHandler();

app.UseStaticFiles();

app.UseControllersWithViews();

//Аутентификация
app.Use(async (context, next) =>
{
    var metadata = context.Endpoint.Metadata;

    var controller = metadata.GetMetadata<Type>();
    if (controller?.BaseType == typeof(ControllerBase) && controller.CustomAttributes.Any(a => a.AttributeType == typeof(Authorize)))
    {
        var requiredCookie = new Cookie("isAuthorized", "some-key");

        var methods = metadata.GetRequireMetadata<IReadOnlyList<ActionDescriptor>>();

        var isAnyAnonymous = methods.Any(m => m.ActionMetadata.Any(t => t.GetType().Name == "AllowAnonymous"));

        if ((!context.Request.Cookie.Contains(requiredCookie) || context.Request.Cookie["isAuthorized"]!.Expired) && !isAnyAnonymous)
        {
            context.Response.Redirect("http://localhost:8888/api/Admin/RegisterPage");

            return;
        }
    }

    await next();
});

app.MapControllerRoute(
    "default",
    "/",
    new CustomMVC.App.Core.Routing.Common.Defaults()
    {
        ControllerName = "Home",
        ActionName = "Index",
    });

app.MapControllerRoute(
    "Hotel",
    "api/{controller=Hotel}/{action=GetAllSearchData}");

app.MapControllerRoute(
    "Hotel",
    "api/{controller=Hotel}/{action=GetAllResorts}");

app.MapControllerRoute(
    "Hotel",
    "api/{controller=Hotel}/{action=GetAllTags}");

app.MapControllerRoute(
    "Hotel",
    "api/{controller=Hotel}/{action=GetHotelById}");

app.MapControllerRoute(
    "Hotel",
    "api/{controller=Hotel}/{action=GetHotelTags}");

app.MapControllerRoute(
    "default",
    "api/{controller=Home}/{action=ApplyFilters}");

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Hotel}");

app.MapControllerRoute(
    "Admin",
    "api/{controller=Admin}/{action=AdminIndex}");

app.MapControllerRoute(
    "Admin",
    "api/{controller=Admin}/{action=Register}");

app.MapControllerRoute(
    "Admin",
    "api/{controller=Admin}/{action=RegisterPage}");

app.MapControllerRoute(
    "Admin",
    "api/{controller=Admin}/{action=DeleteHotel}");

app.MapControllerRoute(
    "Admin",
    "api/{controller=Admin}/{action=GetHotelById}");

app.MapControllerRoute(
    "Admin",
    "api/{controller=Admin}/{action=AdminCreateHotelPartial}");

app.MapControllerRoute(
    "Admin",
    "api/{controller=Admin}/{action=AdminUpdateHotelPartial}");

app.MapControllerRoute(
    "default",
    "api/{controller=Admin}/{action=CreateHotel}",
    new CustomMVC.App.Core.Routing.Common.Defaults() { ControllerName = "Admin", ActionName = "CreateHotel" });

app.MapControllerRoute(
    "Admin",
    "api/{controller=Admin}/{action=UploadHotelImage}");

app.MapControllerRoute(
    "Admin",
    "api/{controller=Admin}/{action=UpdateHotel}");

app.Run();
