using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using ORGHub_Gateway.Factories;
using ORGHub_Gateway.Repositories;
using ORGHub_Gateway.Security;
using ORGHub_Gateway.Services;

var builder = WebApplication.CreateBuilder(args);

#region DbConfig
var mongoClient = new MongoClient(builder.Configuration.GetConnectionString("MongoDB"));
var mongoDatabase = mongoClient.GetDatabase("ORG");

builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase);
#endregion
#region Repositories
builder.Services.AddSingleton<UserRepository>();
#endregion
#region Utils
builder.Services.AddSingleton<PasswordEncoder>();
builder.Services.AddSingleton<JwtService>();
#endregion
#region Services
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<AuthService>();
#endregion
#region Factories
builder.Services.AddSingleton<ApiFactory>();
#endregion
#region Security 
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"], 
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"], 
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero 
    };
});

builder.Services.AddAuthorization();
#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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