using ApiGateway.Core.Interfaces.Core;
using ApiGateway.Core.Interfaces.Persistence;
using ApiGateway.Core.Services;
using ApiGateway.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyección de dependencias
builder.Services.AddScoped<IUserProfileCore, UserProfileServiceCore>();
builder.Services.AddScoped<IUserProfileDataBase, UserProfileDataBase>();
builder.Services.AddScoped<IHealthCore, HealthServiceCore>();

builder.Services.AddScoped<RabbitMqLogListener>();

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var rabbitMqLogListener = scope.ServiceProvider.GetRequiredService<RabbitMqLogListener>();
    rabbitMqLogListener.StartAsync(default).GetAwaiter().GetResult();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
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
