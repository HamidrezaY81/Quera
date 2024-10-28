using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using OnRail.Extensions.OnFail;
using OnRail.Extensions.Try;
using ReadmeGenerator.Collector.Models;
using ReadmeGenerator.Settings;

namespace ReadmeGenerator.Crawler;

public class CrawlerService(AppSettings settings, ILogger<CrawlerService> logger) {
    public async IAsyncEnumerable<Problem> CompleteProblemTitlesAsync(IEnumerable<Problem> problems) {
        var problemsWithoutTitle = problems
            .Where(problem => problem.QueraTitle is null).ToList();
        logger.LogInformation("{Count} problems have not title.", problemsWithoutTitle.Count);

        foreach (var problem in problemsWithoutTitle) {
            logger.LogInformation("Title for {QueraId} is not cached. Try to download it.", problem.QueraId);

            var requestResult = await TryExtensions.Try(() =>
                    GetProblemTitleAsync(problem.QueraId.ToString()), settings.NumberOfTry
            );
            requestResult.OnFailThrowException();

            problem.QueraTitle = requestResult.Value;

            yield return problem;

            logger.LogInformation("Delay {delay}", settings.DelayToRequestQueraInMilliSeconds);
            await Task.Delay(settings.DelayToRequestQueraInMilliSeconds);
        }
    }

    private async Task<string> GetProblemTitleAsync(string queraId) {
        var url = string.Format(settings.ProblemUrlFormat, queraId);
        var web = new HtmlWeb();
        return (await web.LoadFromWebAsync(url))
            .DocumentNode
            .SelectSingleNode("//aside/div/div[1]/div[1]/div/div[1]/h1")
            .InnerText;
    }
}