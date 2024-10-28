﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using OnRail.Extensions.OnFail;
using ReadmeGenerator;
using ReadmeGenerator.Cache;
using ReadmeGenerator.Collector;
using ReadmeGenerator.Crawler;
using ReadmeGenerator.Generator;
using ReadmeGenerator.Helpers;
using ReadmeGenerator.Settings;

var services = new ServiceCollection();

// Add logging
services.AddLogging(builder => builder.AddConsole());

// Add configuration
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
services.AddSingleton<IConfiguration>(configuration);

var appSettings = configuration.GetSection("App")
    .Get<AppSettings>() ?? throw new ArgumentException("Can not load app settings data.");
services.AddSingleton(appSettings);

services.AddDbContext<CacheDbContext>(options =>
    options.UseSqlite(configuration.GetConnectionString("Default")));

// Parse inputs and update the appSettings
await CommandLine.InvokeAsync(args, appSettings);

// Add application services
services.AddScoped<CollectorService>();
services.AddScoped<GeneratorService>();
services.AddScoped<CacheRepository>();
services.AddScoped<CrawlerService>();
services.AddScoped<AppRunner>();

// -----------------------------------------------------------------
await using var serviceProvider = services.BuildServiceProvider();

var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application Starting...");

// Ensure the database is created
var db = serviceProvider.GetRequiredService<CacheDbContext>();
await db.Database.EnsureCreatedAsync();

try {
    var app = serviceProvider.GetRequiredService<AppRunner>();
    await app.RunAsync()
        .OnFailTee(result => {
            //for GitHub action: https://github.com/HamidMolareza/QueraProblems/issues/10
            Environment.ExitCode = -1;

            var detail = result.Detail is null
                ? "That's all we know!"
                : $"See the below text for more information:\n{result.Detail.ToStr()}";
            var message = $"The operation failed. {detail}";

            logger.LogError("{message}", message);
        });
}
catch (Exception ex) {
    //for GitHub action: https://github.com/HamidMolareza/QueraProblems/issues/10
    Environment.ExitCode = -1;

    logger.LogError(ex, "An error occurred.");
}