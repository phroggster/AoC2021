using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day04
{
  [DebuggerDisplay("{DbgDisplay,nq}")]
  public class Tile
  {
    public bool Marked
    {
      get => _marked;
      set
      {
        if (_marked != value)
        {
          _marked = value;
          MarkedChanged?.Invoke(this, EventArgs.Empty);
        }
      }
    }
    public event EventHandler? MarkedChanged;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private bool _marked = false;

    public int Value { get; init; }

    public Tile(int value)
    {
      Value = value;
    }


    private string DbgDisplay
    {
      get
      {
        return $"Tile {Value}: {(_marked ? "marked" : "not marked")}";
      }
    }
  }

  public class Board
  {
    public Board(IEnumerable<int> values)
    {
      _tiles = new ObservableCollection<Tile>(
        values
          .Select(n => new Tile(n))
          .ToList()
      );
    }

    public Board(string values)
    {
      List<Tile> tls = new List<Tile>(25);

      foreach (var v in values.Split(',', '\r', '\n', ' '))
      {
        if (string.IsNullOrEmpty(v))
          continue;

        if (int.TryParse(v, out int result))
        {
          tls.Add(new Tile(result));
          if (tls.Count >= 25)
            break;
        }
      }
      _tiles = new ObservableCollection<Tile>(tls);
    }

    public Board(params int[] values)
    {
      _tiles = new ObservableCollection<Tile>(values.Select(n => new Tile(n)));
    }

    public event EventHandler? HasWon;
    public bool IsWinner
    {
      get => _isWinner;
      set
      {
        if (_isWinner != value)
        {
          _isWinner = value;
          if (value)
            HasWon?.Invoke(this, EventArgs.Empty);
        }
      }
    }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private bool _isWinner = false;

    public IReadOnlyList<Tile> Tiles => _tiles;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private ObservableCollection<Tile> _tiles;





    /// <summary>Mark a tile by number.</summary>
    /// <param name="digit">The tile value to mark.</param>
    /// <returns><c>true</c> if the marked digit has resulted in a win; <c>false</c> otherwise.</returns>
    public bool MarkDigit(int digit)
    {
      var t = _tiles.Where(t => t.Value == digit).FirstOrDefault();
      if (t is null || t.Marked)
        return false;

      t.Marked = true;
      IsWinner = CheckWinConditions();

      return IsWinner;
    }

    private bool CheckWinConditions()
    {
      Debug.Assert(_tiles != null);
      Debug.Assert(_tiles.Count == 25);

      // horizontal rows
      return (_tiles[00].Marked && _tiles[01].Marked && _tiles[02].Marked && _tiles[03].Marked && _tiles[04].Marked)
          || (_tiles[05].Marked && _tiles[06].Marked && _tiles[07].Marked && _tiles[08].Marked && _tiles[09].Marked)
          || (_tiles[10].Marked && _tiles[11].Marked && _tiles[12].Marked && _tiles[13].Marked && _tiles[14].Marked)
          || (_tiles[15].Marked && _tiles[16].Marked && _tiles[17].Marked && _tiles[18].Marked && _tiles[19].Marked)
          || (_tiles[20].Marked && _tiles[21].Marked && _tiles[22].Marked && _tiles[23].Marked && _tiles[24].Marked)
      // vertical columns
          || (_tiles[00].Marked && _tiles[05].Marked && _tiles[10].Marked && _tiles[15].Marked && _tiles[20].Marked)
          || (_tiles[01].Marked && _tiles[06].Marked && _tiles[11].Marked && _tiles[16].Marked && _tiles[21].Marked)
          || (_tiles[02].Marked && _tiles[07].Marked && _tiles[12].Marked && _tiles[17].Marked && _tiles[22].Marked)
          || (_tiles[03].Marked && _tiles[08].Marked && _tiles[13].Marked && _tiles[18].Marked && _tiles[23].Marked)
          || (_tiles[04].Marked && _tiles[09].Marked && _tiles[14].Marked && _tiles[19].Marked && _tiles[24].Marked);
    }
  }
}
