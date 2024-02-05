namespace Day23
{
    public class Part1 : Framework.ISolution
    {
        private readonly IReadOnlyList<(int x, int y)> _directions = new[]
        {
            (0, -1),
            (1, 0),
            (0, 1),
            (-1, 0)
        };

        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
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

            (int x, int y) start = (Enumerable.Range(0, grid.GetLength(0)).First(x => grid[x, 0] == '.'), 0);
            (int x, int y) end = (Enumerable.Range(0, grid.GetLength(0)).First(x => grid[x, grid.GetLength(1) - 1] == '.'), grid.GetLength(1) - 1);

            var taken = new HashSet<(int x, int y)>();
            var stack = new Stack<(int x, int y)>();
            taken.Add(start);
            int? Walk((int x, int y) start, (int x, int y) end)
            {
                stack.Push(start);
                if (start == end)
                {
                    stack.Pop();
                    return 0;
                }
                var walkable = new HashSet<(int x, int y)>();
                bool IsWalkable((int x, int y) pos)
                {
                    var inGrid = pos.x >= 0 && pos.x < grid.GetLength(0) && pos.y >= 0 && pos.y < grid.GetLength(1);
                    var isForest = inGrid && grid[pos.x, pos.y] == '#';
                    var isTaken = taken.Contains(pos);
                    return inGrid && !isForest && !isTaken;
                }
                if (grid[start.x, start.y] == '^')
                {
                    (int x, int y) pos = (start.x, start.y - 1);
                    if (IsWalkable(pos)) { walkable.Add(pos); }
                }
                else if (grid[start.x, start.y] == 'v')
                {
                    (int x, int y) pos = (start.x, start.y + 1);
                    if (IsWalkable(pos)) { walkable.Add(pos); }
                }
                else if (grid[start.x, start.y] == '<')
                {
                    (int x, int y) pos = (start.x - 1, start.y);
                    if (IsWalkable(pos)) { walkable.Add(pos); }
                }
                else if (grid[start.x, start.y] == '>')
                {
                    (int x, int y) pos = (start.x + 1, start.y);
                    if (IsWalkable(pos)) { walkable.Add(pos); }
                }
                else
                {
                    foreach (var direction in _directions)
                    {
                        var pos = (x: start.x + direction.x, y: start.y + direction.y);
                        if (IsWalkable(pos))
                        {
                            walkable.Add(pos);
                        }
                    }
                }
                int? steps = null;
                foreach (var next in walkable)
                {
                    taken.Add(next);
                    var childSteps = Walk(next, end);
                    taken.Remove(next);
                    if (steps is null || childSteps > steps)
                    {
                        steps = childSteps;
                    }
                }
                stack.Pop();
                return steps + 1;
            }

            var result = Walk(start, end);

            if (result is null)
            {
                throw new ArgumentException("No path found");
            }

            return result.Value;
        }

        private static void PrintPath(char[,] grid, Stack<(int x, int y)> stack)
        {
            var path = new HashSet<(int x, int y)>(stack);
            for (int y = 0; y < grid.GetLength(1); ++y)
            {
                for (int x = 0; x < grid.GetLength(0); ++x)
                {
                    if (path.Contains((x, y)))
                    {
                        Console.Write('O');
                    }
                    else
                    {
                        Console.Write(grid[x, y]);
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
