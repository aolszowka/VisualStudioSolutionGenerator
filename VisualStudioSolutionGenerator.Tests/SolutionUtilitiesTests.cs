// -----------------------------------------------------------------------
// <copyright file="SolutionUtilitiesTests.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2020. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace VisualStudioSolutionGenerator.Tests
{
    using System.Collections;
    using System.IO;

    using NUnit.Framework;

    using SolutionGenerator;

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

        [TestCaseSource(typeof(GetProjectTypeGuid_ValidInput_Tests))]
        public void GetProjectTypeGuid_ValidInput(string pathToProj, string expected)
        {
            string actual = SolutionUtilities.GetProjectTypeGuid(pathToProj);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }

    internal class GenerateSolutionFragmentForProject_ValidInput_Tests : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new TestCaseData(TestContext.CurrentContext.TestDirectory, Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "net472csharp.csproj"), $"Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"net472csharp\", \"TestData\\net472csharp.csproj\", \"{{4BC61164-C067-4CFC-8C42-A6CA73C871F9}}\"\r\nEndProject");
            yield return new TestCaseData(TestContext.CurrentContext.TestDirectory, Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "net472fsharp.fsproj"), $"Project(\"{{F2A71F9B-5D33-465A-A702-920D77279786}}\") = \"net472fsharp\", \"TestData\\net472fsharp.fsproj\", \"{{a4e18d6f-b1ac-4751-851d-c8d058ccf375}}\"\r\nEndProject");
            yield return new TestCaseData(TestContext.CurrentContext.TestDirectory, Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "net472syn.synproj"), $"Project(\"{{BBD0F5D1-1CC4-42FD-BA4C-A96779C64378}}\") = \"net472syn\", \"TestData\\net472syn.synproj\", \"{{3e67c72a-5b2e-4d5b-9170-67d010dfa68c}}\"\r\nEndProject");
            yield return new TestCaseData(TestContext.CurrentContext.TestDirectory, Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "net472sql.sqlproj"), $"Project(\"{{00D1A9C2-B5F0-4AF3-8072-F6C62B433612}}\") = \"net472sql\", \"TestData\\net472sql.sqlproj\", \"{{39b82b92-ef84-4b01-a396-12fd3c5829a1}}\"\r\nEndProject");
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
