#addin nuget:?package=Cake.Coverlet&version=2.5.4
var target = Argument("target", "Test");
var configuration = Argument("configuration", "Debug");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Build")
    .Does(() =>
    {
        DotNetCoreBuild("GroceryStore.sln");
    });
Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var testSettings = new DotNetCoreTestSettings
        {
        };

        var coverletSettings = new CoverletSettings
        {
            CollectCoverage = true,
            CoverletOutputFormat = CoverletOutputFormat.cobertura,
            CoverletOutputDirectory = Directory(@".\report"),
            CoverletOutputName = $"coverage"
        };

        DotNetCoreTest("GroceryStore.sln", testSettings, coverletSettings);
    });

/////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);