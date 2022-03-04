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
    [Test,
      TestCase(91.0d, new[] { 83, 91, 94, 89, 89, 96, 91, 92, 90 }),
      TestCase(86.0d, new[] { 91, 90, 81, 83, 84, 83, 88, 91, 89, 84 }),
      TestCase(95.0d, new[] { 101, 100, 91, 93, 96, 95, 94 }),
      TestCase(80.5d, new[] { 78, 82, 81, 77, 79, 81, 80, 81 }),
      TestOf(nameof(MedianExtensions.Median))]
    public void TestMedianExtension(double expectedResult, int[] values)
    {
      Assert.AreEqual(expectedResult, MedianExtensions.Median(values));
    }

    [Test,
      TestOf(nameof(MedianExtensions.Median))]
    public void TestMedianExtExceptions()
    {
      Assert.Multiple(() =>
      {
        Assert.Throws<ArgumentNullException>(() => MedianExtensions.Median(null as IEnumerable<int>));
        Assert.Throws<ArgumentException>(() => MedianExtensions.Median(Enumerable.Empty<int>()));
      });
    }
  }

  [TestFixture,
    TestOf(typeof(MedianExtensions))]
  public class RoundedMedianExtTests
  {
    [Test,
      TestCase(91, new[] { 83, 91, 94, 89, 89, 96, 91, 92, 90 }),
      TestCase(86, new[] { 91, 90, 81, 83, 84, 83, 88, 91, 89, 84 }),
      TestCase(95, new[] { 101, 100, 91, 93, 96, 95, 94 }),
      TestCase(81, new[] { 78, 82, 81, 77, 79, 81, 80, 81 }),
      TestOf(nameof(MedianExtensions.RoundedMedian))]
    public void TestRoundedMedianExtension(int expectedResult, int[] values)
    {
      Assert.AreEqual(expectedResult, MedianExtensions.RoundedMedian(values));
    }

    [Test,
      TestOf(nameof(MedianExtensions.RoundedMedian))]
    public void TestRoundedMedianExtExceptions()
    {
      Assert.Multiple(() =>
      {
        Assert.Throws<ArgumentNullException>(() => MedianExtensions.RoundedMedian(null as IEnumerable<int>));
        Assert.Throws<ArgumentException>(() => MedianExtensions.RoundedMedian(Enumerable.Empty<int>()));
      });
    }
  }
}
