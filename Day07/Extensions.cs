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
  }
}
