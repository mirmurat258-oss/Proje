using API.Conf;
using API.Data;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScalarWithJWT();
// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
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
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<StudentService>();

var app = builder.Build();
// Veritabanı otomatik olarak oluşturulacak.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.EnsureDatabaseCreated(); // Veritabanını oluştur
}
if (app.Environment.IsDevelopment())
{
    app.UseScalarUI();
    //app.MapOpenApi();
    //app.MapScalarApiReference(options =>
    //{
    //    options.Title = "Web Api Çalışma";
    //    options.Theme = ScalarTheme.BluePlanet;
    //    options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
    //    options.CustomCss = "";
    //    options.ShowSidebar = true;
    //});
}



//app.UseHttpsRedirection();

app.UseAuthentication(); // JWT için eklendi
app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllers();

app.UseStaticFiles();
app.MapFallbackToFile("index.html");


app.Run();

