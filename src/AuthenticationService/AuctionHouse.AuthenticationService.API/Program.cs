using System.Text;
using AuctionHouse.AuthenticationService.API.Interface;
using AuctionHouse.AuthenticationService.Domain.Entities;
using AuctionHouse.AuthenticationService.Infrastructure.DbContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

var builder = WebApplication.CreateBuilder(args);

var authMethod =
    new TokenAuthMethodInfo("00000000-0000-0000-0000-000000000000");
var httpClientHandler = new HttpClientHandler();
httpClientHandler.ServerCertificateCustomValidationCallback =
    (_, _, _, _) => true;
var vaultUri = builder.Configuration["VAULT_ADDR"] ?? "https://localhost:8201";
var vaultClientSettings = new VaultClientSettings(vaultUri, authMethod)
{
    Namespace = "",
    MyHttpClientProviderFunc = _
        => new HttpClient(httpClientHandler)
        {
            BaseAddress = new Uri(vaultUri)
        }
};

var vaultClient = new VaultClient(vaultClientSettings);
var kv2Secret = await vaultClient.V1.Secrets.KeyValue.V2
    .ReadSecretAsync(path: "services", mountPoint: "my-app");

var config = new TokenConfig
{
    Key = kv2Secret.Data.Data["JWTKey"].ToString(),
    Audience = kv2Secret.Data.Data["Audience"].ToString(),
    Issuer = kv2Secret.Data.Data["Issuer"].ToString()
};

builder.Services.AddSingleton(provider => config);

// Add services to the container.
builder.Services.AddIdentityCore<User>(options =>
{

})
    .AddEntityFrameworkStores<AuthDbContext>();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config.Issuer,
            ValidAudience = config.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Key))
        };
    }
);

builder.Services.AddAuthorization();

// Register AuthDbContext with the connection string
var connectionString = kv2Secret.Data.Data["authdb_connectionstring"].ToString();
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddCors();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put Bearer + your token in the box below",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            jwtSecurityScheme, Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

try
{
    context.Database.Migrate();
   
}
catch (Exception ex)
{
    logger.LogError(ex, "something went wrong when trying to migrating");
}


app.Run();
