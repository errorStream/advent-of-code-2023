namespace Day17
{
    public struct Vec2 : IEquatable<Vec2>, IComparable<Vec2>, IComparable
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vec2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vec2 operator +(Vec2 a, Vec2 b)
        {
            return new Vec2(a.X + b.X, a.Y + b.Y);
        }

        public static Vec2 Add(Vec2 a, Vec2 b)
        {
            return a + b;
        }

        public static Vec2 operator -(Vec2 a, Vec2 b)
        {
            return new Vec2(a.X - b.X, a.Y - b.Y);
        }

        public static Vec2 Subtract(Vec2 a, Vec2 b)
        {
            return a - b;
        }

        public static Vec2 operator *(Vec2 a, int b)
        {
            return new Vec2(a.X * b, a.Y * b);
        }

        public static Vec2 Multiply(Vec2 a, int b)
        {
            return a * b;
        }

        public static Vec2 operator /(Vec2 a, int b)
        {
            return new Vec2(a.X / b, a.Y / b);
        }

        public static Vec2 Divide(Vec2 a, int b)
        {
            return a / b;
        }

        public static bool operator ==(Vec2 a, Vec2 b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vec2 a, Vec2 b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        public override bool Equals(object? obj)
        {
            return obj is Vec2 other && this == other;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public bool Equals(Vec2 other)
        {
            return this == other;
        }

        public int CompareTo(Vec2 other)
        {
            return (X, Y).CompareTo((other.X, other.Y));
        }

        public int CompareTo(object? obj)
        {
            return obj is Vec2 other ? CompareTo(other) : 1;
        }

        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }

        public static bool operator <(Vec2 a, Vec2 b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator <=(Vec2 a, Vec2 b)
        {
            return a.CompareTo(b) <= 0;
        }

        public static bool operator >(Vec2 a, Vec2 b)
        {
            return a.CompareTo(b) > 0;
        }

        public static bool operator >=(Vec2 a, Vec2 b)
        {
            return a.CompareTo(b) >= 0;
        }

        public static Vec2 operator -(Vec2 a)
        {
            return new Vec2(-a.X, -a.Y);
        }

        public static Vec2 Negate(Vec2 a)
        {
            return -a;
        }
    }
}
