/*
 * SPDX: MIT
 * 
 * The MIT License (MIT)
 * 
 * Copyright © 2021  github.com/i3arnon
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the “Software”), to
 * deal in the Software without restriction, including without limitation the
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
 * sell copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 *   all copies or substantial portions of the Software.
 * The Software is provided “as is”, without warranty of any kind, express or
 *   implied, including but not limited to the warranties of merchantability,
 *   fitness for a particular purpose and noninfringement. In no event shall
 *   the authors or copyright holders be liable for any claim, damages or other
 *   liability, whether in an action of contract, tort or otherwise, arising
 *   from, out of or in connection with the software or the use or other
 *   dealings in the Software.
 */

// With thanks to: https://github.com/i3arnon/ConcurrentHashSet

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;

namespace AoC2021.Day19
{
  [DebuggerDisplay("Count = {Count}")]
  public class ConcurrentHashSet<T> : ICollection<T>, IEnumerable<T>, IReadOnlyCollection<T>
    where T : notnull
  {
    public ConcurrentHashSet()
      : this(DefaultConcurrencyLevel, DefaultCapacity, true, null)
    {
    }

    public ConcurrentHashSet(int concurrencyLevel, int capacity)
      : this(concurrencyLevel, capacity, false, null)
    {
    }

    public ConcurrentHashSet(IEnumerable<T> collection)
      : this(collection, null)
    {
    }

    public ConcurrentHashSet(IEqualityComparer<T>? comparer)
      : this(DefaultConcurrencyLevel, DefaultCapacity, true, comparer)
    {
    }

    public ConcurrentHashSet(IEnumerable<T> collection, IEqualityComparer<T>? comparer)
      : this(comparer)
    {
      _ = collection ?? throw new ArgumentNullException(nameof(collection));
      InitializeFromCollection(collection);
    }

    public ConcurrentHashSet(int concurrencyLevel, IEnumerable<T> collection, IEqualityComparer<T>? comparer)
      : this(concurrencyLevel, DefaultCapacity, false, comparer)
    {
      _ = collection ?? throw new ArgumentNullException(nameof(collection));
      InitializeFromCollection(collection);
    }

    public ConcurrentHashSet(int concurrencyLevel, int capacity, IEqualityComparer<T>? comparer)
      : this(concurrencyLevel, capacity, false, comparer)
    {
    }


    private ConcurrentHashSet(int concurrencyLevel, int capacity, bool growLockArray, IEqualityComparer<T>? comparer)
    {
      if (concurrencyLevel < 1) throw new ArgumentOutOfRangeException(nameof(concurrencyLevel));
      if (capacity < 1) throw new ArgumentOutOfRangeException(nameof(capacity));
      capacity = Math.Max(capacity, concurrencyLevel);

      var locks = new object[concurrencyLevel];
      for (int i = 0; i < locks.Length; ++i)
      {
        locks[i] = new object();
      }

      var cpl = new int[locks.Length];
      var buckets = new Node[capacity];
      _tables = new Tables(buckets, locks, cpl);

      _growLockArray = growLockArray;
      _budget = buckets.Length / locks.Length;
      Comparer = comparer ?? EqualityComparer<T>.Default;
    }


    private const int DefaultCapacity = 384;
    private const int MaxLockNumber = 1024;

    private readonly bool _growLockArray;

    private int _budget;
    private volatile Tables _tables;

    private static int DefaultConcurrencyLevel => Environment.ProcessorCount;


    public IEqualityComparer<T> Comparer { get; init; }

    public int Count
    {
      get
      {
        var result = 0;
        var ackLocks = 0;
        try
        {
          AcquireAllLocks(ref ackLocks);
          var cpls = _tables.CountPerLock;
          for (int i = 0; i < cpls.Length; ++i)
          {
            result += cpls[i];
          }
        }
        finally
        {
          ReleaseLocks(0, ackLocks);
        }
        return result;
      }
    }

    public bool IsEmpty
    {
      get
      {
        if (!AreAllBucketsEmpty())
          return false;
        var ackLocks = 0;
        try
        {
          AcquireAllLocks(ref ackLocks);
          return AreAllBucketsEmpty();
        }
        finally
        {
          ReleaseLocks(0, ackLocks);
        }
      }
    }

    public bool IsReadOnly => false;


    public bool Add(T item) => AddInternal(item, Comparer.GetHashCode(item), true);

    public void AddRange(IEnumerable<T> items)
    {
      foreach (var item in items)
        _ = Add(item);
    }

    public void Clear()
    {
      var locksAcq = 0;
      try
      {
        AcquireAllLocks(ref locksAcq);
        if (AreAllBucketsEmpty())
          return;

        var tables = _tables;
        var newTables = new Tables(new Node[DefaultCapacity], tables.Locks, new int[tables.CountPerLock.Length]);
        _tables = newTables;
        _budget = Math.Max(1, newTables.Buckets.Length / newTables.Locks.Length);
      }
      finally
      {
        ReleaseLocks(0, locksAcq);
      }
    }

    public bool Contains(T item) => TryGetValue(item, out _);

    public bool TryGetValue(T item, [MaybeNullWhen(false)] out T actualValue)
    {
      var hash = Comparer.GetHashCode(item);
      var tables = _tables;
      var bucketNo = GetBucket(hash, tables.Buckets.Length);
      var current = Volatile.Read(ref tables.Buckets[bucketNo]);

      while (current is not null)
      {
        if (hash == current.Hashcode && Comparer.Equals(current.Item, item))
        {
          actualValue = current.Item;
          return true;
        }
        current = current.Next;
      }

      actualValue = default;
      return false;
    }

    public bool TryRemove(T item)
    {
      var hash = Comparer.GetHashCode(item);
      while (true)
      {
        var tables = _tables;
        GetBucketAndLockNo(hash, out int bucketNo, out int lockNo, tables.Buckets.Length, tables.Locks.Length);

        lock (tables.Locks[lockNo])
        {
          if (tables != _tables)
            continue;

          Node? previous = null;
          for (var current = tables.Buckets[bucketNo]; current is not null; current = current.Next)
          {
            if (hash == current.Hashcode && Comparer.Equals(current.Item, item))
            {
              if (previous is null)
                Volatile.Write(ref tables.Buckets[bucketNo], current.Next);
              else
                previous.Next = current.Next;
              tables.CountPerLock[lockNo]--;
              return true;
            }
            previous = current;
          }
        }
        return false;
      }
    }



    private void AcquireAllLocks(ref int locksAcquired)
    {
      AcquireLocks(0, 1, ref locksAcquired);
      AcquireLocks(1, _tables.Locks.Length, ref locksAcquired);
    }

    private void AcquireLocks(int fromInclusive, int toExclusive, ref int locksAcquired)
    {
      var locks = _tables.Locks;
      for (var i = fromInclusive; i < toExclusive; ++i)
      {
        var lockTaken = false;
        try
        {
          Monitor.Enter(locks[i], ref lockTaken);
        }
        finally
        {
          if (lockTaken)
          {
            ++locksAcquired;
          }
        }
      }
    }

    private bool AddInternal(T item, int hashcode, bool ackLock)
    {
      while (true)
      {
        var tables = _tables;

        GetBucketAndLockNo(hashcode, out int bucketNo, out int lockNo, tables.Buckets.Length, tables.Locks.Length);

        var resizeDesired = false;
        var lockTaken = false;
        try
        {
          if (ackLock)
            Monitor.Enter(tables.Locks[lockNo], ref lockTaken);
          if (tables != _tables)
            continue;

          Node? previous = null;
          for (var current = tables.Buckets[bucketNo]; current != null; current = current.Next)
          {
            if (hashcode == current.Hashcode && Comparer.Equals(current.Item, item))
            {
              return false;
            }
            previous = current;
          }

          Volatile.Write(ref tables.Buckets[bucketNo], new Node(item, hashcode, tables.Buckets[bucketNo]));
          checked
          {
            tables.CountPerLock[lockNo]++;
          }

          if (tables.CountPerLock[lockNo] > _budget)
          {
            resizeDesired = true;
          }
        }
        finally
        {
          if (lockTaken)
            Monitor.Exit(tables.Locks[lockNo]);
        }

        if (resizeDesired)
          GrowTable(tables);

        return true;
      }
    }

    private bool AreAllBucketsEmpty()
    {
      var cpl = _tables.CountPerLock;
      for (int i = 0; i < cpl.Length; ++i)
      {
        if (cpl[i] != 0)
        {
          return false;
        }
      }
      return true;
    }

    private void CopyToItems(T[] array, int index)
    {
      var buckets = _tables.Buckets;
      for (int i = 0; i < buckets.Length; ++i)
      {
        for (var current = buckets[i]; current != null; current = current.Next)
        {
          array[index++] = current.Item;
        }
      }
    }

    private void GrowTable(Tables tables)
    {
      const int maxArrayLength = int.MaxValue;
      var locksAcq = 0;
      try
      {
        AcquireLocks(0, 1, ref locksAcq);

        if (tables != _tables)
          return;

        long approxCount = 0L;
        for (int n = 0; n < tables.CountPerLock.Length; n++)
        {
          approxCount += tables.CountPerLock[n];
        }

        if (approxCount < tables.Buckets.Length / 4)
        {
          _budget = 2 * _budget;
          if (_budget < 0)
            _budget = int.MaxValue;
          return;
        }

        var newLen = 0;
        var isMaxSize = false;
        try
        {
          checked
          {
            newLen = tables.Buckets.Length * 2 + 1;
            while (newLen % 3 == 0 || newLen % 5 == 0 || newLen % 7 == 0)
            {
              newLen += 2;
            }
            if (newLen > maxArrayLength)
            {
              isMaxSize = true;
            }
          }
        }
        catch (OverflowException)
        {
          isMaxSize = true;
        }

        if (isMaxSize)
        {
          newLen = maxArrayLength;
          _budget = int.MaxValue;
        }

        AcquireLocks(1, tables.Locks.Length, ref locksAcq);
        var newLocks = tables.Locks;

        if (_growLockArray && tables.Locks.Length < MaxLockNumber)
        {
          newLocks = new object[tables.Locks.Length * 2];
          Array.Copy(tables.Locks, newLocks, tables.Locks.Length);
          for (int n = tables.Locks.Length; n < newLocks.Length; n++)
          {
            newLocks[n] = new object();
          }
        }

        var newBuckets = new Node[newLen];
        var newCPL = new int[newLocks.Length];

        for (int n = 0; n < tables.Buckets.Length; ++n)
        {
          var current = tables.Buckets[n];
          while (current != null)
          {
            var next = current.Next;
            GetBucketAndLockNo(current.Hashcode, out int newBucketNo, out int newLockNo, newBuckets.Length, newLocks.Length);
            newBuckets[newBucketNo] = new Node(current.Item, current.Hashcode, newBuckets[newBucketNo]);

            checked
            {
              newCPL[newLockNo]++;
            }
            current = next;
          }
        }

        _budget = Math.Max(1, newBuckets.Length / newLocks.Length);
        _tables = new Tables(newBuckets, newLocks, newCPL);
      }
      finally
      {
        ReleaseLocks(0, locksAcq);
      }
    }

    private void InitializeFromCollection(IEnumerable<T> collection)
    {
      foreach (var item in collection)
        AddInternal(item, Comparer.GetHashCode(item), false);
      if (_budget == 0)
      {
        var tables = _tables;
        _budget = tables.Buckets.Length / tables.Locks.Length;
      }
    }

    private void ReleaseLocks(int fromInclusive, int toExclusive)
    {
      for (int i = fromInclusive; i < toExclusive; ++i)
      {
        Monitor.Exit(_tables.Locks[i]);
      }
    }


    private static int GetBucket(int hashcode, int bucketCount)
    {
      return (hashcode & int.MaxValue) % bucketCount;
    }

    private static void GetBucketAndLockNo(int hashcode, out int bucketNo, out int lockNo, int bucketCount, int lockCount)
    {
      bucketNo = GetBucket(hashcode, bucketCount);
      lockNo = bucketNo % lockCount;
    }


    #region Explicit ICollection<T> methods

    void ICollection<T>.Add(T item) => Add(item);

    void ICollection<T>.CopyTo(T[] array, int arrayIndex)
    {
      _ = array ?? throw new ArgumentNullException(nameof(array));
      if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));

