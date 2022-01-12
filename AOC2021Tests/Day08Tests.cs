using AoC2021.Day08;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AoC2021.Tests
{
  [TestFixture]
  public class Day08Tests
  {
    [Test]
    public void TestSevenSegmentUniqueDigitCounter()
    {
      Assert.AreEqual(26, SevenSegments.SummateUniqueDigits(Data.Day08ExampleData));
    }

    [Test]
    public void TestSevenSegmentPart2Solution()
    {
      // The example for day 8 part 2 excludes the first line of input (5353) and
      // shows the rest of the input lines as totalling 61229. 61229 + 5353 == 66582.
      Assert.AreEqual(66582, SevenSegments.DecodeAndSummateOutputs(Data.Day08ExampleData));
    }
  }
}
