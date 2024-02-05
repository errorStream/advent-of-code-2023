namespace Day21
{
    public abstract class CommonFunctionality
    {
        private static readonly IReadOnlyCollection<(int x, int y)> _directions = new[]
        {
            (0, -1),
            (1, 0),
            (0, 1),
            (-1, 0)
        };

        protected static char[,] ReadGrid(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            List<string> lines = new();
            while (streamReader.ReadLine() is string line)
            {
                lines.Add(line);
            }
            char[,] grid = new char[lines[0].Length, lines.Count];
            for (int y = 0; y < lines.Count; ++y)
            {
                for (int x = 0; x < lines[0].Length; ++x)
                {
                    grid[x, y] = lines[y][x];
                }
            }
            return grid;
        }

        protected static HashSet<(int x, int y)> CalcEndPoints(int stepCount, char[,] grid, (int x, int y)? start = null)
        {
            ArgumentNullException.ThrowIfNull(grid);
            (int x, int y) FindStart()
            {
                for (int x = 0; x < grid.GetLength(0); ++x)
                {
                    for (int y = 0; y < grid.GetLength(1); ++y)
                    {
                        if (grid[x, y] == 'S')
                        {
                            return (x, y);
                        }
                    }
                }

                throw new ArgumentException("No start found");
            }

            start ??= FindStart();

            bool Walkable(int x, int y)
            {
                return x >= 0 && x < grid.GetLength(0)
                    && y >= 0 && y < grid.GetLength(1)
                    && grid[x, y] != '#';
            }

            HashSet<(int x, int y)> endPoints = new();
            HashSet<(int x, int y)> visited = new();
            Queue<(int x, int y, int stepsLeft)> queue = new();
            queue.Enqueue((start.Value.x, start.Value.y, stepCount));
            while (queue.Count > 0)
            {
                (int x, int y, int stepsLeft) = queue.Dequeue();
                if (stepsLeft % 2 == 0)
                {
                    endPoints.Add((x, y));
                }
                if (stepsLeft == 0)
                {
                    continue;
                }
                foreach ((int dx, int dy) in _directions)
                {
                    (int x, int y) curr = (x + dx, y + dy);
                    if (!visited.Contains(curr) && Walkable(curr.x, curr.y))
                    {
                        queue.Enqueue((x + dx, y + dy, stepsLeft - 1));
                        visited.Add(curr);
                    }
                }
            }

            return endPoints;
        }
    }
}
