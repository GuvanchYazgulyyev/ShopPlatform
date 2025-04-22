using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopPlatform.Server.Context;
using ShopPlatform.Server.Services.Extentions;
using ShopPlatform.Server.Services.Infrastuce;
using ShopPlatform.Server.Services.Services;
using System.Text;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// ✅ Configuration nesnesini al
var configuration = builder.Configuration;

// ✅ Razor Pages ve MVC
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// ✅ Veritabanı bağlantısı (DbContext)
builder.Services.AddDbContext<BlazorShopDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// ✅ JWT Authentication ayarları. Oluşturulan Tokeninin Validasyon işlemi. 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["JwtIssuer"],
        ValidAudience = configuration["JwtAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecurityKey"]))
    };
});

builder.Services.AddScoped<ShopPlatform.Client.ModalManager>(); //////////  Kontrol Edilecek

// ✅ AutoMapper Extension çağrısı
builder.Services.ConfigureMapping();

// ✅ Service Dependency Injection
builder.Services.AddScoped<IUserService, UserService>();

// ✅ Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddBlazoredLocalStorage(conf => conf.JsonSerializerOptions.WriteIndented = true); // UI de Local de tutmak için

var app = builder.Build();

// ✅ Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging(); // Blazor WebAssembly desteği
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles(); // Blazor dosyaları
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // ✅ Authentication middleware
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();


