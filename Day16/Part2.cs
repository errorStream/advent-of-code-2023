namespace Day16
{
    public class Part2 : Framework.ISolution
    {
        private enum Direction { North, East, South, West };

        private struct Ray
        {
            public int X;
            public int Y;
            public Direction Direction;

            public Ray Progress()
            {
                return Direction switch
                {
                    Direction.North => new Ray
                    {
                        X = X,
                        Y = Y - 1,
                        Direction = Direction.North
                    },
                    Direction.East => new Ray
                    {
                        X = X + 1,
                        Y = Y,
                        Direction = Direction.East
                    },
                    Direction.South => new Ray
                    {
                        X = X,
                        Y = Y + 1,
                        Direction = Direction.South
                    },
                    Direction.West => new Ray
                    {
                        X = X - 1,
                        Y = Y,
                        Direction = Direction.West
                    },
                    _ => throw new NotImplementedException(Direction.ToString())
                };
            }
        }

        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            var lines = new List<string>();
            while (streamReader.ReadLine() is string line)
            {
                lines.Add(line);
            }

            var grid = new char[lines[0].Length, lines.Count];
            for (int x = 0; x < grid.GetLength(0); ++x)
            {
                for (int y = 0; y < grid.GetLength(1); ++y)
                {
                    grid[x, y] = lines[y][x];
                }
            }

            var maxEnergizeCount = 0;
            int CalcEnergizeCount(Ray startRay)
            {
                var queue = new Queue<Ray>();
                var history = new HashSet<Ray>();

                queue.Enqueue(startRay);

                while (queue.Count > 0)
                {
                    var curr = queue.Dequeue();
                    if (curr.X < 0 || curr.X >= grid.GetLength(0) ||
                        curr.Y < 0 || curr.Y >= grid.GetLength(1) ||
                        history.Contains(curr))
                    {
                        continue;
                    }
                    history.Add(curr);
                    var tile = grid[curr.X, curr.Y];

                    switch (tile)
                    {
                        case '.':
                            {
                                queue.Enqueue(curr.Progress());
                                break;
                            }
                        case '/':
                            {
                                curr.Direction = curr.Direction switch
                                {
                                    Direction.North => Direction.East,
                                    Direction.East => Direction.North,
                                    Direction.South => Direction.West,
                                    Direction.West => Direction.South,
                                    _ => throw new NotImplementedException(),
                                };
                                queue.Enqueue(curr.Progress());
                                break;
                            }
                        case '\\':
                            {
                                curr.Direction = curr.Direction switch
                                {
                                    Direction.North => Direction.West,
                                    Direction.East => Direction.South,
                                    Direction.South => Direction.East,
                                    Direction.West => Direction.North,
                                    _ => throw new NotImplementedException(),
                                };
                                queue.Enqueue(curr.Progress());
                                break;
                            }
                        case '|':
                            {
                                if (curr.Direction is Direction.East or Direction.West)
                                {
                                    curr.Direction = Direction.North;
                                    queue.Enqueue(curr.Progress());
                                    curr.Direction = Direction.South;
                                    queue.Enqueue(curr.Progress());
                                }
                                else
                                {
                                    queue.Enqueue(curr.Progress());
                                }
                                break;
                            }
                        case '-':
                            {
                                if (curr.Direction is Direction.North or Direction.South)
                                {
                                    curr.Direction = Direction.East;
                                    queue.Enqueue(curr.Progress());
                                    curr.Direction = Direction.West;
                                    queue.Enqueue(curr.Progress());
                                }
                                else
                                {
                                    queue.Enqueue(curr.Progress());
                                }
                                break;
                            }
                        default:
                            break;
                    }
                }

                return history.Select(x => (x.X, x.Y)).Distinct().Count();
            }
            for (int i = 0; i < grid.GetLength(0); ++i)
            {
                maxEnergizeCount = Math.Max(maxEnergizeCount, CalcEnergizeCount(new Ray
                {
                    X = i,
                    Y = 0,
                    Direction = Direction.South
                }));
                maxEnergizeCount = Math.Max(maxEnergizeCount, CalcEnergizeCount(new Ray
                {
                    X = i,
                    Y = grid.GetLength(1) - 1,
                    Direction = Direction.North
                }));
            }
            for (int i = 0; i < grid.GetLength(1); ++i)
            {
                maxEnergizeCount = Math.Max(maxEnergizeCount, CalcEnergizeCount(new Ray
                {
                    X = 0,
                    Y = i,
                    Direction = Direction.East
                }));
                maxEnergizeCount = Math.Max(maxEnergizeCount, CalcEnergizeCount(new Ray
                {
                    X = grid.GetLength(0) - 1,
                    Y = i,
                    Direction = Direction.West
                }));
            }


            return maxEnergizeCount;
        }
    }
}
