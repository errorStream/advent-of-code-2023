namespace Day3
{
    public class Part1 : CommonFunctionality, Framework.ISolution
    {
        private int _sum;
        private bool _lastColumnHadSymbol;
        private bool _numberBySymbol;

        private static bool IsSymbol(char? ch)
        {
            return ch is char c && c != '.' && !char.IsDigit(c);
        }

        protected override void RowStart()
        {
            _lastColumnHadSymbol = false;
            _numberBySymbol = false;
        }

        private bool HasSymbol =>
                        IsSymbol(MiddleCharacter) ||
                        IsSymbol(TopCharacter) ||
                        IsSymbol(BottomCharacter);

        protected override void StartOfNumber()
        {
            _numberBySymbol = _lastColumnHadSymbol || HasSymbol;
        }

        protected override void MiddleOfNumber()
        {
            _numberBySymbol = _numberBySymbol || HasSymbol;
        }

        protected override void AfterNumber()
        {
            _numberBySymbol = _numberBySymbol || HasSymbol;
            if (_numberBySymbol)
            {
                _sum += ActiveNumber;
            }
            _numberBySymbol = false;
        }

        protected override void AfterProcessColumn()
        {
            _lastColumnHadSymbol = HasSymbol;
        }

        protected override void NumberAtEndOfRow()
        {
            if (_numberBySymbol)
            {
                _sum += ActiveNumber;
            }
        }

        protected override long CalcResult()
        {
            return _sum;
        }
    }
}
