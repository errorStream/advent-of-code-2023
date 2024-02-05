namespace Day10
{
    public class Part2 : CommonFunctionality, Framework.ISolution
    {
        public long Run(StreamReader streamReader)
        {
            var (grid, start) = Parse(streamReader);

            var route = FindPath(grid, start).ToHashSet();
            if (route is null)
            {
                throw new ArgumentException("Route not found");
            }
            var minX = int.MaxValue;
            var maxX = int.MinValue;
            var minY = int.MaxValue;
            var maxY = int.MinValue;
            foreach (var (x, y) in route)
            {
                minX = Math.Min(minX, x);
                maxX = Math.Max(maxX, x);
                minY = Math.Min(minY, y);
                maxY = Math.Max(maxY, y);
            }
            int count = 0;
            for (var y = minY; y <= maxY; y++)
            {
                bool inside = false;
                char? edgeStart = null;
                for (var x = minX; x <= maxX; x++)
                {
                    if (route.Contains((x, y)))
                    {
                        var ch = grid[y][x];
                        if (ch == 'S')
                        {
                            bool up = y != minY && "|7F".Contains(grid[y - 1][x], StringComparison.Ordinal);
                            bool left = x != minX && "-LF".Contains(grid[y][x - 1], StringComparison.Ordinal);
                            bool right = x != maxX && "-J7".Contains(grid[y][x + 1], StringComparison.Ordinal);
                            bool down = y != maxY && "|LJ".Contains(grid[y + 1][x], StringComparison.Ordinal);

                            if (up && left)
                            {
                                ch = 'J';
                            }
                            else if (up && right)
                            {
                                ch = 'L';
                            }
                            else if (down && left)
                            {
                                ch = '7';
                            }
                            else if (down && right)
                            {
                                ch = 'F';
                            }
                            else
                            {
                                throw new ArgumentException("Invalid start");
                            }
                        }
                        if (ch == '|')
                        {
                            if (edgeStart is not null)
                            {
                                throw new ArgumentException("Edge start is null");
                            }
                            // Toggle state
                            inside = !inside;
                        }
                        else if (ch == 'L')
                        {
                            if (edgeStart is not null)
                            {
                                throw new ArgumentException("Edge start is not null");
                            }
                            edgeStart = 'L';
                        }
                        else if (ch == 'J')
                        {
                            if (edgeStart is null)
                            {
                                throw new ArgumentException("Edge start is null");
                            }
                            if (edgeStart == 'F')
                            {
                                // toggle inside
                                inside = !inside;
                            }
                            else if (edgeStart == 'L')
                            {
                                // don't toggle inside
                            }
                            else
                            {
                                throw new ArgumentException("Edge start is not L or F");
                            }
                            edgeStart = null;

                        }
                        else if (ch == '7')
                        {
                            if (edgeStart is null)
                            {
                                throw new ArgumentException("Edge start is null");
                            }
                            if (edgeStart == 'L')
                            {
                                // toggle inside
                                inside = !inside;
                            }
                            else if (edgeStart == 'F')
                            {
                                // don't toggle inside
                            }
                            else
                            {
                                throw new ArgumentException("Edge start is not L or F");
                            }
                            edgeStart = null;
                        }
                        else if (ch == 'F')
                        {
                            if (edgeStart is not null)
                            {
                                throw new ArgumentException("Edge start is not null");
                            }
                            edgeStart = 'F';
                        }
                        else if (ch == '-')
                        {
                            if (edgeStart is null)
                            {
                                throw new ArgumentException("Edge start is null");
                            }
                        }
                    }
                    else if (inside)
                    {
                        count++;
                    }
                }
            }


            return count;
        }
    }
}
