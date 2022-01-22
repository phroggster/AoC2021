using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Tests.Extensions
{
  internal static class Utility
  {
    public static void SequenceEquals<T>(
      this IEnumerable<T> expected,
      IEnumerable<T> actual)
    {
      var n = 0;
      var exlen = expected.Count();

      foreach (var item in actual)
      {
        Assert.That(n, Is.LessThan(exlen),
          $"Sequence has more items than expected (received {actual.Count()}, expected {exlen}).");
        var expectation = expected.ElementAt(n);
        Assert.That(expectation, Is.EqualTo(item), $"Unexpected element ({item}) at index {n}; expecting: {expectation}");
        n++;
      }
      Assert.That(n, Is.EqualTo(exlen),
        $"Sequence has fewer items than expected (received {actual.Count()}, expected {exlen}).");
    }
  }
}
