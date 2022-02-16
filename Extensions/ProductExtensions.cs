using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Extensions
{
  public static class ProductExtensions
  {
    /// <summary>Compute the product of a collection of <see cref="int"/> values.</summary>
    /// <param name="source">A collection of <see cref="int"/> values to calculate the product of.
    /// </param>
    /// <returns>The product of the values from the collection.</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="OverflowException"/>
    public static long Product(this IEnumerable<int> source)
    {
      if (source is null)
        throw new ArgumentNullException(nameof(source));
      else if (!source.Any())
        throw new ArgumentException("The collection is empty.", nameof(source));

      long result = 1L;

      foreach (var val in source)
        result = checked(result * val);

      return result;
    }

    /// <summary>Compute the product of a collection of <see cref="long"/> values.</summary>
    /// <param name="collection">A collection of <see cref="long"/> values to calculate the product
    /// of.</param>
    /// <returns>The product of the values from the sequence.</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="OverflowException"/>
    public static long Product(this IEnumerable<long> collection)
    {
      if (collection is null)
        throw new ArgumentNullException(nameof(collection));
      else if (!collection.Any())
        throw new ArgumentException("The collection is empty.", nameof(collection));

      long result = 1L;

      foreach (var val in collection)
        result = checked(result * val);

      return result;
    }


    /// <summary>Compute the product of a collection of <see cref="int"/> values that are obtained
    /// by invoking a transform function on each element of the input sequence.</summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.
    /// </typeparam>
    /// <param name="source">A sequence of elements that are used to calculate a product.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The product of the projected values.</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="OverflowException"/>
    public static long Product<TSource>(this IEnumerable<TSource> source,
      Func<TSource, int> selector)
    {
      if (source is null)
        throw new ArgumentNullException(nameof(source));
      else if (selector is null)
        throw new ArgumentNullException(nameof(selector));
      else if (!source.Any())
        throw new ArgumentException($"The {nameof(source)} parameter is empty.", nameof(source));

      return source
        .Select(selector)
        .Product();
    }

    /// <summary>Compute the product of a collection of <see cref="long"/> values that are obtained
    /// by invoking a transform function on each element of the input sequence.</summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.
    /// </typeparam>
    /// <param name="source">A sequence of elements that are used to calculate a product.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The product of the projected values.</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="OverflowException"/>
    public static long Product<TSource>(this IEnumerable<TSource> source,
      Func<TSource, long> selector)
    {
      if (source is null)
        throw new ArgumentNullException(nameof(source));
      else if (selector is null)
        throw new ArgumentNullException(nameof(selector));
      else if (!source.Any())
        throw new ArgumentException($"The {nameof(source)} parameter is empty.", nameof(source));

      return source
        .Select(selector)
        .Product();
    }
  }
}
