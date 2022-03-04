using AoC2021.Day12;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Tests.Day12
{
  [TestFixture,
    TestOf(typeof(PassagePathing))]
  public class Day12Tests
  {
    static readonly object[] PartATestCases =
    {
      new object[] { 10, Data.Day12ExampleDataA },
      new object[] { 19, Data.Day12ExampleDataB },
      new object[] { 226, Data.Day12ExampleDataC },
    };

    static readonly object[] PartBTestCases =
    {
      new object[] { 36, Data.Day12ExampleDataA },
      new object[] { 103, Data.Day12ExampleDataB },
      new object[] { 3509, Data.Day12ExampleDataC },
    };

    static readonly object[] RestrictionTestCases =
    {
      new object[] { true, new[] { "av", "cv", "gf", "ie", "im", "ng", "wg", "start", "end" } },
      new object[] { false, new[] { "AV", "CV", "GF", "IE", "IM", "NG", "WG", "START", "END" } },
    };

    [Test,
      TestCaseSource(nameof(PartATestCases))]
    public void TestPathingWithoutRevisits(int expectedResult, IEnumerable<string> values)
    {
      Assert.AreEqual(expectedResult, PassagePathing.CountPathsWithoutRevisits(values));
    }

    [Test,
      TestCaseSource(nameof(PartBTestCases))]
    public void TestPathingWithOneRevisit(int expectedResult, IEnumerable<string> values)
    {
      Assert.AreEqual(expectedResult, PassagePathing.CountPathsAllowingOneRevisit(values));
    }

    [Test,
      TestCaseSource(nameof(RestrictionTestCases))]
    public void TestRestrictions(bool expectedIsRestricted, IEnumerable<string> values)
    {
      Assert.Multiple(() =>
      {
        foreach (var value in values)
        {
          Assert.AreEqual(expectedIsRestricted, PassagePathing.IsRestricted(value));
        }
      });
    }
  }
}
