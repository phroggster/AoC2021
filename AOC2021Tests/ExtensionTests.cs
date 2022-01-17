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
    private const double FPTolerance = 0.00001d;

    // various groups of data, odd and even set sizes, designed for prodding.
    private static readonly int[] s_gpA = new[] { 83, 91, 94, 89, 89, 96, 91, 92, 90 };
    private static readonly int[] s_gpB = new[] { 91, 90, 81, 83, 84, 83, 88, 91, 89, 84 };
    private static readonly int[] s_gpC = new[] { 101, 100, 91, 93, 96, 95, 94 };
    private static readonly int[] s_gpD = new[] { 78, 82, 81, 77, 79, 81, 80, 81 };

    private static readonly object[] MedianTestCases =
    {
      new object[] { 91.0d, s_gpA },
      new object[] { 86.0d, s_gpB },
      new object[] { 95.0d, s_gpC },
      new object[] { 80.5d, s_gpD },

      new object[] { 89.0d, s_gpA.Concat(s_gpB) },
      new object[] { 92.5d, s_gpA.Concat(s_gpC) },
      new object[] { 83.0d, s_gpA.Concat(s_gpD) },

      new object[] { 89.0d, s_gpB.Concat(s_gpA) },
      new object[] { 91.0d, s_gpB.Concat(s_gpC) },
      new object[] { 82.5d, s_gpB.Concat(s_gpD) },

      new object[] { 82.0d, s_gpC.Concat(s_gpD) },

      new object[] { 91.0d, s_gpA.Concat(s_gpB).Concat(s_gpC) },
      new object[] { 84.0d, s_gpA.Concat(s_gpB).Concat(s_gpD) },
      new object[] { 84.0d, s_gpB.Concat(s_gpC).Concat(s_gpD) },

      new object[] { 89.0d, s_gpA.Concat(s_gpB).Concat(s_gpC).Concat(s_gpD) }
    };

    private static readonly object[] RoundedMedianTestCases =
    {
      new object[] { 91, s_gpA },
      new object[] { 86, s_gpB },
      new object[] { 95, s_gpC },
      new object[] { 81, s_gpD },

      new object[] { 89, s_gpA.Concat(s_gpB) },
      new object[] { 93, s_gpA.Concat(s_gpC) },
      new object[] { 83, s_gpA.Concat(s_gpD) },
      new object[] { 91, s_gpB.Concat(s_gpC) },
      new object[] { 83, s_gpB.Concat(s_gpD) },
      new object[] { 82, s_gpC.Concat(s_gpD) },

      new object[] { 89, s_gpA.Concat(s_gpB).Concat(s_gpC).Concat(s_gpD) },
    };

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
  }
}
