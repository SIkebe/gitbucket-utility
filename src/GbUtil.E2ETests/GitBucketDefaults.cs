using System;

namespace GbUtil.E2ETests
{
    public static class GitBucketDefaults
    {
        public static string ConnectionStrings => Environment.GetEnvironmentVariable("GbUtil_ConnectionStrings")!;
        public static string Owner => "root";
        public static string Password => "root";
        public static string ApiEndpoint => Environment.GetEnvironmentVariable("GbUtil_GitBucketUri")!;
        public static string BaseUri => Environment.GetEnvironmentVariable("GbUtil_BaseUri")!;
    }
}
