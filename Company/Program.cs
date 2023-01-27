using Company.Domain;
using Company.Domain.Repositories.Abstract;
using Company.Domain.Repositories.EntityFramework;
using Company.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.Bind("Project", new Config());
 
builder.Services.AddScoped<ITextFieldsRepository, EFTextFieldsRepository>();
builder.Services.AddScoped<IServiceItemsRepository, EFServiceItemsRepository>();
builder.Services.AddTransient<DataManager>();


builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(Config.ConnectionString));

builder.Services.AddControllersWithViews((x =>
{
    x.Conventions.Add(new AdmiAreaAuthorization("Admin", "AdminArea"));
}));


builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts =>
{
    opts.User.RequireUniqueEmail = true;
    opts.Password.RequiredLength = 6;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireDigit = false;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "myCompanyAuth";
    options.Cookie.HttpOnly= true;
    options.LoginPath = "/account/login";
    options.AccessDeniedPath= "/account/accessdenied";
    options.SlidingExpiration = true;
});

builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("AdminArea", policy => { policy.RequireRole("admin"); });
});

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );


app.Run();
