// -----------------------------------------------------------------------
// <copyright file="MSBuildUtilities.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2018-2020. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SolutionGenerator
{
    static class MSBuildUtilities
    {
        /// <summary>
        /// The MSBuild XML Namespace
        /// </summary>
        private static XNamespace MSBUILD_NAMESPACE = @"http://schemas.microsoft.com/developer/msbuild/2003";

        /// <summary>
        /// Extracts the Project GUID from the specified MSBuildProject File File.
        /// </summary>
        /// <param name="pathToProjFile">The MSBuild Project File File to extract the Project GUID from.</param>
        /// <returns>The specified Files Project GUID.</returns>
        public static string GetProjectGuid(string pathToProjFile)
        {
            XDocument projFile = XDocument.Load(pathToProjFile);
            XElement projectGuid = projFile.Descendants(MSBUILD_NAMESPACE + "ProjectGuid").First();
            return projectGuid.Value;
        }

        public static IEnumerable<string> ResolveProjectReferenceDependenciesFlat(IEnumerable<string> targetProjects)
        {
            Dictionary<string, IEnumerable<string>> resolvedProjects = ResolveProjectReferenceDependencies(targetProjects);

            HashSet<string> distinctProjects = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            // Include all the target projects
            foreach (string targetProject in targetProjects)
            {
                distinctProjects.Add(targetProject);
            }

            foreach (KeyValuePair<string, IEnumerable<string>> resolvedProject in resolvedProjects)
            {
                foreach (string projectReference in resolvedProject.Value)
                {
                    distinctProjects.Add(projectReference);
                }
            }

            return distinctProjects;
        }

        /// <summary>
        /// Given a IEnumerable of Target Project Files, Resolve All N-Order ProjectReference Dependencies.
        /// </summary>
        /// <param name="targetProjects">An IEnumerable of strings that represent MSBuild Projects.</param>
        /// <returns>A Dictionary in which the Key is the Project, and the Value is an IEnumerable of all its Project Reference projects</returns>
        public static Dictionary<string, IEnumerable<string>> ResolveProjectReferenceDependencies(IEnumerable<string> targetProjects)
        {
            Dictionary<string, IEnumerable<string>> resolvedProjects = new Dictionary<string, IEnumerable<string>>();

            // Load up the initial projects to the stack
            Stack<string> unresolvedProjects = new Stack<string>(targetProjects.Distinct());

            while (unresolvedProjects.Count > 0)
            {
                string currentProject = unresolvedProjects.Pop();

                // First check just to make sure it wasn't already resolved.
                if (!resolvedProjects.ContainsKey(currentProject))
                {
                    // Get all this projects references
                    string[] projectDependencies = ProjectDependencies(currentProject).ToArray();

                    // Add the current project along with all its DIRECT
                    // Dependencies to the result structure
                    resolvedProjects.Add(currentProject, projectDependencies);

                    foreach (string projectDependency in projectDependencies)
                    {
                        // Save the stack by not resolving already resolved projects
                        if (!resolvedProjects.ContainsKey(projectDependency))
                        {
                            unresolvedProjects.Push(projectDependency);
                        }
                    }
                }
            }

            return resolvedProjects;
        }

        /// <summary>
        /// Given a path to a file that is assumed to be an MSBuild Type Project file, Return all ProjectReference Paths as fully qualified paths.
        /// </summary>
        /// <param name="targetProject">The project to load.</param>
        /// <returns>An IEnumerable that contains all the fully qualified ProjectReference paths.</returns>
        static IEnumerable<string> ProjectDependencies(string targetProject)
        {
            XNamespace msbuildNS = "http://schemas.microsoft.com/developer/msbuild/2003";

            XDocument projXml = XDocument.Load(targetProject);

            IEnumerable<XElement> projectReferences = projXml.Descendants(msbuildNS + "ProjectReference");

            foreach (XElement projectReference in projectReferences)
            {
                string relativeProjectPath = projectReference.Attribute("Include").Value;
                string resolvedPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(targetProject), relativeProjectPath));
                yield return resolvedPath;
            }
        }
    }
}
