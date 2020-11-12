namespace VisualStudioSolutionGenerator.UnitTests
{
    using System.Collections;

    using NUnit.Framework;

    [TestFixture]
    public class SolutionUtilitiesTests
    {
        [TestCaseSource(typeof(GetProjectTypeGuid_ValidInput_Tests))]
        public void GetProjectTypeGuid_ValidInput(string pathToProj, string expectedGuid)
        {

        }
    }

    internal class GetProjectTypeGuid_ValidInput_Tests : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new TestCaseData();
        }
    }
}
