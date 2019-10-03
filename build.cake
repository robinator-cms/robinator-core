#addin Cake.Git&version=0.21.0
#addin nuget:?package=Nuget.Core&version=2.14.0
using NuGet;
using System.Xml;

var target = Argument("target", "Default");
var isLocalBuild = BuildSystem.IsLocalBuild;
var branch = BuildSystem.TFBuild.Environment.Repository.Branch ?? Argument<string>("currentBranch", GitBranchCurrent("./").FriendlyName);
var isMasterBranch = StringComparer.OrdinalIgnoreCase.Equals("master", branch);

var artifactsDir = "./artifacts/";
var solutionPath = "./src/Robinator.sln";
var project = "./src/Robinator.App/Robinator.App.csproj";
var configuration = "Release";
var nugetApiKey = EnvironmentVariable<string>("nuget_api_key", null);
var nugetSource = "https://api.nuget.org/v3/index.json";

var version = GetVersion();

if (!isMasterBranch) {
  var gitSha1 = GitLogTip("./").Sha.Substring(0,12);
  Information("This is a dev build." + gitSha1 + " will be added to the version");
  version += "-dev-" + gitSha1;
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
    Information("Version = " + version);
    var settings = new NuGetPackSettings
    {
      OutputDirectory = artifactsDir,
		  IncludeReferencedProjects = true,
      Properties = new Dictionary<string, string>
      {
        { "Configuration", "Release" }
      },
      Version = version
    };
    
    var pkgs = GetFiles("./src/**/*.csproj");
    foreach(var pkg in pkgs)
    {
      var filename = pkg.GetFilenameWithoutExtension().ToString();
      if (!filename.EndsWith("Test") && !filename.EndsWith("Example"))
        NuGetPack(pkg.ToString(), settings);
    }
  });

Task("Publish")
  .IsDependentOn("Package")
  .WithCriteria(isMasterBranch)
  .Does(() => {
    var pushSettings = new DotNetCoreNuGetPushSettings 
    {
      Source = nugetSource,
      ApiKey = nugetApiKey
    };

    var pkgs = GetFiles(artifactsDir + "*.nupkg");
    foreach(var pkg in pkgs) 
    {
      if(!IsNuGetPublished(pkg)) 
      {
        Information($"Publishing \"{pkg}\".");
        DotNetCoreNuGetPush(pkg.FullPath, pushSettings);
      }
      else {
        Information($"Bypassing publishing \"{pkg}\" as it is already published.");
      }
    }
  });
  
private bool IsNuGetPublished(FilePath packagePath) {
  var package = new ZipPackage(packagePath.FullPath);

  var latestPublishedVersions = NuGetList(
    package.Id,
    new NuGetListSettings 
    {
      Prerelease = true
    }
  );

  return latestPublishedVersions.Any(p => package.Version.Equals(new SemanticVersion(p.Version)));
}

private string GetVersion() {
  XmlDocument doc = new XmlDocument();
  doc.LoadXml(System.IO.File.ReadAllText("./src/.targets"));
  XmlNode versionNode = doc.DocumentElement.SelectSingleNode("/Project/PropertyGroup/Version");
  return versionNode.InnerText;
}

Task("Default")
  .IsDependentOn("Build")
  .IsDependentOn("Publish")
  .Does(() => 
  {
  });

RunTarget(target);