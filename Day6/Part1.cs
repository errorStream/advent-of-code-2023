namespace Day6
{
    public class Part1 : CommonFunctionality, Framework.ISolution
    {
        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            List<long> times = new();
            List<long> distances = new();
            while (streamReader.ReadLine() is string line)
            {
                if (line.StartsWith("Time:", StringComparison.Ordinal))
                {
                    times.AddRange(line["Time:".Length..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse));
                }
                else if (line.StartsWith("Distance:", StringComparison.Ordinal))
                {
                    distances.AddRange(line["Distance:".Length..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse));
                }
                else
                {
                    throw new ArgumentException("Invalid line");
                }
            }

            long result = 1;

            for (int i = 0; i < times.Count; ++i)
            {
                result *= ComputeWinOptionCount(times[i], distances[i]);
            }

            return result;
        }
    }
}
