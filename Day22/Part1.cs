namespace Day22
{
    public class Part1 : CommonFunctionality, Framework.ISolution
    {
        public long Run(StreamReader streamReader)
        {
            var allBricks = BuildTree(ParseSnapshot(streamReader));
            var disintegratableBrickCount = 0;

            foreach (var node in allBricks)
            {
                if (!node.Supports.Any(x => x.SupportedBy.Count == 1))
                {
                    disintegratableBrickCount++;
                }
            }

            return disintegratableBrickCount;
        }
    }
}
