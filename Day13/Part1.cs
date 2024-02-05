namespace Day13
{
    public class Part1 : Framework.ISolution
    {
        private static bool CheckVerticalMirror(char[,] block, int pos)
        {
            int size = Math.Min(pos + 1, block.GetLength(1) - pos - 1);
            for (int i = 0; i < size; ++i)
            {
                for (int x = 0; x < block.GetLength(0); ++x)
                {
                    var above = block[x, pos - i];
                    var below = block[x, pos + i + 1];
                    if (above != below)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool CheckHorizontalMirror(char[,] block, int pos)
        {
            int size = Math.Min(pos + 1, block.GetLength(0) - pos - 1);
            for (int i = 0; i < size; ++i)
            {
                for (int y = 0; y < block.GetLength(1); ++y)
                {
                    var left = block[pos - i, y];
                    var right = block[pos + i + 1, y];
                    if (left != right)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            var blocks = new List<char[,]>();
            var currBlockLines = new List<string>();
            void CompleteBlock()
            {
                if (currBlockLines.Count > 0)
                {
                    var block = new char[currBlockLines[0].Length, currBlockLines.Count];
                    for (int x = 0; x < block.GetLength(0); ++x)
                    {
                        for (int y = 0; y < block.GetLength(1); ++y)
                        {
                            block[x, y] = currBlockLines[y][x];
                        }
                    }
                    currBlockLines.Clear();
                    blocks.Add(block);
                }
            }
            while (streamReader.ReadLine() is string line)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    CompleteBlock();
                }
                else
                {
                    currBlockLines.Add(line);
                }
            }
            CompleteBlock();

            var res = 0;

            foreach (var block in blocks)
            {
                for (int i = 0; i < block.GetLength(0) - 1; ++i)
                {
                    if (CheckHorizontalMirror(block, i))
                    {
                        res += i + 1;
                    }
                }
                for (int j = 0; j < block.GetLength(1) - 1; ++j)
                {
                    if (CheckVerticalMirror(block, j))
                    {
                        res += (j + 1) * 100;
                    }
                }
            }

            return res;
        }
    }
}
