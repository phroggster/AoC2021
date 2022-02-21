using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Extensions
{
  public static class SlidingWindowExtensions
  {
    // "Pairs" implies { 1,2,3,4,5 } => { (1,2), (2,3), (3,4), (4,5) }
    // "Sets" implies  { 1,2,3,4,5 } => { (1,2), (3,4) }

    /// <summary>Converts a sequence of <typeparamref name="T"/> values into a
    /// sequence of pairs. Input such as { 1, 2, 3, 4, 5 } would become
    /// { (1,2), (2,3), (3,4), (4,5) }.</summary>
    /// <typeparam name="T">The type of the values in the collection.
    /// </typeparam>
    /// <param name="collection">The sequence of values to pair off.</param>
    /// <returns>The sequence in a sliding group of two members.</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <seealso cref="SlidingWindowSets{T}(IEnumerable{T})"/>
    /// <seealso cref="SlidingWindowTriplets{T}(IEnumerable{T})"/>
    public static IEnumerable<(T left, T right)> SlidingWindowPairs<T>(
      this IEnumerable<T> collection)
    {
      _ = collection ?? throw new ArgumentNullException(nameof(collection));

      return SlidingWindowPairs(collection, p => true);
    }

    public static IEnumerable<(T left, T right)> SlidingWindowPairs<T>(
      this IEnumerable<T> collection,
      Func<(T, T), bool> predicate)
    {
      _ = collection ?? throw new ArgumentNullException(nameof(collection));

      if (!collection.Any() || collection.Count() < 2)
        return Enumerable.Empty<(T, T)>();

      return collection
        .Zip(collection.Skip(1))
        .Where(lr => predicate(lr));
    }


    /// <summary>Converts a sequence of <typeparamref name="T"/> values into a
    /// sequence of pairs, but returning each value only once. Input such as
    /// { 1, 2, 3, 4, 5 } would become { (1,2), (3,4) }.</summary>
    /// <typeparam name="T">The type of the values in the sequence.
    /// </typeparam>
    /// <param name="sequence">The sequence of values to pair off.</param>
    /// <returns>A sequence of pairs that only include each value once.
    /// </returns>
    /// <exception cref="ArgumentNullException"/>
    /// <seealso cref="SlidingWindowPairs{T}(IEnumerable{T})"/>
    /// <seealso cref="SlidingWindowSets{T}(IEnumerable{T}, Func{(T, T), bool})"/>
    public static IEnumerable<(T left, T right)> SlidingWindowSets<T>(
      this IEnumerable<T> sequence)
    {
      _ = sequence ?? throw new ArgumentNullException(nameof(sequence));

      return SlidingWindowSets(sequence, p => true);
    }

    /// <summary>Converts a sequence of <typeparamref name="T"/> values into a
    /// sequence of pairs, but returning each value only once. Input such as
    /// { 1, 2, 3, 4, 5 } would become { (1,2), (3,4) }.</summary>
    /// <typeparam name="T">The type of the values in the sequence.
    /// </typeparam>
    /// <param name="sequence">The sequence of values to pair off.</param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"/>
    /// <seealso cref="SlidingWindowPairs{T}(IEnumerable{T})"/>
    /// <seealso cref="SlidingWindowSets{T}(IEnumerable{T})"/>
    public static IEnumerable<(T left, T right)> SlidingWindowSets<T>(
      this IEnumerable<T> sequence,
      Func<(T, T), bool> predicate)
    {
      _ = sequence ?? throw new ArgumentNullException(nameof(sequence));
      _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

      if (sequence.Any())
      {
        for (int n = 0; n >= 0 && n + 1 < sequence.Count(); ++n)
        {
          if (predicate((sequence.ElementAt(n), sequence.ElementAt(n + 1))))
          {
            yield return (sequence.ElementAt(n), sequence.ElementAt(n + 1));
            ++n;
          }
        }
      }
    }


    /// <summary>Converts a sequence of <typeparamref name="T"/> values into a
    /// sequence of pairs, but returning each value only once and in reverse
    /// order. Input such as { 1, 2, 3, 4, 5 } would become { (4,5), (2,3) }.
    /// </summary>
    /// <typeparam name="T">The type of the values in the sequence.
    /// </typeparam>
    /// <param name="sequence">The sequence of values to pair off in reverse
    /// order.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"/>
    /// <seealso cref="ReverseSlidingWindowSets{T}(IEnumerable{T}, Func{(T, T), bool})"/>
    /// <seealso cref="SlidingWindowPairs{T}(IEnumerable{T})"/>
    /// <seealso cref="SlidingWindowSets{T}(IEnumerable{T})"/>
    public static IEnumerable<(T left, T right)> ReverseSlidingWindowSets<T>(
      this IEnumerable<T> sequence)
    {
      _ = sequence ?? throw new ArgumentNullException(nameof(sequence));

      return ReverseSlidingWindowSets(sequence, p => true);
    }

    /// <summary>Converts a sequence of <typeparamref name="T"/> values into a
    /// sequence of pairs from right to left, but returning each value only
    /// once. Input such as { 1, 2, 3, 4, 5 } would become { (4,5), (2,3) }.
    /// </summary>
    /// <typeparam name="T">The type of the values in the sequence.
    /// </typeparam>
    /// <param name="sequence">The sequence of values to pair off in reverse.
    /// </param>
    /// <param name="predicate"></param>
    /// <exception cref="ArgumentNullException"/>
    /// <seealso cref="ReverseSlidingWindowSets{T}(IEnumerable{T})"/>
    /// <seealso cref="SlidingWindowSets{T}(IEnumerable{T}, Func{(T, T), bool})"/>
    public static IEnumerable<(T left, T right)> ReverseSlidingWindowSets<T>(
      this IEnumerable<T> sequence,
      Func<(T, T), bool> predicate)
    {
      _ = sequence ?? throw new ArgumentNullException(nameof(sequence));
      _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

      if (sequence.Any())
      {
        var rseq = sequence.Reverse();

        for (int n = 0; n >= 0 && n + 1 < rseq.Count(); ++n)
        {
          if (predicate((rseq.ElementAt(n + 1), rseq.ElementAt(n))))
          {
            yield return (rseq.ElementAt(n + 1), rseq.ElementAt(n));
            ++n;
          }
        }
      }
    }


    /// <summary>Converts a sequence of <typeparamref name="T"/> values into a
    /// sequence of triplets. Input such as { 1, 2, 3 } would become
    /// { (1,2,3) }.</summary>
    /// <typeparam name="T">The type of the values in the collection.
    /// </typeparam>
    /// <param name="collection">The sequence of values to triplet off.</param>
    /// <returns>The sequence in a sliding group of three members.</returns>
    /// <seealso cref="SlidingWindowPairs{T}(IEnumerable{T})"/>
    /// <seealso cref="SlidingWindowSets{T}(IEnumerable{T})"/>
    public static IEnumerable<(T left, T middle, T right)>
      SlidingWindowTriplets<T>(this IEnumerable<T> collection)
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
