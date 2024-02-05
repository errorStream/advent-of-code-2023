using System.Collections;
using System.Globalization;

namespace Day19
{
    public abstract class CommonFunctionality
    {
        protected class PrintList<T> : IReadOnlyList<T>
        {
            private readonly IReadOnlyList<T> _list;

            public T this[int index] => _list[index];

            public int Count => _list.Count;

            public IEnumerator<T> GetEnumerator()
            {
                return _list.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)_list).GetEnumerator();
            }

            public PrintList(IReadOnlyList<T> list)
            {
                _list = list;
            }

            public override string ToString()
            {
                return "[ " + string.Join(", ", _list) + " ]";
            }
        }

        protected enum Category { X, M, A, S }

        protected enum Comparison { LT, GT }

        protected abstract record Target;

        protected record WorkflowTarget(string Name) : Target;

        protected record AcceptTarget : Target;

        protected record RejectTarget : Target;

        protected record Rule(Category Category, Comparison Comparison, int Value, Target Target)
        {
            public bool Matches(Part part)
            {
                ArgumentNullException.ThrowIfNull(part);
                return Category switch
                {
                    Category.X => Comparison switch
                    {
                        Comparison.LT => part.X < Value,
                        Comparison.GT => part.X > Value,
                        _ => throw new NotImplementedException(Comparison.ToString()),
                    },
                    Category.M => Comparison switch
                    {
                        Comparison.LT => part.M < Value,
                        Comparison.GT => part.M > Value,
                        _ => throw new ArgumentOutOfRangeException(Comparison.ToString())
                    },
                    Category.A => Comparison switch
                    {
                        Comparison.LT => part.A < Value,
                        Comparison.GT => part.A > Value,
                        _ => throw new ArgumentOutOfRangeException(Comparison.ToString())
                    },
                    Category.S => Comparison switch
                    {
                        Comparison.LT => part.S < Value,
                        Comparison.GT => part.S > Value,
                        _ => throw new ArgumentOutOfRangeException(Comparison.ToString())
                    },
                    _ => throw new ArgumentOutOfRangeException(Category.ToString())
                };
            }
        };

        protected record Workflow(string Name, IReadOnlyList<Rule> Rules, Target Fallback);

        protected record Part(int X, int M, int A, int S);

        protected static Part ParsePart(string line)
        {
            ArgumentNullException.ThrowIfNull(line);
            var ratings = line.Trim()
                .Trim('{', '}')
                .Split(',')
                .Select(x =>
                {
                    var parts = x.Split('=');
                    return new KeyValuePair<string, int>(parts[0], int.Parse(parts[1], CultureInfo.InvariantCulture));
                })
                .ToDictionary(x => x.Key, x => x.Value);
            return new Part(ratings["x"], ratings["m"], ratings["a"], ratings["s"]);
        }

        protected static readonly char[] separator = new[] { '{', '}', ',' };

        protected static Workflow ParseWorkflow(string line)
        {
            ArgumentNullException.ThrowIfNull(line);
            var parts = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            var name = parts[0];
            var rules = new List<Rule>();
            for (int i = 1; i < parts.Length - 1; ++i)
            {
                rules.Add(ParseRule(parts[i]));
            }
            var fallback = ParseTarget(parts[^1]);
            return new Workflow(name, new PrintList<Rule>(rules), fallback);
        }

        protected static Target ParseTarget(string v)
        {
            return v switch
            {
                "A" => new AcceptTarget(),
                "R" => new RejectTarget(),
                _ => new WorkflowTarget(v),
            };
        }

        protected static Rule ParseRule(string v)
        {
            ArgumentNullException.ThrowIfNull(v);
            var category = v[0] switch
            {
                'x' => Category.X,
                'm' => Category.M,
                'a' => Category.A,
                's' => Category.S,
                _ => throw new NotImplementedException(),
            };
            var comparison = v[1] switch
            {
                '<' => Comparison.LT,
                '>' => Comparison.GT,
                _ => throw new NotImplementedException(),
            };
            var parts = v[2..].Split(':');
            var value = int.Parse(parts[0], CultureInfo.InvariantCulture);
            var target = ParseTarget(parts[1]);
            return new Rule(category, comparison, value, target);
        }
    }
}
