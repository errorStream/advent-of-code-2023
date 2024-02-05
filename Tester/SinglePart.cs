using System.Text.RegularExpressions;
using System.Globalization;

namespace Tester
{
    public static partial class SinglePart
    {
        private static StreamReader? MakeInputFileStream(int day)
        {
            var fileName = Path.Combine($"Day{day}", "input.txt");
            if (File.Exists(fileName))
            {
                return File.OpenText(fileName);
            }
            else
            {
                return null;
            }
        }

        [GeneratedRegex(@"Day(\d+)")]
        private static partial Regex DayRegex();
        [GeneratedRegex(@"Part(\d+)")]
        private static partial Regex PartRegex();

        private static (int day, int part)? ExtractDayAndPartNumber(Framework.ISolution solution)
        {
            var type = solution.GetType();
            if (type.Namespace is not string @namespace)
            {
                return null;
            }

            if (type.Name is not string @class)
            {
                return null;
            }

            var dayMatch = DayRegex().Match(@namespace);
            var partMatch = PartRegex().Match(@class);

            if (!dayMatch.Success || !partMatch.Success)
            {
                return null;
            }

            return (int.Parse(dayMatch.Groups[1].Value, CultureInfo.InvariantCulture),
                    int.Parse(partMatch.Groups[1].Value, CultureInfo.InvariantCulture));
        }

        public static void Test(Framework.ISolution solution)
        {
            ArgumentNullException.ThrowIfNull(solution);
            if (ExtractDayAndPartNumber(solution) is not (int day, int part))
            {
                Console.WriteLine("Could not extract day and part number");
                return;
            }

            using StreamReader? inputFileStream = MakeInputFileStream(day);

            if (inputFileStream is null)
            {
                Console.WriteLine("No input file found");
                return;
            }

            var result = solution.Run(inputFileStream);

            long? expected = null;
            {
                var answerFileName = Path.Combine($"Day{day}", $"answer-{part}.txt");
                if (File.Exists(answerFileName))
                {
                    var answerFileContent = File.ReadAllText(answerFileName);
                    if (long.TryParse(answerFileContent, out var answerFromFile))
                    {
                        expected = answerFromFile;
                    }
                }
            }

            if (expected is null)
            {
                Console.WriteLine("No answer file provided for day and part.\nReturned value is " + result);
            }
            else
            {
                Console.WriteLine($"Returned value {result} which " + ((result == expected) ? "matches the expected answer" : ("does not match the expected answer " + expected.Value)));
            }
        }
    }
}
