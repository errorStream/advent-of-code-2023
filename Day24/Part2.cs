using System.Diagnostics;

namespace Day24
{
    public class Part2 : CommonFunctionality, Framework.ISolution
    {
        private static void Tests()
        {
            {
                var actual = Intersection(new(0, 0), new(1, 1), new(0, 1), new(1, -1));
                var expected = new Vec2(0.5, 0.5);
                Debug.Assert(actual == expected, $"Expected {expected}, got {actual}");
            }
            {
                var actual = Intersection(new(0, 0), new(1, 1), new(1, 1), new(2, 2));
                Vec2? expected = null;
                Debug.Assert(actual == expected, $"Expected {expected}, got {actual}");
            }
            {
                var actual = Intersection(new(1, 1), new(-1, -1), new(-1, 1), new(1, -1));
                Vec2? expected = new Vec2(0, 0);
                Debug.Assert(actual == expected, $"Expected {expected}, got {actual}");
            }
            {
                var actual = Intersection(new(1, 2), new(3, 4), new(2, 3), new(4, 5));
                Vec2? expected = new Vec2(-2, -2);
                Debug.Assert(actual == expected, $"Expected {expected}, got {actual}");
            }
            {
                var actual = Intersection(new(0, 0), new(1, 1), new(1, 0), new(0, 1));
                Vec2? expected = new Vec2(1, 1);
                Debug.Assert(actual == expected, $"Expected {expected}, got {actual}");
            }
            {
                var actual = Intersection(new(0, 0), new(1, 1), new(0, 1), new(1, 1));
                Vec2? expected = null;
                Debug.Assert(actual == expected, $"Expected {expected}, got {actual}");
            }
            {
                var actual = Intersection(new(0, 0), new(1, 1), new(2, 2), new(1, 1));
                Vec2? expected = null;
                Debug.Assert(actual == expected, $"Expected {expected}, got {actual}");
            }
            {
                var actual = Intersection(new(0, 0), new(1, 0), new(0, 0), new(0, 1));
                Vec2? expected = new Vec2(0, 0);
                Debug.Assert(actual == expected, $"Expected {expected}, got {actual}");
            }
            {
                var actual = Intersection(new(1000, 2000), new(300, 400), new(2000, 3000), new(400, 500));
                Vec2? expected = new Vec2(-2000, -2000);
                Debug.Assert(actual == expected, $"Expected {expected}, got {actual}");
            }
            {
                var actual = Intersection(new(1000, 1000), new(300, 300), new(2000, 2000), new(300, 300));
                Vec2? expected = null;
                Debug.Assert(actual == expected, $"Expected {expected}, got {actual}");
            }
            {
                var actual = Intersection(new(5000, 5000), new(100, 100), new(5100, 5100), new(100, 100));
                Vec2? expected = null;
                Debug.Assert(actual == expected, $"Expected {expected}, got {actual}");
            }
            {
                var actual = Intersection(new(10000, 10000), new(0, 1000), new(10000, 20000), new(1000, 0));
                Vec2? expected = new Vec2(10000, 20000);
                Debug.Assert(actual == expected, $"Expected {expected}, got {actual}");
            }
        }

        public long Run(StreamReader streamReader)
        {
            var hailstones = ParseInput(streamReader);
            Tests();
            return FindVelocity(hailstones);
        }

        private static bool PointIsOnLine(Vec2 point, Vec2 position, Vec2 velocity)
        {
            var (px, py) = position;
            var (vx, vy) = velocity;
            var (x, y) = point;

            return ((x - px) / vx) == ((y - py) / vy);
        }

        private static Vec2? Intersection(Vec2 position1, Vec2 velocity1, Vec2 position2, Vec2 velocity2)
        {
            var (x1, y1) = position1;
            var (x2, y2) = position2;
            var (vx1, vy1) = velocity1;
            var (vx2, vy2) = velocity2;
            var numerator = (vx2 * (y2 - y1)) + (vy2 * (x1 - x2));
            var denominator = (vy1 * vx2) - (vx1 * vy2);
            if (denominator == 0)
            {
                return null;
            }
            var t1 = numerator / denominator;

            return new Vec2(x1 + (vx1 * t1), y1 + (vy1 * t1));
        }

