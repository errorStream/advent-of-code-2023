namespace Day10
{
    public abstract class CommonFunctionality
    {
        protected static (List<string> grid, (int x, int y) start) Parse(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            var grid = new List<string>();
            while (streamReader.ReadLine() is string line)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                grid.Add(line);
            }

            var sTiles = Enumerable.Range(0, grid.Count)
                .SelectMany(y => Enumerable.Range(0, grid[y].Length).Select(x => (x, y)))
                .Where(p => grid[p.y][p.x] == 'S')
                .ToArray();

            if (sTiles.Length > 1)
            {
                throw new ArgumentException("Multiple start points");
            }

            if (sTiles.Length == 0)
            {
                throw new ArgumentException("Start point not found");
            }

            return (grid, sTiles[0]);
        }

        protected static IList<(int x, int y)> FindPath(IList<string> grid, (int x, int y) start)
        {
            ArgumentNullException.ThrowIfNull(grid);
            List<(int x, int y)> results = new();
            (int x, int y)? prev = null;
            (int x, int y) pos = start;

            char? Lookup((int x, int y) pos)
            {
                if (pos.y < 0 || pos.y >= grid.Count)
                {
                    return null;
                }
                var row = grid[pos.y];
                if (pos.x < 0 || pos.x >= row.Length)
                {
                    return null;
                }
                return row[pos.x];
            }

            bool found = false;

            for (int i = 0; i < 1000000; ++i)
            {
                if (i > 2 && grid[pos.y][pos.x] == 'S')
                {
                    // At end
                    found = true;
                    break;
                }

                results.Add(pos);

                var up = (pos.x, pos.y - 1);
                var left = (pos.x - 1, pos.y);
                var right = (pos.x + 1, pos.y);
                var down = (pos.x, pos.y + 1);

                if (Lookup(pos) is not char ch) { throw new ArgumentException("Invalid position"); }

                var upIsConnected = Lookup(up).HasValue && ((ch == 'S' && "|7F".Contains(grid[pos.y - 1][pos.x], StringComparison.Ordinal)) || "|LJ".Contains(ch, StringComparison.Ordinal));
                var leftIsConnected = Lookup(left).HasValue && ((ch == 'S' && "-LF".Contains(grid[pos.y - 1][pos.x], StringComparison.Ordinal)) || "-J7".Contains(ch, StringComparison.Ordinal));
                var rightIsConnected = Lookup(right).HasValue && ((ch == 'S' && "-J7".Contains(grid[pos.y - 1][pos.x], StringComparison.Ordinal)) || "-LF".Contains(ch, StringComparison.Ordinal));
                var downIsConnected = Lookup(down).HasValue && ((ch == 'S' && "|LJ".Contains(grid[pos.y - 1][pos.x], StringComparison.Ordinal)) || "|7F".Contains(ch, StringComparison.Ordinal));

                if (upIsConnected && prev != up)
                {
                    prev = pos;
                    pos = up;
                }
                else if (leftIsConnected && prev != left)
                {
                    prev = pos;
                    pos = left;
                }
                else if (rightIsConnected && prev != right)
                {
                    prev = pos;
                    pos = right;
                }
                else if (downIsConnected && prev != down)
                {
                    prev = pos;
                    pos = down;
                }
                else
                {
                    throw new ArgumentException("No path found");
                }
            }

            if (!found)
            {
                throw new ArgumentException("No path found");
            }

            return results;
        }
    }
}
