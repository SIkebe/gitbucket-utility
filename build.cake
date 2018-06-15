#addin nuget:?package=Cake.Incubator&version=2.0.2
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
    .DoesForEach(GetFiles("./src/**/*.csproj"), (project) =>
{
    DotNetCoreBuild(
        project.FullPath,
        new DotNetCoreBuildSettings 
        {
            Configuration = configuration 
        }); 
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
        "./packages/GbUtil.0.2.0.nupkg",
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

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);