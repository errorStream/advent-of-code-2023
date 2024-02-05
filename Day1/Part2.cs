namespace Day1
{
    public class Part2 : Framework.ISolution
    {
        private sealed class TrieNode
        {
            public Dictionary<char, TrieNode> Children = new();
            public int? Value;
        }

        private readonly TrieNode _root = new();

        private void Add(string str, int val)
        {
            var node = _root;
            for (int i = 0; i < str.Length; ++i)
            {
                var ch = str[i];
                if (!node.Children.TryGetValue(ch, out var next))
                {
                    next = new TrieNode();
                    node.Children.Add(ch, next);
                }
                node = next;
            }
            if (node.Value is not null)
            {
                throw new InvalidOperationException($"Word conflict at 'str'");
            }
            node.Value = val;
        }

        private int? Find(string str, int start = 0)
        {
            var node = _root;
            for (int i = start; i < str.Length; ++i)
            {
                var ch = str[i];
                if (node.Children.TryGetValue(ch, out var next))
                {
                    if (next.Value is null)
                    {
                        node = next;
                    }
                    else
                    {
                        return next.Value;
                    }
                }
                else
                {
                    return null;
                }
            }
            return node.Value;
        }

        public Part2()
        {
            Add("1", 1);
            Add("2", 2);
            Add("3", 3);
            Add("4", 4);
            Add("5", 5);
            Add("6", 6);
            Add("7", 7);
            Add("8", 8);
            Add("9", 9);
            Add("one", 1);
            Add("two", 2);
            Add("three", 3);
            Add("four", 4);
            Add("five", 5);
            Add("six", 6);
            Add("seven", 7);
            Add("eight", 8);
            Add("nine", 9);
        }

        private int FindCalibrationValue(string str)
        {
            var res = 0;
            for (int i = 0; i < str.Length; ++i)
            {
                if (Find(str, i) is int val)
                {
                    res += val * 10;
                    break;
                }
            }

            for (int i = str.Length - 1; i >= 0; --i)
            {
                if (Find(str, i) is int val)
                {
                    res += val;
                    break;
                }
            }

            return res;
        }

        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            var total = 0;
            while (streamReader.ReadLine() is string line)
            {
                total += FindCalibrationValue(line);
            }
            return total;
        }
    }
}
