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
    TestOf(typeof(ProductExtensions))]
  public class ProductExtTests
  {
    [Test,
      TestCase(0L, new[] { 0, 12, 34, 56, 78 }),
      TestCase(1L, new[] { 1, 1, 1, 1 }),
      TestCase(-69L, new[] { 69, -1, -1, -1 }),
      TestOf(nameof(ProductExtensions.Product))]
    public void TestIntProductExtension(long expectedValue, int[] values)
    {
      Assert.Multiple(() =>
      {
        Assert.AreEqual(expectedValue, ProductExtensions.Product(values));

        var boxed = new List<IntBox>(values.Select(v => new IntBox(v)));
        Assert.AreEqual(expectedValue, ProductExtensions.Product(boxed, b => b.Value));
      });
    }

    [Test,
      TestOf(nameof(ProductExtensions.Product))]
    public void TestIntProductExtExceptions()
    {
      Assert.Multiple(() =>
      {
        Assert.Throws<ArgumentNullException>(() => ProductExtensions.Product(null as IEnumerable<int>));
        Assert.Throws<ArgumentException>(() => ProductExtensions.Product(Enumerable.Empty<int>()));
        Assert.Throws<OverflowException>(() => ProductExtensions.Product(
          new[] { int.MaxValue, int.MaxValue, 3 }));

        Assert.Throws<ArgumentNullException>(() => ProductExtensions.Product(null as IEnumerable<IntBox>, b => b.Value));
        Assert.Throws<ArgumentNullException>(() => ProductExtensions.Product(new IntBox[] { new() }, null as Func<IntBox, int>));
        Assert.Throws<ArgumentException>(() => ProductExtensions.Product(Enumerable.Empty<IntBox>(), b => b.Value));
      });
    }

    [Test,
      TestCase(0L, new[] { 0L, 12L, 34L, 56L, 78L }),
      TestCase(1L, new[] { 1L, 1L, 1L, 1L }),
      TestCase(-69L, new[] { 69L, -1L, -1L, -1L }),
      TestOf(nameof(ProductExtensions.Product))]
    public void TestLongProductExtension(long expectedValue, long[] values)
    {
      Assert.Multiple(() =>
      {
        Assert.AreEqual(expectedValue, ProductExtensions.Product(values));

        var boxed = new List<LongBox>(values.Select(v => new LongBox(v)));
        Assert.AreEqual(expectedValue, ProductExtensions.Product(boxed, b => b.Value));
      });
    }

    [Test,
      TestOf(nameof(ProductExtensions.Product))]
    public void TestLongProductExtExceptions()
    {
      Assert.Multiple(() =>
      {
        Assert.Throws<ArgumentNullException>(() => ProductExtensions.Product(null as IEnumerable<long>));
        Assert.Throws<ArgumentException>(() => ProductExtensions.Product(Enumerable.Empty<long>()));
        Assert.Throws<OverflowException>(() => ProductExtensions.Product(new[] { long.MaxValue, 2 }));

        Assert.Throws<ArgumentNullException>(() => ProductExtensions.Product(null as IEnumerable<LongBox>, b => b.Value));
        Assert.Throws<ArgumentNullException>(() => ProductExtensions.Product(new LongBox[] { new() }, null as Func<LongBox, long>));
        Assert.Throws<ArgumentException>(() => ProductExtensions.Product(Enumerable.Empty<LongBox>(), b => b.Value));
      });
    }

    class IntBox
    {
      public int Value { get; init; }

      public IntBox() : this(0) { }
      public IntBox(int value)
      {
        Value = value;
      }
    }

    class LongBox
    {
      public long Value { get; init; }

      public LongBox() : this(0) { }
      public LongBox(long value)
      {
        Value = value;
      }
    }
  }
}
