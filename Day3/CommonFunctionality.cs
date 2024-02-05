namespace Day3
{
    public abstract class CommonFunctionality
    {
        protected virtual void RowStart() { }
        protected virtual void StartOfNumber() { }
        protected virtual void MiddleOfNumber() { }
        protected virtual void AfterNumber() { }
        protected virtual void NumberAtEndOfRow() { }
        protected virtual void AfterProcessColumn() { }
        protected abstract long CalcResult();

        protected int X { get; private set; }
        protected int Y { get; private set; }
        protected char? TopCharacter { get; private set; }
        protected char? BottomCharacter { get; private set; }
        protected char MiddleCharacter { get; private set; }
        protected int ActiveNumber { get; private set; }

        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            var lines = new List<string>();
            while (streamReader.ReadLine() is string line)
            {
                lines.Add(line);
            }

            var width = lines[0].Length;
            for (Y = 0; Y < lines.Count; ++Y)
            {
                ActiveNumber = 0;
                RowStart();
                for (X = 0; X < width; ++X)
                {
                    MiddleCharacter = lines[Y][X];
                    TopCharacter = (Y - 1 >= 0) ? lines[Y - 1][X] : null;
                    BottomCharacter = (Y + 1 < lines.Count) ? lines[Y + 1][X] : null;

                    if (char.IsDigit(MiddleCharacter))
                    {
                        if (ActiveNumber == 0)
                        {
                            StartOfNumber();
                            ActiveNumber = MiddleCharacter - '0';
                        }
                        else
                        {
                            MiddleOfNumber();
                            ActiveNumber = (ActiveNumber * 10) + (MiddleCharacter - '0');
                        }
                    }
                    else
                    {
                        if (ActiveNumber == 0)
                        {
                            // Last position wasn't number
                        }
                        else
                        {
                            // Just after number
                            AfterNumber();
                            ActiveNumber = 0;
                        }
                    }

                    AfterProcessColumn();
                }
                if (ActiveNumber != 0)
                {
                    NumberAtEndOfRow();
                }
            }

            return CalcResult();
        }
    }
}
