using System.Diagnostics;

namespace Day22
{
    public abstract class CommonFunctionality
    {
        protected readonly record struct Point2D(int X, int Y);

        protected readonly record struct Point3D(int X, int Y, int Z);

        protected readonly record struct Brick(Point3D Point1, Point3D Point2)
        {
            private enum Axis
            {
                X,
                Y,
                Z
            };

            public int MinZ => Math.Min(Point1.Z, Point2.Z);
            public int MaxZ => Math.Max(Point1.Z, Point2.Z);

            public IEnumerable<Point3D> PointsInBounds()
            {
                for (int x = Point1.X; x <= Point2.X; ++x)
                {
                    for (int y = Point1.Y; y <= Point2.Y; ++y)
                    {
                        for (int z = Point1.Z; z <= Point2.Z; ++z)
                        {
                            yield return new Point3D(x, y, z);
                        }
                    }
                }
            }

            public Brick Settle(int floorLevel)
            {
                var newPoint1 = new Point3D(Point1.X, Point1.Y, floorLevel + 1);
                var newPoint2 = new Point3D(Point2.X, Point2.Y, floorLevel + 1 + MaxZ - MinZ);
                return new Brick(newPoint1, newPoint2);
            }

            private Axis LongAxis
            {
                get
                {
                    if (Point1.X != Point2.X)
                    {
                        return Axis.X;
                    }
                    else if (Point1.Y != Point2.Y)
                    {
                        return Axis.Y;
                    }
                    else if (Point1.Z != Point2.Z)
                    {
                        return Axis.Z;
                    }
                    else
                    {
                        return Axis.X;
                    }
                }
            }

            public IEnumerable<(Point2D, int)> RelativeHeights()
            {
                if (LongAxis == Axis.Z)
                {
                    yield return (new Point2D(Point1.X, Point1.Y), MaxZ - MinZ + 1);
                }
                else
                {
                    foreach (var point in PointsInBounds())
                    {
                        yield return (new Point2D(point.X, point.Y), 1);
                    }
                }
            }
        }

        protected static IList<Brick> ParseSnapshot(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            var snapshot = new List<Brick>();

            while (streamReader.ReadLine() is string line)
            {
                var parts = line.Split('~');
                var point1Items = parts[0].Split(',').Select(int.Parse).ToArray();
                var point2Items = parts[1].Split(',').Select(int.Parse).ToArray();

                var point1 = new Point3D(point1Items[0], point1Items[1], point1Items[2]);
                var point2 = new Point3D(point2Items[0], point2Items[1], point2Items[2]);
                snapshot.Add(new Brick(point1, point2));
            }

            snapshot.Sort((b1, b2) => b1.MinZ.CompareTo(b2.MinZ));

            return snapshot;
        }

        protected record Node(Brick Brick)
        {
            public HashSet<Node> SupportedBy { get; } = new();
            public HashSet<Node> Supports { get; } = new();
        };

        protected static void PrintTree(HashSet<Node> allBricks)
        {
            if (allBricks is null || allBricks.Count == 0)
            {
                Console.WriteLine("No bricks");
                return;
            }
            foreach (var node in allBricks)
            {
                Console.WriteLine($"Node: {node.Brick.Point1} {node.Brick.Point2}");
                Console.WriteLine($"  Supports: {string.Join(", ", node.Supports.Select(x => $"{x.Brick.Point1} {x.Brick.Point2}"))}");
                Console.WriteLine($"  SupportedBy: {string.Join(", ", node.SupportedBy.Select(x => $"{x.Brick.Point1} {x.Brick.Point2}"))}");
            }
        }

        protected static HashSet<Node> BuildTree(IList<Brick> snapshot)
        {
            ArgumentNullException.ThrowIfNull(snapshot);
            var heights = new Dictionary<Point2D, (int height, Node node)>();
            var allBricks = new HashSet<Node>();

            for (int i = 0; i < snapshot.Count; i++)
            {
                Brick brick = snapshot[i];
                var maxHeight = 0;
                var supports = new HashSet<Node>();
                foreach (var point in brick.PointsInBounds())
                {
                    if (heights.TryGetValue(new Point2D(point.X, point.Y), out var entry))
                    {
                        if (entry.height > maxHeight)
                        {
                            supports.Clear();
                            supports.Add(entry.node);
                            maxHeight = entry.height;
                        }
                        else if (entry.height == maxHeight)
                        {
                            supports.Add(entry.node);
                        }
                    }
                }
                Debug.Assert(maxHeight < brick.MinZ);
                var settledBrick = brick.Settle(maxHeight);
                var newNode = new Node(brick);
                allBricks.Add(newNode);
                foreach (var support in supports)
                {
                    newNode.SupportedBy.Add(support);
                    support.Supports.Add(newNode);
                }
                foreach (var (point2d, relativeHeight) in brick.RelativeHeights())
                {
                    heights[point2d] = (maxHeight + relativeHeight, newNode);
                }
            }

            return allBricks;
        }
    }
}
