namespace Day12
{
    public abstract class CommonFunctionality
    {
        protected static long ArrangementCount(string springStatus, int[] groups)
        {
            ArgumentNullException.ThrowIfNull(springStatus);
            ArgumentNullException.ThrowIfNull(groups);
            var dp = new long[springStatus.Length + 1, groups.Length + 1];

            long Lookup(int position, int groupIndex)
            {
                if (position >= springStatus.Length)
                {
                    return groupIndex >= groups.Length ? 1 : 0;
                }
                else
                {
                    return dp[position, groupIndex];
                }
            }

            for (int position = dp.GetLength(0) - 1; position >= 0; --position)
            {
                for (int groupIndex = dp.GetLength(1) - 1; groupIndex >= 0; --groupIndex)
                {
                    if (position >= springStatus.Length)
                    {
                        dp[position, groupIndex] = groupIndex >= groups.Length ? 1 : 0;
                        continue;
                    }

                    bool CheckValidPlacement()
                    {
                        if (groupIndex >= groups.Length)
                        {
                            return false;
                        }
                        var groupSize = groups[groupIndex];
                        for (int i = 0; i < groupSize; ++i)
                        {
                            var currPos = position + i;
                            if (currPos >= springStatus.Length || springStatus[currPos] is '.')
                            {
                                return false;
                            }
                        }
                        if (position + groupSize >= springStatus.Length)
                        {
                            return true;
                        }
                        if (springStatus[position + groupSize] is '#')
                        {
                            return false;
                        }
                        return true;
                    }

                    if (springStatus[position] == '.')
                    {
                        dp[position, groupIndex] = Lookup(position + 1, groupIndex);
                        continue;
                    }

                    if (springStatus[position] == '#')
                    {
                        long res = CheckValidPlacement()
                            ? Lookup(position + groups[groupIndex] + 1,
                                     groupIndex + 1)
                            : 0;
                        dp[position, groupIndex] = res;
                        continue;
                    }

                    if (springStatus[position] == '?')
                    {
                        long res = 0;
                        res += Lookup(position + 1, groupIndex);
                        if (CheckValidPlacement())
                        {
                            res += Lookup(position + groups[groupIndex] + 1, groupIndex + 1);
                        }
                        dp[position, groupIndex] = res;
                        continue;
                    }

                    throw new ArgumentException($"Invalid spring status '{springStatus[position]}'");
                }
            }

            return dp[0, 0];
        }
    }
}
