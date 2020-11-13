// -----------------------------------------------------------------------
// <copyright file="MSBuildUtilitiesTests.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2020. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace VisualStudioSolutionGenerator.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    using NUnit.Framework;

    using SolutionGenerator;

    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class MSBuildUtilitiesTests
    {
        [TestCaseSource(typeof(GetProjectGuid_ValidInput_Tests))]
        public void GetProjectGuid_ValidInput(string pathToProj, string expected)
        {
            string actual = MSBuildUtilities.GetProjectGuid(pathToProj);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCaseSource(typeof(IsDotnetCore_ValidInput_Tests))]
        public void IsDotnetCore_ValidInput(string pathToProj, bool expected)
        {
            bool actual = MSBuildUtilities.IsDotnetCore(pathToProj);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCaseSource(typeof(ProjectReferences_ValidInput_Tests))]
        public void ProjectReferences_ValidInput(string pathToProj, IEnumerable<string> expected)
        {
            IEnumerable<string> actual = MSBuildUtilities.ProjectReferences(pathToProj);

            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [TestCaseSource(typeof(ResolveProjectReferenceDependenciesFlat_ValidInput_Tests))]
        public void ResolveProjectReferenceDependenciesFlat_ValidInput(IEnumerable<string> projects, IEnumerable<string> expected)
        {
            IEnumerable<string> actual = MSBuildUtilities.ResolveProjectReferenceDependenciesFlat(projects);

            Assert.That(actual, Is.EquivalentTo(expected));
        }
    }

    internal class ResolveProjectReferenceDependenciesFlat_ValidInput_Tests : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new
                TestCaseData
                (
                    new string[]
                    {
                        Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "net472csharp", "net472csharp_A", "net472csharp_A.csproj"),
                    },
                    new string[]
                    {
                        Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "net472csharp", "net472csharp_A", "net472csharp_A.csproj"),
                        Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "net472csharp", "net472csharp_B", "net472csharp_B.csproj"),
                        Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "net472csharp", "net472csharp_C", "net472csharp_C.csproj"),
                        Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "net472csharp", "net472csharp_D", "net472csharp_D.csproj"),
                    }
                );
            yield return new
                TestCaseData
                (
                    new string[]
                    {
                        Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "netstandard20csharp", "netstandard20csharp_A", "netstandard20csharp_A.csproj"),
                    },
                    new string[]
                    {
                        Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "netstandard20csharp", "netstandard20csharp_A", "netstandard20csharp_A.csproj"),
                        Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "netstandard20csharp", "netstandard20csharp_B", "netstandard20csharp_B.csproj"),
                        Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "netstandard20csharp", "netstandard20csharp_C", "netstandard20csharp_C.csproj"),
                        Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "netstandard20csharp", "netstandard20csharp_D", "netstandard20csharp_D.csproj"),
                    }
                );
        }
    }

    internal class ProjectReferences_ValidInput_Tests : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new
                TestCaseData
                (
                    Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "net472csharp", "net472csharp_A", "net472csharp_A.csproj"),
                    new string[]
                    {
                        Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "net472csharp", "net472csharp_B", "net472csharp_B.csproj"),
                        Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "net472csharp", "net472csharp_C", "net472csharp_C.csproj"),
                    }
                );
            yield return new
                TestCaseData
                (
                    Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "net472csharp", "net472csharp_C", "net472csharp_C.csproj"),
                    new string[]
                    {
                        Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "net472csharp", "net472csharp_D", "net472csharp_D.csproj"),
                    }
                );
            yield return new
                TestCaseData
                (
                    Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "netstandard20csharp", "netstandard20csharp_A", "netstandard20csharp_A.csproj"),
                    new string[]
                    {
                        Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "netstandard20csharp", "netstandard20csharp_B", "netstandard20csharp_B.csproj"),
                        Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "netstandard20csharp", "netstandard20csharp_C", "netstandard20csharp_C.csproj"),
                    }
                );
            yield return new
                TestCaseData
                (
                    Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "netstandard20csharp", "netstandard20csharp_C", "netstandard20csharp_C.csproj"),
                    new string[]
                    {
                        Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "netstandard20csharp", "netstandard20csharp_D", "netstandard20csharp_D.csproj"),
                    }
                );
        }
    }

    internal class GetProjectGuid_ValidInput_Tests : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "net472csharp.csproj"), "{4BC61164-C067-4CFC-8C42-A6CA73C871F9}");
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "net472fsharp.fsproj"), "{a4e18d6f-b1ac-4751-851d-c8d058ccf375}");
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "net472syn.synproj"), "{3e67c72a-5b2e-4d5b-9170-67d010dfa68c}");
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "net472sql.sqlproj"), "{39b82b92-ef84-4b01-a396-12fd3c5829a1}");
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "netcoreapp31csharp.csproj"), "{AE8A776A-53CE-4929-9DB7-85AEB5606100}");
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "netstandard20csharp.csproj"), "{49080A1E-A226-489E-8974-D8DEA2C53661}");
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "netstandard20fsharp.fsproj"), "{500445AE-CA30-4EE5-8828-9D9615B9376C}");
        }
    }

    internal class IsDotnetCore_ValidInput_Tests : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "net472csharp.csproj"), false);
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "net472fsharp.fsproj"), false);
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "net472syn.synproj"), false);
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "net472sql.sqlproj"), false);
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "netcoreapp31csharp.csproj"), true);
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "netstandard20csharp.csproj"), true);
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "netstandard20fsharp.fsproj"), true);
        }
    }
}
