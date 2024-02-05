using System.Diagnostics;

namespace Day25
{
    public class Part1 : Framework.ISolution
    {
        private sealed class Graph
        {
            private readonly Dictionary<string, List<(string otherVertex, int weight)>> _edges = new();

            public void Connect(string vertex1, string vertex2, int weight = 1)
            {
                Debug.Assert(weight > 0);
                if (_edges.TryGetValue(vertex1, out var value) && value.Any(x => x.otherVertex == vertex2))
                {
                    return;
                }

                void AddConnection(string v1, string v2, int w)
                {
                    if (_edges.TryGetValue(v1, out var value))
                    {
                        value.Add((v2, w));
                    }
                    else
                    {
                        _edges[v1] = new() { (v2, w) };
                    }
                }

                AddConnection(vertex1, vertex2, weight);
                AddConnection(vertex2, vertex1, weight);
            }

            public void Remove(string vertex)
            {
                var edges = _edges[vertex];
                _edges.Remove(vertex);
                foreach (var (otherVertex, _) in edges)
                {
                    _edges[otherVertex].RemoveAll(x => x.otherVertex == vertex);
                }
            }

            public IReadOnlyCollection<string> IDs => _edges.Keys;

            public IReadOnlyCollection<(string vertex, int weight)> Edges(string vector)
            {
                return _edges.TryGetValue(vector, out var edges) ? edges : Array.Empty<(string vertex, int weight)>();
            }

            public int Count => IDs.Count;

            public void Merge(string vertex1, string vertex2)
            {
                var oldEdges = Edges(vertex1).Concat(Edges(vertex2));
                Dictionary<string, int> newEdges = new();
                foreach (var (vertex, weight) in oldEdges)
                {
                    if (vertex == vertex1 || vertex == vertex2) { continue; }

                    newEdges[vertex] = (newEdges.TryGetValue(vertex, out int oldWeight) ? oldWeight : 0) + weight;
                }
                var (firstVertex, secondVertex) = string.CompareOrdinal(vertex1, vertex2) < 0 ? (vertex1, vertex2) : (vertex2, vertex1);
                var newVertex = $"{firstVertex},{secondVertex}";
                Remove(vertex1);
                Remove(vertex2);
                foreach (var (vertex, weight) in newEdges)
                {
                    Connect(newVertex, vertex, weight);
                }
            }
        }

        private static (string s, string t, int cutWeight) MaximumAdjacencySearch(Graph g)
        {
            Debug.Assert(g.Count >= 2);
            HashSet<string> remaining = new(g.IDs);
            HashSet<string> found = new(remaining.Count);
            var first = remaining.First();
            string s = first;
            string t = first;
            int cutWeight = 0;
            found.Add(first);
            remaining.Remove(first);
            Dictionary<string, int> totalWeightCache = new();
            var comparer = Comparer<(string, int)>.Create((a, b) =>
            {
                var res = a.Item2.CompareTo(b.Item2);
                if (res == 0)
                {
                    res = string.Compare(a.Item1, b.Item1, StringComparison.Ordinal);
                }
                return res;
            });
            SortedSet<(string vertex, int weight)> queue = new(comparer);
            int TotalWeightToFound(string vertex)
            {
                return g.Edges(vertex)
                    .Where(y => found.Contains(y.vertex))
                    .Select(y => y.weight)
                    .Sum();
            }
            void RefreshWeights(string vertex)
            {
                foreach (var (otherVertex, _) in g.Edges(vertex))
                {
                    if (found.Contains(otherVertex))
                    {
                        continue;
                    }
                    int? oldWeight = totalWeightCache.TryGetValue(otherVertex, out int w) ? w : null;
                    var newWeight = TotalWeightToFound(otherVertex);
                    totalWeightCache[otherVertex] = newWeight;
                    if (oldWeight is int ow)
                    {
                        queue.Remove((otherVertex, ow));
                    }
                    queue.Add((otherVertex, newWeight));
                }
            }

            RefreshWeights(first);

            while (remaining.Count > 0)
            {
                var next = queue.Max;
                s = t;
                t = next.vertex;
                cutWeight = next.weight;
                found.Add(next.vertex);
                remaining.Remove(next.vertex);
                queue.Remove(next);
                RefreshWeights(next.vertex);
            }
            return (s, t, cutWeight);
        }

        private static (string s, string t, int cutWeight) MinCut(Graph g)
        {
            (string s, string t, int cutWeight)? currentMinCut = null;
            while (g.Count > 1)
            {
                var cutOfThePhase = MaximumAdjacencySearch(g);
                if (cutOfThePhase.cutWeight < (currentMinCut?.cutWeight ?? int.MaxValue))
                {
                    currentMinCut = cutOfThePhase;
                }
                g.Merge(cutOfThePhase.s, cutOfThePhase.t);
            }
            return currentMinCut is null ? throw new InvalidOperationException("No minimum cut found") : currentMinCut.Value;
        }

        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            var graph = new Graph();
            while (streamReader.ReadLine() is string line)
            {
                var parts = line.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var targets = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                foreach (var target in targets)
                {
                    graph.Connect(parts[0], target);
                }
            }

            var totalVectorCount = graph.Count;
            var (s, t, cutWeight) = MinCut(graph);
            var tPartsCount = t.Count(x => x == ',') + 1;
            var results = tPartsCount * (totalVectorCount - tPartsCount);

            return results;
        }
    }
}
