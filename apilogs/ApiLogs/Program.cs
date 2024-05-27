using ApiLogs.Core.Interfaces.Core;
using ApiLogs.Core.Interfaces.Persistence;
using ApiLogs.Core.Services;
using ApiLogs.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyeccción de dependencias
builder.Services.AddScoped<ILogsCore, LogsServiceCore>();
builder.Services.AddScoped<ILogsDataBase, LogsDataBase>();
builder.Services.AddScoped<IHealthCore, HealthServiceCore>();

builder.Services.AddScoped<RabbitMqLogListener>();

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var rabbitMqLogListener = scope.ServiceProvider.GetRequiredService<RabbitMqLogListener>();
    rabbitMqLogListener.StartAsync(default).GetAwaiter().GetResult();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment() || app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
