# VisualStudioSolutionGenerator
Utility program to generate Visual Studio Solution (.sln) Files

This tool is similar to other tools that I have open sourced, most specifically [VisualStudioSolutionUpdater](https://github.com/aolszowka/VisualStudioSolutionUpdater). However this is the "grandfather" of these tools and is the most "raw" or flexible of them.

In modern times this tooling should be really replaced by `dotnet new sln` and `dotnet sln` however they are not as mature nor as fast as this legacy tool, it is **STRONGLY RECOMMENDED** that you investigate those tools BEFORE adopting this tool internally to see if the performance is "good enough".

In addition something like Solution Filters ([Filtered solutions in Visual Studio](https://docs.microsoft.com/en-us/visualstudio/ide/filtered-solutions?view=vs-2019) and [Solution filters in MSBuild](https://docs.microsoft.com/en-us/visualstudio/msbuild/solution-filters?view=vs-2019)) may be a more appropriate solution for your use case, again investigate that solution **BEFORE** attempting to implement this one.

## When To Use This Tool
This tool is used to generate solution files from a variety of sources, depending upon the arguments sent to the tooling.

## Usage

## Hacking
### Supported Project Types
The most likely change you will want to make is changing the supported project files. In theory this tool should support any MSBuild Project Format that utilizes a ProjectGuid.

Start by looking at `SolutionUtilities.SUPPORTED_PROJECT_TYPES` and `SolutionUtiliites.GetProjectTypeGuid(string)` and follow the rabbit trail from there.

## Contributing
Pull requests and bug reports are welcomed so long as they are MIT Licensed.

## License
This tool is MIT Licensed.

## Third Party Licenses
This project uses other open source contributions see [LICENSES.md](LICENSES.md) for a comprehensive listing.
