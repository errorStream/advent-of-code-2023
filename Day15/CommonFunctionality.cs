namespace Day15
{
    public abstract class CommonFunctionality
    {
        protected static int HashAlgorithm(string str)
        {
            if (str is null)
            {
                return 0;
            }
            var res = 0;
            foreach (var item in str)
            {
                res += item;
                res *= 17;
                res %= 256;
            }
            return res;
        }
    }
}
