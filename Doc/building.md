# Building

## Requirements

In order to build the project the following components must be installed:

- Visual Studio 2017 Build Tools (Update 3 or newer)
- Nodejs (8.4 or newer)
- .Net Core 2 SDK **and** Runtime

## Build script

This project has a [Cake](http::/buildcake.net) based build script for building the project.

Run the PowerShell script `.\build -target {targetname}` in order to build. Supported values for `targetname` is:

* `Build` - restores and builds the complete project. This is the default target.
* `Publish` - builds, tests and publishes the project to the Dualog Octopus Deploy Server.
* `Deploy` - builds, published and deploys the project to the development Environment.
* `Restore` - restores all external dependencies for this project (client side dependencies are restored using `npm`, server side dependencies are restored using `nuget`)
* `Test` - builds and runs all tests for this project
* `Version` - displays the current version for this project. The build script uses the [GitVersion](http://gitversion.readthedocs.io/en/latest/) tool to automatically maintain the correct version for the project.

> Note: you might need to adjust the PowerShell execution policy in order to be able to run the build script. See https://cakebuild.net/docs/tutorials/powershell-security for more information.

## Tuning the build process

Use the following script arguments in order to fine tune the build process further:

* `-client-only` - perform the `targetname` only for the client side 
* `-server-only` - perform the `targetname` only for the server side
* `-configuration` - specify `Debug` in order to create a debug version. Default value is `Release`
* `-testfilter` - specify a filter in order to limit the tests run
* `-no-restore` - do not perform any restore (even if `targetname` is `Restore`)

 
