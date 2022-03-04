using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day19
{
  [DebuggerDisplay("{" + nameof(DbgDisplay) + ",nq}")]
  public readonly struct ivec3
  {
    public readonly int x;

    public readonly int y;

    public readonly int z;


    public ivec3(int x, int y, int z)
    {
      this.x = x;
      this.y = y;
      this.z = z;
    }

    public ivec3(IEnumerable<int> v)
    {
      _ = v ?? throw new ArgumentNullException(nameof(v));
      x = v.FirstOrDefault();
      y = v.Skip(1).FirstOrDefault();
      z = v.Skip(2).FirstOrDefault();
    }


    public static ivec3 Zero { get; } = new(0, 0, 0);

    public static ivec3 Ones { get; } = new(1, 1, 1);

    public static ivec3 UnitX { get; } = new(1, 0, 0);

    public static ivec3 UnitY { get; } = new(0, 1, 0);

    public static ivec3 UnitZ { get; } = new(0, 0, 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ivec3 FusedMultiplyAdd(ivec3 a, ivec3 b, ivec3 c)
    {
      return new ivec3(
        unchecked((a.x * b.x) + c.x),
        unchecked((a.y * b.y) + c.y),
        unchecked((a.z * b.z) + c.z));
    }


    // negation
    public static ivec3 operator -(ivec3 v) => new(-v.x, -v.y, -v.z);

    // subtraction
    public static ivec3 operator -(ivec3 lhs, ivec3 rhs) => new(
      lhs.x - rhs.x,
      lhs.y - rhs.y,
      lhs.z - rhs.z);

    // addition
    public static ivec3 operator +(ivec3 lhs, ivec3 rhs) => new(
      lhs.x + rhs.x,
      lhs.y + rhs.y,
      lhs.z + rhs.z);

    // multiplication
    public static ivec3 operator *(ivec3 lhs, ivec3 rhs) => new(
      lhs.x * rhs.x,
      lhs.y * rhs.y,
      lhs.z * rhs.z);

    // equality
    public static bool operator ==(ivec3 lhs, ivec3 rhs) =>
      lhs.x == rhs.x &&
      lhs.y == rhs.y &&
      lhs.z == rhs.z;

    // non-equality
    public static bool operator !=(ivec3 lhs, ivec3 rhs) =>
      lhs.x != rhs.x ||
      lhs.y != rhs.y ||
      lhs.z != rhs.z;


    public ivec3 xyz => this;
    public ivec3 xzy => new(x, z, y);
    public ivec3 yxz => new(y, x, z);
    public ivec3 yzx => new(y, z, x);
    public ivec3 zxy => new(z, x, y);
    public ivec3 zyx => new(z, y, x);


    public override bool Equals(object? obj) => obj is ivec3 rhs && this == rhs;

    public override int GetHashCode() => (x, y, z).GetHashCode();

    public override string ToString() => DbgDisplay;

    private string DbgDisplay => $"({x}, {y}, {z})";
  }


  internal class ivec3EqualityComparer : EqualityComparer<ivec3>
  {
    [DebuggerStepThrough,
      MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override bool Equals(ivec3 lhs, ivec3 rhs) =>
      lhs.x == rhs.x &&
      lhs.y == rhs.y &&
      lhs.z == rhs.z;

    [DebuggerStepThrough,
      MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override int GetHashCode([DisallowNull] ivec3 vec)
      => (vec.x, vec.y, vec.z).GetHashCode();
  }
}
