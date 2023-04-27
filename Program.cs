using System.Text.Json.Serialization;
using anuR.Context;
using anuR.DataSeeder;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using anuR.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:3000");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IHashService, HashService>();
builder.Services.AddTransient<DataSeeder>();
builder.Services.AddEntityFrameworkNpgsql()
    .AddDbContext<MainContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));    

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// add cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Error/AccessDenied";
        options.Cookie.Name = "anuR";
    });

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

void SeedData(WebApplication app)
{
    var scopedFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var seeder = scope.ServiceProvider.GetService<DataSeeder>();
        seeder.SeedData();
        
        Console.WriteLine("Data seeded");
        System.Environment.Exit(0);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCookiePolicy();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
