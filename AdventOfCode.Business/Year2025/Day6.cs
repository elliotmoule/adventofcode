namespace AdventOfCode.Business.Year2025
{
    internal class Day6 : IAdventDay
    {
        public void ExecutePart1()
        {
            var day6Input = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Actual", "Day6_Input.txt"));
            var result = CalculateTotalOfAllAnswers(day6Input);

            Console.WriteLine($"\r\nThe grand total found by adding all answers together is: '{result}'.\r\n");
        }

        public void ExecutePart2()
        {
            var day6Input = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Actual", "Day6_Input.txt"));
            var result = $"{day6Input}";

            Console.WriteLine($"\r\nThe grand total found by adding all answers together is: '{result}'.\r\n");
        }

        internal static long CalculateTotalOfAllAnswers(string[] input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var operations = RetrieveColumnOperations(input);

            var columns = RetrieveColumns(operations.Count, input);

            if (operations.Count != columns.Count)
            {
                throw new FormatException("The provided operations count does not equal the column values count.");
            }

            long sumOfAllColumns = 0;
            for (int i = 0; i < operations.Count; i++)
            {
                var column = columns[(uint)i];
                var operation = operations[i];
                sumOfAllColumns += SumOfColumnValues(operation, column);
            }

            return sumOfAllColumns;
        }

        internal static List<char> RetrieveColumnOperations(string[] input)
        {
            ArgumentNullException.ThrowIfNull(input);

            if (input.Length == 0)
            {
                throw new ArgumentException("Provided input needs to have an operations row.");
            }

            var lastRow = input.Last();
            var split = lastRow.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var operations = new List<char>();
            for (int i = 0; i < split.Length; i++)
            {
                var character = split[i];
                if (string.IsNullOrWhiteSpace(character))
                {
                    continue;
                }
                operations.Add(split[i][0]);
            }

            return operations;
        }

        internal static Dictionary<uint, List<long>> RetrieveColumns(int operations, string[] input)
        {
            ArgumentNullException.ThrowIfNull(input);

            if (input.Length == 0)
            {
                throw new ArgumentException("Input must have content to retrieve columns.");
            }

            if (operations <= 0)
            {
                throw new ArgumentException("Operations needs to be greater than zero.");
            }

            var lists = new Dictionary<uint, List<long>>(operations);
            uint index = 0;

            foreach (var row in input)
            {
                if (index == input.Length - 1)
                {
                    break;  // Last column.
                }

                var columnValues = RetrieveColumnValues(row);

                for (uint i = 0; i < columnValues.Count; i++)
                {
                    if (lists.TryGetValue(i, out List<long>? value))
                    {
                        value.Add(columnValues[(int)i]);
                    }
                    else
                    {
                        lists.Add(i, [columnValues[(int)i]]);
                    }
                }
                index++;
            }

            return lists;
        }

        internal static List<long> RetrieveColumnValues(string row)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(row);

            var split = row.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (split.Length < 2)
            {
                throw new ArgumentException("Row must have values in at least 2 columns. Example format: 123   456   789  123");
            }

            if (!char.IsDigit(row.TrimStart().First()))
            {
                throw new FormatException("Row must have numeric values.");
            }

            var values = new List<long>();
            for (int i = 0; i < split.Length; i++)
            {
                var character = split[i];
                if (string.IsNullOrWhiteSpace(character))
                {
                    continue;
                }

                if (!long.TryParse(split[i], out var value))
                {
                    throw new FormatException($"String value found cannot be parsed to long ('{split[i]}'), from row: '{row}'");
                }
                values.Add(value);
            }

            return values;
        }

        internal static long SumOfColumnValues(char operation, List<long> column)
        {
            ArgumentNullException.ThrowIfNull(column);

            if (column.Count == 0)
            {
                return 0;
            }

            long sum = 0;
            foreach (var value in column)
            {
                sum = CalculateByOperation(operation, sum, value);
            }

            return sum;
        }

        internal static long CalculateByOperation(char operation, long value1, long value2)
        {
            if (value2 == 0)
            {
                return value1;
            }

            if (value1 == 0)
            {
                return value2;
            }

            return operation switch
            {
                '/' => value1 / value2,
                '*' => value1 * value2,
                '+' => value1 + value2,
                '-' => value1 - value2,
                _ => throw new ArgumentException("Provided character is not a mathematic operation"),
            };
        }
    }
}
