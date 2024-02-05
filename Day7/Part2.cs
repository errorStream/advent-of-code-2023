namespace Day7
{
    public class Part2 : Framework.ISolution
    {
        private sealed class Hand : IComparable<Hand>
        {
            public readonly string Cards;
            public readonly int Bid;
            public readonly HandType Type;

            private static HandType ComputeHandType(string cards)
            {
                var cardArr = cards.OrderByDescending(CardStrength).ToArray();
                var count = 1;
                var hash = new int[5];
                var jCount = 0;

                for (int i = 1; i < 5; ++i)
                {
                    if (cardArr[i] == cardArr[i - 1])
                    {
                        count++;
                    }
                    else
                    {
                        hash[count - 1]++;
                        count = 1;
                    }
                }

                if (cardArr[^1] == 'J')
                {
                    jCount = count;
                }
                else
                {
                    hash[count - 1]++;
                }

                if (jCount == 5)
                {
                    hash[4] = 1;
                }
                else
                {
                    for (int i = 4; i >= 0; --i)
                    {
                        if (hash[i] != 0)
                        {
                            hash[i]--;
                            hash[i + jCount]++;
                            break;
                        }
                    }
                }

                var hashV = 0;
                for (int i = 4; i >= 0; --i)
                {
                    hashV = (hashV * 10) + hash[i];
                }

                return hashV switch
                {
                    10000 => HandType.FiveOfAKind,
                    01001 => HandType.FourOfAKind,
                    00110 => HandType.FullHouse,
                    00102 => HandType.ThreeOfAkind,
                    00021 => HandType.TwoPair,
                    00013 => HandType.OnePair,
                    00005 => HandType.HighCard,
                    _ => throw new ArgumentException("Unknown hand hash: " + hashV + " for hand " + cards)
                };
            }

            public Hand(string cards, int bid)
            {
                Cards = cards;
                Bid = bid;
                Type = ComputeHandType(cards);
            }

            private static int CardStrength(char c)
            {
                return c switch
                {
                    'A' => 14,
                    'K' => 13,
                    'Q' => 12,
                    'J' => -1,
                    'T' => 10,
                    _ => c - '0',
                };
            }

            public int CompareTo(Hand? other)
            {
                if (other is null) { return 1; }
                var res = Type.CompareTo(other.Type);

                for (int i = 0; i < 5 && res == 0; ++i)
                {
                    res = CardStrength(Cards[i]).CompareTo(CardStrength(other.Cards[i]));
                }

                return res;
            }
        }

        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            var hands = new List<Hand>();
            while (streamReader.ReadLine() is string line)
            {
                var cards = line[0..5];
                var bid = int.Parse(line[6..], System.Globalization.CultureInfo.InvariantCulture);
                hands.Add(new Hand(cards, bid));
            }
            hands.Sort();
            var res = 0;
            for (int i = 0; i < hands.Count; ++i)
            {
                res += (i + 1) * hands[i].Bid;
            }

            return res;
        }
    }
}
