using AoC2021.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day18
{
  public static class SnailfishHomework
  {
    static void Main(string[] _)
    {
      // What is the magnitude of the final sum?
      Console.WriteLine($"Total Magnitude: {SummateAndGetMagnitude(Homework)}");

      // What is the largest magnitude when any two distinct
      // snailfish numbers are added together?
      Console.WriteLine($"Peak Pair Magn:  {MaxSummationOfAnyTwo(Homework)}");
    }

    public static int SummateAndGetMagnitude(IEnumerable<string> inputData)
    {
      // Add up all of the snailfish numbers from the homework assignment in
      // the order they appear. What is the magnitude of the final sum?
      return inputData
        .Select(str => new SnailFish(str))
        .Sum()
        .GetMagnitude();
    }

    public static int MaxSummationOfAnyTwo(IEnumerable<string> inputData)
    {
      // What is the largest magnitude of any sum of two
      // different snailfish numbers from the homework assignment?

      // Note that snailfish addition is not commutative -
      // that is, x + y and y + x can produce different results.
      int bestResult = 0;
      var fishies = inputData
        .Select(str => new SnailFish(str))
        .ToList()
        .AsReadOnly();

      for (int j = 0; j < fishies.Count; ++j)
      {
        for (int k = 0; k < fishies.Count; ++k)
        {
          if (j == k)
            continue;
          bestResult = Math.Max(bestResult, (fishies[j] + fishies[k]).GetMagnitude());
        }
      }

      return bestResult;
    }

    public static readonly string[] Homework = new[]
    {
      "[[[[2,8],[4,6]],[[2,4],[9,4]]],[[[0,6],[4,6]],[1,6]]]",
      "[7,[[5,7],1]]",
      "[[[[8,8],7],5],[[[5,6],1],6]]",
      "[[[8,5],[[0,0],[4,9]]],[2,8]]",
      "[7,[[5,2],[[3,0],[7,7]]]]",
      "[[6,[6,8]],[3,[5,2]]]",
      "[6,[[[8,9],[9,9]],[3,8]]]",
      "[[[1,[0,2]],[7,[3,0]]],8]",
      "[[9,6],6]",
      "[[[2,3],1],[9,[3,7]]]",
      "[5,[[[5,8],3],9]]",
      "[[[[8,8],3],[2,2]],[2,3]]",
      "[[[4,9],3],[[[7,3],8],5]]",
      "[[[3,5],[3,7]],[[[9,7],9],[9,[7,8]]]]",
      "[[7,1],8]",
      "[0,[[[6,8],[1,1]],[1,[5,8]]]]",
      "[[[[2,2],[9,5]],[0,[1,0]]],[4,[[2,4],4]]]",
      "[[[[2,5],[7,3]],[7,6]],[[6,[4,4]],[3,8]]]",
      "[[3,[[7,9],2]],[[0,[4,4]],[[6,9],9]]]",
      "[[[7,7],[[1,4],[1,6]]],[7,[[6,3],6]]]",
      "[[0,8],[[[1,6],2],4]]",
      "[[0,[[2,7],[0,4]]],[[[3,8],[7,7]],5]]",
      "[[[[9,9],[1,3]],[9,[4,3]]],[[[3,4],[6,4]],1]]",
      "[[[9,[0,9]],[2,[7,6]]],[2,[[1,9],[3,3]]]]",
      "[[4,[5,6]],[[[1,5],6],[[1,5],[5,2]]]]",
      "[1,[[3,[2,1]],5]]",
      "[[4,[3,8]],[3,[6,3]]]",
      "[[7,1],[[3,[6,0]],[5,[1,1]]]]",
      "[[8,7],[[[0,1],[2,6]],[5,[4,7]]]]",
      "[9,[[[1,6],[8,9]],[6,6]]]",
      "[4,9]",
      "[[[[0,8],[8,5]],9],[7,[1,3]]]",
      "[[[[8,5],0],[[4,6],4]],[8,4]]",
      "[[[[8,9],8],[[3,1],[7,6]]],2]",
      "[[[[6,3],0],[2,[4,8]]],[[[0,3],[3,5]],4]]",
      "[0,[[9,[0,6]],5]]",
      "[[[[1,9],[2,7]],[[4,0],[9,9]]],[[8,[3,6]],[3,4]]]",
      "[[[[0,7],[8,4]],1],[[8,3],[[3,5],[8,0]]]]",
      "[[[[3,5],4],[0,9]],[[[1,7],5],[9,[8,0]]]]",
      "[[[8,[6,8]],[[3,7],[0,8]]],[[[5,2],[1,7]],[9,5]]]",
      "[[[[5,1],[0,7]],4],[0,4]]",
      "[[[[9,8],[3,9]],[[0,6],3]],[[[9,1],[8,7]],2]]",
      "[[9,[[0,3],6]],[[3,4],[[8,9],5]]]",
      "[[1,[1,8]],[[6,[4,2]],1]]",
      "[7,[[1,[5,2]],[[9,7],0]]]",
      "[0,[8,6]]",
      "[1,4]",
      "[[8,[4,1]],[[[4,0],[0,0]],[7,[3,4]]]]",
      "[2,[[1,[1,8]],[[3,4],1]]]",
      "[[8,[[1,2],[3,1]]],[[[4,4],[7,9]],1]]",
      "[[4,[0,[6,4]]],[9,[0,[1,2]]]]",
      "[[6,[3,1]],[[7,8],[8,[2,5]]]]",
      "[[[2,[3,3]],[[6,4],[9,4]]],[[[1,5],[7,4]],[0,6]]]",
      "[[[[8,0],3],[[4,0],3]],[[7,5],4]]",
      "[[[2,[4,3]],[[2,1],5]],1]",
      "[[[8,1],[0,4]],[9,[[1,4],[9,0]]]]",
      "[[[5,0],[[7,7],9]],[[6,[6,2]],7]]",
      "[[[[5,9],0],[[4,6],[3,8]]],[6,[6,5]]]",
      "[[[6,[7,8]],[5,3]],[[3,[6,5]],[[8,7],[4,7]]]]",
      "[[9,[[8,7],4]],[[[6,3],0],[[2,3],[5,9]]]]",
      "[[[[1,8],6],1],[[[7,8],4],[7,2]]]",
      "[[[[7,1],[6,2]],[[7,8],2]],0]",
      "[[[4,5],[0,3]],[[2,4],1]]",
      "[[[9,1],7],[[[8,8],[0,7]],[8,0]]]",
      "[[5,[[7,5],[7,5]]],[3,[4,8]]]",
      "[[7,[1,0]],[[3,[1,5]],0]]",
      "[[[5,1],[[5,2],[7,3]]],[[7,[3,9]],9]]",
      "[5,[1,[[9,9],[3,0]]]]",
      "[[2,0],[9,[6,[3,3]]]]",
      "[[[[0,4],[4,8]],[[1,9],[5,8]]],[[[7,0],5],[5,1]]]",
      "[[[[1,5],[9,2]],[6,[3,6]]],[4,[1,[1,5]]]]",
      "[[[[1,4],[4,6]],[[5,5],[3,5]]],[[[7,1],4],[[0,7],4]]]",
      "[[6,[3,5]],1]",
      "[8,[[1,[0,7]],[[2,5],6]]]",
      "[[[[1,6],3],[[9,7],9]],[[7,8],3]]",
      "[[[[9,9],[2,0]],0],[1,4]]",
      "[[[[1,3],[5,1]],[[0,4],2]],0]",
      "[[3,2],[7,[[9,3],8]]]",
      "[[9,0],[4,[[8,7],[5,5]]]]",
      "[[[[7,4],8],[[4,4],1]],9]",
      "[[9,[[7,9],1]],[[[6,5],7],[[2,5],2]]]",
      "[7,2]",
      "[[[6,6],[[9,4],4]],6]",
      "[[1,[[5,0],3]],[5,[4,4]]]",
      "[[[3,2],[[4,6],6]],[[3,[9,5]],[[0,2],[4,6]]]]",
      "[5,[[0,[3,0]],[7,[7,9]]]]",
      "[[[[0,4],[1,5]],4],[8,[[4,7],8]]]",
      "[[[[9,1],0],0],4]",
      "[[[[8,4],[4,2]],[9,[1,7]]],[6,3]]",
      "[2,[[[8,3],2],[[3,1],8]]]",
      "[[[[9,0],[7,8]],[[2,7],[0,3]]],[[[8,5],3],[9,[6,8]]]]",
      "[[[[8,9],[9,1]],[4,[0,1]]],[[[7,8],2],2]]",
      "[[[[2,2],[4,1]],[2,[2,8]]],[[[6,5],1],9]]",
      "[[[[3,0],7],7],[[[9,3],7],4]]",
      "[[[[7,5],1],3],[[[0,7],7],[[2,6],[9,9]]]]",
      "[[[[5,2],8],[9,[8,8]]],[2,[[0,8],[5,6]]]]",
      "[[[[7,7],[1,2]],[6,6]],[8,[5,8]]]",
      "[[7,[4,[8,9]]],[[4,[7,2]],8]]",
      "[[[6,4],[7,7]],[[[3,7],0],[0,1]]]",
      "[[1,[5,9]],[8,[4,6]]]"
    };
  }
}
