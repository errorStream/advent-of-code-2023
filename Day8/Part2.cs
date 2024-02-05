using System.Diagnostics;

namespace Day8
{
    public class Part2 : Framework.ISolution
    {
        private static long GreatestCommonDivisor(long a, long b)
        {
            Debug.Assert(a > 0 && b > 0);
            while (b != 0)
            {
                var remainder = a % b;
                a = b;
                b = remainder;
            }
            return a;
        }

        private static long LeastCommonMultiple(long a, long b)
        {
            return (a * b) / GreatestCommonDivisor(a, b);
        }

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

            if (directions is null)
            {
                throw new ArgumentException("No directions found");
            }

            int ComputeEndPoint(string node)
            {
                var step = 0;
                while (node[^1] != 'Z')
                {
                    var normalizedStep = step % directions.Length;
                    node = directions[normalizedStep] == 'L' ? nodes[node].left : nodes[node].right;
                    step++;
                }
                return step;
            }

            return nodes
                .Keys
                .Where(x => x[^1] == 'A')
                .Select(x => (long)(ComputeEndPoint(x) / directions.Length))
                .Aggregate(LeastCommonMultiple)
                * directions.Length;
        }
    }
}
