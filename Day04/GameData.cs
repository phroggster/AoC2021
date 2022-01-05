using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day04
{
  public class GameData
  {
    public GameData(IEnumerable<int> winners, params int[] boards)
    {
      WinningNumbers = new(winners);

      var chunks = Chunk(boards);
      Boards = new List<Board>(chunks.Count());

      foreach (var b in chunks)
      {
        Boards.Add(new(b));
      }
    }


    public List<Board> Boards { get; set; }

    public List<int> WinningNumbers { get; set; }


    public int CalculateWinner(out Board winner)
    {
      for (int n = 0; n < WinningNumbers.Count; n++)
      {
        foreach (var b in Boards)
        {
          b.MarkDigit(WinningNumbers[n]);
        }

        if (n >= 4 && Boards.Any(b => b.IsWinner == true))
        {
          var wnr = Boards.Where(b => b.IsWinner == true).FirstOrDefault();
          if (wnr is not null)
          {
            winner = wnr;
            return WinningNumbers[n];
          }
        }
      }

      throw new InvalidOperationException();
    }

    public int CalculateLoser(out Board loser)
    {
      var boards = Boards as IEnumerable<Board> ?? throw new InvalidOperationException();
      Debug.Assert(boards is not null);
      Debug.Assert(boards.Any());

      Board? lsr = null;
      for (int n = 0; n < WinningNumbers.Count; n++)
      {
        foreach (var b in boards)
        {
          b.MarkDigit(WinningNumbers[n]);
        }

        boards = boards.Where(b => !b.IsWinner);
        if (boards.Count() == 1)
        {
          lsr = boards.FirstOrDefault();
          if (lsr is not null)
          {
            while (!lsr.IsWinner)
            {
              lsr.MarkDigit(WinningNumbers[++n]);
            }
            loser = lsr;
            return WinningNumbers[n];
          }
        }
      }

      throw new InvalidOperationException();
    }


    private static IEnumerable<IEnumerable<int>> Chunk(IEnumerable<int> source, int chunkSize = 25)
    {
      Debug.Assert(chunkSize > 0);

      while (source.Any())
      {
        yield return source.Take(chunkSize);
        source = source.Skip(chunkSize);
      }
    }
  }
}
