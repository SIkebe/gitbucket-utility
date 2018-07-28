using System;
using System.IO;
using System.Linq;
using CommandLine;
using GitBucket.Core;
using GitBucket.Data.Repositories;
using GitBucket.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GbUtil
{
    class Program
    {
        private static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            try
            {
                Configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();

                var connectionString = Configuration.GetConnectionString("GitBucketConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("PostgreSQL ConnectionString is not configured. Add \"ConnectionStrings: GitBucketConnection\" environment variable.");
                    Console.ResetColor();
                    return;
                }

                var serviceProvider = new ServiceCollection()
                    .AddScoped<DbContext>(_ => new GitBucketDbContext(connectionString))
                    .AddTransient<IReleaseNoteService, ReleaseNoteService>()
                    .AddTransient<IssueRepositoryBase, IssueRepository>()
                    .AddTransient<LabelRepositoryBase, LabelRepository>()
                    .BuildServiceProvider();

                using (var scope = serviceProvider.CreateScope())
                {
                    var releaseNoteService = scope.ServiceProvider.GetRequiredService<IReleaseNoteService>();
                    Parser.Default.ParseArguments<ReleaseOptions>(args)
                        .WithParsed(options => releaseNoteService.OutputReleaseNotes(options));
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ResetColor();
            }
        }
    }
}
