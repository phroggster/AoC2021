using AoC2021.Day13;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Tests.Day13
{
  [TestFixture,
    TestOf(typeof(TransparentOrigami))]
  public class Day13Tests
  {
    [Test]
    public void TestSingleFoldOrigami()
    {
      Assert.AreEqual(17, TransparentOrigami.PartOne(Data.Day13ExampleData));
    }

    [Test]
    public void TestOrigamiPart2()
    {
      Assert.AreEqual(FoldedThrice, TransparentOrigami.PartTwo(Data.Day13ExampleData));
    }

    [Test,
      TestCaseSource(nameof(GraphTestData))]
    public void TestDebuggingOutput(string expectedResult, int nFolds, IEnumerable<string> dataSet)
    {
      Assert.AreEqual(expectedResult, new Graph(dataSet).Fold(nFolds).DebugDisplay);
    }

    static readonly object[] GraphTestData = new[]
    {
      new object[] { FoldedOnce, 0, Data.Day13ExampleData },
      new object[] { FoldedTwice, 1, Data.Day13ExampleData },
      new object[] { FoldedThrice, 2, Data.Day13ExampleData },
    };

    const string FoldedOnce = @"...#..#..#.
....#......
...........
#..........
...#....#.#
...........
...........
...........
...........
...........
.#....#.##.
....#......
......#...#
#..........
#.#........
";

    const string FoldedTwice = @"#.##..#..#.
#...#......
......#...#
#...#......
.#.#..#.###
...........
...........
";

    const string FoldedThrice = @"#####
#...#
#...#
#...#
#####
.....
.....
";
  }
}
