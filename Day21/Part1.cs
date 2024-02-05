namespace Day21
{
    public class Part1 : CommonFunctionality, Framework.ISolution
    {
        public long Run(StreamReader streamReader)
        {
            char[,] grid = ReadGrid(streamReader);

            return CalcEndPoints(64, grid).Count;
        }
    }
}
