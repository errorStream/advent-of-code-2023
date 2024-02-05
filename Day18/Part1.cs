using System.Globalization;

namespace Day18
{
    public class Part1 : CommonFunctionality, Framework.ISolution
    {
        protected override (char direction, long distance) ParseDirectionAndDistance(string line)
        {
            ArgumentNullException.ThrowIfNull(line);
            string[] parts = line.Split(' ');
            char direction = char.Parse(parts[0]);
            long distance = long.Parse(parts[1], CultureInfo.InvariantCulture);
            return (direction, distance);
        }
    }
}
