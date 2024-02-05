namespace Day5
{
    public class Part2 : CommonFunctionality, Framework.ISolution
    {
        public long Run(StreamReader streamReader)
        {
            var almanac = Parse(streamReader);
            var seedPairs = new List<(uint, uint)>();
            for (int i = 0; i < almanac.Seeds.Count; i += 2)
            {
                seedPairs.Add((almanac.Seeds[i], almanac.Seeds[i + 1]));
            }

            const string start = "seed";
            const string end = "location";

            var currProperty = start;
            while (currProperty != end)
            {
                var nextSeedPairs = new List<(uint, uint)>(seedPairs.Count);
                var map = almanac.Maps.First(x => x.Source == currProperty);
                foreach (var seedPair in seedPairs)
                {
                    var seedPairStart = seedPair.Item1;
                    var seedPairEnd = seedPair.Item1 + seedPair.Item2 - 1;
                    foreach (var clause in map.Clauses)
                    {
                        if (seedPairEnd < clause.SourceRangeStart)
                        {
                            // Range before
                            // Ignore
                        }
                        else if (seedPairStart > clause.SourceRangeMax)
                        {
                            // Range after
                            // Ignore
                        }
                        else if (seedPairStart >= clause.SourceRangeStart && seedPairEnd <= clause.SourceRangeMax)
                        {
                            // Range inside
                            var newSeedPairStart = clause.Map(seedPairStart);
                            var newSeedPairEnd = clause.Map(seedPairEnd);
                            nextSeedPairs.Add((newSeedPairStart, newSeedPairEnd - newSeedPairStart + 1));
                        }
                        else
                        {
                            // Range contains clause or range and clause overlap without clause surrounding range
                            var newSeedPairStart = clause.Map(Math.Max(seedPairStart, clause.SourceRangeStart));
                            var newSeedPairEnd = clause.Map(Math.Min(seedPairEnd, clause.SourceRangeMax));
                            nextSeedPairs.Add((newSeedPairStart, newSeedPairEnd - newSeedPairStart + 1));
                        }
                    }
                }
                seedPairs.Clear();
                seedPairs.AddRange(nextSeedPairs);
                currProperty = map.Destination;
            }

            uint result = seedPairs.Min(x => x.Item1);
            return result;
        }
    }
}