      var locksAcq = 0;
      try
      {
        var count = 0;

        AcquireAllLocks(ref locksAcq);
        var cpl = _tables.CountPerLock;
        for (int n = 0; n < cpl.Length; ++n)
        {
          count += cpl[n];
        }

        if (array.Length - count < arrayIndex || count < 0)
          throw new ArgumentException("The index is equal to or greater than the length of the array, or the number of elements in the set is greater than the available space.");

        CopyToItems(array, arrayIndex);
      }
      finally
      {
        ReleaseLocks(0, locksAcq);
      }
    }

    bool ICollection<T>.Remove(T item) => TryRemove(item);

    #endregion

    #region Explicit IEnumerable<T> methods

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>)this).GetEnumerator();

    #endregion


    public struct Enumerator : IEnumerator<T>, IEnumerator
    {
      public Enumerator(ConcurrentHashSet<T> set)
      {
        _set = set;
        _buckets = null;
        _node = null;
        Current = default!;
        _i = -1;
        _state = StateUninitialized;
      }

      private readonly ConcurrentHashSet<T> _set;

      private Node?[]? _buckets;
      private Node? _node;
      private int _i;
      private int _state;

      private const int StateUninitialized = 0;
      private const int StateOuterLoop = 1;
      private const int StateInnerLoop = 2;
      private const int StateDone = 3;


      public T Current { get; private set; }

      object? IEnumerator.Current => Current;

      public void Dispose() { }

      public bool MoveNext()
      {
        switch (_state)
        {
          case StateUninitialized:
            _buckets = _set._tables.Buckets;
            _i = -1;
            goto case StateOuterLoop;

          case StateOuterLoop:
            Node?[]? buckets = _buckets;
            int i = ++_i;
            if ((uint)i < (uint)buckets!.Length)
            {
              _node = Volatile.Read(ref buckets[i]);
              _state = StateInnerLoop;
              goto case StateInnerLoop;
            }
            goto default;

          case StateInnerLoop:
            Node? node = _node;
            if (node is not null)
            {
              Current = node.Item;
              _node = node.Next;
              return true;
            }
            goto case StateOuterLoop;

          default:
            _state = StateDone;
            return false;
        }
      }

      public void Reset()
      {
        _buckets = null;
        _node = null;
        Current = default!;
        _i = -1;
        _state = StateUninitialized;
      }
    }


    private class Node
    {
      public readonly T Item;
      public readonly int Hashcode;

      public volatile Node? Next;

      public Node(T item, int hashcode, Node? next)
      {
        Item = item;
        Hashcode = hashcode;
        Next = next;
      }
    }

    private class Tables
    {
      public readonly Node?[] Buckets;
      public readonly object[] Locks;

      public readonly int[] CountPerLock;

      public Tables(Node?[] buckets, object[] locks, int[] countPerLock)
      {
        Buckets = buckets;
        Locks = locks;
        CountPerLock = countPerLock;
      }
    }
  }
}
