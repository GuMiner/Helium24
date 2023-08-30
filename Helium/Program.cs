using Helium.Data;
using Helium.Data.Identity;
using Helium.Tasks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("IdentitiesDb")));
builder.Services.AddDbContext<PuzzleDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("PuzzleDb")));
builder.Services.AddDbContextFactory<SystemDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SystemDb")));
builder.Services.AddDbContextFactory<ImageDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ImageDb")));
builder.Services.AddDbContextFactory<AirQualityDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("AirQualityDb")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddHostedService<SystemStatusTask>();
builder.Services.AddHostedService<PhotoScalerTask>();
builder.Services.AddHttpClient();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Utility/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseResponseCaching();
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseStatusCodePages(async context =>
{
    context.HttpContext.Response.ContentType = "text/html";

    Console.WriteLine($"Couldn't find page: {context.HttpContext.Request.Path}");

    await context.HttpContext.Response.WriteAsync(
        $"<h1>HTTP {context.HttpContext.Response.StatusCode}</h1><h2>Something isn't working right</h2>" +
        "<p>Please send an email to <a href=\"mailto: gus.gran@helium24.net\">gus.gran@helium24.net</a> and I'll investigate.</p>");
});

app.UseRouting();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseAuthentication();
app.UseAuthorization();

// app.UseMiddleware<TrackerMiddleware>();

app.MapControllers();
app.MapRazorPages();

app.Run();
