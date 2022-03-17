using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;

namespace AoC2021.Day19
{
  /// <summary>A list of beacon locations, with Location and Orientation levers.</summary>
  public class Scanner : List<ivec3>, IEquatable<Scanner>
  {
    // Full-spec
    public Scanner(int id, ivec3 location, EOrientation orientation, IEnumerable<ivec3> vecCollection)
      : base(vecCollection)
    {
      Id = id;
      IsSolved = Id == 0;
      Location = location;
      Orientation = orientation;
    }

    // Deserialization
    public Scanner(IEnumerable<string> serializedScanner)
      : base(serializedScanner.Count() - 1)
    {
      Debug.Assert(serializedScanner.First().StartsWith("--- scanner "));

      Id = serializedScanner
        .First()
        .Split(' ')
        .Skip(2)
        .Where(str => int.TryParse(str, out _))
        .Select(str => int.Parse(str))
        .First();

      IsSolved = Id == 0;
      Orientation = IsSolved ? EOrientation.RightUpIn : EOrientation.Unknown;

      // "Each scanner is capable of detecting all beacons in a large cube centered on the scanner;
      // beacons that are at most 1000 units away from the scanner in each of the three axes
      // (x, y, and z) have their precise position determined relative to the scanner."
      const int limMin = -1000;
      const int limMax = 1000;
      var beaconLocs = serializedScanner
        .Skip(1)
        .Select(line => line
          .Split(',')
          .AsEnumerable()
          .Select(nstr => int.Parse(nstr)))
        .Select(i3 => new ivec3(i3))
        .Where(iv
          => (limMin <= iv.x && iv.x <= limMax)
          && (limMin <= iv.y && iv.y <= limMax)
          && (limMin <= iv.z && iv.z <= limMax));

      AddRange(beaconLocs);
    }



    /// <summary>The unique numeric identifier of the scanner.</summary>
    public int Id { get; set; } = 0;

    /// <summary>The position of the scanner in world-space.</summary>
    public ivec3 Location { get; set; } = ivec3.Zero;

    /// <summary>The orientation or rotation of the scanner.</summary>
    public EOrientation Orientation { get; set; } = EOrientation.Unknown;

    /// <summary>Gets a value indicating whether both the Location and Orientation are known valid.</summary>
    public bool IsSolved { get; set; }


    /// <summary>Gets pinged beacons in world-coordinate space, dependant upon the current
    /// <see cref="Location"/> and <see cref="Orientation"/>.</summary>
    public IEnumerable<ivec3> BeaconsInWorldSpace()
      => RawBeacons().Select(b => LocalToWorld(b, Location, Orientation));

    public bool Equals(Scanner? other) => other?.Id == Id;

    public override bool Equals(object? obj) => Equals(obj as Scanner);

    public override int GetHashCode() => Id.GetHashCode();

    /// <summary>Returns all pinged beacons in their raw coordinate. This will generally be
    /// local-coordinate space, but will be world-coordinate space after the scanner's location and
    /// orientation have been marked as solved.</summary>
    public IEnumerable<ivec3> RawBeacons()
      => this.AsEnumerable();


    public bool TryToOrient(IReadOnlyCollection<ivec3> targetLocations,
      out ivec3 scannerLocation,
      [MaybeNullWhen(false)] out IEnumerable<ivec3> beaconLocations)
    {
      Debug.Assert(!IsSolved);

      const int nRequiredMatches = 12;
      int nMatches = 0;
      var scannerLoc = ivec3.Zero;
      var rawBeacons = this.AsEnumerable()
        .ToList()
        .AsReadOnly();
      IReadOnlyCollection<ivec3>? worldBeacons = null;

      // TODO: less brute-force, more jedi.
      // Random pairs? Level-of-detail abstractions? Deterministic clustering?
      // No, let's just have BeaconScanner massively multithread us.
      foreach (var target in targetLocations)
      {
        foreach (var beacLoc in rawBeacons)
        {
          foreach (var ori in Data.ValidOrientations)
          {
            scannerLoc = target - LocalToWorld(beacLoc, ivec3.Zero, ori);
            worldBeacons = rawBeacons
              .Select(b => LocalToWorld(b, scannerLoc, ori))
              .ToList()
              .AsReadOnly();

            // Compute scoring
            nMatches = worldBeacons
              .Where(beac => targetLocations.Contains(beac))
              .Count();

            // The keyStone beacon used above should *ALWAYS* overlap at the very least. Otherwise, we've screwed something up.
            Debug.Assert(nMatches >= 1);

            if (nMatches >= nRequiredMatches)
            {
              Location = scannerLoc;
              Orientation = ori;
              IsSolved = true;

              beaconLocations = worldBeacons;
              scannerLocation = scannerLoc;

              return true;
            } 
          }
        }
      }

      Debug.Assert(nMatches < nRequiredMatches);
      beaconLocations = null;
      scannerLocation = ivec3.Zero;
      return false;
    }

