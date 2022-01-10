using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day07
{
  public static class Extensions
  {
    /// <summary>Returns the triangular number at the specified <paramref name="index"/>
    /// in the series.</summary>
    /// <param name="index">The index of the triangular number to retrieve.</param>
    /// <returns>The triangular number at the specified <paramref name="index"/>.</returns>
    /// <remarks><para>This code will invert any negative parameters, treating it as if it
    /// was positive.</para>
    /// <para>The triangular numbers, otherwise known as OEIS A000217, is a set of
    /// integers where each value is the sum of all previous integers plus the
    /// next-larger integer.</para><para>
    /// results = 0->0+0(0), 1->1+0(1), 2->2+1(3), 3->3+3(6), 4->4+6(10), 5->5+10(15), 6->6+15(21), ..., n->(n + GetTriangularNumber(n-1))
    /// </para></remarks>
    public static int GetTriangularNumber(int index)
    {
      // Greetings from The On-Line Encyclopedia of Integer Sequences! https://oeis.org/A000217

      int acc = 0;
      index = Math.Abs(index);

      for (int n = 1; n <= index; n++)
      {
        acc += n;
      }
      return acc;
    }


    /// <summary>Compute the median of a sequence of <see cref="int"/> values.</summary>
    /// <param name="sequence">A sequence of <see cref="int"/> values to calculate the median of.</param>
    /// <returns>The median value of the sequence.</returns>
    /// <remarks><para>The median is the minimum sum of distances of a set.</para>
    /// <para>Arithmetically, this is done as follows:
    /// <list type="number"><item>Sort the set.</item>
    /// <item>Return the value from the middle of the sorted set.</item>
    /// <item>If the set has an even number of items (no direct "middle" value), return the mean
    /// of the two values surrounding the true middle of the sorted set.</item></list></para></remarks>
    public static double Median(this IEnumerable<int> sequence)
    {
      return Median(sequence.OrderBy(n => n));
    }

    private static double Median(this IOrderedEnumerable<int> sorted)
    {
      var nCount = sorted.Count();
      var halfIdx = nCount / 2;

      if (nCount % 2 == 0)
      {
        return (sorted.ElementAt(halfIdx) + sorted.ElementAt(halfIdx - 1)) * 0.5d;
      }
      return sorted.ElementAt(halfIdx);
    }


    /// <summary>Computes the <see cref="int"/> value closest to the median value of a
    /// sequence of <see cref="int"/> values.</summary>
    /// <param name="sequence">A sequence of <see cref="int"/> values to calculate the median of.</param>
    /// <returns>The closest <see cref="int"/> value to the median of the values from
    /// <paramref name="sequence"/>.</returns>
    public static int RoundedMedian(this IEnumerable<int> sequence)
    {
      return (int)Math.Floor(Median(sequence) + 0.5d);
    }
  }
}
