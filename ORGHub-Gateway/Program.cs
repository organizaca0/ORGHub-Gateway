using MongoDB.Driver;
using ORGHub_Gateway.Factories;
using ORGHub_Gateway.Models;
using ORGHub_Gateway.Repositories;
using ORGHub_Gateway.Security;
using ORGHub_Gateway.Services;

var builder = WebApplication.CreateBuilder(args);
var mongoClient = new MongoClient(builder.Configuration.GetConnectionString("MongoDB"));
var mongoDatabase = mongoClient.GetDatabase("ORG");

builder.Services.AddSingleton(mongoDatabase.GetCollection<User>("Users"));

#region Repositories
builder.Services.AddSingleton<UserRepository>();
#endregion
#region Utils
builder.Services.AddSingleton<PasswordEncoder>();
builder.Services.AddSingleton<JwtService>();
#endregion
#region Services
builder.Services.AddSingleton<AuthService>();
#endregion
#region Factories
builder.Services.AddSingleton<ApiFactory>();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
