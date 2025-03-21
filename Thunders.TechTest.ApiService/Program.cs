using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Thunders.TechTest.ApiService;
using Thunders.TechTest.ApiService.Context;
using Thunders.TechTest.ApiService.Dtos;
using Thunders.TechTest.ApiService.Messages;
using Thunders.TechTest.ApiService.Persistence;
using Thunders.TechTest.ApiService.Persistence.Interfaces;
using Thunders.TechTest.ApiService.Repositories;
using Thunders.TechTest.ApiService.Repositories.Interfaces;
using Thunders.TechTest.ApiService.Services.Implementations;
using Thunders.TechTest.ApiService.Services.Interfaces;
using Thunders.TechTest.OutOfBox.Database;
using Thunders.TechTest.OutOfBox.Queues;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddControllers();

var features = Features.BindFromConfiguration(builder.Configuration);

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Thunders.TechTest API",
        Version = "v1",
        Description = "API para gestão de pedagios e relatórios.",
    });
});

// Add services to the container.
builder.Services.AddProblemDetails();

if (features.UseMessageBroker)
{
    builder.Services.AddBus(builder.Configuration, new SubscriptionBuilder()
        .Add<PedagioMessage>()
        .Add<FalhaProcessamentoRelatorioMessage>()
        .Add<FalhaProcessamentoPedagioMessage>()
        .Add<RelatorioValorTotalPorHoraMessage>()
        .Add<RelatorioRankingPracasPorMesMessage>()
        .Add<RelatorioQuantidadeDeTiposDeVeiculosPorPracaMessage>());
}

if (features.UseEntityFramework)
{
    builder.Services.AddSqlServerDbContext<ThundersTechTestDbContext>(builder.Configuration);

    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IPedagioRepository, PedagioRepository>();
    builder.Services.AddScoped<IRelatorioRepository, RelatorioRepository>();

    builder.Services.AddScoped<IPedagioService, PedagioService>();
    builder.Services.AddScoped<IRelatorioService, RelatorioService>();

    builder.Services.AddScoped<IMessageSender, RebusMessageSender>();
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ThundersTechTestDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Thunders.TechTest API v1");
        c.RoutePrefix = string.Empty;
    });
}

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapDefaultEndpoints();

app.MapControllers();

app.Run();
