// -----------------------------------------------------------------------
// <copyright file="SolutionUtilitiesTests.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2020. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace VisualStudioSolutionGenerator.Tests
{
    using System;
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
            yield return new TestCaseData(TestContext.CurrentContext.TestDirectory, Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "GetProjectTypeGuid", "net472csharp.csproj"), $"Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"net472csharp\", \"TestData\\GetProjectTypeGuid\\net472csharp.csproj\", \"{{4BC61164-C067-4CFC-8C42-A6CA73C871F9}}\"{Environment.NewLine}EndProject");
            yield return new TestCaseData(TestContext.CurrentContext.TestDirectory, Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "GetProjectTypeGuid", "net472fsharp.fsproj"), $"Project(\"{{F2A71F9B-5D33-465A-A702-920D77279786}}\") = \"net472fsharp\", \"TestData\\GetProjectTypeGuid\\net472fsharp.fsproj\", \"{{a4e18d6f-b1ac-4751-851d-c8d058ccf375}}\"{Environment.NewLine}EndProject");
            yield return new TestCaseData(TestContext.CurrentContext.TestDirectory, Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "GetProjectTypeGuid", "net472syn.synproj"), $"Project(\"{{BBD0F5D1-1CC4-42FD-BA4C-A96779C64378}}\") = \"net472syn\", \"TestData\\GetProjectTypeGuid\\net472syn.synproj\", \"{{3e67c72a-5b2e-4d5b-9170-67d010dfa68c}}\"{Environment.NewLine}EndProject");
            yield return new TestCaseData(TestContext.CurrentContext.TestDirectory, Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "GetProjectTypeGuid", "net472sql.sqlproj"), $"Project(\"{{00D1A9C2-B5F0-4AF3-8072-F6C62B433612}}\") = \"net472sql\", \"TestData\\GetProjectTypeGuid\\net472sql.sqlproj\", \"{{39b82b92-ef84-4b01-a396-12fd3c5829a1}}\"{Environment.NewLine}EndProject");
            yield return new TestCaseData(TestContext.CurrentContext.TestDirectory, Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "GetProjectTypeGuid", "netcoreapp31csharp.csproj"), "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}");
            yield return new TestCaseData(TestContext.CurrentContext.TestDirectory, Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "GetProjectTypeGuid", "netstandard20csharp.csproj"), "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}");
            yield return new TestCaseData(TestContext.CurrentContext.TestDirectory, Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "GetProjectTypeGuid", "netstandard20fsharp.fsproj"), "{6EC3EE1D-3C4E-46DD-8F32-0CC8E7565705}");
        }
    }

    internal class GetProjectTypeGuid_ValidInput_Tests : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "GetProjectTypeGuid", "net472csharp.csproj"), "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}");
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "GetProjectTypeGuid", "net472fsharp.fsproj"), "{F2A71F9B-5D33-465A-A702-920D77279786}");
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "GetProjectTypeGuid", "net472syn.synproj"), "{BBD0F5D1-1CC4-42FD-BA4C-A96779C64378}");
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "GetProjectTypeGuid", "net472sql.sqlproj"), "{00D1A9C2-B5F0-4AF3-8072-F6C62B433612}");
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "GetProjectTypeGuid", "netcoreapp31csharp.csproj"), "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}");
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "GetProjectTypeGuid", "netstandard20csharp.csproj"), "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}");
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "GetProjectTypeGuid", "netstandard20fsharp.fsproj"), "{6EC3EE1D-3C4E-46DD-8F32-0CC8E7565705}");
        }
    }
}
