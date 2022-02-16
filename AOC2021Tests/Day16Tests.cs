using AoC2021.Day16;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Tests
{
  [TestFixture,
    TestOf(typeof(PacketDecoder))]
  public class Day16Tests
  {
    [Test,
      TestCase(16, "8A004A801A8002F478"),
      TestCase(12, "620080001611562C8802118E34"),
      TestCase(23, "C0015000016115A2E0802F182340"),
      TestCase(31, "A0016C880162017C3686B18A3D4780"),
      TestOf(nameof(PacketDecoder.SummateVersions))]
    public void TestVersionSummations(int expectedResult, string packet)
    {
      Assert.AreEqual(expectedResult, PacketDecoder.SummateVersions(packet));
    }

    [Test,
      TestCase(3L, "C200B40A82"),
      TestCase(54L, "04005AC33890"),
      TestCase(7L, "880086C3E88112"),
      TestCase(9L, "CE00C43D881120"),
      TestCase(1L, "D8005AC2A8F0"),
      TestCase(0L, "F600BC2D8F"),
      TestCase(0L, "9C005AC2F8F0"),
      TestCase(1L, "9C0141080250320F1802104A08"),
      TestOf(nameof(PacketDecoder.EvaluatePacket))]
    public void TestPacketEvaluation(long expectedResult, string packet)
    {
      Assert.AreEqual(expectedResult, PacketDecoder.EvaluatePacket(packet));
    }
  }
}
