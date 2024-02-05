namespace Day6
{
    public class Part2 : CommonFunctionality, Framework.ISolution
    {
        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            long time = default;
            long distance = default;
            while (streamReader.ReadLine() is string line)
            {
                if (line.StartsWith("Time:", StringComparison.Ordinal))
                {
                    time = long.Parse(line["Time:".Length..].Replace(" ", "", StringComparison.Ordinal), System.Globalization.CultureInfo.InvariantCulture);
                }
                else if (line.StartsWith("Distance:", StringComparison.Ordinal))
                {
                    distance = long.Parse(line["Distance:".Length..].Replace(" ", "", StringComparison.Ordinal), System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    throw new ArgumentException("Invalid line");
                }
            }

            return ComputeWinOptionCount(time, distance);
        }
    }
}
