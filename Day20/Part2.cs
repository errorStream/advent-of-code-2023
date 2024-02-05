using System.Diagnostics;

namespace Day20
{
    public class Part2 : CommonFunctionality, Framework.ISolution
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
            var modules = ParseInput(streamReader);

            List<long> cycleCounts = new();

            foreach (var module in modules["broadcaster"].Outputs)
            {
                var cycleCount = 0;
                int place = 0;
                BaseModule? currentModule = modules[module];
                BaseModule? start = currentModule;
                if (modules[module].Outputs.Select(x => modules[x]).First(x => x is ConjunctionModule) is not ConjunctionModule conjunction)
                {
                    throw new ArgumentException($"No conjunction found for {module}");
                }

                while (currentModule is not null)
                {
                    if (!conjunction.Outputs.Contains(currentModule.Name) || currentModule == start)
                    {
                        cycleCount += 1 << place;
                    }
                    place++;
                    currentModule = currentModule.Outputs.Select(x => modules[x]).FirstOrDefault(x => x is FlipFlopModule);
                }
                cycleCounts.Add(cycleCount);
            }

            return cycleCounts.Aggregate(LeastCommonMultiple);
        }
    }
}
