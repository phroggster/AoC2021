using AoC2021.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day18
{
  /// <summary>A numerical value element of a <see cref="SnailFish"/>.</summary>
  public class SnailfishToken
  {
    /// <summary>The numerical value that the <see cref="SnailfishToken"/> represents.</summary>
    public int Value { get; set; }

    /// <summary>The number of elements wrapping the token.</summary>
    public int Depth { get; set; }
    /// <summary>The index at the end of the string that the token represents.</summary>
    public int EndIndex => StartIndex + Length;
    /// <summary>The length of the string that the token represents.</summary>
    public int Length { get; set; }
    /// <summary>The index of the start of the string that the token represents.</summary>
    public int StartIndex { get; set; }

    public SnailfishToken(int startIdx, int len, int value, int depth)
    {
      StartIndex = startIdx;
      Length = len;
      Value = value;
      Depth = depth;
    }

    public override string ToString() => $"{Value}";
  }

  public class SnailFish
  {
    public SnailFish(string value)
    {
      Value = value;
      _deconstruction = Deconstruct(value)
        .ToList()
        .AsReadOnly();
    }


    public bool CanExplode => _deconstruction.Any(dc => dc.Depth >= 5);

    public bool CanSplit => _deconstruction.Any(dc => dc.Value >= 10);

    public bool CanReduce => CanExplode || CanSplit;

    public string Value { get; init; }

    readonly ReadOnlyCollection<SnailfishToken> _deconstruction;


    public static SnailFish operator +(SnailFish lhs, SnailFish rhs)
    {
      _ = lhs ?? throw new ArgumentNullException(nameof(lhs));
      _ = rhs ?? throw new ArgumentNullException(nameof(rhs));

      if (string.IsNullOrWhiteSpace(lhs.Value))
        throw new ArgumentException($"{nameof(lhs)} parameter has an empty value.", nameof(lhs));
      else if (string.IsNullOrWhiteSpace(rhs.Value))
        throw new ArgumentException($"{nameof(rhs)} parameter has an empty value.", nameof(rhs));

      return new SnailFish($"[{lhs.Value},{rhs.Value}]")
        .Reduce();
    }

    [Pure]
    public SnailFish Reduce(int nMaxIterations = int.MaxValue)
    {
      var result = new SnailFish(Value);

      for (int n = 0; result.CanReduce && n < nMaxIterations; ++n)
      {
        if (result.CanExplode)
          result = result.Explode();
        else if (result.CanSplit)
          result = result.Split();
      }

      return result;
    }

    [Pure]
    public SnailFish Explode()
    {
      if (!CanExplode)
        return this;

      var buf = new StringBuilder(Value);
      var lToken = _deconstruction
        .First(x => x.Depth >= 5);
      var rToken = _deconstruction
        .First(x => lToken.EndIndex < x.StartIndex);
      var lNeighbor = _deconstruction
        .LastOrDefault(v => v.EndIndex < lToken.StartIndex);
      var rNeighbor = _deconstruction
        .FirstOrDefault(v => rToken.EndIndex < v.StartIndex);

      // The pair's right value is added to the first regular
      // number to the right of the exploding pair(if any).
      if (rNeighbor != null)
      {
        rNeighbor.Value += rToken.Value;
        buf = buf
          .Remove(rNeighbor.StartIndex, rNeighbor.Length)
          .Insert(rNeighbor.StartIndex, rNeighbor.Value.ToString());
      }

      // The entire exploding pair is replaced with the regular number 0.
      buf = buf
        .Remove(lToken.StartIndex - 1, lToken.Length + rToken.Length + 3)
        .Insert(lToken.StartIndex - 1, "0");

      // The pair's left value is added to the first regular
      // number to the left of the exploding pair(if any).
      if (lNeighbor is not null)
      {
        lNeighbor.Value += lToken.Value;
        buf = buf
          .Remove(lNeighbor.StartIndex, lNeighbor.Length)
          .Insert(lNeighbor.StartIndex, lNeighbor.Value.ToString());
      }

      var flattened = buf.ToString();
      if (flattened.Any(c => !(char.IsNumber(c) || c == '[' || c == ',' || c == ']')))
        throw new InvalidOperationException($"Value \"{Value}\" contains invalid characters after Explode.");

      return new SnailFish(flattened);
    }

    [Pure]
    public SnailFish Split()
    {
      if (!CanSplit)
        return this;

      var buf = new StringBuilder(Value);
      var target = _deconstruction
        .First(dc => dc.Value >= 10);
      var lVal = target.Value / 2;
      var rVal = target.Value - lVal;

      buf = buf
        .Remove(target.StartIndex, target.Length)
        .Insert(target.StartIndex, $"[{lVal},{rVal}]");

      var flattened = buf.ToString();
      if (flattened.Any(c => !(char.IsNumber(c) || c == '[' || c == ',' || c == ']')))
        throw new InvalidOperationException($"Value \"{Value}\" contains invalid characters after Split.");

      return new SnailFish(buf.ToString());
    }

    [Pure]
    public int GetMagnitude()
    {
      var buf = new StringBuilder(Value);
      var sf = buf.ToString();
      while (sf.Contains(','))
      {
        var pairs = Deconstruct(sf)
          .ReverseSlidingWindowSets(p => sf[p.Item1.EndIndex] == ',' && p.Item1.EndIndex + 1 == p.Item2.StartIndex);
        foreach (var (left, right) in pairs)
        {
          buf = buf
            .Remove(left.StartIndex - 1, left.Length + right.Length + 3)
            .Insert(left.StartIndex - 1, left.Value * 3 + right.Value * 2);
        }
        sf = buf.ToString();
      }

      if (!int.TryParse(sf, out var result))
        throw new InvalidOperationException($"GetMagnitude(\"{Value}\") result (\"{sf}\") isn't an integral value.");

      return result;
    }


    private static IEnumerable<SnailfishToken> Deconstruct(string snailFish)
    {
      var openStack = new Stack<int>();
      var result = new List<SnailfishToken>();

      for (int n = 0; n < snailFish.Length; n++)
      {
        if (snailFish[n] == ',')
          continue;
        else if (snailFish[n] == '[')
          openStack.Push(n);
        else if (snailFish[n] == ']')
          _ = openStack.Pop();
        else if (!char.IsNumber(snailFish[n]))
          throw new ArgumentException($"Invalid character '{snailFish[n]}' at index {n} of \"{snailFish}\".", nameof(snailFish));
        else
        {
          int nStart = n;
          while (char.IsNumber(snailFish[n + 1]))
          {
            ++n;
          }
          int nLen = n + 1 - nStart;
          result.Add(new SnailfishToken(nStart, nLen, int.Parse(snailFish.Substring(nStart, nLen)), openStack.Count));
        }
      }

      return result;
    }

    [Pure]
    public override string ToString() => Value;
  }

  public static class SnailFishExtensions
  {
    public static SnailFish Sum(this IEnumerable<SnailFish> collection)
    {
      if (collection is null)
        throw new ArgumentNullException(nameof(collection));
      else if (!collection.Any())
        throw new ArgumentException($"The {nameof(collection)} parameter is empty.", nameof(collection));

      var result = collection.First();
      foreach (var sf in collection.Skip(1))
        result += sf;

      return result;
    }
  }
}
