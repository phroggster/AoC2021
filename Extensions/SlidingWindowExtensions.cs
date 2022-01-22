using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Extensions
{
  public static class SlidingWindowExtensions
  {
    /// <summary>Converts a sequence of <typeparamref name="T"/> values into a
    /// sequence of pairs. Input such as { 1, 2, 3 } would become
    /// { (1,2), (2,3) }.</summary>
    /// <typeparam name="T">The type of the values in the collection.
    /// </typeparam>
    /// <param name="collection">The sequence of values to pair off.</param>
    /// <returns>The sequence in a sliding group of two members.</returns>
    /// <exception cref="ArgumentNullException"/>
    public static IEnumerable<(T left, T right)>
      SlidingPairs<T>(this IEnumerable<T> collection)
    {
      _ = collection
        ?? throw new ArgumentNullException(nameof(collection));
      if (!collection.Any() || collection.Count() < 2)
        throw new ArgumentException("The collection is empty.", nameof(collection));

      return collection.Zip(collection.Skip(1));
    }

    /// <summary>Converts a sequence of <typeparamref name="T"/> values into a
    /// sequence of triplets. Input such as { 1, 2, 3 } would become
    /// { (1,2,3) }.</summary>
    /// <typeparam name="T">The type of the values in the collection.
    /// </typeparam>
    /// <param name="collection">The sequence of values to triplet off.</param>
    /// <returns>The sequence in a sliding group of three members.</returns>
    public static IEnumerable<(T left, T middle, T right)>
      SlidingTriplets<T>(this IEnumerable<T> collection)
    {
      _ = collection ?? throw new ArgumentNullException(nameof(collection));
      var len = collection.Count() - 1;
      for (int i = 1; i < len; i++)
      {
        yield return new(
          collection.ElementAt(i - 1),
          collection.ElementAt(i),
          collection.ElementAt(i + 1));
      }
    }
  }
}
