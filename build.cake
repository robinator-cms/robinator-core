#addin Cake.Git&version=0.21.0
#addin nuget:?package=Nuget.Core&version=2.14.0
using NuGet;
using System.Xml;

var target = Argument("target", "Default");
var isLocalBuild = BuildSystem.IsLocalBuild;
var branch = BuildSystem.TFBuild.Environment.Repository.Branch;
if (string.IsNullOrWhiteSpace(branch)) {
  branch = Argument<string>("currentBranch", GitBranchCurrent("./").FriendlyName);
}
var isMasterBranch = StringComparer.OrdinalIgnoreCase.Equals("master", branch);
var isDevBranch = StringComparer.OrdinalIgnoreCase.Equals("dev", branch);

var artifactsDir = "./artifacts/";
var solutionPath = "./src/Robinator.sln";
var project = "./src/Robinator.App/Robinator.App.csproj";
var configuration = "Release";
var nugetApiKey = EnvironmentVariable<string>("NUGET_API_KEY", null);
var nugetSource = "https://www.nuget.org/api/v2/package";

var versionSuffix = "";

if (!isMasterBranch) {
  var gitSha1 = GitLogTip("./").Sha.Substring(0,12);
  versionSuffix = "dev+" + gitSha1;
  Information("This is a dev build. " + versionSuffix + " will be added to the version");
}

Task("Clean")
  .Does(() => {
    if (DirectoryExists(artifactsDir))
    {
      DeleteDirectory(
        artifactsDir,
        new DeleteDirectorySettings {
          Recursive = true,
          Force = true
        }
      );
    }
    CreateDirectory(artifactsDir);
    DotNetCoreClean(solutionPath);
  });

Task("Restore")
  .Does(() => 
  {
    DotNetCoreRestore(solutionPath);
  });

Task("Build")
  .IsDependentOn("Clean")
  .IsDependentOn("Restore")  
  .Does(() =>
  {
    DotNetCoreBuild(
      solutionPath,
      new DotNetCoreBuildSettings 
      {
        Configuration = configuration
      }
    );
  });

Task("Package")
  .Does(() => {
    Information("Version = " + GetVersion() + versionSuffix);
    var settings = new DotNetCorePackSettings
    {
      OutputDirectory = artifactsDir,
      Configuration = "Release",
      VersionSuffix = versionSuffix,
      NoBuild = true
    };
    
    var pkgs = GetFiles("./src/**/*.csproj");
    foreach(var pkg in pkgs)
    {
      var filename = pkg.GetFilenameWithoutExtension().ToString();
      if (!filename.EndsWith("Test") && !filename.EndsWith("Example"))
        DotNetCorePack(pkg.ToString(), settings);
    }
  });

Task("Publish")
  .WithCriteria(isMasterBranch || isDevBranch)
  .Does(() => {
    if (string.IsNullOrWhiteSpace(nugetApiKey))
    {
      throw new ArgumentException("NUGET_API_KEY environment variable is not defined.");
    }
    var pushSettings = new DotNetCoreNuGetPushSettings 
    {
      Source = nugetSource,
      ApiKey = nugetApiKey
    };

    var pkgs = GetFiles(artifactsDir + "*.nupkg");
    foreach(var pkg in pkgs) 
    {
      Information($"Publishing \"{pkg}\".");
      DotNetCoreNuGetPush(pkg.FullPath, pushSettings);
    }
  });

private string GetVersion() {
  XmlDocument doc = new XmlDocument();
  doc.LoadXml(System.IO.File.ReadAllText("./src/.targets"));
  XmlNode versionNode = doc.DocumentElement.SelectSingleNode("/Project/PropertyGroup/VersionPrefix");
  return versionNode.InnerText;
}

Task("Default")
  .IsDependentOn("Build")
  .IsDependentOn("Package")
  .IsDependentOn("Publish")
  .Does(() => 
  {
  });

RunTarget(target);
