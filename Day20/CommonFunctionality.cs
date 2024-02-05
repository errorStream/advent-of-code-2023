using System.Diagnostics;

namespace Day20
{
    public abstract class CommonFunctionality
    {
        protected enum Pulse
        {
            Low,
            High
        }

        protected abstract class BaseModule
        {
            protected BaseModule(string name, IReadOnlyList<string> outputs, Action<string, Pulse, string> queuePulse)
            {
                Name = name;
                QueuePulse = queuePulse;
                Outputs = outputs;
            }

            public string Name { get; }
            protected Action<string, Pulse, string> QueuePulse { get; }
            public IReadOnlyList<string> Outputs { get; }
            public abstract void Send(Pulse pulse, string sender);
        }

        protected class BroadcasterModule : BaseModule
        {
            public BroadcasterModule(IReadOnlyList<string> outputs, Action<string, Pulse, string> queuePulse)
                : base("broadcaster", outputs, queuePulse)
            {
            }

            public override void Send(Pulse pulse, string sender)
            {
                foreach (var output in Outputs)
                {
                    QueuePulse(output, pulse, Name);
                }
            }
        }

        protected class FlipFlopModule : BaseModule
        {
            public FlipFlopModule(string name, IReadOnlyList<string> outputs, Action<string, Pulse, string> queuePulse)
                : base(name, outputs, queuePulse)
            {
            }

            public bool On { get; private set; }

            public override void Send(Pulse pulse, string sender)
            {
                if (pulse == Pulse.High)
                {
                    return;
                }
                if (On)
                {
                    On = false;
                    foreach (var output in Outputs)
                    {
                        QueuePulse(output, Pulse.Low, Name);
                    }
                }
                else
                {
                    On = true;
                    foreach (var output in Outputs)
                    {
                        QueuePulse(output, Pulse.High, Name);
                    }
                }
            }
        }

        protected class ConjunctionModule : BaseModule
        {
            private readonly Dictionary<string, Pulse> _lastInputs = new();

            public void AddInput(string input)
            {
                _lastInputs.Add(input, Pulse.Low);
            }

            public ConjunctionModule(string name, IReadOnlyList<string> outputs, Action<string, Pulse, string> queuePulse)
                : base(name, outputs, queuePulse)
            {
            }

            public override void Send(Pulse pulse, string sender)
            {
                Debug.Assert(_lastInputs.ContainsKey(sender));
                _lastInputs[sender] = pulse;
                if (_lastInputs.Values.All(p => p == Pulse.High))
                {
                    foreach (var output in Outputs)
                    {
                        QueuePulse(output, Pulse.Low, Name);
                    }
                }
                else
                {
                    foreach (var output in Outputs)
                    {
                        QueuePulse(output, Pulse.High, Name);
                    }
                }
            }
        }

        protected static Dictionary<string, BaseModule> ParseInput(StreamReader streamReader, Action<string, Pulse, string>? queuePulse = null)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            Dictionary<string, BaseModule> modules = new();
            queuePulse ??= (output, pulse, sender) => { };
            while (streamReader.ReadLine() is string line)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                var parts = line.Split("->");
                var name = parts[0].Trim();
                var outputs = parts[1].Trim().Split(',').Select(s => s.Trim()).ToArray();
                if (name.StartsWith('%'))
                {
                    name = name[1..];
                    modules.Add(name, new FlipFlopModule(name, outputs, queuePulse));
                }
                else if (name.StartsWith('&'))
                {
                    name = name[1..];
                    modules.Add(name, new ConjunctionModule(name, outputs, queuePulse));
                }
                else if (name == "broadcaster")
                {
                    var module = new BroadcasterModule(outputs, queuePulse);
                    modules.Add(name, module);
                }
                else
                {
                    throw new ArgumentException($"Can't parse type for '{name}'");
                }
            }
            foreach (var module in modules.Values)
            {
                foreach (var output in module.Outputs)
                {
                    if (modules.TryGetValue(output, out var outMod) && outMod is ConjunctionModule conjunction)
                    {
                        conjunction.AddInput(module.Name);
                    }
                }
            }

            return modules;
        }
    }
}
