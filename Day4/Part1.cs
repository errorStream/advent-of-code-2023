namespace Day4
{
    public static class Extensions
    {
        public static void Deconstruct<T>(this IList<T> list, out T first, out T second)
        {
            ArgumentNullException.ThrowIfNull(list);
            first = list[0];
            second = list[1];
        }
    }

    public class Part1 : Framework.ISolution
    {
        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            var total = 0;
            while (streamReader.ReadLine() is string line)
            {
                var (_, gameBody) = line.Split(':');
                var (winningNumbersSection, yourNumbersSection) = gameBody.Split('|');
                var yourWinningNumberCount = yourNumbersSection
                    .Trim()
                    .Split(' ')
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(int.Parse)
                    .Count(winningNumbersSection
                           .Trim()
                           .Split(' ')
                           .Where(x => !string.IsNullOrWhiteSpace(x))
                           .Select(int.Parse)
                           .ToHashSet()
                           .Contains);
                if (yourWinningNumberCount > 0)
                {
                    total += 1 << (yourWinningNumberCount - 1);
                }
            }

            return total;
        }
    }
}
