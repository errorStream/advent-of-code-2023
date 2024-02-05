namespace Day2
{
    public class Part1 : CommonFunctionality, Framework.ISolution
    {
        public long Run(StreamReader streamReader)
        {
            var target = new Reveal
            {
                Red = 12,
                Green = 13,
                Blue = 14
            };
            IList<Game> games = ReadGames(streamReader);
            var sum = 0;

            foreach (var game in games)
            {
                if (game.Reveals.All(reveal =>
                                     reveal.Red <= target.Red
                                     && reveal.Green <= target.Green
                                     && reveal.Blue <= target.Blue))
                {
                    sum += game.ID;
                }
            }

            return sum;
        }
    }
}
