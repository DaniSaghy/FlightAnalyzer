using FlightAnalyzer;
using FlightAnalyzer.Services;
using Microsoft.Extensions.Options;
using System.IO.Abstractions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Bind configuration settings
builder.Services.Configure<FlightServiceSettings>(builder.Configuration.GetSection("FlightServiceSettings"));

// Register IFileSystem for abstraction
builder.Services.AddSingleton<IFileSystem, FileSystem>();

// Register FlightService with injected settings
builder.Services.AddScoped<IFlightService>(serviceProvider =>
{
    var logger = serviceProvider.GetRequiredService<ILogger<FlightService>>();
    var fileSystem = serviceProvider.GetRequiredService<IFileSystem>();
    var settings = serviceProvider.GetRequiredService<IOptions<FlightServiceSettings>>().Value;

    return new FlightService(logger, fileSystem, settings.CsvFilePath!);
});

// Register FlightAnalysisService
builder.Services.AddScoped<IFlightAnalysisService, FlightAnalysisService>();

// Configure Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
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

app.Run();
