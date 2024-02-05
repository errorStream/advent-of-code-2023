using System.Diagnostics;

namespace Day21
{
    public class Part2 : CommonFunctionality, Framework.ISolution
    {
        private static int CountEndPointsInRegion(char[,] grid, int minX, int minY, int width, int height)
        {
            int res = 0;
            for (int y = minY; y < minY + height; ++y)
            {
                for (int x = minX; x < minX + width; ++x)
                {
                    char ch = grid[x, y];
                    if (ch is 'O' or '@')
                    {
                        res++;
                    }
                }
            }
            return res;
        }

        private static (char[,] grid, (int x, int y)? start) RepeatGrid(char[,] grid, int radius)
        {
            int baseWidth = grid.GetLength(0);
            int baseHeight = grid.GetLength(1);
            char[,] repeatedGrid = new char[baseWidth * ((2 * radius) + 1), baseHeight * ((2 * radius) + 1)];
            (int x, int y)? start = null;
            for (int gx = 0; gx < ((2 * radius) + 1); ++gx)
            {
                for (int gy = 0; gy < ((2 * radius) + 1); ++gy)
                {
                    for (int x = 0; x < baseWidth; ++x)
                    {
                        for (int y = 0; y < baseHeight; ++y)
                        {
                            int gridX = x + (gx * baseWidth);
                            int gridY = y + (gy * baseHeight);
                            repeatedGrid[gridX, gridY] = grid[x, y];
                            if (repeatedGrid[gridX, gridY] == 'S')
                            {
                                if (gx == radius && gy == radius)
                                {
                                    start = (gridX, gridY);
                                }
                                else
                                {
                                    repeatedGrid[gridX, gridY] = '.';
                                }
                            }
                        }
                    }
                }
            }
            return (repeatedGrid, start);
        }

        private static long Perform(int cellRadius, char[,] rawGrid)
        {
            int baseWidth = rawGrid.GetLength(0);
            int baseHeight = rawGrid.GetLength(1);
            Debug.Assert(baseWidth == baseHeight);
            int stepCount = 65 + (131 * cellRadius);
            int radius = (int)Math.Ceiling(stepCount / ((float)baseWidth)) - 1;
            (char[,] grid, (int x, int y)? start) = RepeatGrid(rawGrid, radius);
            if (start is null)
            {
                throw new ArgumentException("No start found");
            }
            HashSet<(int x, int y)> lastReachable = CalcEndPoints(stepCount, grid, start.Value);
            foreach ((int x, int y) in lastReachable)
            {
                grid[x, y] = grid[x, y] == 'S' ? '@' : 'O';
            }
            int center = CountEndPointsInRegion(grid, 4 * 131, 4 * 131, 131, 131);

            int odd = CountEndPointsInRegion(grid, 3 * 131, 4 * 131, 131, 131);
            int even = CountEndPointsInRegion(grid, 3 * 131, 3 * 131, 131, 131);

            int left = CountEndPointsInRegion(grid, 0 * 131, 4 * 131, 131, 131);
            int right = CountEndPointsInRegion(grid, 8 * 131, 4 * 131, 131, 131);
            int top = CountEndPointsInRegion(grid, 4 * 131, 0 * 131, 131, 131);
            int bottom = CountEndPointsInRegion(grid, 4 * 131, 8 * 131, 131, 131);

            int topLeftSmall = CountEndPointsInRegion(grid, 0 * 131, 3 * 131, 131, 131);
            int topLeftBig = CountEndPointsInRegion(grid, 1 * 131, 3 * 131, 131, 131);
            int topRightSmall = CountEndPointsInRegion(grid, 8 * 131, 3 * 131, 131, 131);
            int topRightBig = CountEndPointsInRegion(grid, 7 * 131, 3 * 131, 131, 131);

            int bottomLeftSmall = CountEndPointsInRegion(grid, 0 * 131, 5 * 131, 131, 131);
            int bottomLeftBig = CountEndPointsInRegion(grid, 1 * 131, 5 * 131, 131, 131);
            int bottomRightSmall = CountEndPointsInRegion(grid, 8 * 131, 5 * 131, 131, 131);
            int bottomRightBig = CountEndPointsInRegion(grid, 7 * 131, 5 * 131, 131, 131);

            long cellCount = 202300;

            long res = center
                + left
                + right
                + top
                + bottom
                + (topLeftSmall * cellCount)
                + (topLeftBig * (cellCount - 1))
                + (topRightSmall * cellCount)
                + (topRightBig * (cellCount - 1))
                + (bottomLeftSmall * cellCount)
                + (bottomLeftBig * (cellCount - 1))
                + (bottomRightSmall * cellCount)
                + (bottomRightBig * (cellCount - 1))
                + (odd * (cellCount * cellCount))
                + (even * (((cellCount - 1) * (cellCount - 1)) - 1));

            return res;
        }

        public long Run(StreamReader streamReader)
        {
            char[,] rawGrid = ReadGrid(streamReader);
            return Perform(4, rawGrid);
        }
    }
}
