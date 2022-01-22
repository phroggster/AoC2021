using AoC2021.Extensions;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AoC2021.Tests.Extensions
{
  [TestFixture,
    TestOf(typeof(MedianExtensions)),
    DefaultFloatingPointTolerance(double.Epsilon * 100.0d)]
  public class MedianExtTests
  {
    [TestCaseSource(nameof(TestCases))]
    public void TestMedianExtension(double expectedResult, IEnumerable<int> values)
    {
      Assert.Throws<ArgumentNullException>(() => MedianExtensions.Median((IEnumerable<int>)null));
      Assert.Throws<ArgumentException>(() => MedianExtensions.Median(Enumerable.Empty<int>()));

      Assert.AreEqual(expectedResult, MedianExtensions.Median(values));
    }

    static readonly object[] TestCases =
    {
        new object[] { 91.0d, new[] { 83, 91, 94, 89, 89, 96, 91, 92, 90 } },
        new object[] { 86.0d, new[] { 91, 90, 81, 83, 84, 83, 88, 91, 89, 84 } },
        new object[] { 95.0d, new[] { 101, 100, 91, 93, 96, 95, 94 } },
        new object[] { 80.5d, new[] { 78, 82, 81, 77, 79, 81, 80, 81 } }
      };
  }

  [TestFixture,
 TestOf(typeof(MedianExtensions))]
  public class RoundedMedianExtTests
  {
    [TestCaseSource(nameof(TestCases))]
    public void TestRoundedMedianExtension(int expectedResult, IEnumerable<int> values)
    {
      Assert.Throws<ArgumentNullException>(() => MedianExtensions.Median((IEnumerable<int>)null));
      Assert.Throws<ArgumentException>(() => MedianExtensions.Median(Enumerable.Empty<int>()));

      Assert.AreEqual(expectedResult, MedianExtensions.RoundedMedian(values));
    }

    static readonly object[] TestCases =
    {
        new object[] { 91, new[] { 83, 91, 94, 89, 89, 96, 91, 92, 90 } },
        new object[] { 86, new[] { 91, 90, 81, 83, 84, 83, 88, 91, 89, 84 } },
        new object[] { 95, new[] { 101, 100, 91, 93, 96, 95, 94 } },
        new object[] { 81, new[] { 78, 82, 81, 77, 79, 81, 80, 81 } }
      };
  }
}
