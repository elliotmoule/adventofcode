namespace AdventOfCode.Business.Year2025
{
    internal class Day2 : IAdventDay
    {
        public void ExecutePart1()
        {
            var day2Input = File.ReadAllText(Path.Combine("Year2025", "Resources", "Day2_Input.txt"));
            var result = SumOfBasicRepeatedSequenceProductsInRanges(
                CommaDelimitedList(day2Input)
                .Select(ParseProductRange)
            );

            Console.WriteLine($"\r\nThe sum of all products with repeated sequences in the given ranges is {result}.\r\n");
        }

        public void ExecutePart2()
        {
            throw new NotImplementedException();
        }

        internal static ulong SumOfBasicRepeatedSequenceProductsInRanges(IEnumerable<ProductRange> ranges)
        {
            if (ranges == null)
            {
                throw new ArgumentNullException(nameof(ranges), "Ranges cannot be null or empty.");
            }

            if (!ranges.Any())
            {
                return 0;
            }

            ulong totalSum = 0;
            foreach (var range in ranges)
            {
                totalSum += SumOfBasicRepeatedSequenceProductsInRange(range);
            }
            return totalSum;
        }

        internal static ulong SumOfBasicRepeatedSequenceProductsInRange(ProductRange range)
        {
            if (range == null)
            {
                throw new ArgumentNullException(nameof(range), "Range cannot be null.");
            }

            if (range.Start > range.End)
            {
                throw new ArgumentException("Range start cannot be greater than range end.");
            }

            if (range.Start == range.End)
            {
                return HasBasicRepeatedSequence(range.Start) ? range.Start : 0;
            }

            ulong sum = 0;
            for (ulong number = range.Start; number <= range.End; number++)
            {
                if (HasBasicRepeatedSequence(number))
                {
                    sum += number;
                }
            }
            return sum;
        }

        internal static bool HasBasicRepeatedSequence(ulong number)
        {
            if (number.ToString().Length == 1)
            {
                return false;
            }

            if (IsOddProductNumberLength(number))
            {
                return false;
            }

            var numberStr = number.ToString();
            var halfLength = numberStr.Length / 2;
            var firstHalf = numberStr.Substring(0, halfLength);
            var secondHalf = numberStr.Substring(halfLength, halfLength);
            return firstHalf == secondHalf;
        }

        internal static bool IsOddProductNumberLength(ulong number)
        {
            return number.ToString().Length % 2 != 0;
        }

        internal static ProductRange ParseProductRange(string range)
        {
            if (string.IsNullOrWhiteSpace(range))
            {
                throw new ArgumentException("Range cannot be null or whitespace.", nameof(range));
            }

            if (range.Length < 3)
            {
                throw new FormatException("Range is too short to be valid.");
            }

            if (range.Count(c => c == '-') != 1 || range.StartsWith('-') || range.EndsWith('-'))
            {
                throw new FormatException("Range is not in a valid format.");
            }

            var parts = range.Split('-');
            var start = ulong.Parse(parts[0]);
            var end = ulong.Parse(parts[1]);
            return new ProductRange(start, end);
        }

        internal static string[] CommaDelimitedList(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Input cannot be null or whitespace.", nameof(input));
            }

            if (input.Count(c => c == ',') < 2)
            {
                throw new FormatException("Input is not a valid comma-delimited list.");
            }

            return input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }
    }

    internal record ProductRange(ulong Start, ulong End);
}
