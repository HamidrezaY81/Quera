using System.CommandLine;
using ReadmeGenerator.Settings;

namespace ReadmeGenerator;

public static class CommandLine {
    public static Task<int> InvokeAsync(string[] args, AppSettings settings) {
        var delayToRequestQueraInMilliSecondsOption = new Option<int>(
            ["-d", "--request-delay"],
            () => settings.DelayToRequestQueraInMilliSeconds
        );
        delayToRequestQueraInMilliSecondsOption.AddValidator(result => {
            if (result.GetValueOrDefault<int>() < 1)
                result.ErrorMessage = "The delay value must more than 1";
        });

        var readmeTemplatePathOption = new Option<string>(
            ["-t", "--readme-template"],
            () => settings.ReadmeTemplatePath
        );

        var outputOption = new Option<string>(
            ["-o", "--output"],
            () => settings.ReadmeOutputPath,
            "The readme output path");

        var solutionsOption = new Option<string>(
            ["-s", "--solutions"],
            () => settings.SolutionsPath,
            "The solutions directory");

        // Create root command and add options
        var rootCommand = new RootCommand {
            delayToRequestQueraInMilliSecondsOption,
            readmeTemplatePathOption,
            outputOption,
            solutionsOption
        };

        // Set handler for root command
        rootCommand.SetHandler(CommandHandler(settings),
            delayToRequestQueraInMilliSecondsOption, readmeTemplatePathOption,
            outputOption, solutionsOption
        );

        return rootCommand.InvokeAsync(args);
    }

    private static Func<int, string, string, string, Task> CommandHandler(AppSettings settings) {
        return (delayToRequestQueraInMilliSeconds, readmeTemplatePath, outputPath, solutionsPath) => {
            settings.DelayToRequestQueraInMilliSeconds = delayToRequestQueraInMilliSeconds;
            settings.ReadmeTemplatePath = readmeTemplatePath;
            settings.ReadmeOutputPath = outputPath;
            settings.SolutionsPath = solutionsPath;

            return Task.CompletedTask;
        };
    }
}