using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using YouTubeApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<YouTubeApiService>(provider =>
    new YouTubeApiService("AIzaSyDx0586INs3gwSQ23uBY1bSUhgTEUTGhT4")); // Replace with your actual YouTube API key

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Set default route to Home page
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Home is the default controller and Index action

app.MapControllerRoute(
    name: "youtube",
    pattern: "youtube/{action=Index}/{query?}",
    defaults: new { controller = "YouTube", action = "Index" });

app.Run();
