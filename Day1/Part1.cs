namespace Day1
{
    public class Part1 : Framework.ISolution
    {
        private static int FindCalibrationValue(string str)
        {
            var res = 0;
            for (int i = 0; i < str.Length; ++i)
            {
                if (char.IsDigit(str[i]))
                {
                    res += (str[i] - '0') * 10;
                    break;
                }
            }

            for (int i = str.Length - 1; i >= 0; --i)
            {
                if (char.IsDigit(str[i]))
                {
                    res += str[i] - '0';
                    break;
                }
            }

            return res;
        }

        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            var total = 0;
            while (streamReader.ReadLine() is string line)
            {
                total += FindCalibrationValue(line);
            }
            return total;
        }
    }
}
