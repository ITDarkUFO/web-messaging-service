using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Resources;
using System.Diagnostics;
using WebServer.Repositories;
using WebServer.Services;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers()
    .AddMvcLocalization()
    .AddDataAnnotationsLocalization()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var validationProblemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Title = SharedResources.ValidationError,
                Detail = SharedResources.ValidationErrorDetails,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            validationProblemDetails.Extensions["traceId"] = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;

            return new BadRequestObjectResult(validationProblemDetails)
            {
                ContentTypes = { Application.ProblemJson }
            };
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddAutoMapper(typeof(Program));

var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_WEBSERVER_CONNECTIONSTRING");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Строка подключения не найдена. Убедитесь, что переменная окружения 'ASPNETCORE_WEBSERVER_CONNECTIONSTRING' задана.");
}

builder.Services
    .AddNpgsqlDataSource(connectionString,
        sb =>
        {
            sb.UseLoggerFactory(LoggerFactory.Create(loggingBuilder => loggingBuilder.AddConsole()));

            if (builder.Environment.IsDevelopment())
                sb.EnableParameterLogging();
        });

builder.Services.AddSingleton<MessageEventAggregator>();
builder.Services.AddScoped<MessagesRepository>();
builder.Services.AddSingleton<WebServer.Services.WebSocketManager>();
builder.Services.AddHostedService<WebSocketHostedService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllCors",
                      builder =>
                      {
                          builder.AllowAnyOrigin();
                      });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var supportedCultures = new[] { "en", "ru" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();

app.UseCors("AllCors");

app.UseAuthorization();

app.UseWebSockets();

app.MapControllers();

app.Run();
