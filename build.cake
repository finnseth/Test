///////////////////////////////////////////////////////////////////////////
// This is the main build file for this project.
//
// The following targets are the prefered entrypoints:
// * Build
// * Publish
// * Deploy
//
// You can call these targets by using the bootstrapper powershell script
// next to this file: ./build -target <target>
///////////////////////////////////////////////////////////////////////////


///////////////////////////////////////////////////////////////////////////
// Load additional cake addins
///////////////////////////////////////////////////////////////////////////

#addin Cake.Json
#addin Cake.Endpoint
#addin Cake.Npm
#addin Cake.DoInDirectory


///////////////////////////////////////////////////////////////////////////
// Load additional tools
///////////////////////////////////////////////////////////////////////////

#tool "nuget:?package=Cake.CoreCLR"
#tool "nuget:?package=GitVersion.CommandLine&version=4.0.0-beta0012"
#tool "nuget:?package=OctopusTools"


//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument<string>("target", "Default");
var configuration = Argument<string>("configuration", "Release");
var framework = Argument<string>("framework", "net461");
var testFilter = Argument<string>("testfilter", "");
var noRestore = Argument<string>("no-restore", null);
var clientOnly = Argument<string>("client-only", null);
var serverOnly = Argument<string>("server-only", null);


//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var service = "Dualog.PortalService";
FilePath serviceProject = $"./Src/WebService/{service}/{service}/{service}.csproj";
FilePath serviceUnitTestProject = $"./test/WebService/{service}.UnitTests/{service}.UnitTests.csproj";
DirectoryPath webClientProject = "./Src/WebClient/";
var author = "Dualog AS";
var copyright = $"{author} Ⓒ {DateTime.Now.ToString("yyyy")}";
var dualogOctopusDeployServer = "http://192.168.1.150:88";
var dualogCakeOctopusDeploymentApiKey = "API-E8JX7N6QMUIYB2KOYHNVPYH1FM"; 
DirectoryPath artifacts = "./artifacts";
var endpoints = DeserializeJsonFromFile<IEnumerable<Endpoint>>( "./endpoints.json" );

// Get the product version using GitVersion
GitVersion gitVersion = GitVersion();
var version = gitVersion.NuGetVersion;
var buildServer = BuildSystem.IsRunningOnTeamCity ? "TeamCity" : "local";

Warning($"Building {service} version {version} on {buildServer}");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("SetTeamCityVersion")
    .Does(() => {
        if (BuildSystem.IsRunningOnTeamCity)
            BuildSystem.TeamCity.SetBuildNumber(version);
    });

Task("Clean")
    .IsDependentOn("SetTeamCityVersion")
    .Does(() =>
    {
        // clean artifacts folder
        CleanDirectory(artifacts);

        // clean webapp output folder
    	CleanDirectory(webClientProject.Combine("dist"));
    	CleanDirectory(webClientProject.Combine("node_modules"));

        // clean webapi output folders
        CleanDirectories("./src/**/obj");
        CleanDirectories("./test/**/obj");
        CleanDirectories($"./src/**/{configuration}");
        CleanDirectories($"./test/**/{configuration}");
    });

Task("Restore")
	.Does(() =>
    {
        if (noRestore != null) return; // -no-restore flag specified?

        if (clientOnly == null) 
        {
	        DotNetCoreRestore(serviceProject.FullPath);
        }

        if (serverOnly == null)
            DoInDirectory(webClientProject, () => { NpmInstall(); });
    });

