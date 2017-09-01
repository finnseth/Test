///////////////////////////////////////////////////////////////////////////
// This is the main build file for this project.
//
// The following targets are the prefered entrypoints:
// * Build
// * Publish
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
#tool "nuget:?package=GitVersion.CommandLine&version=4.0.0-beta0011"
#tool "nuget:?package=OctopusTools"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
var target = Argument<string>("target", "Default");
var configuration = Argument<string>("configuration", "Release");
var framework = Argument<string>("framework", "net461");
var testFilter = Argument<string>("testfilter", "");
var noRestore = Argument<string>("no-restore", null);

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////
var service = "Dualog.PortalService";
FilePath serviceProject = $"./Src/WebService/{service}/{service}/{service}.csproj";
FilePath serviceUnitTestProject = $"./test/WebService/{service}.UnitTests/{service}.UnitTests.csproj";
DirectoryPath webClientProject = "./Src/WebClient/";
var copyright = $"\"Dualog Ⓒ {DateTime.Now.ToString("yyyy")}\"";
var dualogOctopusDeployServer = "http://192.168.1.150:88";
var dualogCakeOctopusDeploymentApiKey = "API-E8JX7N6QMUIYB2KOYHNVPYH1FM"; 

///////////////////////////////////////////////////////////////////////////
// Constants, initial variables
///////////////////////////////////////////////////////////////////////////
DirectoryPath artifacts = "./artifacts";
GitVersion version = GitVersion();
var endpoints = DeserializeJsonFromFile<IEnumerable<Endpoint>>( "./endpoints.json" );

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("SetTeamCityVersion")
    .Does(() => {
        if (BuildSystem.IsRunningOnTeamCity)
            BuildSystem.TeamCity.SetBuildNumber(version.NuGetVersion);
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
        if (noRestore != null) return;

        // run dotnet restore for webapi projects
        var settings = new DotNetCoreRestoreSettings
        {
             ArgumentCustomization = args => args.Append("--verbosity minimal")
        };

	    DotNetCoreRestore(serviceProject.FullPath);
	    DotNetCoreRestore(serviceUnitTestProject.FullPath);

        // run npm install for webapp project
        DoInDirectory(webClientProject, () => { NpmInstall(); });
    });

Task("Build")
	.IsDependentOn("Restore")
    .Does(() =>
	{
        DotNetCorePublish(serviceProject.FullPath, new DotNetCorePublishSettings
    	{
	    	Configuration = configuration,
		    ArgumentCustomization = args => args.Append($"/p:Version={version.NuGetVersion} /p:Copyright={copyright}").Append($"--verbosity minimal").Append("--no-restore")
	    });

        // run ng build to build into dist
    	DoInDirectory( webClientProject, () => { NpmRunScript("ng", new List<string>{"build"}); });
	});

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
		DotNetCoreTest(serviceUnitTestProject.FullPath, new DotNetCoreTestSettings
		{
			Configuration = configuration,
			ArgumentCustomization = args => {
				if (!string.IsNullOrEmpty(testFilter)) {
					args = args.Append("--where").AppendQuoted(testFilter);
				}
				return args.Append("--logger:trx").Append($"--verbosity minimal").Append("--no-restore");
			}
		});
	});

Task("Pack")
	.IsDependentOn("Test")
    .Does(() =>
    {
        // orchestrate package contents for octopus packages
        EndpointCreate(endpoints, new EndpointCreatorSettings()
        {
            TargetRootPath = artifacts.Combine("publish").FullPath,
            TargetPathPostFix = "." + version.NuGetVersion,
            BuildConfiguration = configuration,
            ZipTargetPath = false
        });

        // iterate endpoints, postfix resulting packages and run octo pack
        foreach (var endpoint in endpoints)
        {
            var publishPath = artifacts.Combine("publish").Combine(endpoint.Id + "." + version.NuGetVersion);
            var packageId = endpoint.Id + ".Deploy";

            // Create Octopus Package (NuGet)
            OctoPack(packageId, new OctopusPackSettings
            {
                Author = "Dualog AS",
                BasePath = publishPath,
                Description = endpoint.Id,
                Title = packageId,
                OutFolder = artifacts.Combine("nuget"),
                Overwrite = true,
                Version = version.NuGetVersion
            });
        }
    });

Task("Publish")
	.IsDependentOn("Pack")
    .Does(() =>
    {
        // orchestrate package contents for octopus packages
        EndpointCreate(endpoints, new EndpointCreatorSettings()
        {
            TargetRootPath = artifacts.Combine("publish").FullPath,
            TargetPathPostFix = "." + version.NuGetVersion,
            BuildConfiguration = configuration,
            ZipTargetPath = false
        });

        // iterate endpoints, postfix resulting packages and run octo pack
        foreach (var endpoint in endpoints)
        {
            var publishPath = artifacts.Combine("publish").Combine(endpoint.Id + "." + version.NuGetVersion);
            var packageId = endpoint.Id + ".Deploy";

            // Upload package to Octopus server
            var octopusNuGetPath = artifacts.Combine("nuget/" + packageId + "." + version.NuGetVersion + ".nupkg");
            OctoPush(dualogOctopusDeployServer, dualogCakeOctopusDeploymentApiKey, octopusNuGetPath.FullPath, new OctopusPushSettings 
            {
                ReplaceExisting = true
            });     
            // Create Octopus release from this package
            OctoCreateRelease(endpoint.Id, new CreateReleaseSettings 
            {
                Server = dualogOctopusDeployServer,
                ApiKey = dualogCakeOctopusDeploymentApiKey,
                ReleaseNumber = version.NuGetVersion,
                IgnoreExisting = true
            });  

            // Make Octopus perform the actual deployment for this project
            OctoDeployRelease(dualogOctopusDeployServer, dualogCakeOctopusDeploymentApiKey, endpoint.Id, "Development",
                version.NuGetVersion, new OctopusDeployReleaseDeploymentSettings 
            {
                ShowProgress = true,
                WaitForDeployment = true,
                DeploymentTimeout = TimeSpan.FromMinutes(1),
                CancelOnTimeout = true,
                Force = true
            });
        }
    });

Task("Run")
	.IsDependentOn("Restore")
	.Does( () =>
    {
        // start dotnet run task
        var webApi = System.Threading.Tasks.Task.Factory.StartNew( () =>
        {
            DotNetCoreRun( serviceProject.GetFilename().ToString(), null, new DotNetCoreRunSettings
            {
                WorkingDirectory = serviceProject.GetDirectory().FullPath
            });
        });

        // start npm run task
        var webApp = System.Threading.Tasks.Task.Factory.StartNew( () =>
        {
            NpmRunScript( new NpmRunScriptSettings
            {
                ScriptName = "start",
                WorkingDirectory = webClientProject.FullPath
            });
        });
        System.Threading.Tasks.Task.WaitAll(webApi, webApp);
    });

Task("Default")
    .IsDependentOn("Build");

RunTarget(target);
