using System.Text;
using AuctionHouse.CatalogService.API.Services;
using AuctionHouse.CatalogService.Infrastructure.DbContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
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


// Add services to the container.

builder.Services.AddScoped<ICatalogService, CatalogService>();

builder.Services.AddDbContext<MongoDbContext>(options =>
{
    var connectionString = kv2Secret.Data.Data["mongodb_connectionstring"].ToString();
    var databaseName = kv2Secret.Data.Data["mongodb_DatabaseName"].ToString();
    if (string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(databaseName))
    {
        throw new NullReferenceException("Connectiontring and databaseName can not be null");
    }
    options.UseMongoDB(connectionString, databaseName);
});

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
            ValidIssuer = kv2Secret.Data.Data["Issuer"].ToString(),
            ValidAudience = kv2Secret.Data.Data["Audience"].ToString(),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(kv2Secret.Data.Data["JWTKey"].ToString()))
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token successfully validated.");
                return Task.CompletedTask;
            }
        };
    });

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
