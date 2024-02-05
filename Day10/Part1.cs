namespace Day10
{
    public class Part1 : CommonFunctionality, Framework.ISolution
    {
        public long Run(StreamReader streamReader)
        {
            var (grid, start) = Parse(streamReader);

            return FindPath(grid, start).Count / 2;
        }
    }
}
