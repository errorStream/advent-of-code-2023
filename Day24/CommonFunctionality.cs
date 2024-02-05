namespace Day24
{
    public abstract class CommonFunctionality
    {
        protected record struct Vec2(double X, double Y)
        {
            public Vec2 Sign()
            {
                return new Vec2(Math.Sign(X), Math.Sign(Y));
            }

            public static Vec2 operator -(Vec2 v1, Vec2 v2)
            {
                return new Vec2(v1.X - v2.X, v1.Y - v2.Y);
            }

            public static Vec2 Subtract(Vec2 v1, Vec2 v2)
            {
                return v1 - v2;
            }

            public static Vec2 operator +(Vec2 v1, Vec2 v2)
            {
                return new Vec2(v1.X + v2.X, v1.Y + v2.Y);
            }

            public static Vec2 Add(Vec2 v1, Vec2 v2)
            {
                return v1 + v2;
            }

            public Vec2 Abs()
            {
                return new Vec2(Math.Abs(X), Math.Abs(Y));
            }

            public double Length()
            {
                return Math.Sqrt((X * X) + (Y * Y));
            }

            public double DistanceFrom(Vec2 other)
            {
                return (this - other).Length();
            }

            public static Vec2 operator *(double scalar, Vec2 v)
            {
                return new Vec2(scalar * v.X, scalar * v.Y);
            }

            public static Vec2 Multiply(double scalar, Vec2 v)
            {
                return scalar * v;
            }

            public static Vec2 operator *(Vec2 v, double scalar)
            {
                return new Vec2(scalar * v.X, scalar * v.Y);
            }

            public static Vec2 Multiply(Vec2 v, double scalar)
            {
                return v * scalar;
            }

            public static explicit operator Vec3(Vec2 v)
            {
                return new Vec3(v.X, v.Y, 0);
            }

            public static Vec3 ToVec3(Vec2 v)
            {
                return (Vec3)v;
            }
        }

        protected record struct Vec3(double X, double Y, double Z)
        {
            public Vec3 Sign()
            {
                return new Vec3(Math.Sign(X), Math.Sign(Y), Math.Sign(Z));
            }

            public static Vec3 operator -(Vec3 v1, Vec3 v2)
            {
                return new Vec3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
            }

            public static Vec3 Subtract(Vec3 v1, Vec3 v2)
            {
                return v1 - v2;
            }

            public static Vec3 operator +(Vec3 v1, Vec3 v2)
            {
                return new Vec3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
            }

            public static Vec3 Add(Vec3 v1, Vec3 v2)
            {
                return v1 + v2;
            }

            public Vec3 Abs()
            {
                return new Vec3(Math.Abs(X), Math.Abs(Y), Math.Abs(Z));
            }

            public double Length()
            {
                return Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
            }

            public double DistanceFrom(Vec3 other)
            {
                return (this - other).Length();
            }

            public static Vec3 operator *(double scalar, Vec3 v)
            {
                return new Vec3(scalar * v.X, scalar * v.Y, scalar * v.Z);
            }

            public static Vec3 Multiply(double scalar, Vec3 v)
            {
                return scalar * v;
            }

            public static Vec3 operator *(Vec3 v, double scalar)
            {
                return new Vec3(scalar * v.X, scalar * v.Y, scalar * v.Z);
            }

            public static Vec3 Multiply(Vec3 v, double scalar)
            {
                return v * scalar;
            }

            public static explicit operator Vec2(Vec3 v)
            {
                return new Vec2(v.X, v.Y);
            }

            public static Vec2 ToVec2(Vec3 v)
            {
                return (Vec2)v;
            }

            public Vec2 XY => (Vec2)this;
            public Vec2 XZ => new(X, Z);
            public Vec2 YZ => new(Y, Z);
        }

        protected record Hailstone(Vec3 Position, Vec3 Velocity);

        protected static IReadOnlyList<Hailstone> ParseInput(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            var res = new List<Hailstone>();
            while (streamReader.ReadLine() is string line)
            {
                var parts = line.Split('@');
                var positionParts = parts[0].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(double.Parse).ToArray();
                var velocityParts = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(double.Parse).ToArray();
                res.Add(new Hailstone(new Vec3(positionParts[0], positionParts[1], positionParts[2]),
                                      new Vec3(velocityParts[0], velocityParts[1], velocityParts[2])));
            }
            return res;
        }
    }
}
