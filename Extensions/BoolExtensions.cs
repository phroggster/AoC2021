using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Extensions
{
  public static class BoolExtensions
  {
    /// <summary>Converts a sequence of <see cref="bool"/> values to a <see cref="long"/>,
    /// most-significant bit first.</summary>
    /// <param name="sequence">A sequence of <see cref="bool"/> values representing an integral
    /// value, MSB-first.</param>
    /// <returns>A <see cref="long"/> value that repesents the <paramref name="sequence"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentException"/>
    public static long ToLong(this IEnumerable<bool> sequence)
    {
      if (sequence is null)
        throw new ArgumentNullException(nameof(sequence));
      else if (!sequence.Any())
        throw new ArgumentException("Argument is empty.", nameof(sequence));

      long result = 0L;
      int index = sequence.Count();
      if (index == 64 && sequence.First())
        throw new ArgumentException("Argument is negative. This implementation does not support such cases.", nameof(sequence));
      else if (index > 64)
        throw new ArgumentException("Argument exceeds the capacity of the resulting type.", nameof(sequence));

      foreach (var bit in sequence)
      {
        index--;
        if (bit)
          result += 1L << index;
      }
      return result;
    }

    /// <summary>Converts a sequence of <see cref="bool"/> values to an <see cref="int"/>,
    /// most-significant bit first.</summary>
    /// <param name="sequence">A sequence of <see cref="bool"/> values representing an integral
    /// value, MSB-first.</param>
    /// <returns>A <see cref="int"/> value that repesents the <paramref name="sequence"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentException"/>
    public static int ToInt(this IEnumerable<bool> sequence)
    {
      if (sequence is null)
        throw new ArgumentNullException(nameof(sequence));
      else if (!sequence.Any())
        throw new ArgumentException("Argument is empty.", nameof(sequence));

      int result = 0;
      int index = sequence.Count();
      if (index == 32 && sequence.First())
        throw new ArgumentException("Argument is negative. This implementation does not support such cases.", nameof(sequence));
      else if (index > 32)
        throw new ArgumentException("Argument exceeds the capacity of the resulting type.", nameof(sequence));

      foreach (var bit in sequence)
      {
        index--;
        if (bit)
          result += 1 << index;
      }
      return result;
    }
  }
}
