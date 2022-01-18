using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Extensions
{
  public static class RectArrayExtensions
  {
    /// <summary>Determines whether all elements of a rectangular array satisfy
    /// a condition.</summary>
    /// <typeparam name="T">The type of the elements of
    /// <paramref name="source"/>.</typeparam>
    /// <param name="source">A rectangular array that contians the elements to
    /// apply the <paramref name="predicate"/> to.</param>
    /// <param name="predicate">A function to test each element for a
    /// condition.</param>
    /// <returns><see langword="true"/> if every element of the
    /// <paramref name="source"/> array passes the test in the specified
    /// <paramref name="predicate"/>, or if the sequence is empty; otherwise,
    /// <see langword="false"/>.</returns>
    public static bool All<T>(this T[,] source, Func<T, bool> predicate)
    {
      foreach (var item in source)
      {
        if (predicate(item))
          continue;
        else
          return false;
      }
      return true;
    }

    /// <summary>Determines whether any element of a rectangular array
    /// satisfies a condition.</summary>
    /// <typeparam name="T">The type of the elements of
    /// <paramref name="source"/>.</typeparam>
    /// <param name="source">The rectangular array whose elements to apply the
    /// <paramref name="predicate"/> to.</param>
    /// <param name="predicate">A function to test each element for a
    /// condition.</param>
    /// <returns><see langword="true"/> if the source sequence is not empty and
    /// at least one of its elements passes the test in the specified
    /// <paramref name="predicate"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Any<T>(this T[,] source, Func<T, bool> predicate)
    {
      foreach (var item in source)
      {
        if (predicate(item))
          return true;
      }

      return false;
    }

    /// <summary>Returns the number of elements in a rectangular array that
    /// satisfy a condition.</summary>
    /// <typeparam name="T">The type of the elements of
    /// <paramref name="source"/>.</typeparam>
    /// <param name="source">A rectangular array containing the elements to be
    /// counted.</param>
    /// <param name="predicate">A function to test each element for a
    /// condition.</param>
    /// <returns>The number of elements in the rectangular array that satisfy
    /// the condition in <paramref name="predicate"/>.</returns>
    public static int Count<T>(this T[,] source, Func<T, bool> predicate)
    {
      int result = 0;
      foreach (var item in source)
      {
        if (predicate(item))
          result++;
      }
      return result;
    }
  }
}
