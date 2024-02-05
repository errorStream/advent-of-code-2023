namespace Day4
{
    public class Part2 : Framework.ISolution
    {
        /// <remarks> O(n) time complexity </remarks>
        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            var winningNumbersCounts = new List<int>();
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
                winningNumbersCounts.Add(yourWinningNumberCount);
            }
            var cardCountChanges = new int[winningNumbersCounts.Count];
            var currCount = 1;
            var totalCount = 0;
            for (int i = 0; i < winningNumbersCounts.Count; ++i)
            {
                var winningNumbersCount = winningNumbersCounts[i];
                currCount += cardCountChanges[i];
                totalCount += currCount;
                if (i + 1 < cardCountChanges.Length)
                {
                    cardCountChanges[i + 1] += currCount;
                }
                if (i + winningNumbersCount + 1 < cardCountChanges.Length)
                {
                    cardCountChanges[i + winningNumbersCount + 1] -= currCount;
                }
            }
            return totalCount;
        }
    }
}
