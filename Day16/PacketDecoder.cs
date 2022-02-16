using AoC2021.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day16
{
  public static class PacketDecoder
  {
    static void Main(string[] _)
    {
      // What do you get if you add up the version numbers in all packets?
      Console.WriteLine($"Version Summation: {SummateVersions(Data.Day16Data)}");

      // What do you get if you evaluate the expression ...?
      Console.WriteLine($"Evaluated Packet:  {EvaluatePacket(Data.Day16Data)}");
    }

    public static int SummateVersions(string packet)
    {
      return new Parser(packet)
        .Parse()
        .VersionSum;
    }

    public static long EvaluatePacket(string packet)
    {
      return new Parser(packet)
        .Parse()
        .Value;
    }
  }

  enum EOpLenType : int
  {
    NPackets = 11,
    NBits = 15
  }

  class Parser
  {
    IEnumerable<bool> _stream;

    public Parser(string hexPacket)
    {
      _stream = new HexToBitStreamConverter(hexPacket).Bits;
    }

    public Packet Parse()
    {
      int version = ReadBits(3).ToInt();
      int type = ReadBits(3).ToInt();

      if (type == 4)
      {
        // Literal packet
        var bits = new List<bool>();
        int bitCount = 0;
        bool moreGroups = true;

        while (moreGroups)
        {
          moreGroups = ReadBits(1).First();
          bits.AddRange(ReadBits(4));
          bitCount += 5;
        }
        return new LitPacket(version, bits.ToLong(), bitCount);
      }
      else
      {
        // Operator packet
        var subPackets = new List<Packet>();
        var lenType = ReadBits(1).First()
          ? EOpLenType.NPackets
          : EOpLenType.NBits;
        var loopCount = ReadBits((int)lenType).ToInt();

        if (lenType == EOpLenType.NPackets)
          for (int n = 0; n < loopCount; n++)
            subPackets.Add(Parse());
        else
          for (int n = 0; n < loopCount; n += subPackets.Last().Size)
            subPackets.Add(Parse());

        return new OpPacket(version, type, lenType, subPackets);
      }
    }

    IEnumerable<bool> ReadBits(int count)
    {
      Debug.Assert(count > 0);

      var result = _stream.Take(count);
      _stream = _stream.Skip(count);
      return result;
    }
  }

  abstract class Packet
  {
    public int Size { get; init; }
    public long Value { get; init; }
    public int Version { get; init; }
    public int VersionSum { get; init; }
  }

  class LitPacket : Packet
  {
    public LitPacket(int version, long value, int nBits)
    {
      Debug.Assert(version >= 0);
      Debug.Assert(nBits > 0);

      Size = 3      // version num
        + 3         // type id
        + nBits;    // value length
      Value = value;
      Version = version;
      VersionSum = version;
    }
  }

  class OpPacket : Packet
  {
    /// <exception cref="ArgumentOutOfRangeException"/>
    public OpPacket(int version, int typeId, EOpLenType lengthType, IEnumerable<Packet> subPackets)
    {
      Debug.Assert(version >= 0);
      Debug.Assert(subPackets is not null);
      Debug.Assert(subPackets.Any());

      LengthType = lengthType;
      OperatorTypeId = typeId;
      SubPackets = subPackets.ToList();
      Version = version;

      Size = SubPackets.Select(pk => pk.Size).Sum()   // subpacket sizes
        + ((int)LengthType)                           // subpacket length
        + 3                                           // version num
        + 3                                           // type id
        + 1;                                          // EOpLenType bit
      VersionSum = SubPackets.Select(pk => pk.VersionSum).Sum()
        + Version;

      if (OperatorTypeId == 0)
        Value = SubPackets.Sum(pk => pk.Value);
      else if (OperatorTypeId == 1)
        Value = SubPackets.Product(pk => pk.Value);
      else if (OperatorTypeId == 2)
        Value = SubPackets.Min(pk => pk.Value);
      else if (OperatorTypeId == 3)
        Value = SubPackets.Max(pk => pk.Value);
      else if (OperatorTypeId == 5)
        Value = SubPackets[0].Value > SubPackets[1].Value ? 1L : 0L;
      else if (OperatorTypeId == 6)
        Value = SubPackets[0].Value < SubPackets[1].Value ? 1L : 0L;
      else if (OperatorTypeId == 7)
        Value = SubPackets[0].Value == SubPackets[1].Value ? 1L : 0L;
      else
        throw new ArgumentOutOfRangeException(nameof(typeId));
    }

    public EOpLenType LengthType { get; init; }
    public int OperatorTypeId { get; init; }
    public List<Packet> SubPackets { get; init; }
  }

  class HexToBitStreamConverter
  {
    public string HexValue { get; init; }

    public HexToBitStreamConverter(string hex)
    {
      HexValue = hex;
    }

    public IEnumerable<bool> Bits => HexValue
      .SelectMany(c => c.HexToBinary());
  }
}
