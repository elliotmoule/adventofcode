namespace AdventOfCode.Business.Year2025
{
    internal class Day3 : IAdventDay
    {
        public void ExecutePart1()
        {
            var day3Input = File.ReadAllText(Path.Combine("Year2025", "Resources", "Actual", "Day3_Input.txt"));
            var result = SumOfProducedJoltage(day3Input, 2);

            Console.WriteLine($"\r\nThe total output joltage is {result}.\r\n");
        }

        public void ExecutePart2()
        {
            var day3Input = File.ReadAllText(Path.Combine("Year2025", "Resources", "Actual", "Day3_Input.txt"));
            var result = SumOfProducedJoltage(day3Input, 12);

            Console.WriteLine($"\r\nThe total output joltage is {result}.\r\n");
        }

        internal static ulong SumOfProducedJoltage(string input, uint digits)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Input cannot be null or empty.", nameof(input));
            }

            var banks = ExtractBatteryBanks(input, digits);

            var sumJoltage = 0UL;
            foreach (var bank in banks)
            {
                sumJoltage += bank.Joltage;
            }

            return sumJoltage;
        }

        internal static List<BatteryBank> ExtractBatteryBanks(string input, uint digits)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Input cannot be null or empty.", nameof(input));
            }

            var banks = new List<BatteryBank>();
            var bankStrings = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
            foreach (var bankString in bankStrings)
            {
                banks.Add(new BatteryBank(bankString.Trim(), digits));
            }
            return banks;
        }

        internal class BatteryBank
        {
            internal ulong Joltage { get; private set; }
            internal List<Battery> Batteries { get; private set; }

            public BatteryBank(string bankString, uint digits)
            {
                Batteries = ExtractBatteries(bankString);
                Joltage = CalculateProducedJoltage(Batteries, digits);
            }

            internal static List<Battery> ExtractBatteries(string input)
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    throw new ArgumentException("Input cannot be null or empty.", nameof(input));
                }

                var batteries = new List<Battery>();
                foreach (var rating in input)
                {
                    batteries.Add(new Battery(rating));
                }

                return batteries;
            }

            internal static List<ulong> GetLargestNDigitNumber(List<ulong> numbers, uint digits)
            {
                if (numbers == null || numbers.Count < digits)
                {
                    throw new ArgumentException($"List must contain at least {digits} numbers");
                }

                var result = new List<ulong>((int)digits);
                uint remaining = digits;
                int startPos = 0;

                while (remaining > 0)
                {
                    uint searchLimit = (uint)numbers.Count - remaining + 1;

                    ulong maxDigit = 0;
                    int maxPos = startPos;

                    for (int i = startPos; i < searchLimit; i++)
                    {
                        if (numbers[i] > maxDigit)
                        {
                            maxDigit = numbers[i];
                            maxPos = i;
                        }
                    }

                    result.Add(maxDigit);
                    startPos = maxPos + 1;
                    remaining--;
                }

                return result;
            }

            internal static ulong CalculateProducedJoltage(List<Battery> batteries, uint digits)
            {
                if (batteries == null || batteries.Count < digits)
                {
                    throw new ArgumentException($"List must contain at least {digits} numbers");
                }

                var ratings = GetLargestNDigitNumber([.. batteries.Select(b => b.JoltageRating)], digits);
                var combinedString = string.Empty;
                foreach (var rating in ratings)
                {
                    combinedString += rating.ToString();
                }

                return ulong.Parse(combinedString);
            }

            internal class Battery
            {
                internal ulong JoltageRating { get; }

                internal Battery(char rating)
                {
                    if (!char.IsDigit(rating))
                    {
                        throw new ArgumentException("Battery rating must be a digit character.", nameof(rating));
                    }

                    JoltageRating = (ulong)(rating - '0');
                }
            }
        }
    }
}
