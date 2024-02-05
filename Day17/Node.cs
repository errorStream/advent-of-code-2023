namespace Day17
{
    public class Node
    {
        public int Distance { get; set; } = int.MaxValue;
        public Node? Parent { get; set; }
        public int Weight { get; }
        public Vec2 Position { get; }
        public Vec2 Direction { get; }
        public int ConsecutiveLength { get; }

        public Node(int weight, Vec2 position, Vec2 direction, int consecutiveLength)
        {
            Weight = weight;
            Position = position;
            Direction = direction;
            ConsecutiveLength = consecutiveLength;
        }
    }
}
