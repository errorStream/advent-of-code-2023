namespace Day15
{
    public class Part1 : CommonFunctionality, Framework.ISolution
    {
        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            var total = 0;
            var operations = new List<string>();
            while (streamReader.ReadLine() is string line)
            {
                operations.AddRange(line.Split(','));
            }

            foreach (var item in operations)
            {
                total += HashAlgorithm(item);
            }

            return total;
        }
    }
}
