namespace Day22
{
    public class Part2 : CommonFunctionality, Framework.ISolution
    {
        public long Run(StreamReader streamReader)
        {
            var allBricks = BuildTree(ParseSnapshot(streamReader));

            var total = 0;
            foreach (var currBrick in allBricks)
            {
                var fallNodes = new HashSet<Node>();
                var queue = new PriorityQueue<Node, int>();
                queue.Enqueue(currBrick, currBrick.Brick.MaxZ);
                while (queue.Count > 0)
                {
                    var node = queue.Dequeue();
                    var willFall = node == currBrick || node.SupportedBy.All(fallNodes.Contains);
                    if (!willFall)
                    {
                        continue;
                    }
                    fallNodes.Add(node);
                    foreach (var item in node.Supports)
                    {
                        queue.Enqueue(item, item.Brick.MaxZ);
                    }
                }
                total += fallNodes.Count - 1;
            }

            return total;
        }
    }
}
