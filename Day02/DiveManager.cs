using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day02
{
  public static class DiveManager
  {
    static void Main(string[] args)
    {
      Console.WriteLine($"Hpos * VPos simple: {CalculateSimple(Data.Day02Data)}");
      Console.WriteLine($"Hpos * VPos aimed:  {CalculateWithAim(Data.Day02Data)}");
    }

    public static int CalculateSimple(IEnumerable<Instruction> instructions)
    {
      if (instructions is null || !instructions.Any()) throw new ArgumentException("Argument doesn't appear to be valid.", nameof(instructions));

      int hpos = 0, vpos = 0;

      foreach (var i in instructions)
      {
        switch (i.Direction)
        {
          case Direction.down:
            vpos += i.Amount;
            break;
          case Direction.up:
            vpos -= i.Amount;
            break;
          case Direction.forward:
            hpos += i.Amount;
            break;


          default:
          case Direction.Invalid:
            throw new ArgumentException("Invalid direction.", nameof(instructions));
        }
      }
      return hpos * vpos;
    }

    public static int CalculateWithAim(IEnumerable<Instruction> instructions)
    {
      if (instructions is null || !instructions.Any()) throw new ArgumentException("Argument doesn't appear to be valid.", nameof(instructions));

      int hpos = 0, vpos = 0, aim = 0;

      foreach (var i in instructions)
      {
        switch (i.Direction)
        {
          case Direction.down:
            aim += i.Amount;
            break;
          case Direction.up:
            aim -= i.Amount;
            break;
          case Direction.forward:
            hpos += i.Amount;
            vpos += aim * i.Amount;
            break;


          default:
          case Direction.Invalid:
            throw new ArgumentException("Invalid direction.", nameof(instructions));
        }
      }
      return hpos * vpos;
    }
  }
}
