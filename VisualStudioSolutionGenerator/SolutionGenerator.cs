namespace VisualStudioSolutionGenerator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    internal static class SolutionGenerator
    {
        public static void FromProjectListing(string solutionFile, string projectListingFile)
        {
            // Get all the Projects to Add
            IEnumerable<string> projectsToAdd = File.ReadLines(projectListingFile);
            IEnumerable<string> projectsAndNOrderDependencies = MSBuildUtilities.ResolveProjectReferenceDependenciesFlat(projectsToAdd);

            GenerateAndSaveSolution(solutionFile, projectsAndNOrderDependencies);
        }

        public static void FromRelativePathListing(string solutionFile, string relativeProjectListingFile)
        {
            // Resolve the relative paths of the projects
            string solutionRoot = Path.GetDirectoryName(solutionFile);
            IEnumerable<string> pathsToProjects =
                File
                    .ReadLines(relativeProjectListingFile)
                    .Select(relativePath =>
                    {
                        string combinedRelativePath = Path.Combine(solutionRoot, relativePath);
                        string actualRelativePath = Path.GetFullPath(combinedRelativePath);
                        return actualRelativePath;
                    });

            // Now Resolve all N-Order Dependencies
            IEnumerable<string> projFilesAndNOrderDependencies = MSBuildUtilities.ResolveProjectReferenceDependenciesFlat(pathsToProjects);

            GenerateAndSaveSolution(solutionFile, projFilesAndNOrderDependencies);
        }

        public static void ForFolder(string directoryToScan, string targetSolution, string excludeFile)
        {
            IEnumerable<string> excludedProjects = new string[0];

            if (!string.IsNullOrEmpty(excludeFile) && File.Exists(excludeFile))
            {
                excludedProjects =
                    File
                    .ReadLines(excludeFile)
                    .Select(current => Path.Combine(directoryToScan, current));
            }

            IEnumerable<string> csprojFilesInDirectory = Directory.EnumerateFiles(directoryToScan, "*.csproj", SearchOption.AllDirectories).Except(excludedProjects);
            IEnumerable<string> projFilesAndNOrderDependencies = MSBuildUtilities.ResolveProjectReferenceDependenciesFlat(csprojFilesInDirectory);

            GenerateAndSaveSolution(targetSolution, projFilesAndNOrderDependencies);
        }

        /// <summary>
        /// Used to generate multiple SLN's for multiple projects. The name of the SLN is the same as the target project in the same path.
        /// </summary>
        /// <param name="targetProjectFile">A text file that contains the projects to generate the SLN For</param>
        public static void ForMultipleSingleProjects(string targetProjectFile)
        {
            IEnumerable<string> targetProjects = File.ReadLines(targetProjectFile);
            foreach (string targetProject in targetProjects)
            {
                ForSingleProject(targetProject);
            }
        }

        /// <summary>
        /// Generate an SLN for a single project. The name of the SLN is the same as the target project in the same path.
        /// </summary>
        /// <param name="targetProject"></param>
        public static void ForSingleProject(string targetProject)
        {
            string targetSolution = Path.ChangeExtension(targetProject, ".sln");
            ForSingleProject(targetSolution, targetProject);
        }

        public static void ForSingleProject(string targetSolution, string targetProject)
        {
            IEnumerable<string> flatProjectReferences = MSBuildUtilities.ResolveProjectReferenceDependenciesFlat(new string[] { targetProject });

            GenerateAndSaveSolution(targetSolution, flatProjectReferences);
        }

        /// <summary>
        /// Generate an SLN based off a listing of Solution Files. All N-Order Dependencies are re-resolved.
        /// </summary>
        /// <param name="solutionFile">The destination solution file name.</param>
        /// <param name="targetSolutions">The solutions to join into a single solution</param>
        public static void ForSolutions(string solutionFile, IEnumerable<string> targetSolutions)
        {
            // First get all distinct projects referenced from within the solutions
            IEnumerable<string> allDistinctProjectsInSolutions =
                targetSolutions
                .Distinct(StringComparer.InvariantCultureIgnoreCase)
                .SelectMany(currentSolution => SolutionUtilities.GetProjectsFromSolution(currentSolution))
                .Distinct(StringComparer.InvariantCultureIgnoreCase);

            // At this point find any missing N-Order Dependencies
            IEnumerable<string> allProjectsIncludingNOrder = MSBuildUtilities.ResolveProjectReferenceDependenciesFlat(allDistinctProjectsInSolutions);

            GenerateAndSaveSolution(solutionFile, allProjectsIncludingNOrder);
        }

        public static void FromASolution(string targetSolution)
        {
            IEnumerable<string> existingProjectsInSolution = SolutionUtilities.GetProjectsFromSolution(targetSolution);
            IEnumerable<string> projectsAndNOrderDependencies = MSBuildUtilities.ResolveProjectReferenceDependenciesFlat(existingProjectsInSolution);

            GenerateAndSaveSolution(targetSolution, projectsAndNOrderDependencies);
        }

        internal static void GenerateAndSaveSolution(string solutionFile, IEnumerable<string> projects)
        {
            // Find our solution root
            string solutionRoot = Path.GetDirectoryName(solutionFile) + Path.DirectorySeparatorChar;

            string solutionFileContent = SolutionUtilities.GenerateSolutionForProjects(solutionRoot, projects);

            File.WriteAllText(solutionFile, solutionFileContent, Encoding.UTF8);
        }
    }
}
