using App.Interfaces;
using App.Services;
using Domain.Interfaces;
using Domain.Messaging;
using Infra.Configuration;
using Infra.Data;
using Infra.Messaging;
using Infra.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var rabbitSettings = builder.Configuration
    .GetSection("RabbitMQSettings")
    .Get<RabbitMQSettings>();
builder.Services.AddSingleton<DBContext>();
builder.Services.AddSingleton(rabbitSettings);
builder.Services.AddScoped<IFraseRomanticaServices, FraseRomanticaServices>();
builder.Services.AddScoped<IFraseRepository, FraseRepository>();
builder.Services.AddSingleton<IFraseProducer, FraseProducer>();

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
