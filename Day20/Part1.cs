namespace Day20
{
    public class Part1 : CommonFunctionality, Framework.ISolution
    {
        public long Run(StreamReader streamReader)
        {
            Queue<(string target, Pulse pulse, string sender)> queue = new();
            int highCount = 0;
            int lowCount = 0;
            void QueuePulse(string target, Pulse pulse, string sender)
            {
                if (pulse == Pulse.High)
                {
                    ++highCount;
                }
                else
                {
                    ++lowCount;
                }
                queue.Enqueue((target, pulse, sender));
            }
            var modules = ParseInput(streamReader, QueuePulse);
            for (int i = 0; i < 1000; ++i)
            {
                QueuePulse("broadcaster", Pulse.Low, "button");
                while (queue.TryDequeue(out var entry))
                {
                    if (modules.TryGetValue(entry.target, out var module))
                    {
                        module.Send(entry.pulse, entry.sender);
                    }
                }
            }
            return highCount * lowCount;
        }
    }
}
