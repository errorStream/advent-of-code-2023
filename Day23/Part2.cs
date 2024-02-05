using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Day23
{
    public class Part2 : Framework.ISolution
    {
        private readonly record struct Vec2(int X, int Y)
        {
            public static Vec2 operator +(Vec2 a, Vec2 b)
            {
                return new Vec2(a.X + b.X, a.Y + b.Y);
            }

            public override string ToString()
            {
                return $"({X}, {Y})";
            }

            public static bool operator <(Vec2 a, Vec2 b)
            {
                return a.X < b.X || (a.X == b.X && a.Y < b.Y);
            }

            public static bool operator >(Vec2 a, Vec2 b)
            {
                return a.X > b.X || (a.X == b.X && a.Y > b.Y);
            }
        }

        private sealed record StartNode(Vec2 Position) : Node(Position)
        {
            public override string ToString()
            {
                return $"StartNode({Position})";
            }
        }

        private sealed record EndNode(Vec2 Position) : Node(Position)
        {

            public override string ToString()
            {
                return $"EndNode({Position})";
            }
        }

        private sealed record MiddleNode(Vec2 Position) : Node(Position)
        {
            public override string ToString()
            {
                return $"MiddleNode({Position})";
            }
        }

        private record BaseEdge;

        private sealed record OpenEdge : BaseEdge
        {
            public static readonly OpenEdge Instance = new OpenEdge();
        }

        private sealed record ClosedEdge(Node Node, int Weight) : BaseEdge;

        private abstract record Node(Vec2 Position)
        {
            public override int GetHashCode()
            {
                return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this);
            }

            public BaseEdge? Up;
            public BaseEdge? Right;
            public BaseEdge? Down;
            public BaseEdge? Left;

            public BaseEdge? this[Direction direction]
            {
                get => direction switch
                {
                    Direction.Up => Up,
                    Direction.Right => Right,
                    Direction.Down => Down,
                    Direction.Left => Left,
                    _ => throw new ArgumentOutOfRangeException(nameof(direction))
                };
                set
                {
                    switch (direction)
                    {
                        case Direction.Up:
                            Up = value;
                            break;
                        case Direction.Right:
                            Right = value;
                            break;
                        case Direction.Down:
                            Down = value;
                            break;
                        case Direction.Left:
                            Left = value;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(direction));
                    }
                }
            }

            public IEnumerable<ClosedEdge> Edges()
            {
                if (Up is ClosedEdge up)
                {
                    yield return up;
                }

                if (Right is ClosedEdge right)
                {
                    yield return right;
                }

                if (Down is ClosedEdge down)
                {
                    yield return down;
                }

                if (Left is ClosedEdge left)
                {
                    yield return left;
                }
            }
        }

        private enum Direction
        {
            Up,
            Right,
            Down,
            Left
        }

        private static Vec2 Offset(Direction direction)
        {
            return direction switch
            {
                Direction.Up => new(0, -1),
                Direction.Right => new(1, 0),
                Direction.Down => new(0, 1),
                Direction.Left => new(-1, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };
        }

        private static Direction Invert(Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Down,
                Direction.Right => Direction.Left,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };
        }

        private readonly IReadOnlyList<Direction> _directions = new[]
        {
            Direction.Up,
            Direction.Right,
            Direction.Down,
            Direction.Left
        };

        /// <remarks> Positive y is down </remarks>
        private static char[,] ParseGrid(StreamReader streamReader)
        {
            var lines = new List<string>();
            while (streamReader.ReadLine() is string line)
            {
                lines.Add(line);
            }

            var grid = new char[lines[0].Length, lines.Count];
            for (int y = 0; y < lines.Count; ++y)
            {
                for (int x = 0; x < lines[0].Length; ++x)
                {
                    grid[x, y] = lines[y][x];
                }
            }
            return grid;
        }

        private static (Vec2 p1, Vec2 p2) MakeEdgeKey(Node node, ClosedEdge edge)
        {
            var p1 = node.Position;
            var p2 = edge.Node.Position;
            var key = p1 < p2 ? (p1, p2) : (p2, p1);
            return key;
        }

        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            var grid = ParseGrid(streamReader);

            Vec2 start = new(Enumerable.Range(0, grid.GetLength(0)).First(x => grid[x, 0] == '.'), 0);
            Vec2 end = new(Enumerable.Range(0, grid.GetLength(0)).First(x => grid[x, grid.GetLength(1) - 1] == '.'), grid.GetLength(1) - 1);

            bool IsWalkable(Vec2 pos)
            {
                var inGrid = pos.X >= 0 && pos.X < grid.GetLength(0) && pos.Y >= 0 && pos.Y < grid.GetLength(1);
                var isForest = inGrid && grid[pos.X, pos.Y] == '#';
                return inGrid && !isForest;
            }

            var nodes = new Dictionary<Vec2, Node>
            {
                { start, new StartNode(start) },
                { end, new EndNode(end) }
            };

            for (int x = 0; x < grid.GetLength(0); ++x)
            {
                for (int y = 0; y < grid.GetLength(1); ++y)
                {
                    Vec2 pos = new(x, y);
                    if (!IsWalkable(pos)) continue;
                    var connectionCount = 0;
                    foreach (var direction in _directions)
                    {
                        var connectedPos = pos + Offset(direction);
                        if (IsWalkable(connectedPos))
                        {
                            ++connectionCount;
                        }
                    }
                    if (connectionCount > 2)
                    {
                        nodes.Add(pos, new MiddleNode(pos));
                    }
                }
            }

            bool FindNodeAtOtherEnd(Vec2 pos, Direction direction, out Node? otherNode, out Direction enterDirection, out int length)
            {
                var currPos = pos;
                var currDirection = direction;
                var currLength = 0;
                for (int i = 0; i <= 1_000_000; ++i)
                {
                    if (i == 1_000_000) { throw new InvalidOperationException($"Infinite loop for pos: {pos}, direction: {direction}"); }
                    currPos += Offset(currDirection);
                    ++currLength;
                    if (nodes.TryGetValue(currPos, out otherNode))
                    {
                        enterDirection = Invert(currDirection);
                        length = currLength;
                        return true;
                    }
                    Direction? nextDirection = _directions.Where(d => (d != Invert(currDirection)) && IsWalkable(currPos + Offset(d))).Select(x => (Direction?)x).FirstOrDefault();
                    if (nextDirection is null)
                    {
                        otherNode = null;
                        enterDirection = default;
                        length = default;
                        return false;
                    }
                    currDirection = nextDirection.Value;
                }
                throw new InvalidOperationException("Should not happen");
            }

            foreach (var (pos, node) in nodes)
            {
                foreach (var direction in _directions)
                {
                    if (node[direction] is not null) continue;
                    if (IsWalkable(pos + Offset(direction)))
                    {
                        if (FindNodeAtOtherEnd(pos, direction, out var otherNode, out var enterDirection, out var length))
                        {
                            if (otherNode is null) throw new InvalidOperationException("Should not happen");
                            node[direction] = new ClosedEdge(otherNode, length);
                            Debug.Assert(otherNode[enterDirection] is null, $"Node at {otherNode.Position} already has a connection in direction {Invert(enterDirection)} when attempting to connect {node.Position} in direction {direction}");
                            otherNode[enterDirection] = new ClosedEdge(node, length);
                        }
                        else
                        {
                            node[direction] = OpenEdge.Instance;
                        }
                    }
                    else
                    {
                        node[direction] = OpenEdge.Instance;
                    }
                }
            }

            var taken = new HashSet<Node>();
            var startNode = nodes.Values.First(x => x is StartNode);
            var stack = new Stack<((Vec2 p1, Vec2 p2) key, int weight)>();
            var foundPaths = new List<((Vec2 p1, Vec2 p2) key, int weight)[]>();

            /* Optimization based on special shape */
            var edgeNodesBannedDirection = new Dictionary<Vec2, HashSet<Vec2>>();
            {
                void AddBan(Vec2 fromPos, Vec2 toPos)
                {
                    if (!edgeNodesBannedDirection.TryGetValue(fromPos, out var banned))
                    {
                        banned = new HashSet<Vec2>();
                        edgeNodesBannedDirection.Add(fromPos, banned);
                    }
                    banned.Add(toPos);
                }
                var startCorner = startNode.Edges().First().Node;
                var endCorner = nodes.Values.First(x => x is EndNode).Edges().First().Node;
                var paths = startCorner.Edges().Where(x => x.Node is not StartNode).Select(x => x.Node).ToArray();
                AddBan(startCorner.Position, startNode.Position);
                AddBan(paths[0].Position, startCorner.Position);
                AddBan(paths[1].Position, startCorner.Position);
                foreach (var item in endCorner.Edges().Select(x => x.Node).Where(x => x is not EndNode))
                {
                    AddBan(endCorner.Position, item.Position);
                }
                void PopulateEdgeNodesBannedDirection(int pathIndex)
                {
                    for (int i = 0; i < 1000; ++i)
                    {
                        var nextEdgeNode = paths[pathIndex].Edges().Select(x => x.Node).FirstOrDefault(x => x.Edges().Count() == 3 && !edgeNodesBannedDirection.ContainsKey(x.Position));
                        if (nextEdgeNode is null)
                        {
                            break;
                        }
                        AddBan(nextEdgeNode.Position, paths[pathIndex].Position);
                        paths[pathIndex] = nextEdgeNode;
                    }
                }

                PopulateEdgeNodesBannedDirection(0);
                PopulateEdgeNodesBannedDirection(1);
            }

            int? Walk(Node currNode)
            {
                if (currNode is EndNode)
                {
                    foundPaths.Add(stack.Reverse().ToArray());
                    return 0;
                }
                int? steps = null;
                foreach (var edge in currNode.Edges().Where(e =>
                {
                    var notTaken = !taken.Contains(e.Node);
                    var isBanned = edgeNodesBannedDirection.TryGetValue(currNode.Position, out var banned) && banned.Contains(e.Node.Position);
                    return notTaken && !isBanned;
                }))
                {
                    var edgeSteps = edge.Weight;
                    taken.Add(edge.Node);
                    stack.Push((MakeEdgeKey(currNode, edge), edge.Weight));
                    var childSteps = Walk(edge.Node);
                    stack.Pop();
                    taken.Remove(edge.Node);
                    if (childSteps is not null && (steps is null || (edgeSteps + childSteps) > steps))
                    {
                        steps = edgeSteps + childSteps;
                    }
                }
                return steps;
            }

            taken.Add(startNode);
            var distance = Walk(startNode);
            taken.Remove(startNode);

            if (distance is null)
            {
                throw new InvalidOperationException("No path found");
            }

            return distance.Value;
        }

        private string GraphToDot(Dictionary<Vec2, Node>.ValueCollection values, HashSet<(Vec2 p1, Vec2 p2)>? highlightEdges = null)
        {
            var sb = new StringBuilder();
            var printedEdges = new HashSet<(Vec2, Vec2)>();
            sb.AppendLine("graph {");
            foreach (Node node in values)
            {
                foreach (var direction in _directions)
                {
                    var edge = node[direction];
                    if (edge is ClosedEdge closedEdge)
                    {
                        var key = MakeEdgeKey(node, closedEdge);
                        if (printedEdges.Contains(key)) continue;

                        static string Prnt(Node n)
                        {
                            var prefix =
                                n is EndNode ? "E"
                                : n is StartNode ? "S"
                                : "";
                            return prefix + n.Position.ToString();
                        }

                        var highlight = "";
                        if (highlightEdges is not null && highlightEdges.Contains(key))
                        {
                            highlight = ";color=\"red\";penwidth=3";
                        }
                        sb.AppendLine(CultureInfo.InvariantCulture, $"  \"{Prnt(node)}\" -- \"{Prnt(closedEdge.Node)}\" [label=\"{closedEdge.Weight}\";weight={closedEdge.Weight}{highlight}];");
                        printedEdges.Add(key);
                    }
                }
            }
            return sb.AppendLine("}").ToString();
        }
    }
}
