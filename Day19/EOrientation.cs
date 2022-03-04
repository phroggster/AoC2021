using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day19
{
  public enum EOrientation : ushort
  {
    Default = ushort.MinValue,

    RightUpIn,
    RightDownOut,
    RightOutUp,
    RightInDown,
    LeftUpOut,
    LeftDownIn,
    LeftInUp,
    LeftOutDown,

    InDownRight,
    InUpLeft,
    InLeftDown,
    InRightUp,
    OutDownLeft,
    OutUpRight,
    OutRightDown,
    OutLeftUp,

    UpLeftIn,
    UpOutLeft,
    UpRightOut,
    UpInRight,
    DownOutRight,
    DownRightIn,
    DownInLeft,
    DownLeftOut,

    Unknown = ushort.MaxValue
  };

  public static class EOrientationExts
  {
    public static ushort ToUShort(this EOrientation orientation)
    {
      return (ushort)orientation;
    }

    public static EOrientation ToOrientation(this ushort orientation)
    {
      return (EOrientation)orientation;
    }
  }
}
