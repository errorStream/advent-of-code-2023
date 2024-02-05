using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Day5
{
    public abstract partial class CommonFunctionality
    {
        protected readonly struct AlmanacMapClause : IEquatable<AlmanacMapClause>
        {
            public uint DestinationRangeStart { get; }
            public uint SourceRangeStart { get; }
            public uint RangeLength { get; }

            public AlmanacMapClause(uint destinationRangeStart, uint sourceRangeStart, uint rangeLength)
            {
                DestinationRangeStart = destinationRangeStart;
                SourceRangeStart = sourceRangeStart;
                RangeLength = rangeLength;
            }

            public override string ToString()
            {
                return $"({DestinationRangeStart}, {SourceRangeStart}, {RangeLength})";
            }

            public uint SourceRangeMax => SourceRangeStart + RangeLength - 1;
            public uint DestinationRangeMax => DestinationRangeStart + RangeLength - 1;

            public bool InSourceRange(uint v)
            {
                return v >= SourceRangeStart && v <= SourceRangeMax;
            }

            public uint Map(uint v)
            {
                Debug.Assert(InSourceRange(v));
                return v - SourceRangeStart + DestinationRangeStart;
            }

            public override bool Equals(object? obj)
            {
                return obj is AlmanacMapClause clause && Equals(clause);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(DestinationRangeStart, SourceRangeStart, RangeLength, SourceRangeMax, DestinationRangeMax);
            }

            public bool Equals(AlmanacMapClause other)
            {
                return DestinationRangeStart == other.DestinationRangeStart &&
                       SourceRangeStart == other.SourceRangeStart &&
                       RangeLength == other.RangeLength &&
                       SourceRangeMax == other.SourceRangeMax &&
                       DestinationRangeMax == other.DestinationRangeMax;
            }

            public static bool operator ==(AlmanacMapClause left, AlmanacMapClause right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(AlmanacMapClause left, AlmanacMapClause right)
            {
                return !(left == right);
            }
        }
        protected class AlmanacMap
        {
            public string Source { get; }
            public string Destination { get; }
            private readonly List<AlmanacMapClause> _clauses = new();
            public IList<AlmanacMapClause> Clauses => _clauses;

            public AlmanacMap(string source, string destination)
            {
                Source = source;
                Destination = destination;
            }

            public override string ToString()
            {
                var clauses = string.Join("\n\t", _clauses);
                return $"{Source} -> {Destination}\n\t" + clauses;
            }

            public void CompleteClauses()
            {
                var newClauses = new List<AlmanacMapClause>();
                _clauses.Sort((a, b) => a.SourceRangeStart.CompareTo(b.SourceRangeStart));
                foreach (var clause in _clauses)
                {
                    if (newClauses.Count == 0)
                    {
                        if (clause.SourceRangeStart == 0)
                        {
                            newClauses.Add(clause);
                        }
                        else
                        {
                            newClauses.Add(new AlmanacMapClause(0, 0, clause.SourceRangeStart));
                            newClauses.Add(clause);
                        }
                    }
                    else
                    {
                        var lastClause = newClauses[^1];
                        if (lastClause.SourceRangeStart + lastClause.RangeLength == clause.SourceRangeStart)
                        {
                            newClauses.Add(clause);
                        }
                        else
                        {
                            var start = lastClause.SourceRangeStart + lastClause.RangeLength;
                            newClauses.Add(new AlmanacMapClause(start, start, clause.SourceRangeStart - start));
                            newClauses.Add(clause);
                        }
                    }
                }
                if (newClauses[^1].SourceRangeMax != uint.MaxValue)
                {
                    var lastClause = newClauses[^1];
                    var start = lastClause.SourceRangeStart + lastClause.RangeLength;
                    newClauses.Add(new AlmanacMapClause(start, start, uint.MaxValue - start + 1));
                }
                _clauses.Clear();
                _clauses.AddRange(newClauses);
                Validate();
            }

            public void Validate()
            {
                Debug.Assert(_clauses[0].SourceRangeStart == 0);
                long lastEnd = -1;
                foreach (var clause in _clauses)
                {
                    Debug.Assert(lastEnd + 1 == clause.SourceRangeStart, $"lastEnd: {lastEnd}, clause: {clause}");
                    lastEnd = clause.SourceRangeStart + clause.RangeLength - 1;
                }
                Debug.Assert(lastEnd == uint.MaxValue);
            }
        }
        protected class Almanac
        {
            private readonly List<uint> _seeds = new();
            public IList<uint> Seeds => _seeds;
            private readonly List<AlmanacMap> _maps = new();
            public IList<AlmanacMap> Maps => _maps;

            public void AddSeeds(IEnumerable<uint> seeds)
            {
                _seeds.AddRange(seeds);
            }
        }

        [GeneratedRegex("([a-z]+)-to-([a-z]+) map:")]
        private static partial Regex MapHeaderPattern();

        protected static Almanac Parse(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            var almanac = new Almanac();
            while (streamReader.ReadLine() is string line)
            {
                Match match;
                if (line.StartsWith("seeds: ", StringComparison.InvariantCulture))
                {
                    almanac.AddSeeds(line["seeds: ".Length..].Split(' ').Select(uint.Parse));
                }
                else if (string.IsNullOrWhiteSpace(line))
                {
                    // DO NOTHING
                }
                else if ((match = MapHeaderPattern().Match(line)).Success)
                {
                    var newMap = new AlmanacMap(
                        match.Groups[1].Value,
                        match.Groups[2].Value
                        );
                    almanac.Maps.Add(newMap);
                }
                else
                {
                    var clauseParts = line.Split(' ').Select(uint.Parse).ToArray();
                    if (almanac.Maps.Count == 0)
                    {
                        throw new ArgumentException("Map clause found outside a map");
                    }
                    almanac.Maps[^1].Clauses.Add(new(clauseParts[0], clauseParts[1], clauseParts[2]));
                }
            }
            foreach (var x in almanac.Maps)
            {
                x.CompleteClauses();
            }
            return almanac;
        }
    }
}
