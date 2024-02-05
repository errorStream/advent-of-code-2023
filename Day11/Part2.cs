namespace Day11
{
    public class Part2 : Framework.ISolution
    {
        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            var lines = new List<string>();
            while (streamReader.ReadLine() is string line)
            {
                lines.Add(line);
            }
            var grid = new char[lines[0].Length, lines.Count];
            for (int x = 0; x < grid.GetLength(0); ++x)
            {
                for (int y = 0; y < grid.GetLength(1); ++y)
                {
                    grid[x, y] = lines[y][x];
                }
            }

            var columnsToExpand = new SortedSet<int>(Enumerable.Range(0, grid.GetLength(0)));
            var rowsToExpand = new SortedSet<int>(Enumerable.Range(0, grid.GetLength(1)));
            var galaxyLocations = new List<(int x, int y)>();
            for (int x = 0; x < grid.GetLength(0); ++x)
            {
                for (int y = 0; y < grid.GetLength(1); ++y)
                {
                    if (grid[x, y] == '#')
                    {
                        columnsToExpand.Remove(x);
                        rowsToExpand.Remove(y);
                        galaxyLocations.Add((x, y));
                    }
                }
            }

            long res = 0;

            for (int i = 0; i < galaxyLocations.Count; ++i)
            {
                for (int j = 0; j < galaxyLocations.Count; ++j)
                {
                    if (j >= i)
                    {
                        continue;
                    }
                    var g1 = galaxyLocations[i];
                    var g2 = galaxyLocations[j];
                    var expandColumnCount = columnsToExpand.GetViewBetween(Math.Min(g1.x, g2.x), Math.Max(g1.x, g2.x)).Count;
                    var expandRowCount = rowsToExpand.GetViewBetween(Math.Min(g1.y, g2.y), Math.Max(g1.y, g2.y)).Count;
                    long d = Math.Abs(g2.x - g1.x)
                        + (expandColumnCount * 999_999)
                        + Math.Abs(g2.y - g1.y)
                        + (expandRowCount * 999_999);
                    res += d;
                }
            }

            return res;
        }
    }
}
