using AoC2021.Extensions;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Tests
{
  [TestFixture]
  public class ExtensionTests
  {
    [TestCaseSource(nameof(MedianTestCases)),
      TestOf(typeof(MedianExtensions))]
    public void TestMedianExtension(double expectedResult, IEnumerable<int> values)
    {
      Assert.AreEqual(expectedResult, MedianExtensions.Median(values), FPTolerance);
    }

    [TestCaseSource(nameof(RoundedMedianTestCases)),
      TestOf(typeof(MedianExtensions))]
    public void TestRoundedMedianExtension(int expectedResult, IEnumerable<int> values)
    {
      Assert.AreEqual(expectedResult, MedianExtensions.RoundedMedian(values), FPTolerance);
    }

    [TestCaseSource(nameof(RowToStringTestCases)),
      TestOf(typeof(RectArrayExtensions))]
    public void TestRectArrayRowToString(IEnumerable<string> expectedResults, char[,] buffer)
    {
      Assert.Multiple(() =>
      {
        for (int n = 0; n < expectedResults.Count(); n++)
        {
          Assert.AreEqual(expectedResults.ElementAt(n), RectArrayExtensions.RowToString(buffer, n));
        }
      });
    }

    const double FPTolerance = 0.00001d;

    static readonly object[] MedianTestCases =
    {
      new object[] { 91.0d, new[] { 83, 91, 94, 89, 89, 96, 91, 92, 90 } },
      new object[] { 86.0d, new[] { 91, 90, 81, 83, 84, 83, 88, 91, 89, 84 } },
      new object[] { 95.0d, new[] { 101, 100, 91, 93, 96, 95, 94 } },
      new object[] { 80.5d, new[] { 78, 82, 81, 77, 79, 81, 80, 81 } }
    };

    static readonly object[] RoundedMedianTestCases =
    {
      new object[] { 91, new[] { 83, 91, 94, 89, 89, 96, 91, 92, 90 } },
      new object[] { 86, new[] { 91, 90, 81, 83, 84, 83, 88, 91, 89, 84 } },
      new object[] { 95, new[] { 101, 100, 91, 93, 96, 95, 94 } },
      new object[] { 81, new[] { 78, 82, 81, 77, 79, 81, 80, 81 } }
    };

    static readonly object[] RowToStringTestCases =
    {
      new object[] {
        new string[] { "abc", "def", "ghi" },
        new char[,] {
          { 'a', 'b', 'c' },
          { 'd', 'e', 'f' },
          { 'g', 'h', 'i' }
      } },
    };
  }
}
