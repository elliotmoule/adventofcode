namespace AdventOfCode.Business.Year2025
{
    internal class Day3 : IAdventDay
    {
        public void ExecutePart1()
        {
            var day3Input = File.ReadAllText(Path.Combine("Year2025", "Resources", "Day3_Input.txt"));
            var result = SumOfProducedJoltage(day3Input);

            Console.WriteLine($"\r\nThe dial was at position 0 a total of {result} times.\r\n");
        }

        public void ExecutePart2()
        {
            throw new NotImplementedException();
        }

        internal static uint SumOfProducedJoltage(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Input cannot be null or empty.", nameof(input));
            }

            var banks = ExtractBatteryBanks(input);
            return (uint)banks.Sum(b => b.Joltage);
        }

        internal static List<BatteryBank> ExtractBatteryBanks(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Input cannot be null or empty.", nameof(input));
            }

            var banks = new List<BatteryBank>();
            var bankStrings = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
            foreach (var bankString in bankStrings)
            {
                banks.Add(new BatteryBank(bankString.Trim()));
            }
            return banks;
        }
    }

    internal class BatteryBank
    {
        internal uint Joltage { get; private set; }
        internal List<Battery> Batteries { get; private set; }

        public BatteryBank(string bankString)
        {
            Batteries = ExtractBatteries(bankString);
            Joltage = CalculateProducedJoltage(Batteries);
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

        internal static (uint first, uint second) GetLargestTwoDigitNumber(List<uint> numbers)
        {
            if (numbers == null || numbers.Count < 2)
            {
                throw new ArgumentException("List must contain at least two numbers");
            }

            uint maxNumber = 0;
            uint bestFirst = 0;
            uint bestSecond = 0;
            uint currentFirst = numbers[0];

            // Go left to right, track the best first digit seen so far
            for (int i = 1; i < numbers.Count; i++)
            {
                // Form a two-digit number with current best first digit and current number
                uint candidate = currentFirst * 10 + numbers[i];
                if (candidate > maxNumber)
                {
                    maxNumber = candidate;
                    bestFirst = currentFirst;
                    bestSecond = numbers[i];
                }

                // Update the best first digit if current number is better
                if (numbers[i] > currentFirst)
                {
                    currentFirst = numbers[i];
                }
            }

            return (bestFirst, bestSecond);
        }

        internal static uint CalculateProducedJoltage(List<Battery> batteries)
        {
            if (batteries == null || batteries.Count < 2)
            {
                throw new ArgumentException("List must contain at least two numbers");
            }

            var (first, second) = GetLargestTwoDigitNumber([.. batteries.Select(b => b.JoltageRating)]);
            var combinedString = first.ToString() + second.ToString();
            return uint.Parse(combinedString);
        }
    }

    internal class Battery
    {
        internal uint JoltageRating { get; }

        internal Battery(char rating)
        {
            if (!char.IsDigit(rating))
            {
                throw new ArgumentException("Battery rating must be a digit character.", nameof(rating));
            }

            JoltageRating = (uint)(rating - '0');
        }
    }
}