Task("Build")
	.IsDependentOn("SetTeamCityVersion")
	.IsDependentOn("Restore")
    .Does(() =>
	{
        if (clientOnly == null) 
        {
            DotNetCorePublish(serviceProject.FullPath, new DotNetCorePublishSettings
            {
                Configuration = configuration,
                ArgumentCustomization = args => args.Append($"/p:Version={version} /p:Copyright=\"{copyright}\" /p:Authors=\"{author}\"").
                    Append("--verbosity minimal --no-restore")
            });
        }

        if (serverOnly == null)
        	DoInDirectory( webClientProject, () => { NpmRunScript("ng", new List<string>{"build"}); });
	});

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        if (clientOnly == null) 
        {
            DotNetCoreTest(serviceUnitTestProject.FullPath, new DotNetCoreTestSettings
            {
                Configuration = configuration,
                ArgumentCustomization = args => {
                    if (!string.IsNullOrEmpty(testFilter)) {
                        args = args.Append("--where").AppendQuoted(testFilter);
                    }
                    return args.Append("--logger:trx").Append($"--verbosity minimal");
                }
            });
        }
	});

Task("Publish")
	.IsDependentOn("Test")
    .Does(() =>
    {
        // orchestrate package contents for octopus packages
        EndpointCreate(endpoints, new EndpointCreatorSettings()
        {
            TargetRootPath = artifacts.Combine("publish").FullPath,
            TargetPathPostFix = "." + version,
            BuildConfiguration = configuration,
            ZipTargetPath = false
        });

        // iterate endpoints, postfix resulting packages and run octo pack
        foreach (var endpoint in endpoints)
        {
            var publishPath = artifacts.Combine("publish").Combine(endpoint.Id + "." + version);
            var packageId = endpoint.Id + ".Deploy";

            if (packageId.Contains("Client") && serverOnly != null) continue;
            if (packageId.Contains("Service") && clientOnly != null) continue;

            // Create Octopus Package (NuGet)
            OctoPack(packageId, new OctopusPackSettings
            {
                Author = author,
                BasePath = publishPath,
                Description = endpoint.Id,
                Title = packageId,
                OutFolder = artifacts.Combine("nuget"),
                Overwrite = true,
                Version = version
            });

            // Upload package to Octopus server
            var octopusNuGetPath = artifacts.Combine("nuget/" + packageId + "." + version + ".nupkg");
            OctoPush(dualogOctopusDeployServer, dualogCakeOctopusDeploymentApiKey, octopusNuGetPath.FullPath, new OctopusPushSettings 
            {
                ReplaceExisting = true
            });   

            // Create Octopus release from this package
            OctoCreateRelease(endpoint.Id, new CreateReleaseSettings 
            {
                Server = dualogOctopusDeployServer,
                ApiKey = dualogCakeOctopusDeploymentApiKey,
                ReleaseNumber = version,
                IgnoreExisting = true
            });  
        }
    });

Task("Deploy")
	.IsDependentOn("Publish")
    .Does(() =>
    {
        if (BuildSystem.IsLocalBuild) 
            Warning("Doing DEPLOY from local build");

        // orchestrate package contents for octopus packages
        EndpointCreate(endpoints, new EndpointCreatorSettings()
        {
            TargetRootPath = artifacts.Combine("publish").FullPath,
            TargetPathPostFix = "." + version,
            BuildConfiguration = configuration,
            ZipTargetPath = false
        });

        // iterate endpoints, postfix resulting packages and run octo pack
        foreach (var endpoint in endpoints)
        {
            var publishPath = artifacts.Combine("publish").Combine(endpoint.Id + "." + version);
            var packageId = endpoint.Id + ".Deploy";

            if (packageId.Contains("Client") && serverOnly != null) continue;
            if (packageId.Contains("Service") && clientOnly != null) continue;

            // Make Octopus perform the actual deployment for this project
            OctoDeployRelease(dualogOctopusDeployServer, dualogCakeOctopusDeploymentApiKey, endpoint.Id, "Development",
                version, new OctopusDeployReleaseDeploymentSettings 
            {
                ShowProgress = true,
                WaitForDeployment = true,
                DeploymentTimeout = TimeSpan.FromMinutes(1),
                CancelOnTimeout = true,
                Force = true
            });
        }
    });

Task("Default")
    .IsDependentOn("Build");

RunTarget(target);
