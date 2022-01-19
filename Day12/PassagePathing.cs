using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day12
{
  public static class PassagePathing
  {
    static void Main(string[] _)
    {
      // How many paths through this cave system are there that visit small caves at most once?
      Console.WriteLine($"0-Revisit Paths EXAMPLE A:  {CountPathsWithoutRevisits(Data.Day12ExampleDataA)}");
      Console.WriteLine($"0-Revisit Paths EXAMPLE B:  {CountPathsWithoutRevisits(Data.Day12ExampleDataB)}");
      Console.WriteLine($"0-Revisit Paths EXAMPLE C:  {CountPathsWithoutRevisits(Data.Day12ExampleDataC)}");
      Console.WriteLine($"0-Revisit Paths ACTUAL:     {CountPathsWithoutRevisits(Data.Day12ActualData)}");
      Console.WriteLine("---------------------------|----");

      // How many paths through this cave system are there that can visit a small cave at most twice?
      Console.WriteLine($"1-Revisit Paths EXAMPLE A:  {CountPathsAllowingOneRevisit(Data.Day12ExampleDataA)}");
      Console.WriteLine($"1-Revisit Paths EXAMPLE B:  {CountPathsAllowingOneRevisit(Data.Day12ExampleDataB)}");
      Console.WriteLine($"1-Revisit Paths EXAMPLE C:  {CountPathsAllowingOneRevisit(Data.Day12ExampleDataC)}");
      Console.WriteLine($"1-Revisit Paths ACTUAL:     {CountPathsAllowingOneRevisit(Data.Day12ActualData)}");
    }

    public static int CountPathsWithoutRevisits(IEnumerable<string> data)
      => CountPaths(data, 0);

    public static int CountPathsAllowingOneRevisit(IEnumerable<string> data)
      => CountPaths(data, 1);

    public static bool IsRestricted(string node)
      => node.Any(c => char.IsLower(c));

    /// <summary>Count the number of paths available, revisiting any small cave
    /// only <paramref name="nRevisitsAllowed"/> times.</summary>
    /// <param name="data"></param>
    /// <param name="nRevisitsAllowed">The number of revisits that small caves
    /// are allowed to receive.</param>
    /// <returns></returns>
    public static int CountPaths(IEnumerable<string> data, int nRevisitsAllowed)
    {
      int result = 0;

      var edgeDict = ParseGraph(data);
      var visits = new Dictionary<string, List<int>>(edgeDict.Count);
      foreach (var node in edgeDict.Keys)
      {
        visits.Add(node, new());
      }

      TraverseGraph(StartNode, 1, edgeDict, ref visits, ref result, nRevisitsAllowed);
      return result;
    }


    const string StartNode = "start";
    const string EndNode = "end";

    /// <summary>Parses a graph from a collection of edges.</summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private static IReadOnlyDictionary<string, HashSet<string>> ParseGraph(IEnumerable<string> data)
    {
      var result = new Dictionary<string, HashSet<string>>(data.Count() / 4);

      foreach (var edge in data)
      {
        var left = edge.Split('-')[0];
        var right = edge.Split('-')[1];

        if (!result.ContainsKey(left))
          result.Add(left, new HashSet<string>());

        if (!result.ContainsKey(right))
          result.Add(right, new HashSet<string>());

        if (left != EndNode && right != StartNode)
          result[left].Add(right);

        if (right != EndNode && left != StartNode)
          result[right].Add(left);
      }

      if (!result.ContainsKey(StartNode))
        throw new ArgumentException("No start node found.", nameof(data));
      else if (!result.ContainsKey(EndNode))
        throw new ArgumentException("No end node found.", nameof(data));

      return result;
    }

    /// <summary>Recursively traverse a graph, counting the number of viable paths.</summary>
    /// <param name="curNode">The node to probe.</param>
    /// <param name="depth">The depth of the path to the node.</param>
    /// <param name="graph">The dictionary of connections between all nodes.</param>
    /// <param name="visits">A dictionary of visited nodes and the depth were visited at.</param>
    /// <param name="pathCount">The number of paths that are available.</param>
    /// <param name="nRevisitsAllowed">The number of times a path can traverse restricted nodes.</param>
    private static void TraverseGraph(string curNode, int depth,
      IReadOnlyDictionary<string, HashSet<string>> graph,
      ref Dictionary<string, List<int>> visits,
      ref int pathCount,
      int nRevisitsAllowed)
    {
      // mark node as visited
      if (IsRestricted(curNode) && curNode != StartNode)
        visits[curNode].Add(depth);

      // iterate available edges
      foreach (var node in graph[curNode])
      {
        // a path has been completed
        if (node == EndNode)
        {
          pathCount++;
          continue;
        }

        // we've exceded our allotment of small cave revisits
        if (IsRestricted(node) && visits[node].Any()
          && (visits.Values.Any(vc => vc.Count > nRevisitsAllowed)))
        {
          continue;
        }

        // recurse upon the neighbor
        TraverseGraph(node, depth + 1, graph, ref visits, ref pathCount, nRevisitsAllowed);

        // clean up after our visit to the neighbor
        foreach (var child in visits.Keys)
        {
          visits[child] = visits[child]
            .Where(s => s <= depth)
            .ToList();
        }
      }
    }
  }
}
