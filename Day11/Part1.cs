namespace Day11
{
    public class Part1 : Framework.ISolution
    {
        private static char[,] Expand(char[,] src)
        {
            var columnsToExpand = new SortedSet<int>(Enumerable.Range(0, src.GetLength(0)));
            var rowsToExpand = new SortedSet<int>(Enumerable.Range(0, src.GetLength(1)));

            for (int x = 0; x < src.GetLength(0); ++x)
            {
                for (int y = 0; y < src.GetLength(1); ++y)
                {
                    if (src[x, y] == '#')
                    {
                        columnsToExpand.Remove(x);
                        rowsToExpand.Remove(y);
                    }
                }
            }

            var res = new char[src.GetLength(0) + columnsToExpand.Count, src.GetLength(1) + rowsToExpand.Count];
            var data = new List<char>(res.GetLength(0) * res.GetLength(1));

            for (int y = 0; y < src.GetLength(1); ++y)
            {
                if (rowsToExpand.Contains(y))
                {
                    data.AddRange(Enumerable.Repeat('.', res.GetLength(0)));
                }
                for (int x = 0; x < src.GetLength(0); ++x)
                {
                    if (columnsToExpand.Contains(x))
                    {
                        data.Add('.');
                    }
                    data.Add(src[x, y]);
                }
            }

            for (int i = 0; i < data.Count; ++i)
            {
                var x = i % res.GetLength(0);
                var y = i / res.GetLength(0);
                res[x, y] = data[i];
            }

            return res;
        }

        private static void PrintGrid(char[,] grid)
        {
            for (int y = 0; y < grid.GetLength(1); ++y)
            {
                for (int x = 0; x < grid.GetLength(0); ++x)
                {
                    Console.Write(grid[x, y]);
                }
                Console.WriteLine();
            }
        }

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

            grid = Expand(grid);

            var galaxyLocations = new List<(int x, int y)>();

            for (int x = 0; x < grid.GetLength(0); ++x)
            {
                for (int y = 0; y < grid.GetLength(1); ++y)
                {
                    if (grid[x, y] == '#')
                    {
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
                    var d = Math.Abs(g2.x - g1.x) + Math.Abs(g2.y - g1.y);
                    res += d;
                }
            }

            return res;
        }
    }
}
