namespace ReadmeGenerator.Settings;

public class ProblemSetting {
    public int QueraId { get; set; }
    public List<Contributor> Contributors { get; } = [];

    public class Contributor {
        public string UserName { get; set; } = default!;
        public string AvatarUrl { get; set; } = default!;
        public string ProfileUrl { get; set; } = default!;
    }
}