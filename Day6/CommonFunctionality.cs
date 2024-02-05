namespace Day6
{
    public abstract class CommonFunctionality
    {
        protected static long ComputeWinOptionCount(long time, long distance)
        {
            var s = Math.Sqrt((time * time) - (4d * distance));
            var lowX = (time - s) / 2d;
            var highX = (time + s) / 2d;

            return (long)(Math.Ceiling(highX) - Math.Floor(lowX) - 1);
        }
    }
}
