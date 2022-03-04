using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AoC2021.Day19
{
  public class BeaconScanner
  {
    static void Main(string[] _)
    {
      var bs = new BeaconScanner(Data.ScannerDetails);

      // Assemble the full map of beacons. How many beacons are there?
      Console.WriteLine($"Beacon Count: {bs.BeaconCount}");

      // What is the largest Manhattan distance between any two scanners?
      Console.WriteLine($"Max Distance: {bs.MaxScannerDistance}");
    }


    public int BeaconCount { get; init; }
    public int MaxScannerDistance { get; init; }

    public IReadOnlyCollection<ivec3> BeaconLocations => _beaconLocs;
    private volatile ConcurrentHashSet<ivec3> _beaconLocs;

    public IReadOnlyCollection<ivec3> ScannerLocs => _scannerLocs;
    private volatile ConcurrentHashSet<ivec3> _scannerLocs;


    public BeaconScanner(IEnumerable<string> serializedScanners)
    {
      var nScanners = serializedScanners.Count(scn => scn.StartsWith("--- scanner "));
      var scanners = new List<Scanner>(nScanners);
      var iv3ec = new ivec3EqualityComparer();

      for (int n = 0, nlen = serializedScanners.Count(); n < nlen; n++)
      {
        var scannerLines = serializedScanners
          .Skip(n)
          .TakeWhile(bd => !string.IsNullOrWhiteSpace(bd));

        Debug.Assert(scannerLines.First().StartsWith("--- scanner "));
        Debug.Assert(scannerLines.First().EndsWith(" ---"));

        scanners.Add(new Scanner(scannerLines));

        n += scannerLines.Count();
      }

      // This capacity will likely be slightly large, but that's better than undersize.
      _beaconLocs = new(Environment.ProcessorCount, scanners.Select(s => s.Count - 12).Append(12).Sum(), iv3ec);
      _scannerLocs = new(Environment.ProcessorCount, scanners.Count, iv3ec);

      // Add all solved scanners to the HashSets.
      foreach (var preSolved in scanners.Where(s => s.IsSolved))
      {
        _beaconLocs.AddRange(preSolved.BeaconsInWorldSpace());
        _scannerLocs.Add(preSolved.Location);
      }

      int taskId = 0;
      // Then repeatedly attempt to solve each one.
      while (scanners.Any(s => !s.IsSolved))
      {
        var unsolved = scanners
          .Where(s => !s.IsSolved)
          .ToList();
        int taskIdx = 0;
        var tasks = new Task[unsolved.Count];

        foreach (var scanner in unsolved)
        {
          tasks[taskIdx] = Task.Factory.StartNew((idxObj) =>
          {
            _ = idxObj ?? throw new ArgumentNullException(nameof(idxObj));
            if (idxObj is not (Scanner scnr, int num))
              throw new ArgumentException(nameof(idxObj));

            Thread.CurrentThread.Name = $"TryToOrient_{num:D2}";
            if (scnr.TryToOrient(_beaconLocs, out var scanLoc, out var beacLocs))
            {
              Debug.Assert(scnr.IsSolved);

              _beaconLocs.AddRange(beacLocs);
              _scannerLocs.Add(scanLoc);
            }
          }, (scanner, taskId++), TaskCreationOptions.LongRunning);
          ++taskIdx;
        }

        // Finish the entire generation before moving on to the next one, otherwise we'd quickly bottleneck elsewhere.
        Task.WaitAll(tasks);
      }

      BeaconCount = _beaconLocs.Count;
      MaxScannerDistance = 0;
      foreach (var lhs in _scannerLocs)
      {
        foreach (var rhs in _scannerLocs)
        {
          if (!iv3ec.Equals(lhs, rhs))
          {
            // |x₁-x₂|+|y₁-y₂|+|z₁-z₂| -- https://en.wikipedia.org/wiki/Taxicab_geometry
            MaxScannerDistance = Math.Max(MaxScannerDistance,
              Math.Abs(lhs.x - rhs.x) +
              Math.Abs(lhs.y - rhs.y) +
              Math.Abs(lhs.z - rhs.z));
          }
        }
      }
    }
  }
}
