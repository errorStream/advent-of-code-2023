using System.Diagnostics;

namespace Day19
{
    public class Part1 : CommonFunctionality, Framework.ISolution
    {
        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            bool inWorkflowList = true;
            List<Workflow> workflows = new();
            List<Part> parts = new();
            while (streamReader.ReadLine() is string line)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    inWorkflowList = false;
                }
                else if (inWorkflowList)
                {
                    workflows.Add(ParseWorkflow(line));
                }
                else
                {
                    parts.Add(ParsePart(line));
                }
            }

            int result = Evaluate(workflows, parts);

            return result;
        }

        private static int Evaluate(IReadOnlyCollection<Workflow> workflows, IReadOnlyCollection<Part> parts)
        {
            var workflowMap = workflows.ToDictionary(x => x.Name, x => x);
            var accepted = new List<Part>();
            if (!workflowMap.TryGetValue("in", out var start))
            {
                throw new ArgumentException("No workflow named 'in'");
            }

            foreach (var part in parts)
            {
                Workflow current = start;
                bool done = false;
                while (!done)
                {
                    Workflow? next = null;
                    foreach (var rule in current.Rules)
                    {
                        if (rule.Matches(part))
                        {
                            switch (rule.Target)
                            {
                                case AcceptTarget:
                                    accepted.Add(part);
                                    done = true;
                                    break;
                                case RejectTarget:
                                    done = true;
                                    break;
                                case WorkflowTarget workflowTarget:
                                    next = workflowMap[workflowTarget.Name];
                                    break;
                                default:
                                    throw new NotImplementedException();
                            }
                            break;
                        }
                    }
                    if (!done && next is null)
                    {
                        switch (current.Fallback)
                        {
                            case AcceptTarget:
                                accepted.Add(part);
                                done = true;
                                break;
                            case RejectTarget:
                                done = true;
                                break;
                            case WorkflowTarget workflowTarget:
                                next = workflowMap[workflowTarget.Name];
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                    Debug.Assert(done || next is not null);
                    current = next!;
                }
            }

            return accepted.Select(x => x.X + x.M + x.A + x.S).Sum();
        }
    }
}
