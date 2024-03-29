using CommandLine;
using GbUtil;
using GbUtil.Extensions;
using GitBucket.Core;
using GitBucket.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Octokit;
using Octokit.Internal;

IConfiguration configuration;
var console = new GbUtilConsole();

try
{
    configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true)
        .AddEnvironmentVariables()
        .Build();

    var options = Parser.Default.ParseArguments<ReleaseOptions, MilestoneOptions, IssueOptions, BackupOptions>(args)
        .WithNotParsed(errors =>
        {
            if (errors.Any(e =>
                e.Tag != ErrorType.HelpVerbRequestedError &&
                e.Tag != ErrorType.VersionRequestedError &&
                e.Tag != ErrorType.NoVerbSelectedError))
            {
                throw new InvalidConfigurationException($"Failed to parse arguments.");
            }
        })
        .MapResult<ReleaseOptions, MilestoneOptions, IssueOptions, BackupOptions, CommandLineOptionsBase?>(
            (ReleaseOptions options) => options,
            (MilestoneOptions options) => options,
            (IssueOptions options) => options,
            (BackupOptions options) => options,
            _ => null
        );

    // In case of default verbs (--help or --version)
    if (options == null)
    {
        return 0;
    }

    var requireDbConnection = options is ReleaseOptions || options is MilestoneOptions || options is BackupOptions;
    using var scope = CreateServiceProvider(configuration, requireDbConnection).CreateScope();
    var result = options switch
    {
        ReleaseOptions releaseOptions
            => await scope.ServiceProvider.GetRequiredService<IReleaseService>().Execute(releaseOptions, CreateGitBucketClient(configuration, console)),
        MilestoneOptions milestoneOptions
            => await scope.ServiceProvider.GetRequiredService<IMilestoneService>().ShowMilestones(milestoneOptions),
        IssueOptions issueOptions
            => await scope.ServiceProvider.GetRequiredService<IIssueService>().Execute(issueOptions, CreateGitBucketClient(configuration, console)),
        BackupOptions backupOptions
            => scope.ServiceProvider.GetRequiredService<IBackupService>().Backup(backupOptions, configuration.GetSection("GbUtil_ConnectionStrings").Value!),
        _ => 1
    };

    return result;
}
catch (InvalidConfigurationException ex)
{
    console.WriteWarnLine(ex.Message);
    return 1;
}
catch (Exception ex)
{
    console.WriteErrorLine(ex.Message);
    console.WriteErrorLine(ex.StackTrace);
    return 1;
}

static ServiceProvider CreateServiceProvider(
        IConfiguration configuration,
        bool requireDbConnection = false)
{
    var connectionString = "";
    if (requireDbConnection)
    {
        connectionString = configuration.GetSection("GbUtil_ConnectionStrings")?.Value;
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidConfigurationException("PostgreSQL ConnectionString is not configured. Add \"GbUtil_ConnectionStrings\" environment variable.");
        }

        try
        {
            using var sqlConnection = new NpgsqlConnection(connectionString);
            sqlConnection.Open();
        }
        catch (Exception)
        {
            throw new InvalidConfigurationException("Cannot open connection with PostgreSQL.");
        }
    }

    return new ServiceCollection()
        .AddScopedIf<DbContext>(requireDbConnection, _ => new GitBucketDbContext(connectionString))
        .AddTransient<IReleaseService, ReleaseService>()
        .AddTransient<IMilestoneService, MilestoneService>()
        .AddTransient<IIssueService, IssueService>()
        .AddTransient<IBackupService, BackupService>()
        .AddTransient<IConsole, GbUtilConsole>()
        .BuildServiceProvider();
}

static IGitHubClient CreateGitBucketClient(IConfiguration configuration, IConsole console)
{
    var gitbucketUri = configuration.GetSection("GbUtil_GitBucketUri")?.Value;
    if (string.IsNullOrEmpty(gitbucketUri))
    {
        throw new InvalidConfigurationException("GitBucket URI is not configured. Add \"GbUtil_GitBucketUri\" environment variable.");
    }

    Credentials credentials;
    var token = configuration.GetSection("GbUtil_AccessToken")?.Value;
    if (!string.IsNullOrEmpty(token))
    {
        credentials = new Credentials(token);
    }
    else
    {
        var user = configuration.GetSection("GbUtil_UserName")?.Value;
        if (string.IsNullOrEmpty(user))
        {
            console.Write("Enter your Username: ");
            user = console.ReadLine();
            if (string.IsNullOrEmpty(user))
            {
                throw new InvalidConfigurationException("Username is required");
            }
        }

        var password = configuration.GetSection("GbUtil_Password")?.Value;
        if (string.IsNullOrEmpty(password))
        {
            console.Write("Enter your Password: ");
            password = console.GetPassword();
            if (string.IsNullOrEmpty(password))
            {
                throw new InvalidConfigurationException("Password is required");
            }
        }

        credentials = new Credentials(user, password);
    }

    return new GitHubClient(
        new Connection(
            new ProductHeaderValue("gbutil"),
            new Uri(gitbucketUri),
            new InMemoryCredentialStore(credentials),
            new HttpClientAdapter(() => new GitBucketMessageHandler()),
            new SimpleJsonSerializer()
        ));
}
