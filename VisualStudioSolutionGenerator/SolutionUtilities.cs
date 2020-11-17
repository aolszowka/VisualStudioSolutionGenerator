// -----------------------------------------------------------------------
// <copyright file="SolutionUtilities.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2018-2020. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace VisualStudioSolutionGenerator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using Microsoft.Build.Construction;

    static class SolutionUtilities
    {
        internal static ISet<string> SUPPORTED_PROJECT_TYPES =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".csproj",
                ".fsproj",
                ".sqlproj",
                ".synproj",
            };

        public static string GenerateSolutionForProjects(string targetDirectory, IEnumerable<string> projFilePaths)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine("Microsoft Visual Studio Solution File, Format Version 12.00");
            sb.AppendLine("# Visual Studio 15");
            sb.AppendLine("VisualStudioVersion = 15.0.27703.2035");
            sb.AppendLine("MinimumVisualStudioVersion = 10.0.40219.1");

            // Sort the solutions by their names
            projFilePaths = projFilePaths.OrderBy(projFile => Path.GetFileNameWithoutExtension(projFile));

            foreach (string projFilePath in projFilePaths)
            {
                string fragement = GenerateSolutionFragmentForProject(targetDirectory, projFilePath);
                sb.AppendLine(fragement);
            }

            sb.AppendLine("Global");
            sb.AppendLine("	GlobalSection(SolutionConfigurationPlatforms) = preSolution");
            sb.AppendLine("		Debug|Any CPU = Debug|Any CPU");
            sb.AppendLine("		Release|Any CPU = Release|Any CPU");
            sb.AppendLine("	EndGlobalSection");
            sb.AppendLine("	GlobalSection(ProjectConfigurationPlatforms) = postSolution");

            GenerateConfigurationFragmentForProjects(projFilePaths, sb);

            sb.AppendLine("	EndGlobalSection");
            sb.AppendLine("EndGlobal");

            // The solution format uses CRLF as the line endings
            string solutionFile = Regex.Replace(sb.ToString(), "(\r\n|\r|\n)", "\r\n");

            return solutionFile;
        }

        internal static void GenerateConfigurationFragmentForProjects(IEnumerable<string> projFilePaths, StringBuilder sb)
        {
            foreach (var proj in projFilePaths)
            {
                string projectGuid = MSBuildUtilities.GetProjectGuid(proj);

                // The Project GUID is Uppercased and surrounded in braces
                projectGuid = Guid.Parse(projectGuid).ToString("B").ToUpperInvariant();

                string configuration = DetermineConfigurationForProject(proj);

                sb.AppendLine($"		{projectGuid}.Debug|Any CPU.ActiveCfg = Debug|{configuration}");
                sb.AppendLine($"		{projectGuid}.Debug|Any CPU.Build.0 = Debug|{configuration}");
                sb.AppendLine($"		{projectGuid}.Release|Any CPU.ActiveCfg = Release|{configuration}");
                sb.AppendLine($"		{projectGuid}.Release|Any CPU.Build.0 = Release|{configuration}");
            }
        }

        internal static string DetermineConfigurationForProject(string proj)
        {
            string configuration = "Any CPU";

            return configuration;
        }

        /// <summary>
        /// Generates a "Solution Fragment" that contains the correct syntax
        /// for adding a Project to a Visual Studio Solution.
        /// </summary>
        /// <param name="solutionRoot">The Directory that contains the solution file. This is used to generate the relative path to the CSPROJ File.</param>
        /// <param name="pathToProjFile">The fully qualified path to the Project File.</param>
        /// <returns>A <c>string</c> that represents a properly formatted Project Fragment for a Visual Studio Solution.</returns>
        internal static string GenerateSolutionFragmentForProject(string solutionRoot, string pathToProjFile)
        {
            // On Non-Windows Platforms We need to change this to a Backslash (\)
            string relativePath = Path.GetRelativePath(solutionRoot, pathToProjFile).Replace(Path.DirectorySeparatorChar, '\\');
            string projectTypeGuid = GetProjectTypeGuid(pathToProjFile);
            string projectName = Path.GetFileNameWithoutExtension(pathToProjFile);

            // The Project GUID is upper cased as well as thrown in braces
            string projectGuid = Guid.Parse(MSBuildUtilities.GetProjectGuid(pathToProjFile)).ToString("B").ToUpperInvariant();

            string fragment = $"Project(\"{projectTypeGuid}\") = \"{projectName}\", \"{relativePath}\", \"{projectGuid}\"\r\nEndProject";
            return fragment;
        }

        /// <summary>
        /// Returns the Project Type Guid for this project type.
        /// </summary>
        /// <param name="pathToProjFile">The path to the project file.</param>
        /// <returns>The Guid to be used in the Solution File.</returns>
        internal static string GetProjectTypeGuid(string pathToProjFile)
        {
            string projectExtension = Path.GetExtension(pathToProjFile);

            string result;
            switch (projectExtension.ToLower())
            {
                case ".csproj":
                    {
                        // Due to .NET Core We have to Inspect the File
                        if (MSBuildUtilities.IsDotnetCore(pathToProjFile))
                        {
                            result = "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}";
                        }
                        else
                        {
                            result = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";
                        }
                        break;
                    }
                case ".synproj":
                    {
                        result = "{BBD0F5D1-1CC4-42FD-BA4C-A96779C64378}";
                        break;
                    }
                case ".fsproj":
                    {
                        // Due to .NET Core We have to Inspect the File
                        if (MSBuildUtilities.IsDotnetCore(pathToProjFile))
                        {
                            result = "{6EC3EE1D-3C4E-46DD-8F32-0CC8E7565705}";
                        }
                        else
                        {
                            result = "{F2A71F9B-5D33-465A-A702-920D77279786}";
                        }
                        break;
                    }
                case ".sqlproj":
                    {
                        result = "{00D1A9C2-B5F0-4AF3-8072-F6C62B433612}";
                        break;
                    }
                default:
                    {
                        throw new NotSupportedException($"The extension {projectExtension.ToLower()} was not recognized by this tool.");
                    }
            }

            return result;
        }

        /// <summary>
        ///     Gets an IEnumerable of strings representing the fully qualified
        /// paths to all of the projects referenced by the given solution.
        /// </summary>
        /// <param name="targetSolutionFile">The solution to parse.</param>
        /// <returns>The fully qualified paths to all of the projects in the solution.</returns>
        public static IEnumerable<string> GetProjectsFromSolution(string targetSolutionFile)
        {
            string[] additionalSupportedTypes =
                new string[]
                {
                    ".synproj"
                };

            string solutionFolder = Path.GetDirectoryName(targetSolutionFile);
            SolutionFile solution = SolutionFile.Parse(targetSolutionFile);

            return
                solution
                .ProjectsInOrder
                .Where(project => project.ProjectType == SolutionProjectType.KnownToBeMSBuildFormat || additionalSupportedTypes.Any(supportedType => supportedType.Equals(Path.GetExtension(project.RelativePath), StringComparison.InvariantCultureIgnoreCase)))
                .Select(project => project.RelativePath)
                .Select(projectRelativePath => Path.GetFullPath(Path.Combine(solutionFolder, projectRelativePath)));
        }
    }
}
