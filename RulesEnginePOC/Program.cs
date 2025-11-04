using Microsoft.EntityFrameworkCore;
using RulesEnginePOC.Entities;
using RulesEnginePOC.Services;
using RulesEnginePOC.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IRuleEngineService, RuleEngineService>();
builder.Services.AddScoped<IProviderRefundRulesService, ProviderRefundRulesService>();
builder.Services.AddDbContext<RuleEngineContext>(opt => opt.UseInMemoryDatabase("RulesEngineDB")); 
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
