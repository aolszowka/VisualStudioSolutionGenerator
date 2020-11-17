// -----------------------------------------------------------------------
// <copyright file="SolutionUtilitiesTests.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2020. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace VisualStudioSolutionGenerator.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    using NUnit.Framework;

    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class SolutionUtilitiesTests
    {
        [TestCaseSource(typeof(GenerateSolutionFragmentForProject_ValidInput_Tests))]
        public void GenerateSolutionFragmentForProject_ValidInput(string solutionRoot, string pathToProjFile, string expected)
        {
            string actual = SolutionUtilities.GenerateSolutionFragmentForProject(solutionRoot, pathToProjFile);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCaseSource(typeof(GetProjectsFromSolution_ValidInput_Tests))]
        public void GetProjectsFromSolution_ValidInput(string pathToSln, IEnumerable<string> expected)
        {
            IEnumerable<string> actual = SolutionUtilities.GetProjectsFromSolution(pathToSln);

            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [TestCaseSource(typeof(GenerateSolutionForProjects_ValidInput_Tests))]
        public void GenerateSolutionForProjects_ValidInput(string targetDirectory, IEnumerable<string> projFilePaths, string expected)
        {
            string actual = SolutionUtilities.GenerateSolutionForProjects(targetDirectory, projFilePaths);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCaseSource(typeof(GetProjectTypeGuid_ValidInput_Tests))]
        public void GetProjectTypeGuid_ValidInput(string pathToProj, string expected)
        {
            string actual = SolutionUtilities.GetProjectTypeGuid(pathToProj);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }

    internal class GenerateSolutionForProjects_ValidInput_Tests : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new TestCaseData
                (
                    Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "net472csharp", "net472csharp_C"),
                    new string[]
                    {
                        Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "net472csharp", "net472csharp_C", "net472csharp_C.csproj")
                    },
                    "\r\nMicrosoft Visual Studio Solution File, Format Version 12.00\r\n# Visual Studio 15\r\nVisualStudioVersion = 15.0.27703.2035\r\nMinimumVisualStudioVersion = 10.0.40219.1\r\nProject(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"net472csharp_C\", \"net472csharp_C.csproj\", \"{74E3C5F3-0384-450D-95A6-0D5CE3ABE9A4}\"\r\nEndProject\r\nGlobal\r\n\tGlobalSection(SolutionConfigurationPlatforms) = preSolution\r\n\t\tDebug|Any CPU = Debug|Any CPU\r\n\t\tRelease|Any CPU = Release|Any CPU\r\n\tEndGlobalSection\r\n\tGlobalSection(ProjectConfigurationPlatforms) = postSolution\r\n\t\t{74E3C5F3-0384-450D-95A6-0D5CE3ABE9A4}.Debug|Any CPU.ActiveCfg = Debug|Any CPU\r\n\t\t{74E3C5F3-0384-450D-95A6-0D5CE3ABE9A4}.Debug|Any CPU.Build.0 = Debug|Any CPU\r\n\t\t{74E3C5F3-0384-450D-95A6-0D5CE3ABE9A4}.Release|Any CPU.ActiveCfg = Release|Any CPU\r\n\t\t{74E3C5F3-0384-450D-95A6-0D5CE3ABE9A4}.Release|Any CPU.Build.0 = Release|Any CPU\r\n\tEndGlobalSection\r\nEndGlobal\r\n"
                );
            yield return new TestCaseData
                (
                    Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "net472csharp"),
                    new string[]
                    {
                        Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "net472csharp", "net472csharp_A", "net472csharp_A.csproj"),
                        Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "net472csharp", "net472csharp_B", "net472csharp_B.csproj"),
                    },
                    "\r\nMicrosoft Visual Studio Solution File, Format Version 12.00\r\n# Visual Studio 15\r\nVisualStudioVersion = 15.0.27703.2035\r\nMinimumVisualStudioVersion = 10.0.40219.1\r\nProject(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"net472csharp_A\", \"net472csharp_A\\net472csharp_A.csproj\", \"{4014FC7F-9131-497B-87BB-6BC38AD77F79}\"\r\nEndProject\r\nProject(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"net472csharp_B\", \"net472csharp_B\\net472csharp_B.csproj\", \"{E4D24C15-243F-41C9-AB0F-B5AC68A2C0F9}\"\r\nEndProject\r\nGlobal\r\n\tGlobalSection(SolutionConfigurationPlatforms) = preSolution\r\n\t\tDebug|Any CPU = Debug|Any CPU\r\n\t\tRelease|Any CPU = Release|Any CPU\r\n\tEndGlobalSection\r\n\tGlobalSection(ProjectConfigurationPlatforms) = postSolution\r\n\t\t{4014FC7F-9131-497B-87BB-6BC38AD77F79}.Debug|Any CPU.ActiveCfg = Debug|Any CPU\r\n\t\t{4014FC7F-9131-497B-87BB-6BC38AD77F79}.Debug|Any CPU.Build.0 = Debug|Any CPU\r\n\t\t{4014FC7F-9131-497B-87BB-6BC38AD77F79}.Release|Any CPU.ActiveCfg = Release|Any CPU\r\n\t\t{4014FC7F-9131-497B-87BB-6BC38AD77F79}.Release|Any CPU.Build.0 = Release|Any CPU\r\n\t\t{E4D24C15-243F-41C9-AB0F-B5AC68A2C0F9}.Debug|Any CPU.ActiveCfg = Debug|Any CPU\r\n\t\t{E4D24C15-243F-41C9-AB0F-B5AC68A2C0F9}.Debug|Any CPU.Build.0 = Debug|Any CPU\r\n\t\t{E4D24C15-243F-41C9-AB0F-B5AC68A2C0F9}.Release|Any CPU.ActiveCfg = Release|Any CPU\r\n\t\t{E4D24C15-243F-41C9-AB0F-B5AC68A2C0F9}.Release|Any CPU.Build.0 = Release|Any CPU\r\n\tEndGlobalSection\r\nEndGlobal\r\n"
                );
        }
    }

    internal class GetProjectsFromSolution_ValidInput_Tests : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new TestCaseData
                (
                    Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "net472csharp", "net472csharp.sln"),
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
                    Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "ProjectDependencies", "netstandard20csharp", "netstandard20csharp.sln"),
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

    internal class GenerateSolutionFragmentForProject_ValidInput_Tests : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new TestCaseData(TestContext.CurrentContext.TestDirectory, Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "net472csharp.csproj"), $"Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"net472csharp\", \"TestData\\net472csharp.csproj\", \"{{4BC61164-C067-4CFC-8C42-A6CA73C871F9}}\"\r\nEndProject");
            yield return new TestCaseData(TestContext.CurrentContext.TestDirectory, Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "net472fsharp.fsproj"), $"Project(\"{{F2A71F9B-5D33-465A-A702-920D77279786}}\") = \"net472fsharp\", \"TestData\\net472fsharp.fsproj\", \"{{A4E18D6F-B1AC-4751-851D-C8D058CCF375}}\"\r\nEndProject");
            yield return new TestCaseData(TestContext.CurrentContext.TestDirectory, Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "net472syn.synproj"), $"Project(\"{{BBD0F5D1-1CC4-42FD-BA4C-A96779C64378}}\") = \"net472syn\", \"TestData\\net472syn.synproj\", \"{{3E67C72A-5B2E-4D5B-9170-67D010DFA68C}}\"\r\nEndProject");
            yield return new TestCaseData(TestContext.CurrentContext.TestDirectory, Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "net472sql.sqlproj"), $"Project(\"{{00D1A9C2-B5F0-4AF3-8072-F6C62B433612}}\") = \"net472sql\", \"TestData\\net472sql.sqlproj\", \"{{39B82B92-EF84-4B01-A396-12FD3C5829A1}}\"\r\nEndProject");
            yield return new TestCaseData(TestContext.CurrentContext.TestDirectory, Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "netcoreapp31csharp.csproj"), $"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"netcoreapp31csharp\", \"TestData\\netcoreapp31csharp.csproj\", \"{{AE8A776A-53CE-4929-9DB7-85AEB5606100}}\"\r\nEndProject");
            yield return new TestCaseData(TestContext.CurrentContext.TestDirectory, Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "netstandard20csharp.csproj"), $"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"netstandard20csharp\", \"TestData\\netstandard20csharp.csproj\", \"{{49080A1E-A226-489E-8974-D8DEA2C53661}}\"\r\nEndProject");
            yield return new TestCaseData(TestContext.CurrentContext.TestDirectory, Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "netstandard20fsharp.fsproj"), $"Project(\"{{6EC3EE1D-3C4E-46DD-8F32-0CC8E7565705}}\") = \"netstandard20fsharp\", \"TestData\\netstandard20fsharp.fsproj\", \"{{500445AE-CA30-4EE5-8828-9D9615B9376C}}\"\r\nEndProject");
        }
    }

    internal class GetProjectTypeGuid_ValidInput_Tests : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "net472csharp.csproj"), "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}");
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "net472fsharp.fsproj"), "{F2A71F9B-5D33-465A-A702-920D77279786}");
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "net472syn.synproj"), "{BBD0F5D1-1CC4-42FD-BA4C-A96779C64378}");
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "net472sql.sqlproj"), "{00D1A9C2-B5F0-4AF3-8072-F6C62B433612}");
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "netcoreapp31csharp.csproj"), "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}");
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "netstandard20csharp.csproj"), "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}");
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "netstandard20fsharp.fsproj"), "{6EC3EE1D-3C4E-46DD-8F32-0CC8E7565705}");
        }
    }
}
