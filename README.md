# VisualStudioSolutionGenerator
![CI - Main](https://github.com/aolszowka/VisualStudioSolutionGenerator/workflows/CI/badge.svg?branch=main)

Utility program to generate Visual Studio Solution (.sln) Files from various sources.

This tool is similar to other tools that I have open sourced, most specifically [VisualStudioSolutionUpdater](https://github.com/aolszowka/VisualStudioSolutionUpdater). However this is the "grandfather" of these tools and is the most "raw" or flexible of them.

In modern times this tooling should be really replaced by `dotnet new sln` and `dotnet sln` however they are not as mature,  nor as fast as this legacy tool, it is **STRONGLY RECOMMENDED** that you investigate those tools **BEFORE** adopting this tool internally to see if the performance is "good enough".

In addition something like Solution Filters ([Filtered solutions in Visual Studio](https://docs.microsoft.com/en-us/visualstudio/ide/filtered-solutions?view=vs-2019) and [Solution filters in MSBuild](https://docs.microsoft.com/en-us/visualstudio/msbuild/solution-filters?view=vs-2019)) may be a more appropriate solution for your use case. Again investigate that solution **BEFORE** attempting to implement this one.

This tool should be used only in the harshest of environments.

## When To Use This Tool
This tool is used to generate solution files from a variety of sources, depending upon the arguments sent to the tooling.

## Usage
There are now two ways to run this tool:

1. (Compiled Executable) Invoke the tool via `VisualStudioSolutionGenerator` and pass the arguments.
2. (Dotnet Tool) Install this tool using the following command `dotnet tool install VisualStudioSolutionGenerator` (assuming that you have the nuget package in your feed) then invoke it via `dotnet generate-solution`

In both cases the flags to the tooling are identical:

```text
Usage: -fpl -sln Solution.sln -l ProjectListing.txt

The behavior of this tool depends on the arguments passed to it. Most
importantly is the "Mode" which will determine the behavior and required
environment variables.

      --FromProjectListing, --fpl
                             Generate a solution from a project listing file.
                               Requires -sln and -l.
      --FromProjectListingRelative, --fplr
                             Generate a solution from a project listing file
                               that contains paths relative to the solution
                               file. Requires -sln and -l.
      --FromSolutionListing, --fsl
                             Generate a solution from a solution listing fil-
                               e. Requires -sln and -l.
      --ForFolder, --ff      Generate a solution for a given folder path.
                               Requires -sln, -d, and optionally -i.
      --solution, --sln      Path to the Solution File to create
      --listing, -l          Path to a plain text file containing a list of
                               elements, one per line, depending on the mode.
      --directory, -d        For certain modes, a directory to scan for
                               projects.
      --ignorefile, -i       For certain modes, a plain-text file containing
                               paths to ignore.
  -?, -h, --help             Show this message and exit
```

## Hacking
### Supported Project Types
The most likely change you will want to make is changing the supported project files. In theory this tool should support any MSBuild Project Format that utilizes a ProjectGuid.

Start by looking at `SolutionUtilities.SUPPORTED_PROJECT_TYPES` and `SolutionUtiliites.GetProjectTypeGuid(string)` and follow the rabbit trail from there.

## GOTCHAs
### Relative Paths
The Visual Studio Solution file format appears to use Windows style directory separator chars `\`, this can cause problems on other OSes supported by the .NET Core Runtime, it is believed these are all flushed out in the unit tests, but day to day usage of this tool really only occurs on Windows. I welcome any well formed bug reports!

### .NET Core Project Types
Historically this tool (and others that derive from it), assumed that the file extension (such as .csproj) would always map 1:1 with the ProjectTypeGuid. This probably was never true, but historically was "true enough" for our usage. With the introduction of .NET Core and the porting of the project type back into msbuild, the file extension mapping is no longer 1:1. Efforts have been made to guess if the project type is a .NET Core version, but this probably could use more stress testing.

## Contributing
Pull requests and bug reports are welcomed so long as they are MIT Licensed.

## License
This tool is MIT Licensed.

## Third Party Licenses
This project uses other open source contributions see [LICENSES.md](LICENSES.md) for a comprehensive listing.
