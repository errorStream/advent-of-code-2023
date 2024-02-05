using System.Diagnostics;

namespace Day18
{
    public abstract class CommonFunctionality
    {
        protected static void PrintGrid(HashSet<(long x, long y)> grid)
        {
            if (grid == null)
            {
                return;
            }
            long minX = grid.Min(x => x.x);
            long maxX = grid.Max(x => x.x);
            long minY = grid.Min(x => x.y);
            long maxY = grid.Max(x => x.y);
            for (long y = maxY; y >= minY; y--)
            {
                for (long x = minX; x <= maxX; x++)
                {
                    if (grid.Contains((x, y)))
                    {
                        Console.Write('#');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
            }
        }

        protected abstract (char direction, long distance) ParseDirectionAndDistance(string line);

        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            (long x, long y) position = (x: 0, y: 0);
            long minX = long.MaxValue;
            long maxX = long.MinValue;
            long minY = long.MaxValue;
            long maxY = long.MinValue;
            List<(bool start, long x, long y)> events = new();
            while (streamReader.ReadLine() is string line)
            {
                (char direction, long distance) = ParseDirectionAndDistance(line);
                (long lastX, long lastY) = position;

                switch (direction)
                {
                    case 'L':
                        position.x -= distance;
                        break;
                    case 'R':
                        position.x += distance;
                        break;
                    case 'U':
                        position.y += distance;
                        break;
                    case 'D':
                        position.y -= distance;
                        break;
                    default:
                        throw new ArgumentException($"Unknown direction: {direction}");
                }
                minX = Math.Min(minX, position.x);
                maxX = Math.Max(maxX, position.x);
                minY = Math.Min(minY, position.y);
                maxY = Math.Max(maxY, position.y);

                if (direction is 'U')
                {
                    events.Add((true, lastX, lastY));
                    events.Add((false, position.x, position.y));
                }
                else if (direction is 'D')
                {
                    events.Add((false, lastX, lastY));
                    events.Add((true, position.x, position.y));
                }
            }

            Debug.Assert(Math.Abs(position.x) <= 1, "Didn't end up at the origin");
            Debug.Assert(Math.Abs(position.y) <= 1, "Didn't end up at the origin");

            events.Sort((a, b) =>
            {
                int res = a.y.CompareTo(b.y);
                return res != 0 ? res : a.start != b.start ? a.start ? -1 : 1 : a.x.CompareTo(b.x);
            });

            SortedList<long, long> state = new();
            long lastLevel = minY - 1;
            long filledTileCount = 0;
            int eventIndex = 0;
            List<(bool start, long x, long y)> currentLevelEvents = new();

            void AddInternalSpace(long blockHeight)
            {
                long filledTileCountBefore = filledTileCount;
                bool inside = false;
                long lastX = minX;
                foreach (KeyValuePair<long, long> item in state)
                {
                    if (inside)
                    {
                        filledTileCount += (item.Key - lastX + 1) * blockHeight;
                    }
                    lastX = item.Key;
                    inside = !inside;
                }
                if (inside)
                {
                    throw new InvalidOperationException("Should not be inside at end of line");
                }
            }

            void AddInternalEvent()
            {
                long filledTileCountBefore = filledTileCount;
                bool inside = false;
                long lastX = minX;
                Dictionary<long, string> types = new();
                foreach ((bool start, long x, long y) in currentLevelEvents)
                {
                    types[x] = start ? "start" : "end";
                }
                int index = 0;
                long lastWall = 0;
                while (index < state.Count)
                {
                    long currState = state.GetValueAtIndex(index);
                    if (!types.TryGetValue(currState, out string? currType)) { currType = "passthrough"; }
                    long? nextState = index + 1 < state.Count ? state.GetValueAtIndex(index + 1) : null;
                    string? nextType = null;
                    {
                        if (nextState is long ns && !types.TryGetValue(ns, out nextType))
                        {
                            nextType = "passthrough";
                        }
                    }

                    if (currType is "start" or "end" && nextType is "start" or "end")
                    {
                        if (inside)
                        {
                            filledTileCount += currState - lastX - 1;
                        }
                        filledTileCount += lastWall;
                        long wall = nextState!.Value - currState + 1;
                        lastWall = wall;
                        lastX = nextState.Value;
                        if (currType != nextType)
                        {
                            inside = !inside;
                        }
                        index += 2;
                    }
                    else
                    {
                        if (inside)
                        {
                            filledTileCount += currState - lastX - 1;
                        }
                        filledTileCount += lastWall;
                        lastWall = 1;
                        lastX = currState;
                        inside = !inside;
                        index++;
                    }
                }
                filledTileCount += lastWall;

                if (inside)
                {
                    throw new InvalidOperationException("Should not be inside at end of line");
                }
            }
            while (eventIndex < events.Count)
            {
                (bool _, long _, long y) = events[eventIndex];
                currentLevelEvents.Clear();
                while (eventIndex < events.Count && events[eventIndex].y == y)
                {
                    currentLevelEvents.Add(events[eventIndex]);
                    eventIndex++;
                }

                // add area from just below level to last level
                AddInternalSpace(y - lastLevel - 1);

                // add new starts from current level
                foreach ((bool start, long x, long y) item in currentLevelEvents)
                {
                    if (item.start)
                    {
                        state.Add(item.x, item.x);
                    }
                }

                // add single layer of current level
                AddInternalEvent();

                // set current level
                lastLevel = y;

                // remove stopped levels
                foreach ((bool start, long x, long y) item in currentLevelEvents)
                {
                    if (!item.start)
                    {
                        if (!state.Remove(item.x))
                        {
                            throw new InvalidOperationException("Couldn't remove " + item.x + " from " + string.Join(", ", state));
                        }
                    }
                }
            }

            // add area from just below last level to maxY
            AddInternalSpace(maxY - lastLevel);

            return filledTileCount;
        }
    }
}
