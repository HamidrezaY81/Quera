using System.Text;
using OnRail;
using OnRail.Extensions.OnFail;
using OnRail.Extensions.OnSuccess;
using OnRail.Extensions.Try;
using ReadmeGenerator.Collector.Models;
using ReadmeGenerator.Settings;

namespace ReadmeGenerator.Generator;

public class GeneratorService(AppSettings settings) {
    public Result<StringBuilder> GenerateReadmeSection(List<Problem> problems, int? limit = null) =>
        TryExtensions.Try(() => limit is null or < 1 ? problems : problems.Take((int)limit).ToList())
            .OnSuccess(targetProblems =>
                GenerateReadme(targetProblems, problems.Count, CountSolutions(problems))
            );

    private static int CountSolutions(IEnumerable<Problem> problems) =>
        problems.Sum(p => p.Solutions.Count);

    private StringBuilder GenerateReadme(List<Problem> problems, int? allProblems = null,
        int? allSolutions = null) {
        var readme = new StringBuilder();

        var numberOfProblemsSolved = allProblems ?? problems.Count;
        readme.AppendLine($"Number of problems solved: **{numberOfProblemsSolved}**\n");

        var numberOfSolutions = allSolutions ?? CountSolutions(problems);
        if (numberOfProblemsSolved != numberOfSolutions)
            readme.AppendLine($"Number of solutions: **{numberOfSolutions}**\n");

        readme.AppendLine("<table>")
            .AppendLine("  <tr>")
            .AppendLine("    <th>Question</th>")
            .AppendLine("    <th>Title</th>")
            .AppendLine("    <th>Solutions</th>")
            .AppendLine("    <th>Last commit</th>")
            .AppendLine("    <th>Contributors</th>")
            .AppendLine("  </tr>");

        foreach (var problem in problems) {
            AppendProblemData(readme, problem, settings.SolutionUrlFormat, settings.ProblemUrlFormat)
                .OnFailThrowException();
        }

        readme.AppendLine("</table>");

        return readme;
    }

    private static Result AppendProblemData(StringBuilder source,
        Problem problem, string solutionUrlFormat, string problemUrlFormat) =>
        TryExtensions.Try(() => {

            var queraIdLink = $"<a href=\"#{problem.QueraId}\">{problem.QueraId}</a>";

            var url = string.Format(problemUrlFormat, problem.QueraId);
            var queraTitleLink = $"<a href=\"{url}\">{problem.QueraTitle}</a>";
            
            var solutionsSection = GenerateSolutionsSection(problem, solutionUrlFormat);
            var lastCommitFormatted = problem.LastSolutionsCommit.ToString("dd-MM-yyyy");
            var contributorsDiv = GenerateContributorDiv(problem);

            source.AppendLine($"  <tr id=\"{problem.QueraId}\">")
                .AppendLine($"    <td>{queraIdLink}</td>")
                .AppendLine($"    <td>{queraTitleLink}</td>")
                .AppendLine($"    <td>{solutionsSection}</td>")
                .AppendLine($"    <td>{lastCommitFormatted}</td>")
                .AppendLine($"    <td>{contributorsDiv}</td>")
                .AppendLine("  </tr>");
        });

    private static string GenerateContributorDiv(Problem problem) {
        var contributorLinks = problem.Contributors
            .OrderByDescending(contributor => contributor.NumOfCommits)
            .Select(contributor => {
                    var titleValue = contributor.NumOfCommits is null
                        ? $"{contributor.Name}"
                        : $"{contributor.NumOfCommits} commits";
                    return
                        $"<a href=\"{contributor.ProfileUrl}\" title=\"{titleValue}\"><img src=\"{contributor.AvatarUrl}\" alt=\"{contributor.Name}\" style=\"border-radius:100%\" width=\"32px\" height=\"32px\"></a>";
                }
            );

        var contributorDiv =
            $"<div style=\"display: flex; flex-direction: row; gap: 2px;\">{string.Join(" ", contributorLinks)}</div>";
        return contributorDiv;
    }

    private static string GenerateSolutionsSection(Problem problem, string solutionUrlFormat) {
        var solutionLinks = problem.Solutions
            .OrderByDescending(solution => solution.LastCommitDate)
            .ThenBy(solution => solution.LanguageName)
            .Select(solution => {
                var solutionUrl = GetSolutionUrl(problem, solutionUrlFormat, solution);

                return $"<a href=\"{solutionUrl}\">{new FileInfo(solution.LanguageName).Name}</a>";
            });
        var solutionsSection = string.Join(" - ", solutionLinks);
        return solutionsSection;
    }

    private static string GetSolutionUrl(Problem problem, string solutionUrlFormat, Solution solution) {
        var solutionPath = Path.Combine(problem.QueraId.ToString(), solution.LanguageName);
        if (!string.IsNullOrWhiteSpace(solution.SingleFileName))
            solutionPath = Path.Combine(solutionPath, solution.SingleFileName);

        var solutionUrl = string.Format(solutionUrlFormat, solutionPath);
        return solutionUrl;
    }
}