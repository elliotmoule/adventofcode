using System.Collections.ObjectModel;

namespace AdventOfCode.Business.Year2025
{
    internal class Day6 : IAdventDay
    {
        string _root = string.Empty;
        public void SetRoot(string path)
        {
            _root = path;
        }

        public void ExecutePart1()
        {
            var day6Input = File.ReadAllLines(Path.Combine(_root, "Day6_Input.txt"));
            var result = CalculateTotalOfAllAnswers(day6Input, false);

            Console.WriteLine($"\r\nThe grand total found by adding all answers together is: '{result}'.\r\n");
        }

        public void ExecutePart2()
        {
            var day6Input = File.ReadAllLines(Path.Combine(_root, "Day6_Input.txt"));
            var result = CalculateTotalOfAllAnswers(day6Input, true);

            Console.WriteLine($"\r\nThe grand total found by adding all answers together is: '{result}'.\r\n");
        }

        internal static long CalculateTotalOfAllAnswers(string[] input, bool rightToLeft)
        {
            ArgumentNullException.ThrowIfNull(input);

            var operators = RetrieveColumnOperators(input, rightToLeft);

            long sumOfAllColumns = 0;

            if (rightToLeft)
            {
                var inputWithoutOperators = input.Take(input.Length - 1).ToArray();
                var columns = RetrieveColumnsRightToLeft(inputWithoutOperators);

                if (operators.Count != columns.Count)
                {
                    throw new FormatException("The provided operations count does not equal the column values count.");
                }

                for (int i = 0; i < operators.Count; i++)
                {
                    var column = columns[i];
                    var operation = operators[i];
                    sumOfAllColumns += CalculateByColumns(operation, column);
                }
            }
            else
            {
                Dictionary<uint, List<long>> columns = RetrieveColumns(operators.Count, input);

                if (operators.Count != columns.Count)
                {
                    throw new FormatException("The provided operations count does not equal the column values count.");
                }

                for (int i = 0; i < operators.Count; i++)
                {
                    var column = columns[(uint)i];
                    var operation = operators[i];
                    sumOfAllColumns += SumOfColumnValues(operation, column);
                }
            }

            return sumOfAllColumns;
        }

        internal static List<char> RetrieveColumnOperators(string[] input, bool reverse = false)
        {
            ArgumentNullException.ThrowIfNull(input);

            if (input.Length == 0)
            {
                throw new ArgumentException("Provided input needs to have an operations row.");
            }

            var lastRow = input.Last();
            var split = lastRow.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var operations = new Collection<char>();
            for (int i = 0; i < split.Length; i++)
            {
                var character = split[i];
                if (string.IsNullOrWhiteSpace(character))
                {
                    continue;
                }
                operations.Add(split[i][0]);
            }

            return [.. (reverse ? operations.Reverse() : operations)];
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

        internal static long CalculateByColumns(char operatorSymbol, List<string> column)
        {
            ArgumentNullException.ThrowIfNull(column);

            if (column.Count == 0)
            {
                return 0;
            }

            int maxLength = column.Max(s => s.Length);  // Number of columns.

            var firstColumn = true;
            long result = 0;

            for (int col = maxLength - 1; col >= 0; col--)
            {
                string columnDigits = string.Empty;

                // Cephalopods read right to left, most significant at the top.
                for (int row = 0; row < column.Count; row++)
                {
                    if (col < column[row].Length)
                    {
                        char digitChar = column[row][col];

                        if (digitChar == ' ')
                        {
                            continue;
                        }

                        if (!char.IsDigit(digitChar))
                        {
                            throw new ArgumentException($"Invalid character '{digitChar}' in input");
                        }

                        columnDigits += digitChar;
                    }
                }

                // Check the column has any digits, form a number and apply the operator
                if (columnDigits.Length > 0)
                {
                    long columnNumber = long.Parse(columnDigits);

                    if (firstColumn)
                    {
                        result = columnNumber; // First column (rightmost with data)
                        firstColumn = false;
                    }
                    else
                    {
                        result = CalculateByOperation(operatorSymbol, result, columnNumber);
                    }
                }
            }

            return result;
        }

        internal static List<List<string>> RetrieveColumnsRightToLeft(string[] rows)
        {
            ArgumentNullException.ThrowIfNull(rows);

            if (rows.Length == 0)
            {
                throw new ArgumentException("Column blocks cannot be extracted from empty rows.");
            }

            int maxLength = rows.Max(r => r.Length);

            var columns = new List<List<string>>();
            var currentColumn = new List<string>();

            for (int charIndex = maxLength - 1; charIndex >= 0; charIndex--)
            {
                bool allSpaces = true;
                for (int rowIndex = 0; rowIndex < rows.Length; rowIndex++)
                {
                    if (rows[rowIndex][charIndex] != ' ')
                    {
                        allSpaces = false;
                        break;
                    }
                }

                if (allSpaces && currentColumn.Count > 0)
                {
                    for (int i = 0; i < currentColumn.Count; i++)
                    {
                        currentColumn[i] = new string([.. currentColumn[i].Reverse()]); // Right to left.
                    }

                    columns.Add([.. currentColumn]);
                    currentColumn.Clear();
                }
                else if (!allSpaces)
                {
                    // This character belongs to the current column
                    if (currentColumn.Count == 0)
                    {
                        for (int i = 0; i < rows.Length; i++)
                        {
                            currentColumn.Add("");  // Initialising each row now.
                        }
                    }

                    // Add this character to each row's string in the current column
                    for (int rowIndex = 0; rowIndex < rows.Length; rowIndex++)
                    {
                        currentColumn[rowIndex] += rows[rowIndex][charIndex];
                    }
                }
            }

            // Need to do this separately, as we don't have a separator on the far left.
            if (currentColumn.Count > 0)
            {
                for (int i = 0; i < currentColumn.Count; i++)
                {
                    currentColumn[i] = new string([.. currentColumn[i].Reverse()]);
                }
                columns.Add([.. currentColumn]);
            }

            return columns;
        }
    }
}
