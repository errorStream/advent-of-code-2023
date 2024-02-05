using System.Diagnostics;

namespace Day19
{
    public class Part2 : CommonFunctionality, Framework.ISolution
    {
        private record struct Interval(int Start, int End)
        {
            public Interval Intersect(Interval other)
            {
                return new Interval(Math.Max(Start, other.Start), Math.Min(End, other.End));
            }

            public int Length => End - Start + 1;

            public override string ToString()
            {
                return $"[{Start}, {End}]";
            }

            public bool Contains(int value)
            {
                return value >= Start && value <= End;
            }

            public bool Overlaps(Interval other)
            {
                return Contains(other.Start) || Contains(other.End) || other.Contains(Start) || other.Contains(End);
            }
        }

        private record struct RatingIntervals(Interval X, Interval M, Interval A, Interval S)
        {
            public override string ToString()
            {
                return $"X: {X}, M: {M}, A: {A}, S: {S}";
            }

            public Interval this[Category category]
            {
                get => category switch
                {
                    Category.X => X,
                    Category.M => M,
                    Category.A => A,
                    Category.S => S,
                    _ => throw new ArgumentOutOfRangeException(category.ToString())
                };
                set
                {
                    switch (category)
                    {
                        case Category.X:
                            X = value;
                            break;
                        case Category.M:
                            M = value;
                            break;
                        case Category.A:
                            A = value;
                            break;
                        case Category.S:
                            S = value;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(category.ToString());
                    }
                }
            }

            public bool Overlaps(RatingIntervals other)
            {
                return X.Overlaps(other.X) && M.Overlaps(other.M) && A.Overlaps(other.A) && S.Overlaps(other.S);
            }

            public long Permutations()
            {
                return ((long)X.Length) * M.Length * A.Length * S.Length;
            }
        }

        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            List<Workflow> workflows = new();
            while (streamReader.ReadLine() is string line)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }
                else
                {
                    workflows.Add(ParseWorkflow(line));
                }
            }

            List<RatingIntervals> acceptedIntervals = new();
            Dictionary<string, Workflow> workflowMap = workflows.ToDictionary(x => x.Name, x => x);
            void FindIntervalsRec(RatingIntervals ratingIntervals, Workflow workflow)
            {
                // Return if any intervals are empty
                if (ratingIntervals.X.Length <= 0 || ratingIntervals.M.Length <= 0 || ratingIntervals.A.Length <= 0 || ratingIntervals.S.Length <= 0)
                {
                    return;
                }
                foreach (var rule in workflow.Rules)
                {
                    var passInterval = rule.Comparison switch
                    {
                        Comparison.LT => new Interval(1, rule.Value - 1),
                        Comparison.GT => new Interval(rule.Value + 1, 4000),
                        _ => throw new NotImplementedException(rule.Comparison.ToString())
                    };
                    var failInterval = rule.Comparison switch
                    {
                        Comparison.LT => new Interval(rule.Value, 4000),
                        Comparison.GT => new Interval(1, rule.Value),
                        _ => throw new NotImplementedException(rule.Comparison.ToString())
                    };
                    var oldInterval = ratingIntervals[rule.Category];
                    ratingIntervals[rule.Category] = oldInterval.Intersect(passInterval);
                    switch (rule.Target)
                    {
                        case WorkflowTarget workflowTarget:
                            FindIntervalsRec(ratingIntervals, workflowMap[workflowTarget.Name]);
                            break;
                        case AcceptTarget:
                            acceptedIntervals.Add(ratingIntervals);
                            break;
                        case RejectTarget:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(rule.Target.ToString());
                    }
                    ratingIntervals[rule.Category] = oldInterval.Intersect(failInterval);
                }

                switch (workflow.Fallback)
                {
                    case WorkflowTarget workflowTarget:
                        FindIntervalsRec(ratingIntervals, workflowMap[workflowTarget.Name]);
                        break;
                    case AcceptTarget:
                        acceptedIntervals.Add(ratingIntervals);
                        break;
                    case RejectTarget:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(workflow.Fallback.ToString());
                }
            }

            FindIntervalsRec(new RatingIntervals(new Interval(1, 4000),
                                                 new Interval(1, 4000),
                                                 new Interval(1, 4000),
                                                 new Interval(1, 4000)),
                             workflowMap["in"]);

            for (int i = 0; i < acceptedIntervals.Count - 1; ++i)
            {
                for (int j = i + 1; j < acceptedIntervals.Count; ++j)
                {
                    Debug.Assert(!acceptedIntervals[i].Overlaps(acceptedIntervals[j]), "Overlapping intervals found in accepted intervals list at indices " + i + " and " + j);
                }
            }

            long totalPermutations = acceptedIntervals.Sum(x => x.Permutations());

            return totalPermutations;
        }

    }
}
