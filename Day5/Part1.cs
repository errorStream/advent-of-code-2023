namespace Day5
{
    public class Part1 : CommonFunctionality, Framework.ISolution
    {
        public long Run(StreamReader streamReader)
        {
            var almanac = Parse(streamReader);

            var lowestLocationNumber = uint.MaxValue;
            const string start = "seed";
            const string end = "location";
            foreach (var seed in almanac.Seeds)
            {
                var currID = seed;
                var currProperty = start;
                while (currProperty != end)
                {
                    var map = almanac.Maps.First(x => x.Source == currProperty);
                    foreach (var x in map.Clauses)
                    {
                        if (x.InSourceRange(currID))
                        {
                            currID = x.Map(currID);
                            break;
                        }
                    }
                    currProperty = map.Destination;
                }
                lowestLocationNumber = Math.Min(lowestLocationNumber, currID);
            }

            return lowestLocationNumber;
        }
    }
}
