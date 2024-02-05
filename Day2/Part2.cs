namespace Day2
{
    public class Part2 : CommonFunctionality, Framework.ISolution
    {
        public long Run(StreamReader streamReader)
        {
            IList<Game> games = ReadGames(streamReader);
            var sum = 0;

            foreach (var game in games)
            {
                var minRed = 0;
                var minGreen = 0;
                var minBlue = 0;
                foreach (var reveal in game.Reveals)
                {
                    minRed = Math.Max(minRed, reveal.Red);
                    minGreen = Math.Max(minGreen, reveal.Green);
                    minBlue = Math.Max(minBlue, reveal.Blue);
                }

                sum += minRed * minGreen * minBlue;
            }

            return sum;
        }
    }
}
