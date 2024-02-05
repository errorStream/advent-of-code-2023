namespace Day14
{
    public class Part1 : Framework.ISolution
    {
        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            List<string> lines = new();
            while (streamReader.ReadLine() is string line)
            {
                lines.Add(line);
            }
            char[,] grid = new char[lines[0].Length, lines.Count];
            int[] heights = new int[grid.GetLength(0)];
            int totalLoad = 0;
            for (int x = 0; x < grid.GetLength(0); ++x)
            {
                for (int y = 0; y < grid.GetLength(1); ++y)
                {
                    grid[x, y] = lines[y][x];
                }
            }
            for (int y = 0; y < grid.GetLength(1); ++y)
            {
                for (int x = 0; x < grid.GetLength(0); ++x)
                {
                    var ch = grid[x, y];
                    if (ch == '#')
                    {
                        heights[x] = y + 1;
                    }
                    else if (ch == 'O')
                    {
                        var load = grid.GetLength(1) - heights[x];
                        totalLoad += load;
                        heights[x]++;
                    }
                }
            }
            return totalLoad;
        }
    }
}