        private static IEnumerable<long> AlternatingGenerator(int limit = 100000)
        {
            var i = 0;
            while (i < limit)
            {
                yield return i;
                yield return -i;
                i++;
            }
        }

        private static IEnumerable<Vec2> DiamondGenerator2D(int limit = 100000)
        {
            yield return new Vec2(0, 0);

            for (int i = 1; i < limit; i++)
            {
                for (int j = 1; j < i; j++)
                {
                    int x = i - j;
                    int y = j;
                    yield return new Vec2(x, y);
                    yield return new Vec2(x, -y);
                    yield return new Vec2(-x, y);
                    yield return new Vec2(-x, -y);
                }

                yield return new Vec2(i, 0);
                yield return new Vec2(-i, 0);
                yield return new Vec2(0, i);
                yield return new Vec2(0, -i);
            }
        }

        private long FindVelocity(IReadOnlyList<Hailstone> hailstones)
        {
            var zRange = (int)(hailstones.Select(h => Math.Abs(h.Velocity.Z)).Max() * 2);
            foreach (var currVelocityXY in DiamondGenerator2D())
            {
                Vec2? CheckCommonIntersectionXY()
                {
                    var p1 = hailstones[0].Position.XY;
                    var p2 = hailstones[1].Position.XY;
                    var v1 = hailstones[0].Velocity.XY - currVelocityXY;
                    var v2 = hailstones[1].Velocity.XY - currVelocityXY;
                    var intersectionXY = Intersection(p1, v1, p2, v2);
                    if (intersectionXY is null)
                    {
                        return null;
                    }
                    var eachHailstonePassesThroughIntersection = Enumerable.Range(2, hailstones.Count - 2)
                        .All(i => PointIsOnLine(intersectionXY.Value, hailstones[i].Position.XY, hailstones[i].Velocity.XY - currVelocityXY));
                    if (!eachHailstonePassesThroughIntersection)
                    {
                        return null;
                    }
                    return intersectionXY;
                }
                Vec2? CheckCommonIntersectionXZ(long vz)
                {
                    var currVelocityXZ = new Vec2(currVelocityXY.X, vz);
                    var p1 = hailstones[0].Position.XZ;
                    var p2 = hailstones[1].Position.XZ;
                    var v1 = hailstones[0].Velocity.XZ - currVelocityXZ;
                    var v2 = hailstones[1].Velocity.XZ - currVelocityXZ;
                    var intersectionXZ = Intersection(p1, v1, p2, v2);
                    if (intersectionXZ is null)
                    {
                        return null;
                    }
                    var eachHailstonePassesThroughIntersection = Enumerable.Range(2, hailstones.Count - 2)
                        .All(i => PointIsOnLine(intersectionXZ.Value, hailstones[i].Position.XZ, hailstones[i].Velocity.XZ - currVelocityXZ));
                    if (!eachHailstonePassesThroughIntersection)
                    {
                        return null;
                    }
                    return intersectionXZ;
                }
                var intersectionXY = CheckCommonIntersectionXY();
                if (intersectionXY is null) { continue; }
                var intersectionXZ = AlternatingGenerator(zRange).Select(CheckCommonIntersectionXZ).FirstOrDefault(x => x is not null);
                if (intersectionXZ is null)
                {
                    Console.WriteLine($"No intersection found for velocity: {currVelocityXY}");
                    continue;
                }
                var intersection = new Vec3(intersectionXY.Value.X, intersectionXY.Value.Y, intersectionXZ.Value.Y);
                return (long)(intersection.X + intersection.Y + intersection.Z);
            }
            throw new ArgumentException("No velocity found");
        }
    }
}
