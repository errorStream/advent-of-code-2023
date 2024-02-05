using System.Diagnostics;

namespace Day24
{
    public class Part1 : CommonFunctionality, Framework.ISolution
    {
        public long Run(StreamReader streamReader)
        {
            // (long min, long max) bounds = (min: 7, max: 27);
            (long min, long max) bounds = (min: 200000000000000, max: 400000000000000);
            var hailstones = ParseInput(streamReader);
            var intersectionCount = 0;
            for (int i = 0; i < hailstones.Count - 1; ++i)
            {
                for (int j = i + 1; j < hailstones.Count; ++j)
                {
                    var (h1, h2) = (hailstones[i], hailstones[j]);
                    var intersection = Intersection(h1, h2);
                    if (intersection is not Vec3 isec)
                    {
                        continue;
                    }

                    if (isec.X >= bounds.min && isec.X <= bounds.max
                        && isec.Y >= bounds.min && isec.Y <= bounds.max)
                    {
                        intersectionCount++;
                        continue;
                    }
                }
            }

            return intersectionCount;
        }

        private static Vec3? Intersection(Hailstone h1, Hailstone h2)
        {
            Debug.Assert(h1.Velocity.X != 0);
            Debug.Assert(h2.Velocity.X != 0);
            Debug.Assert(h1.Velocity.Y != 0);
            Debug.Assert(h2.Velocity.Y != 0);

            var m1 = h1.Velocity.Y / h1.Velocity.X;
            var m2 = h2.Velocity.Y / h2.Velocity.X;

            if (m1 == m2)
            {
                return null;
            }

            var b1 = h1.Position.Y - (h1.Position.X * m1);
            var b2 = h2.Position.Y - (h2.Position.X * m2);

            var x = (b2 - b1) / (m1 - m2);
            var y = (m1 * x) + b1;

            var intersection = new Vec3(x, y, 0);

            if (((intersection - h1.Position).Sign() with { Z = 0 }) != (h1.Velocity with { Z = 0 }).Sign()
                || ((intersection - h2.Position).Sign() with { Z = 0 }) != (h2.Velocity with { Z = 0 }).Sign())
            {
                return null;
            }

            return new Vec3(x, y, 0);
        }
    }
}
