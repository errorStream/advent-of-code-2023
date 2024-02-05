namespace Day3
{
    public class Part2 : CommonFunctionality, Framework.ISolution
    {
        private readonly Dictionary<(int x, int y), List<int>> _numbersByGears = new();
        private readonly HashSet<(int x, int y)> _starsByCurr = new();
        private readonly List<(int x, int y)> _lastColumnStars = new(3);

        protected override void RowStart()
        {
            _starsByCurr.Clear();
            _lastColumnStars.Clear();
        }

        protected override void StartOfNumber()
        {
            foreach (var item in _lastColumnStars) { _ = _starsByCurr.Add(item); }
            if (TopCharacter == '*') { _ = _starsByCurr.Add((X, Y - 1)); }
            if (BottomCharacter == '*') { _ = _starsByCurr.Add((X, Y + 1)); }
        }

        protected override void MiddleOfNumber()
        {
            if (TopCharacter == '*') { _ = _starsByCurr.Add((X, Y - 1)); }
            if (BottomCharacter == '*') { _ = _starsByCurr.Add((X, Y + 1)); }
        }

        protected override void AfterNumber()
        {
            if (TopCharacter == '*') { _ = _starsByCurr.Add((X, Y - 1)); }
            if (BottomCharacter == '*') { _ = _starsByCurr.Add((X, Y + 1)); }
            if (MiddleCharacter == '*') { _ = _starsByCurr.Add((X, Y)); }

            foreach (var coord in _starsByCurr)
            {
                if (!_numbersByGears.TryGetValue(coord, out var nums))
                {
                    nums = new();
                    _numbersByGears.Add(coord, nums);
                }
                nums.Add(ActiveNumber);
            }

            _starsByCurr.Clear();
        }

        protected override void AfterProcessColumn()
        {
            _lastColumnStars.Clear();
            if (TopCharacter == '*') { _lastColumnStars.Add((X, Y - 1)); }
            if (MiddleCharacter == '*') { _lastColumnStars.Add((X, Y)); }
            if (BottomCharacter == '*') { _lastColumnStars.Add((X, Y + 1)); }
        }

        protected override void NumberAtEndOfRow()
        {
            foreach (var coord in _starsByCurr)
            {
                if (!_numbersByGears.TryGetValue(coord, out var nums))
                {
                    nums = new();
                    _numbersByGears.Add(coord, nums);
                }
                nums.Add(ActiveNumber);
            }

            _starsByCurr.Clear();
        }

        protected override long CalcResult()
        {
            var result = 0;

            foreach (var (gear, nums) in _numbersByGears)
            {
                if (nums.Count == 2)
                {
                    result += nums[0] * nums[1];
                }
            }

            return result;
        }
    }
}
