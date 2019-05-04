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
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreTest(
        "./GbUtil.sln",
        new DotNetCoreTestSettings 
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
        "./packages/GbUtil.0.5.0.nupkg",
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