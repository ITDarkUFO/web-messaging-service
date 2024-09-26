﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;
using WebServer.Repositories;
using WebServer.Resources;
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
                ContentTypes = { Application.ProblemJson },
            };
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddScoped(sp =>
{
    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();

    return new MessagesRepository(loggerFactory, "Host=192.168.0.101;Username=postgres;Password=postgres;Database=web-messaging-service");
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

app.UseAuthorization();

app.MapControllers();

app.Run();
