// -----------------------------------------------------------------------
// <copyright file="MSBuildUtilities.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2018-2020. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace VisualStudioSolutionGenerator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    static class MSBuildUtilities
    {
        /// <summary>
        /// The MSBuild XML Namespace
        /// </summary>
        private static readonly XNamespace MSBUILD_NAMESPACE = @"http://schemas.microsoft.com/developer/msbuild/2003";

        /// <summary>
        /// Extracts the Project GUID from the specified MSBuildProject File File.
        /// </summary>
        /// <param name="pathToProjFile">The MSBuild Project File File to extract the Project GUID from.</param>
        /// <returns>The specified Files Project GUID.</returns>
        public static string GetProjectGuid(string pathToProjFile)
        {
            XDocument projFile = XDocument.Load(pathToProjFile);

            XElement projectGuid;

            if (IsDotnetCore(pathToProjFile))
            {
                projectGuid = projFile.Descendants("ProjectGuid").FirstOrDefault();
            }
            else
            {
                projectGuid = projFile.Descendants(MSBUILD_NAMESPACE + "ProjectGuid").FirstOrDefault();
            }

            if (projectGuid == null)
            {
                throw new InvalidOperationException($"Project `{pathToProjFile}` did not have a ProjectGuid Element; This tool will not work.");
            }

            return projectGuid.Value;
        }

        /// <summary>
        /// Given a IEnumerable of Target Project Files, Resolve All N-Order ProjectReference Dependencies.
        /// </summary>
        /// <param name="targetProjects">An IEnumerable of strings that represent MSBuild Projects.</param>
        /// <returns>A IEnumerable of all the distinct projects, including the target projects.</returns>
        public static IEnumerable<string> ResolveProjectReferenceDependenciesFlat(IEnumerable<string> targetProjects)
        {
            IDictionary<string, IEnumerable<string>> resolvedProjects = ResolveProjectReferenceDependencies(targetProjects);

            HashSet<string> distinctProjects = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

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
        public static IDictionary<string, IEnumerable<string>> ResolveProjectReferenceDependencies(IEnumerable<string> targetProjects)
        {
            Dictionary<string, IEnumerable<string>> resolvedProjects = new Dictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase);

            // Load up the initial projects to the stack
            Stack<string> unresolvedProjects = new Stack<string>(targetProjects.Distinct(StringComparer.OrdinalIgnoreCase));

            while (unresolvedProjects.Count > 0)
            {
                string currentProject = unresolvedProjects.Pop();

                // First check just to make sure it wasn't already resolved.
                if (!resolvedProjects.ContainsKey(currentProject))
                {
                    // Get all this projects references
                    string[] projectReferences = ProjectReferences(currentProject).ToArray();

                    // Add the current project along with all its DIRECT
                    // Dependencies to the result structure
                    resolvedProjects.Add(currentProject, projectReferences);

                    foreach (string projectReference in projectReferences)
                    {
                        // Save the stack by not resolving already resolved projects
                        if (!resolvedProjects.ContainsKey(projectReference))
                        {
                            unresolvedProjects.Push(projectReference);
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
        internal static IEnumerable<string> ProjectReferences(string targetProject)
        {
            XDocument projXml = XDocument.Load(targetProject);

            IEnumerable<XElement> projectReferences;

            if (IsDotnetCore(targetProject))
            {
                projectReferences = projXml.Descendants("ProjectReference");
            }
            else
            {
                projectReferences = projXml.Descendants(MSBUILD_NAMESPACE + "ProjectReference");
            }

            foreach (XElement projectReference in projectReferences)
            {
                // The MSBuild format uses a backlash (\) for relative paths,
                // however on non-windows platforms this breaks Path.Combine,
                // replace this with the appropriate DirectorySeparatorChar
                string relativeProjectPath = projectReference.Attribute("Include").Value.Replace('\\', Path.DirectorySeparatorChar);
                string resolvedPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(targetProject), relativeProjectPath));
                yield return resolvedPath;
            }
        }

        /// <summary>
        /// Attempt to determine if a project is a .NET Core Project type.
        /// </summary>
        /// <param name="pathToProjFile">The path to the project file.</param>
        /// <returns><c>true</c> if this is a .NET Core project; otherwise, <c>false</c>.</returns>
        internal static bool IsDotnetCore(string pathToProjFile)
        {
            XDocument projXml = XDocument.Load(pathToProjFile);
            bool hasSdkAttribute =
                projXml
                .Descendants("Project")
                .Where(projectElement => projectElement.Attribute("Sdk") != null)
                .Any();

            return hasSdkAttribute;
        }
    }
}
