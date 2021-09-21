namespace GbUtil.E2ETests
{
    public static class GitBucketDefaults
    {
        public static string ConnectionStrings { get; set; } = "Host=localhost;Port=3306;Username=gitbucket;Password=gitbucket;Database=gitbucket";
        public static string Owner { get; set; } = "root";
        public static string Password { get; set; } = "root";
        public static string ApiEndpoint { get; set; } = "http://localhost:8080/api/v3/";
        public static string BaseUri { get; set; } = "http://localhost:8080/";
    }
}
