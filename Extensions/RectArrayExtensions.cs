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
    /// <exception cref="ArgumentNullException"/>
    public static bool All<T>(this T[,] source, Func<T, bool> predicate)
    {
      _ = source ?? throw new ArgumentNullException(nameof(source));
      _ = predicate ?? throw new ArgumentNullException(nameof(predicate));
      foreach (var item in source)
      {
        if (predicate(item))
          continue;
        else
          return false;
      }
      return true;
    }

    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="OverflowException"/>
    public static bool Any<T>(this T[,] source)
    {
      _ = source ?? throw new ArgumentNullException(nameof(source));
      return source.Length > 0;
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
    /// <exception cref="ArgumentNullException"/>
    public static bool Any<T>(this T[,] source, Func<T, bool> predicate)
    {
      _ = source ?? throw new ArgumentNullException(nameof(source));
      _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

      foreach (var item in source)
      {
        if (predicate(item))
          return true;
      }

      return false;
    }

    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="OverflowException"/>
    public static int Count<T>(this T[,] source)
    {
      _ = source ?? throw new ArgumentNullException(nameof(source));
      return source.Length;
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
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentException"/>
    public static int Count<T>(this T[,] source, Func<T, bool> predicate)
    {
      _ = source ?? throw new ArgumentNullException(nameof(source));
      _ = predicate ?? throw new ArgumentNullException(nameof(predicate));
      if (!source.Any())
        throw new ArgumentException($"The {nameof(source)} parameter seems empty", nameof(source));

      int result = 0;
      foreach (var item in source)
      {
        if (predicate(item))
          result++;
      }
      return result;
    }

    /// <summary>Convert a row from a rectangular char array into a string.
    /// </summary>
    /// <param name="source">A rectangular array containing the chars of the
    /// output string.</param>
    /// <param name="rowIndex">The 0-based row index of the array to convert.
    /// </param>
    /// <returns>A string representation of the characters from the row of the
    /// array.</returns>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="IndexOutOfRangeException"/>
    public static string RowToString(this char[,] source, int rowIndex)
    {
      var nRows = source?.GetLength(0)
        ?? throw new ArgumentNullException(nameof(source));
      var nCols = source.GetLength(1);
      if (nCols < 1 || nRows < 1)
        throw new ArgumentException($"The {nameof(source)} parameter seems empty", nameof(source));
      else if (rowIndex < 0 || rowIndex >= nRows)
        throw new IndexOutOfRangeException(nameof(rowIndex));

      var row = new char[nCols];
      // TODO: Buffer.BlockCopy(source, rowLen * rowIndex, row, 0, rowLen);
      // But this seems to have problems?
      for (int n = 0; n < nCols; n++)
      {
        row[n] = source[rowIndex, n];
      }

      return new string(row);
    }
  }
}
