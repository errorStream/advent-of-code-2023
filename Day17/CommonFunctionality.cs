namespace Day17
{
    public abstract class CommonFunctionality
    {
        protected static int[,] MakeWeightsGrid(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            List<string> lines = new();
            while (streamReader.ReadLine() is string line)
            {
                lines.Add(line);
            }
            int[,] weights = new int[lines[0].Length, lines.Count];
            for (int x = 0; x < weights.GetLength(0); ++x)
            {
                for (int y = 0; y < weights.GetLength(1); ++y)
                {
                    weights[x, y] = lines[y][x] - '0';
                }
            }

            return weights;
        }

        protected static readonly IReadOnlyCollection<Vec2> Offsets = new Vec2[]
        {
            new (0, 1),
            new (0, -1),
            new (1, 0),
            new (-1, 0),
        };

        protected class NodeComparer : IComparer<Node>
        {
            public int Compare(Node? x, Node? y)
            {
                if (x is null && y is null)
                {
                    return 0;
                }
                else if (x is null)
                {
                    return -1;
                }
                else if (y is null)
                {
                    return 1;
                }
                else
                {
                    int res = x.Distance.CompareTo(y.Distance);
                    if (res == 0)
                    {
                        res = (x.Position, x.Direction, x.ConsecutiveLength).CompareTo((y.Position, y.Direction, y.ConsecutiveLength));
                    }
                    return res;
                }
            }
        }

        protected static void PrintGridWithFinalPath(int[,] weights, Node endNode)
        {
            ArgumentNullException.ThrowIfNull(weights);
            int?[,] grid = new int?[weights.GetLength(0), weights.GetLength(1)];
            Node? curr = endNode;
            while (curr is not null)
            {
                grid[curr.Position.X, curr.Position.Y] = curr.Distance;
                curr = curr.Parent;
            }

            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    int? weight = grid[x, y];
                    if (weight is null)
                    {
                        Console.Write("[   ]");
                    }
                    else
                    {
                        Console.Write($"{weight.Value,5}");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
