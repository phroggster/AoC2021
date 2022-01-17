using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Extensions
{
  public static class MedianExtensions
  {
    /// <summary>
    /// Compute the median of a sequence of <see cref="int"/> values.
    /// </summary>
    /// <param name="sequence">A sequence of <see cref="int"/> values to
    /// calculate the median of.</param>
    /// <returns>The median value of the sequence.</returns>
    /// <remarks><para>The median is the minimum sum of distances of a set.
    /// This is done as follows:</para><para>
    /// <list type="number"><item>Sort the set.</item>
    /// <item>If the set has an odd number of items, return the value from the
    /// middle of the sorted set.</item>
    /// <item>If the set has an even number of items (no direct "middle"
    /// value), return the mean of the two values surrounding the true middle
    /// of the sorted set.</item></list></para></remarks>
    public static double Median(this IEnumerable<int> sequence)
    {
      if (sequence is null || !sequence.Any())
        return double.NaN;

      var sorted = sequence.OrderBy(n => n);
      var nCount = sorted.Count();
      var halfIdx = nCount / 2;

      if (nCount % 2 == 0)
      {
        return (sorted.ElementAt(halfIdx) + sorted.ElementAt(halfIdx - 1))
          * 0.5d;
      }
      return sorted.ElementAt(halfIdx);
    }

    /// <summary>
    /// Computes the <see cref="int"/> value closest to the median value of a
    /// sequence of <see cref="int"/> values.
    /// </summary>
    /// <param name="sequence">A sequence of <see cref="int"/> values to
    /// calculate the median of.</param>
    /// <returns>The closest <see cref="int"/> value to the median of the
    /// values from the <paramref name="sequence"/>.</returns>
    public static int RoundedMedian(this IEnumerable<int> sequence)
    {
      return (int)Math.Floor(Median(sequence) + 0.5d);
    }
  }
}
