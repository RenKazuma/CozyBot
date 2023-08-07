using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using DiscordApi.Controllers;
using Discord_Core.Database;
using Discord_Core;
using Discord_Core.Database;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Discord Bot Cozy", Version = "v1", Description = "Api made for Discord Bot Cozy" });

    var assemblyNames = new[] { typeof(UserController) };

    var xmlFiles = assemblyNames.Select(assemblyName => $"{assemblyName.Assembly.GetName().Name}.xml");
    var xmlPaths = xmlFiles.Select(xmlFile => Path.Combine(AppContext.BaseDirectory, xmlFile)).ToArray();
    foreach (var xmlPath in xmlPaths)
    {
        c.IncludeXmlComments(xmlPath);
    }
});

builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Service.GetConnectionString()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Discord Bot Cozy v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
