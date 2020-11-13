namespace VisualStudioSolutionGenerator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    internal static class SolutionGenerator
    {
        /// <summary>
        /// Generates a Solution File at the specified location with the given projects pulled from a listing file
        /// </summary>
        /// <param name="solutionFile">The path to the solution to create</param>
        /// <param name="projectListingFile">The path to the listing file which contains the fully qualified paths</param>
        public static void FromProjectListing(string solutionFile, string projectListingFile)
        {
            // Get all the Projects to Add
            IEnumerable<string> projectsToAdd = File.ReadLines(projectListingFile);
            IEnumerable<string> projectsAndNOrderDependencies = MSBuildUtilities.ResolveProjectReferenceDependenciesFlat(projectsToAdd);

            GenerateAndSaveSolution(solutionFile, projectsAndNOrderDependencies);
        }

        /// <summary>
        /// Generates a Solution File at the specified location with the given projects pulled from a listing file containing project paths RELATIVE to the solution file.
        /// </summary>
        /// <param name="solutionFile">The path to the solution to create</param>
        /// <param name="projectListingFile">The path to the listing file which contains the relative paths</param>
        public static void FromProjectListingRelative(string solutionFile, string relativeProjectListingFile)
        {
            // Resolve the relative paths of the projects
            string solutionRoot = Path.GetDirectoryName(solutionFile);
            IEnumerable<string> pathsToProjects =
                File
                    .ReadLines(relativeProjectListingFile)
                    .Select(relativePath =>
                    {
                        string combinedRelativePath = Path.Combine(solutionRoot, relativePath);
                        string fullPath = Path.GetFullPath(combinedRelativePath);
                        return fullPath;
                    });

            // Now Resolve all N-Order Dependencies
            IEnumerable<string> projFilesAndNOrderDependencies = MSBuildUtilities.ResolveProjectReferenceDependenciesFlat(pathsToProjects);

            GenerateAndSaveSolution(solutionFile, projFilesAndNOrderDependencies);
        }

        /// <summary>
        /// Generates a Solution File at the specified location with the given Solutions pulled from a listing file
        /// </summary>
        /// <param name="solutionFile"></param>
        /// <param name="targetSolutionFile"></param>
        public static void FromSolutionListing(string solutionFile, string targetSolutionFile)
        {
            IEnumerable<string> targetSolutions = File.ReadLines(targetSolutionFile);
            ForSolutions(solutionFile, targetSolutions);
        }

        /// <summary>
        /// Generates a Solution file at the specified location based on scanning a given directory for supported projects, optionally with an ignore file.
        /// </summary>
        /// <param name="directoryToScan">The directory to scan for projects</param>
        /// <param name="targetSolution">The path to the solution to create</param>
        /// <param name="ignoreFile">Optional path to file containing a list of ignored projects</param>
        public static void ForFolder(string directoryToScan, string targetSolution, string ignoreFile)
        {
            IEnumerable<string> excludedProjects = new string[0];

            if (!string.IsNullOrEmpty(ignoreFile) && File.Exists(ignoreFile))
            {
                excludedProjects =
                    File
                    .ReadLines(ignoreFile)
                    .Select(current => Path.Combine(directoryToScan, current));
            }

            IEnumerable<string> supportedProjectsInDirectory =
                Directory
                .EnumerateFiles(directoryToScan, "*proj", SearchOption.AllDirectories)
                .Where(projectPath => SolutionUtilities.SUPPORTED_PROJECT_TYPES.Contains(Path.GetExtension(projectPath)))
                .Except(excludedProjects);
            IEnumerable<string> projFilesAndNOrderDependencies = MSBuildUtilities.ResolveProjectReferenceDependenciesFlat(supportedProjectsInDirectory);

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
