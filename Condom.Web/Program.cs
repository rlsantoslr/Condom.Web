using Condom.Infra;
using Condom.Infra.Global;
using Condom.Web.Data;
using Condom.Web.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddTransient<UserSession>();
builder.Services.AddTransient<JS>();

IOC.ConfigureServices(builder.Services);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(options =>
{
    options.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
    options.MapRazorPages();
    options.MapControllers();
    options.MapBlazorHub(o =>
    {
        o.TransportMaxBufferSize = 1000000;
        o.ApplicationMaxBufferSize = 1000000;
    });
    app.MapFallbackToPage("/_Host");
});

app.Run();
