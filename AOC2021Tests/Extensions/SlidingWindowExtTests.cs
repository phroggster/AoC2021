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
    TestOf(typeof(SlidingWindowExtensions))]
  public class SlidingWindowExtTests
  {
    // TODO: is there any way to test this generically, maybe with just one
    // test case that gets converted to differing types for testing?

    [Test,
      TestCaseSource(nameof(iTestCases))]
    public void TestSlidingPairInts(IEnumerable<int> testData, IEnumerable<(int, int)> expected)
    {
      Assert.Throws<ArgumentNullException>(()
        => SlidingWindowExtensions.SlidingPairs<int>(null));
      Assert.Throws<ArgumentException>(()
        => SlidingWindowExtensions.SlidingPairs(Enumerable.Empty<int>()));

      Utility.SequenceEquals(expected,
        SlidingWindowExtensions.SlidingPairs(testData));
    }

    [Test,
      TestCaseSource(nameof(cTestCases))]
    public void TestSlidingPairChars(IEnumerable<char> testData, IEnumerable<(char, char)> expected)
    {
      Assert.Throws<ArgumentNullException>(()
        => SlidingWindowExtensions.SlidingPairs<char>(null));
      Assert.Throws<ArgumentException>(()
        => SlidingWindowExtensions.SlidingPairs(Enumerable.Empty<char>()));

      Utility.SequenceEquals(expected,
        SlidingWindowExtensions.SlidingPairs(testData));
    }

    static readonly object[] iTestCases = new[]
    {
      new object[] {
        new[] { 1, 2, 3, 4, 5, 6 },
        new[] { (1,2), (2,3), (3,4), (4,5), (5,6) }
      },
      new object[] {
        new[] { 6, 4, 2, 1, 3, 5 },
        new[] { (6,4), (4,2), (2,1), (1,3), (3,5) }
      },
    };

    static readonly object[] cTestCases = new[]
    {
      new object[] {
        new[] { 'a', 'b', 'C', 'd', 'e', 'f' },
        new[] { ('a','b'), ('b','C'), ('C','d'), ('d','e'), ('e','f') }
      },
      new object[] {
        new[] { 'F', 'D', 'B', 'A', 'c', 'e' },
        new[] { ('F','D'), ('D','B'), ('B','A'), ('A','c'), ('c','e') }
      },
    };
  }
}
