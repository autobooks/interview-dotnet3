var target = Argument("target", "Test");
var configuration = Argument("configuration", "Debug");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("csharpier_check")
    .Does(() =>
    {
        DotNetCoreTool("csharpier --check");
    });
Task("Build")
    .IsDependentOn("csharpier_check")
    .Does(() =>
    {
        DotNetCoreBuild("GroceryStore.sln");
    });
Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        DotNetCoreTool("test");
    });

/////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);