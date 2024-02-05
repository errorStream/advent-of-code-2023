namespace Day9
{
    public class Part2 : Framework.ISolution
    {
        private static int[] ComputeDifferences(IList<int> items)
        {
            var res = new int[items.Count - 1];
            for (int i = 0; i < res.Length; ++i)
            {
                res[i] = items[i + 1] - items[i];
            }
            return res;
        }

        private static int[] ComputePrev(IList<int> differences, IList<int> items)
        {
            var res = new int[items.Count + 1];
            for (int i = 0; i < items.Count; ++i)
            {
                res[i + 1] = items[i];
            }
            res[0] = items[0] - differences[0];
            return res;
        }

        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            var sum = 0;
            while (streamReader.ReadLine() is string line)
            {
                var items = line.Split(' ').Select(int.Parse).ToArray();
                if (items.Length == 0)
                {
                    continue;
                }
                var stack = new Stack<int[]>();
                stack.Push(items);
                while (stack.Peek().Any(x => x != 0))
                {
                    stack.Push(ComputeDifferences(stack.Peek()));
                }
                stack.Push(stack.Pop().Append(0).ToArray());
                while (stack.Count > 1)
                {
                    stack.Push(ComputePrev(stack.Pop(), stack.Pop()));
                }
                sum += stack.Pop()[0];
            }

            return sum;
        }
    }
}
