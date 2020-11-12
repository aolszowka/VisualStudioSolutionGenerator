// -----------------------------------------------------------------------
// <copyright file="MSBuildUtilitiesTests.cs" company="Ace Olszowka">
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
    public class MSBuildUtilitiesTests
    {
        [TestCaseSource(typeof(IsDotnetCore_ValidInput_Tests))]
        public void IsDotnetCore_ValidInput(string pathToProj, bool expected)
        {
            bool actual = MSBuildUtilities.IsDotnetCore(pathToProj);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }

    internal class IsDotnetCore_ValidInput_Tests : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "GetProjectTypeGuid", "net472csharp.csproj"), false);
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "GetProjectTypeGuid", "net472fsharp.fsproj"), false);
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "GetProjectTypeGuid", "net472syn.synproj"), false);
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "GetProjectTypeGuid", "net472sql.sqlproj"), false);
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "GetProjectTypeGuid", "netcoreapp31csharp.csproj"), true);
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "GetProjectTypeGuid", "netstandard20csharp.csproj"), true);
            yield return new TestCaseData(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "GetProjectTypeGuid", "netstandard20fsharp.fsproj"), true);
        }
    }
}
