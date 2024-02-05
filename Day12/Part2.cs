namespace Day12
{
    public class Part2 : CommonFunctionality, Framework.ISolution
    {
        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            long total = 0;
            while (streamReader.ReadLine() is string line)
            {
                var parts = line.Split(' ');
                var springStatus = string.Join('?', Enumerable.Repeat(parts[0], 5));
                var groups = Enumerable.Repeat(parts[1].Split(',').Select(int.Parse), 5).SelectMany(x => x).ToArray();

                long validCount = ArrangementCount(springStatus, groups);

                total += validCount;
            }

            return total;
        }
    }
}
