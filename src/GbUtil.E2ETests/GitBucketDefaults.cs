using System;

namespace GbUtil.E2ETests
{
    public static class GitBucketDefaults
    {
        private static string repository1;
        private static string repository2;
        public static string ConnectionStrings { get; set; } = "Host=localhost;Username=gitbucket;Password=gitbucket;Database=gitbucket";
        public static string GbUtilDll { get; set; } = @"C:\GitHub\gitbucket-utility\src\GbUtil\bin\Debug\netcoreapp3.0\GbUtil.dll";
        public static string Owner { get; set; } = "root";
        public static string Password { get; set; } = "root";
        public static string Repository1
        {
            get
            {
                if (string.IsNullOrEmpty(repository1))
                {
                    repository1 = Guid.NewGuid().ToString();
                }

                return repository1;
            }
            set
            {
                repository1 = value;
            }
        }

        public static string Repository2
        {
            get
            {
                if (string.IsNullOrEmpty(repository2))
                {
                    repository2 = Guid.NewGuid().ToString();
                }

                return repository2;
            }
            set
            {
                repository2 = value;
            }
        }

        public static string ApiEndpoint { get; set; } = "http://localhost:8080/api/v3/";
        public static string BaseUri { get; set; } = "http://localhost:8080/";
    }
}
