namespace Day8
{
    public class Part1 : Framework.ISolution
    {
        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            string? directions = null;
            Dictionary<string, (string left, string right)> nodes = new();
            while (streamReader.ReadLine() is string line)
            {
                if (directions == default)
                {
                    directions = line;
                }
                else if (string.IsNullOrWhiteSpace(line))
                {
                    // SKIP
                }
                else
                {
                    var parent = line[0..3];
                    var leftChild = line[7..10];
                    var rightChild = line[12..15];

                    nodes.Add(parent, (leftChild, rightChild));
                }
            }

            const string start = "AAA";
            const string target = "ZZZ";
            if (directions is null)
            {
                throw new ArgumentException("No directions found");
            }
            var curr = start;
            int steps = 0;
            while (curr != target)
            {
                var direction = directions[steps % directions.Length];
                curr = direction == 'L' ? nodes[curr].left : nodes[curr].right;
                steps++;
            }

            return steps;
        }
    }
}
