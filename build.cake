#addin nuget:?package=Cake.Docker&version=0.10.1

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDirectories = GetDirectories("./src/**/bin/" + configuration);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectories(buildDirectories);
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore();
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreBuild(
        "./GbUtil.sln",
        new DotNetCoreBuildSettings 
        {
            Configuration = configuration 
        });
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetCoreTest(
        "./src/GitBucket.Service.Tests/GitBucket.Service.Tests.csproj",
        new DotNetCoreTestSettings 
        {
            Configuration = configuration 
        });
});

Task("Run-E2E-Tests")
    .IsDependentOn("Build")
    .Does(async (ctx) =>
{
    Information("Recreating docker containers...");
    DockerComposeUp(new DockerComposeUpSettings { ForceRecreate = true, DetachedMode = true });

    bool gitbucketStarted = false;
    int count = 0;
    var runner = new GenericDockerComposeRunner<DockerComposeLogsSettings>(ctx.FileSystem, ctx.Environment, ctx.ProcessRunner, ctx.Tools);
    IEnumerable<string> dockerComposeLogs() => runner.RunWithResult("logs", new DockerComposeLogsSettings(), r => r.ToArray(), Array.Empty<string>());

    do
    {
        await System.Threading.Tasks.Task.Delay(1500);
        Information($"Waiting for GitBucket to have started...{count + 1}");
        var logs = dockerComposeLogs();
        gitbucketStarted = logs.Any(log => log.IndexOf("oejs.Server:main: Started") > 0);

        count++;
        if (30 < count)
        {
            throw new Exception("Exceeded the maximum number of attempts.");
        }
    }
    while(!gitbucketStarted);

    DotNetCoreTest(
        "./src/GbUtil.E2ETests/GbUtil.E2ETests.csproj",
        new DotNetCoreTestSettings 
        {
            Configuration = configuration 
        });

    DockerComposeRm(new DockerComposeRmSettings { Force = true, Volumes = true, Stop = true }, Array.Empty<string>());
});

Task("Pack")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCorePack(
        "./src/GbUtil/GbUtil.csproj",
        new DotNetCorePackSettings 
        {
            OutputDirectory = "./packages",
            Configuration = configuration 
        });
});

Task("Publish")
    .IsDependentOn("Clean")
    .Does(() =>
{
    var apiKey = EnvironmentVariable("NUGET_API_KEY");
    if (string.IsNullOrEmpty(apiKey)) 
    {
        throw new InvalidOperationException("Could not resolve NuGet API key.");
    }

    DotNetCoreNuGetPush(
        "./packages/GbUtil.0.7.0.nupkg",
        new DotNetCoreNuGetPushSettings 
        {
            ApiKey = apiKey,
            Source = "https://api.nuget.org/v3/index.json",
        });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build");

Task("Run-All-Tests")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Run-E2E-Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);