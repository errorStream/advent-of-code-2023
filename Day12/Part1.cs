namespace Day12
{
    public class Part1 : CommonFunctionality, Framework.ISolution
    {
        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            long total = 0;
            while (streamReader.ReadLine() is string line)
            {
                var parts = line.Split(' ');
                var springStatus = parts[0];
                var groups = parts[1].Split(',').Select(int.Parse).ToArray();

                var validCount = ArrangementCount(springStatus, groups);

                total += validCount;
            }

            return total;
        }
    }
}
