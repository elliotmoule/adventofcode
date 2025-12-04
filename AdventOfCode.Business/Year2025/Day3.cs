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
            var batteries = new List<Battery>();
            foreach (var rating in input)
            {
                batteries.Add(new Battery(rating));
            }

            return batteries;
        }

        internal static (uint first, uint second) GetLargestNumbers(List<uint> numbers)
        {
            if (numbers == null || numbers.Count < 2)
            {
                throw new ArgumentException("The list must contain at least two numbers.");
            }

            uint firstLargest = 0;
            uint secondLargest = 0;
            foreach (var number in numbers)
            {
                if (number > firstLargest)
                {
                    secondLargest = firstLargest;
                    firstLargest = number;
                }
                else if (number > secondLargest && number != firstLargest)
                {
                    secondLargest = number;
                }
            }

            return (firstLargest, secondLargest);
        }

        internal static uint CalculateProducedJoltage(List<Battery> batteries)
        {
            var (first, second) = GetLargestNumbers([.. batteries.Select(b => b.JoltageRating)]);
            var combinedString = first.ToString() + second.ToString();
            return uint.Parse(combinedString);
        }
    }

    internal class Battery
    {
        internal uint JoltageRating { get; private set; }

        public Battery(char rating)
        {
        }
    }
}
