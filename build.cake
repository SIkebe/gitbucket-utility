#addin nuget:?package=Cake.Docker&version=1.1.2

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var rid = Argument("rid", "win-x64");
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    var buildDirectories = GetDirectories("./src/**/bin/" + configuration) + GetDirectories("./src/**/obj/" + configuration);
    foreach (var dir in buildDirectories)
    {
        DeleteDirectoryWithReadonlyFiles(dir.FullPath);
    }
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetBuild(
        "./GbUtil.sln",
        new DotNetBuildSettings
        {
            Configuration = configuration
        });
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetTest(
        "./src/GitBucket.Service.Tests/GitBucket.Service.Tests.csproj",
        new DotNetTestSettings
        {
            Configuration = configuration
        });
});

Task("Run-E2E-Tests")
    .IsDependentOn("Build")
    .Does(async (ctx) =>
{
    await RunE2ETests(ctx);
});

Task("Run-E2E-Tests-Using-SingleFileExe")
    .IsDependentOn("Publish-SingleFile")
    .IsDependentOn("Build")
    .Does(async (ctx) =>
{
    var path = MakeAbsolute(File("./executable/GbUtil"));
    Environment.SetEnvironmentVariable("GbUtil_UseSingleFileExe", "true");
    Environment.SetEnvironmentVariable("GbUtil_SingleFileExePath", path.FullPath);
    await RunE2ETests(ctx);
});

async Task RunE2ETests(ICakeContext ctx)
{
    System.IO.Directory.CreateDirectory("docker");
    Information("Recreating docker containers...");
    DockerComposeRm(new DockerComposeRmSettings { Force = true, Stop = true, Volumes = true });
    DeleteDirectoryWithReadonlyFiles("docker");
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
        if (100 < count)
        {
            throw new Exception("Exceeded the maximum number of attempts.");
        }
    }
    while(!gitbucketStarted);

    try
    {
        DotNetTest("./src/GbUtil.E2ETests/GbUtil.E2ETests.csproj", new DotNetTestSettings { Configuration = configuration });
    }
    finally
    {
        DockerComposeRm(new DockerComposeRmSettings { Force = true, Volumes = true, Stop = true }, Array.Empty<string>());
    }
}

static void DeleteDirectoryWithReadonlyFiles(string path)
{
    var directoryInfo = new DirectoryInfo(path);
    RemoveReadonlyAttribute(directoryInfo);
    directoryInfo.Delete(recursive: true);
}

static void RemoveReadonlyAttribute(DirectoryInfo directoryInfo)
{
    if (directoryInfo is null)
    {
        throw new ArgumentNullException(nameof(directoryInfo));
    }
    if ((directoryInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
    {
        directoryInfo.Attributes = FileAttributes.Normal;
    }
    foreach (var fi in directoryInfo.GetFiles())
    {
        if ((fi.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
        {
            fi.Attributes = FileAttributes.Normal;
        }
    }
    foreach (var di in directoryInfo.GetDirectories())
    {
        RemoveReadonlyAttribute(di);
    }
}

Task("Pack")
    .IsDependentOn("Clean")
    .Does(() =>
{
    CleanDirectory("packages");
    DotNetPack(
        "./src/GbUtil/GbUtil.csproj",
        new DotNetPackSettings
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

    DotNetNuGetPush(
        "./packages/GbUtil.0.13.0.nupkg",
        new DotNetNuGetPushSettings
        {
            ApiKey = apiKey,
            Source = "https://api.nuget.org/v3/index.json",
        });
});

Task("Publish-SingleFile")
    .Does(() =>
{
    CleanDirectory("executable");

    DotNetPublish(
        "./src/GbUtil/GbUtil.csproj",
        new DotNetPublishSettings
        {
            Configuration = configuration,
            OutputDirectory = "executable",
            Runtime = rid,
            PublishSingleFile = true,
            PublishTrimmed = false,
            SelfContained = true,
        });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build");

Task("Run-All-Tests")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Run-E2E-Tests")
    .IsDependentOn("Run-E2E-Tests-Using-SingleFileExe");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
