using System.Globalization;

namespace Day18
{
    public class Part2 : CommonFunctionality, Framework.ISolution
    {
        protected override (char direction, long distance) ParseDirectionAndDistance(string line)
        {
            ArgumentNullException.ThrowIfNull(line);
            string[] parts = line.Split(' ');
            char direction = parts[2][7] switch
            {
                '0' => 'R',
                '1' => 'D',
                '2' => 'L',
                '3' => 'U',
                _ => throw new NotImplementedException(),
            };
            long distance = long.Parse(parts[2].AsSpan(2, 5), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            return (direction, distance);
        }
    }
}
