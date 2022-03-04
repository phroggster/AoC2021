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
    TestOf(typeof(BoolExtensions))]
  public class BoolExtTests
  {
    [Test,
      TestCase(8L,
        false, false, false, false, true, false, false, false),
      TestCase(85L,
        false, true, false, true, false, true, false, true),
      TestCase(170L,
        true, false, true, false, true, false, true, false),
      TestCase(long.MaxValue,
        false, true, true, true, true, true, true, true,
        true, true, true, true, true, true, true, true,
        true, true, true, true, true, true, true, true,
        true, true, true, true, true, true, true, true,
        true, true, true, true, true, true, true, true,
        true, true, true, true, true, true, true, true,
        true, true, true, true, true, true, true, true,
        true, true, true, true, true, true, true, true),
      TestOf(nameof(BoolExtensions.ToLong))]
    public void TestBoolToLongExtension(long expectedValue, params bool[] values)
    {
      Assert.AreEqual(expectedValue, BoolExtensions.ToLong(values));
    }

    [Test,
      TestCase(8,
        false, false, false, false, true, false, false, false),
      TestCase(85,
        false, true, false, true, false, true, false, true),
      TestCase(170,
        true, false, true, false, true, false, true, false),
      TestCase(int.MaxValue,
        false, true, true, true, true, true, true, true,
        true, true, true, true, true, true, true, true,
        true, true, true, true, true, true, true, true,
        true, true, true, true, true, true, true, true),
      TestOf(nameof(BoolExtensions.ToInt))]
    public void TestBoolToIntExtension(int expectedValue, params bool[] values)
    {
      Assert.AreEqual(expectedValue, BoolExtensions.ToInt(values));
    }

    [Test,
      TestOf(nameof(BoolExtensions.ToLong))]
    public void TestBoolToLongExceptions()
    {
      Assert.Multiple(() =>
      {
        Assert.Throws<ArgumentNullException>(() => BoolExtensions.ToLong(null));
        Assert.Throws<ArgumentException>(() => BoolExtensions.ToLong(Enumerable.Empty<bool>()));

        const int sz = 64;
        if (C0Long[0] != true || C0Long.Length < sz
          || 4611686018427387904L != BoolExtensions.ToLong(C0Long[1..sz]))
          Assert.Inconclusive();
        else
          Assert.Throws<ArgumentException>(() => BoolExtensions.ToLong(C0Long));
      });
    }

    [Test,
      TestOf(nameof(BoolExtensions.ToInt))]
    public void TestBoolToIntExceptions()
    {
      Assert.Multiple(() =>
      {
        Assert.Throws<ArgumentNullException>(() => BoolExtensions.ToInt(null));
        Assert.Throws<ArgumentException>(() => BoolExtensions.ToInt(Enumerable.Empty<bool>()));

        const int sz = 32;
        if (C0Long[0] != true || C0Long.Length < sz
          || 1073741824 != BoolExtensions.ToInt(C0Long[1..sz]))
        {
          Assert.Inconclusive();
        }
        else
        {
          Assert.Throws<ArgumentException>(() => BoolExtensions.ToInt(C0Long));
          Assert.Throws<ArgumentException>(() => BoolExtensions.ToInt(C0Long.Take(sz)));
        }
      });
    }

    // 64 bits (long) = 0xC000_0000_0000_0000 = -4,611,686,018,427,387,904
    // 32 bits (int)  = 0xC000_0000           = -1,073,741,824
    static readonly bool[] C0Long = new[]
    {
      true, true, false, false, false, false, false, false,
      false, false, false, false, false, false, false, false,
      false, false, false, false, false, false, false, false,
      false, false, false, false, false, false, false, false,
      false, false, false, false, false, false, false, false,
      false, false, false, false, false, false, false, false,
      false, false, false, false, false, false, false, false,
      false, false, false, false, false, false, false, false
    };
  }
}