    static ivec3 LocalToWorld(ivec3 beacon, ivec3 scannerPosition, EOrientation scannerOrientation)
      => scannerOrientation switch
      {
        EOrientation.Default => ivec3.Fma(beacon.xyz, new ivec3(1, 1, 1), scannerPosition),

        EOrientation.RightUpIn => ivec3.Fma(beacon, new ivec3(1, 1, 1), scannerPosition),
        EOrientation.RightDownOut => ivec3.Fma(beacon, new ivec3(1, -1, -1), scannerPosition),
        EOrientation.LeftUpOut => ivec3.Fma(beacon, new ivec3(-1, +1, -1), scannerPosition),
        EOrientation.LeftDownIn => ivec3.Fma(beacon, new ivec3(-1, -1, 1), scannerPosition),
        EOrientation.RightOutUp => ivec3.Fma(beacon.swizzle.xzy, new ivec3(1, 1, -1), scannerPosition),
        EOrientation.RightInDown => ivec3.Fma(beacon.swizzle.xzy, new ivec3(1, -1, 1), scannerPosition),
        EOrientation.LeftInUp => ivec3.Fma(beacon.swizzle.xzy, new ivec3(-1, 1, 1), scannerPosition),
        EOrientation.LeftOutDown => ivec3.Fma(beacon.swizzle.xzy, new ivec3(-1, -1, -1), scannerPosition),

        EOrientation.UpInRight => ivec3.Fma(beacon.swizzle.zxy, new ivec3(1, 1, 1), scannerPosition),
        EOrientation.DownOutRight => ivec3.Fma(beacon.swizzle.zxy, new ivec3(1, -1, -1), scannerPosition),
        EOrientation.UpOutLeft => ivec3.Fma(beacon.swizzle.zxy, new ivec3(-1, 1, -1), scannerPosition),
        EOrientation.DownInLeft => ivec3.Fma(beacon.swizzle.zxy, new ivec3(-1, -1, 1), scannerPosition),
        EOrientation.UpRightOut => ivec3.Fma(beacon.swizzle.yxz, new ivec3(1, 1, -1), scannerPosition),
        EOrientation.DownRightIn => ivec3.Fma(beacon.swizzle.yxz, new ivec3(1, -1, 1), scannerPosition),
        EOrientation.UpLeftIn => ivec3.Fma(beacon.swizzle.yxz, new ivec3(-1, 1, 1), scannerPosition),
        EOrientation.DownLeftOut => ivec3.Fma(beacon.swizzle.yxz, new ivec3(-1, -1, -1), scannerPosition),

        EOrientation.InRightUp => ivec3.Fma(beacon.swizzle.yzx, new ivec3(1, 1, 1), scannerPosition),
        EOrientation.OutRightDown => ivec3.Fma(beacon.swizzle.yzx, new ivec3(1, -1, -1), scannerPosition),
        EOrientation.OutLeftUp => ivec3.Fma(beacon.swizzle.yzx, new ivec3(-1, 1, -1), scannerPosition),
        EOrientation.InLeftDown => ivec3.Fma(beacon.swizzle.yzx, new ivec3(-1, -1, 1), scannerPosition),
        EOrientation.OutUpRight => ivec3.Fma(beacon.swizzle.zyx, new ivec3(1, 1, -1), scannerPosition),
        EOrientation.InDownRight => ivec3.Fma(beacon.swizzle.zyx, new ivec3(1, -1, 1), scannerPosition),
        EOrientation.InUpLeft => ivec3.Fma(beacon.swizzle.zyx, new ivec3(-1, 1, 1), scannerPosition),
        EOrientation.OutDownLeft => ivec3.Fma(beacon.swizzle.zyx, new ivec3(-1, -1, -1), scannerPosition),

        _ => throw new ArgumentOutOfRangeException(nameof(scannerOrientation)),
      };
  }
}
