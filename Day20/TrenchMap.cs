using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GlmSharp;

namespace AoC2021.Day20
{
  public class TrenchMap : HashSet<ivec2>
  {
    static void Main(string[] _)
    {
      Console.WriteLine($"Number Illuminated after 2:  {EnhanceTwiceAndCount(Data.Day20PuzzleInput)}");
      Console.WriteLine($"Number Illuminated after 50: {EnhanceFiftyAndCount(Data.Day20PuzzleInput)}");
    }

    public static int EnhanceTwiceAndCount(string slzdAlgoAndMap)
    {
      return new TrenchMap(slzdAlgoAndMap)
        .Enhance(2)
        .Count;
    }

    public static int EnhanceFiftyAndCount(string slzdAlgoAndMap)
    {
      return new TrenchMap(slzdAlgoAndMap)
        .Enhance(50)
        .Count;
    }


    public List<bool> Algorithm { get; init; }


    public TrenchMap(string serializedAlgoAndImage)
      : this(serializedAlgoAndImage.Split(Environment.NewLine).AsEnumerable())
    {
    }

    public TrenchMap(IEnumerable<string> serializedAlgoAndImageLines)
      : base(serializedAlgoAndImageLines.Skip(2).Select(line => line.Count(c => c == '#')).Sum())
    {
      Algorithm = new(DeserializeRange(serializedAlgoAndImageLines.First()));
      Debug.Assert(Algorithm.Count == 512);
      Debug.Assert(string.IsNullOrEmpty(serializedAlgoAndImageLines.Skip(1).First()));

      int y = 0;
      var imgStrs = serializedAlgoAndImageLines.Skip(2);
      foreach (var rowBits in imgStrs.Select(line => DeserializeRange(line).ToList()))
      {
        for (int x = 0; x < rowBits.Count; ++x)
        {
          if (rowBits[x])
            Add(new ivec2(x, y));
        }
        ++y;
      }
    }


    public static bool ApplyAlgorithm(IEnumerable<bool> algorithm, IEnumerable<bool> ninePixels)
    {
      Debug.Assert(algorithm.Count() == 512);
      Debug.Assert(ninePixels.Count() == 9);

      int key = 0;
      int iter = 9;
      foreach (var pixel in ninePixels)
      {
        key |= (pixel ? 1 : 0) << --iter;
      }
      Debug.Assert(key >= 0);
      Debug.Assert(key < 512);

      return algorithm.ElementAt(key);
    }

    public static IEnumerable<bool> DeserializeRange(string data)
    {
      return data
        .ToCharArray()
        .Select(c =>
          c == '#' ? true :
          c == '.' ? false :
          throw new ArgumentOutOfRangeException($"Unknown character '{c}' in argument.", nameof(data)));
    }


    [Pure]
    public IReadOnlyCollection<ivec2> Enhance(int nIterations)
    {
      if (nIterations < 0)
        throw new ArgumentOutOfRangeException(nameof(nIterations));
      else if (nIterations == 0)
        return this;

      HashSet<ivec2> result = new(this);
      var algo = Algorithm;
      bool defFill = false;
      for (int nIter = 0; nIter < nIterations; ++nIter)
      {
        ivec2 topLeft = new ivec2(result.Select(p => p.x).Min(), result.Select(p => p.y).Min());
        ivec2 btmRight = new ivec2(result.Select(p => p.x).Max(), result.Select(p => p.y).Max());
        HashSet<ivec2> buf = new();

        for (int y = topLeft.y - 1; y <= btmRight.y + 1; ++y)
        {
          for (int x = topLeft.x - 1; x <= btmRight.x + 1; ++x)
          {
            var nbrCoords = new ivec2[]
            {
              new(x - 1, y - 1),
              new(x    , y - 1),
              new(x + 1, y - 1),

              new(x - 1, y),
              new(x    , y),
              new(x + 1, y),

              new(x - 1, y + 1),
              new(x    , y + 1),
              new(x + 1, y + 1),
            };

            var grid = Enumerable.Repeat(defFill, 9).ToList();
            for (int n = 0; n < grid.Count; ++n)
            {
              if ((nbrCoords[n] >= topLeft).All && (nbrCoords[n] <= btmRight).All)
              {
                grid[n] = result.Contains(nbrCoords[n]);
              }
            }

            if (ApplyAlgorithm(algo, grid))
              buf.Add(new(x, y));
          }
        }

        result = buf;
        defFill = ApplyAlgorithm(algo, Enumerable.Repeat(defFill, 9));
      }

      return result;
    }
  }
}
