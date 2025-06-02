using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Presentation.Data.Contexts;
using Presentation.Interfaces;
using Presentation.Services;
using System.Text;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri(builder.Configuration.GetValue<string>("VaultUri")!);
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

/* Services */
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

/* Database and Identity */
builder.Services.AddDbContext<AuthDataContext>(x => x.UseSqlServer(builder.Configuration["SqlConnection"]));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(x =>
{
    /* By default requires 1 Uppercase! */
    x.Password.RequiredLength = 6;
    x.Password.RequireNonAlphanumeric = false;
    x.User.RequireUniqueEmail = true;
    x.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AuthDataContext>()
.AddDefaultTokenProviders();

/* JWT - Token */
var key = builder.Configuration["Jwt-Key"];
var issuer = builder.Configuration["Jwt-Issuer"];

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
    };
});


var app = builder.Build();

app.MapOpenApi();
app.UseHttpsRedirection();
app.UseCors(x => { x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();});
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth API");
    c.RoutePrefix = string.Empty;
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
