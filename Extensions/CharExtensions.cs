using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Extensions
{
  public static class CharExtensions
  {
    /// <summary>Converts a hexadecimal <see cref="char"/> value into a collection of four
    /// <see cref="bool"/> values representing the bits of the digit.</summary>
    /// <param name="hexDigit">A 0-9 or A-F character representing a hexidecimal digit.</param>
    /// <returns>The binary representation of the bits comprising <paramref name="hexDigit"/>,
    /// most-significant bit first.</returns>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static IEnumerable<bool> HexToBinary(this char hexDigit)
      => char.ToUpper(hexDigit) switch
      {
        '0' => new[] { false, false, false, false },
        '1' => new[] { false, false, false, true },
        '2' => new[] { false, false, true, false },
        '3' => new[] { false, false, true, true },
        '4' => new[] { false, true, false, false },
        '5' => new[] { false, true, false, true },
        '6' => new[] { false, true, true, false },
        '7' => new[] { false, true, true, true },
        '8' => new[] { true, false, false, false },
        '9' => new[] { true, false, false, true },
        'A' => new[] { true, false, true, false },
        'B' => new[] { true, false, true, true },
        'C' => new[] { true, true, false, false },
        'D' => new[] { true, true, false, true },
        'E' => new[] { true, true, true, false },
        'F' => new[] { true, true, true, true },
        _ => throw new ArgumentOutOfRangeException(nameof(hexDigit),
          $"Argument {nameof(hexDigit)} (value '{hexDigit}') is not a valid hexadecimal digit (0-9, a-f, or A-F)."),
      };
  }
}
