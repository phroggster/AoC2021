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
    TestOf(typeof(RectArrayExtensions))]
  public class RectArrayExtTests
  {
    [Test,
      TestCaseSource(nameof(TestCases))]
    public void TestRectArrayRowToString(IEnumerable<string> expectedResults, char[,] buffer)
    {
      Assert.Throws<ArgumentNullException>(() =>
        RectArrayExtensions.RowToString(null, 1));
      Assert.Throws<ArgumentException>(() =>
        RectArrayExtensions.RowToString(new char[,] { }, 0));
      Assert.Throws<IndexOutOfRangeException>(() =>
        RectArrayExtensions.RowToString(new char[,] { { 'a', 'b' }, { 'c', 'd' } }, 3));

      for (int n = 0; n < expectedResults.Count(); n++)
      {
        Assert.AreEqual(expectedResults.ElementAt(n),
          RectArrayExtensions.RowToString(buffer, n));
      }
    }

    internal static readonly object[] TestCases =
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
