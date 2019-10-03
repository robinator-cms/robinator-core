var target = Argument("target", "Default");
var isLocalBuild = BuildSystem.IsLocalBuild;
var isAzureBuild = BuildSystem.IsRunningOnAzurePipelines;
var branch = BuildSystem.TFBuild.Environment.Repository.Branch;
var isMasterBranch = StringComparer.OrdinalIgnoreCase.Equals("master", branch);
var isDevBranch = StringComparer.OrdinalIgnoreCase.Equals("dev", branch);

Task("Clean")
  .Does(() =>
  {
  });

Task("Restore")
  .Does(() => 
  {
  });

Task("Build")    
  .Does(() =>
  {
  });

Task("Deploy")
  .WithCriteria(!isLocalBuild)
  .WithCriteria(isMasterBranch || isDevBranch)
  .Does(() => 
  {
    var postfix = "";
    if (isDevBranch) {
      postfix = "-dev";
    }
  });    

Task("Default")
  .IsDependentOn("Clean")
  .IsDependentOn("Restore")
  .IsDependentOn("Build")
  .IsDependentOn("Deploy")    
  .Does(() => 
  {
    Information(EnvironmentVariable("nuget_api_key"));
  });

RunTarget(target);