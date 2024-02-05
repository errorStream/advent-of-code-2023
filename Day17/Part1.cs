namespace Day17
{
    public class Part1 : CommonFunctionality, Framework.ISolution
    {
        public long Run(StreamReader streamReader)
        {
            int[,] weights = MakeWeightsGrid(streamReader);

            Vec2 start = new(0, 0);
            Vec2 end = new(weights.GetLength(0) - 1, weights.GetLength(1) - 1);

            SortedSet<Node> open = new(new NodeComparer())
            {
                new Node(weight: weights[start.X, start.Y], position: start, direction: new(0, 0), consecutiveLength: 0) {
                    Distance = 0
                },
            };

            int? MaybeGetWeight(Vec2 v)
            {
                return v.X < 0 || v.X >= weights.GetLength(0) ||
                    v.Y < 0 || v.Y >= weights.GetLength(1)
                    ? null
                    : weights[v.X, v.Y];
            }

            Dictionary<(Vec2 position, Vec2 direction, int consecutiveLength), Node> nodes = new();

            Node? endNode = null;

            while (open.Count > 0)
            {
                Node? curr = open.Min;
                if (curr is null)
                {
                    throw new InvalidOperationException("Failed to get node from open list");
                }
                if (curr.Position == end)
                {
                    endNode = curr;
                    break;
                }
                if (!open.Remove(curr))
                {
                    throw new InvalidOperationException("Failed to remove node from open list: " + curr.Position);
                }

                foreach (Vec2 direction in Offsets)
                {
                    void MoveToNext(bool resetConsecutive)
                    {
                        Vec2 nextPosition = curr.Position + direction;
                        Vec2 nextDirection = direction;
                        int nextConsecutiveLength = (resetConsecutive ? 0 : curr.ConsecutiveLength) + 1;
                        int? neighborWeight = MaybeGetWeight(nextPosition);
                        if (neighborWeight is null)
                        {
                            return;
                        }
                        int nextWeight = neighborWeight.Value;
                        (Vec2 nextPosition, Vec2 nextDirection, int nextConsecutiveLength) key = (nextPosition, nextDirection, nextConsecutiveLength);

                        if (!nodes.TryGetValue(key, out Node? node))
                        {
                            node = new Node(nextWeight, nextPosition, nextDirection, nextConsecutiveLength);
                            nodes.Add(key, node);
                            if (!open.Add(node))
                            {
                                throw new InvalidOperationException("Failed to add node to open list: " + node.Position);
                            }
                        }

                        int newDistance = curr.Distance + nextWeight;
                        if (newDistance < node.Distance)
                        {
                            if (!open.Remove(node))
                            {
                                throw new InvalidOperationException("Failed to remove node from open list: " + node.Position);
                            }
                            node.Parent = curr;
                            node.Distance = newDistance;
                            if (!open.Add(node))
                            {
                                throw new InvalidOperationException("Failed to add node to open list: " + node.Position);
                            }
                        }
                    }

                    if (curr.Direction == direction)
                    {
                        if (curr.ConsecutiveLength == 3)
                        {
                            continue;
                        }
                        else if (curr.ConsecutiveLength > 3)
                        {
                            throw new InvalidOperationException("Invalid consecutive length");
                        }
                        else
                        {
                            MoveToNext(resetConsecutive: false);
                        }
                    }
                    else
                    {
                        if (direction == -curr.Direction)
                        {
                            continue;
                        }

                        MoveToNext(resetConsecutive: true);
                    }
                }
            }

            var results = endNode?.Distance ?? -1;

            return results;
        }
    }
}
