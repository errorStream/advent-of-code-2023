using System.Diagnostics;
using System.Reflection;

namespace Tester
{
    public static class Display
    {
        private static Func<StreamReader, long?>? FindRunFunctionForDayAndPart(int i, int part)
        {
            var type = Assembly.GetExecutingAssembly().GetType($"Day{i}.Part{part}");
            if (type == null) { return null; }

            var method = type.GetMethod("Run", new[] { typeof(StreamReader) });
            if (method == null) { return null; }

            var constructorInfo = type.GetConstructor(Type.EmptyTypes);
            if (constructorInfo == null) { return null; }

            if (method.ReturnType != typeof(long)) { return null; }

            return (StreamReader reader) =>
            {
                object? instance = Activator.CreateInstance(type);
                if (instance == null)
                {
                    return null;
                }
                var res = method.Invoke(instance, new object[] { reader });
                if (res is long resi)
                {
                    return resi;
                }
                else
                {
                    throw new InvalidOperationException("Unexpected return type");
                }
            };
        }

        private readonly record struct AnswersForDay(long Part1, long Part2)
        {
            public long this[int i] => i switch
            {
                1 => Part1,
                2 => Part2,
                _ => throw new ArgumentException("Invalid part number: " + i)
            };
        }

        private const string _indent = "    ";

        private static readonly IReadOnlyList<string> _spinnerChars = new[] { "|", "/", "-", "\\" };

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

        private static void Spinner(CancellationToken token)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Blue;
            Thread.Sleep(50);
            while (!token.IsCancellationRequested)
            {
                for (int i = 0; i < _spinnerChars.Count && (!token.IsCancellationRequested); ++i)
                {
                    Console.Write(_spinnerChars[i]);
                    Thread.Sleep(125);
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                }
            }
            Console.ResetColor();
            Console.Write(' ');
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
        }

        private static void WritePassTag()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write(" PASS ");
            Console.ResetColor();
        }

        private static void WriteFailTag()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(" FAIL ");
            Console.ResetColor();
        }

        private readonly record struct TestResult(bool Pass, string Message, ConsoleColor MessageColor);

        private static TestResult RunPart(int day, int part)
        {
            using var stream = MakeInputFileStream(day);
            if (stream is null)
            {
                return new TestResult(false, "Cannot find input file", ConsoleColor.Red);
            }
            var runFunction = FindRunFunctionForDayAndPart(day, part);
            if (runFunction is null)
            {
                return new TestResult(false, "Cannot find function", ConsoleColor.Red);
            }
            try
            {
                Stopwatch stopwatch = new();
                stopwatch.Start();
                var res = runFunction(stream);
                stopwatch.Stop();
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
                if (res is null)
                {
                    return new TestResult(false, "Could not instantiate part", ConsoleColor.Red);
                }
                else if (expected is null)
                {
                    return new TestResult(false, "Could not find or parse answer file. Yielded result: " + res.Value, ConsoleColor.Red);
                }
                else if (res.Value != expected)
                {
                    return new TestResult(false, "Returned incorrect value " + res.Value + ", expected " + expected, ConsoleColor.Red);
                }
                else
                {
                    return new TestResult(true,
                                          stopwatch.ElapsedMilliseconds + "ms",
                                          stopwatch.ElapsedMilliseconds switch
                                          {
                                              < 1000 => ConsoleColor.Green,
                                              < 15000 => ConsoleColor.Yellow,
                                              _ => ConsoleColor.Red
                                          });
                }
            }
#pragma warning disable CA1031
            catch (Exception e)
            {
                return new TestResult(false, "Function threw exception: " + e.Message, ConsoleColor.Red);
            }
#pragma warning restore CA1031
        }

        public static void Start()
        {
            var rnd = new Random();
            Console.WriteLine("_ ADVENT OF CODE 2023 _");
            Console.WriteLine(_indent + "by ErrorStream");
            Console.WriteLine();

            Thread spinnerThread;

            void StartSpinner(CancellationTokenSource cancellationTokenSource)
            {
                var token = cancellationTokenSource.Token;
                spinnerThread = new Thread(() => Spinner(token));
                spinnerThread.Start();
            }

            void StopSpinner(CancellationTokenSource cancellationTokenSource)
            {
                cancellationTokenSource.Cancel();
                spinnerThread.Join();
            }

            for (int day = 1; day <= 25; ++day)
            {
                Console.WriteLine("Day " + day);

                void PartClause(int part)
                {
                    Debug.Assert(part is 1 or 2);
                    Console.Write(_indent + "Part " + part + _indent);
                    TestResult result;
                    using (CancellationTokenSource cancellationTokenSource = new())
                    {
                        StartSpinner(cancellationTokenSource);
                        result = RunPart(day, part);
                        StopSpinner(cancellationTokenSource);
                    }
                    if (result.Pass)
                    {
                        WritePassTag();
                    }
                    else
                    {
                        WriteFailTag();
                    }
                    if (!string.IsNullOrWhiteSpace(result.Message))
                    {
                        Console.Write(" (");
                        Console.ForegroundColor = result.MessageColor;
                        Console.Write(result.Message);
                        Console.ResetColor();
                        Console.Write(")");
                    }
                    Console.WriteLine();
                }

                PartClause(1);
                PartClause(2);

                Console.WriteLine();
            }
        }
    }
}
