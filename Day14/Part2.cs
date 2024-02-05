namespace Day14
{
    public class Part2 : Framework.ISolution
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
            for (int x = 0; x < grid.GetLength(0); ++x)
            {
                for (int y = 0; y < grid.GetLength(1); ++y)
                {
                    grid[x, y] = lines[y][x];
                }
            }
            var gridBU = (char[,])grid.Clone();
            void SlideNorth()
            {
                int[] heights = new int[grid.GetLength(0)];
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
                            grid[x, y] = '.';
                            grid[x, heights[x]] = 'O';
                            heights[x]++;
                        }
                    }
                }
            }

            void SlideEast()
            {
                int[] heights = new int[grid.GetLength(1)];
                for (int x = grid.GetLength(0) - 1; x >= 0; --x)
                {
                    for (int y = 0; y < grid.GetLength(1); ++y)
                    {
                        var ch = grid[x, y];
                        if (ch == '#')
                        {
                            heights[y] = grid.GetLength(0) - x;
                        }
                        else if (ch == 'O')
                        {
                            grid[x, y] = '.';
                            grid[grid.GetLength(0) - heights[y] - 1, y] = 'O';
                            heights[y]++;
                        }
                    }
                }
            }

            void SlideSouth()
            {
                int[] heights = new int[grid.GetLength(0)];
                for (int y = grid.GetLength(1) - 1; y >= 0; --y)
                {
                    for (int x = 0; x < grid.GetLength(0); ++x)
                    {
                        var ch = grid[x, y];
                        if (ch == '#')
                        {
                            heights[x] = grid.GetLength(1) - y;
                        }
                        else if (ch == 'O')
                        {
                            grid[x, y] = '.';
                            grid[x, grid.GetLength(1) - heights[x] - 1] = 'O';
                            heights[x]++;
                        }
                    }
                }
            }

            void SlideWest()
            {
                int[] heights = new int[grid.GetLength(1)];
                for (int x = 0; x < grid.GetLength(0); ++x)
                {
                    for (int y = 0; y < grid.GetLength(1); ++y)
                    {
                        var ch = grid[x, y];
                        if (ch == '#')
                        {
                            heights[y] = x + 1;
                        }
                        else if (ch == 'O')
                        {
                            grid[x, y] = '.';
                            grid[heights[y], y] = 'O';
                            heights[y]++;
                        }
                    }
                }
            }

            int ComputeGridHash()
            {
                int hash = 0;
                for (int y = 0; y < grid.GetLength(1); ++y)
                {
                    for (int x = 0; x < grid.GetLength(0); ++x)
                    {
                        hash = (hash, grid[x, y]).GetHashCode();
                    }
                }
                return hash;
            }

            void Cycle()
            {
                SlideNorth();
                SlideWest();
                SlideSouth();
                SlideEast();
            }

            var hashHistory = new Dictionary<int, int>();

            int loopStart = 0;
            int loopEnd = 0;

            for (int i = 0; true; ++i)
            {
                Cycle();
                var hash = ComputeGridHash();
                if (hashHistory.TryGetValue(hash, out int value))
                {
                    loopStart = value;
                    loopEnd = i;
                    break;
                }
                else
                {
                    hashHistory.Add(hash, i);
                }
            }

            int ComputeLoad()
            {
                int totalLoad = 0;
                for (int y = 0; y < grid.GetLength(1); ++y)
                {
                    for (int x = 0; x < grid.GetLength(0); ++x)
                    {
                        var ch = grid[x, y];
                        if (ch == 'O')
                        {
                            var load = grid.GetLength(1) - y;
                            totalLoad += load;
                        }
                    }
                }
                return totalLoad;
            }

            var equivalentIndex = ((1000000000 - loopStart) % (loopStart - loopEnd)) + loopStart;

            grid = gridBU;

            for (int i = 0; i < equivalentIndex; ++i)
            {
                Cycle();
            }

            return ComputeLoad();
        }
    }
}
