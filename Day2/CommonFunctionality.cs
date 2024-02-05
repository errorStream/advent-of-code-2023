using System.Globalization;

namespace Day2
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

    public abstract class CommonFunctionality
    {
        protected struct Reveal : IEquatable<Reveal>
        {
            public int Red { get; set; }
            public int Green { get; set; }
            public int Blue { get; set; }

            public override bool Equals(object? obj)
            {
                return obj is Reveal reveal && Equals(reveal);
            }

            public bool Equals(Reveal other)
            {
                return Red == other.Red &&
                       Green == other.Green &&
                       Blue == other.Blue;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Red, Green, Blue);
            }

            public override string ToString()
            {
                return $"({Red}, {Green}, {Blue})";
            }

            public static bool operator ==(Reveal left, Reveal right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(Reveal left, Reveal right)
            {
                return !(left == right);
            }
        }

        protected class Game
        {
            public int ID { get; set; }
            public ICollection<Reveal> Reveals { get; } = new List<Reveal>();

            public override string ToString()
            {
                return $"Game {ID}: " + string.Join(", ", Reveals);
            }
        }

        protected static Game ParseGame(string line)
        {
            ArgumentNullException.ThrowIfNull(line);
            var game = new Game();
            var (gameSection, revealsSection) = line.Split(':');
            game.ID = int.Parse(gameSection["Game ".Length..], CultureInfo.InvariantCulture);
            foreach (var revealClause in revealsSection.Split(';'))
            {
                var reveal = new Reveal();
                foreach (var revealColor in revealClause.Split(','))
                {
                    var (countStr, color) = revealColor.Trim().Split(' ');
                    if (color == "red")
                    {
                        reveal.Red = int.Parse(countStr, CultureInfo.InvariantCulture);
                    }
                    else if (color == "green")
                    {
                        reveal.Green = int.Parse(countStr, CultureInfo.InvariantCulture);
                    }
                    else if (color == "blue")
                    {
                        reveal.Blue = int.Parse(countStr, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        throw new ArgumentException($"Unknown color {color}");
                    }
                }
                game.Reveals.Add(reveal);
            }
            return game;
        }

        protected static IList<Game> ReadGames(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            List<Game> games = new();
            while (streamReader.ReadLine() is string line)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                games.Add(ParseGame(line));
            }
            return games;
        }
    }
}
