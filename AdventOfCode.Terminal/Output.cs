namespace AdventOfCode.Terminal
{
    internal static class Output
    {
        public static bool TryRead(out string input)
        {
            var rawInput = Console.ReadLine();
            input = string.Empty;

            if (rawInput is not string userInput || string.IsNullOrWhiteSpace(userInput.Trim()))
            {
                return false;
            }

            if (userInput.Trim() is not string trimmedInput)
            {
                return false;
            }

            input = trimmedInput;

            return true;
        }

        public static void WriteMultiNewLine(string message)
        {
            if (IsNullOrWhiteSpace(message))
            {
                return;
            }

            Console.WriteLine($"{Environment.NewLine}{message}{Environment.NewLine}");
        }

        public static void WriteLine(string message)
        {
            if (IsNullOrWhiteSpace(message))
            {
                return;
            }

            Console.WriteLine($"{message}");
        }

        public static void Write(string message)
        {
            if (IsNullOrWhiteSpace(message))
            {
                return;
            }

            Console.Write(message);
        }

        public static void WriteImportant(string message)
        {
            if (IsNullOrWhiteSpace(message))
            {
                return;
            }
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            WriteMultiNewLine($"{message}");
            Console.ForegroundColor = previousColor;
        }

        public static void WriteError(string message)
        {
            if (IsNullOrWhiteSpace(message))
            {
                return;
            }
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            WriteLine($"{message}");
            Console.ForegroundColor = previousColor;
        }

        public static void SetColorTemporarily(ConsoleColor color, Action action)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            action.Invoke();
            Console.ForegroundColor = previousColor;
        }

        public static bool QuestionYesNo(string message)
        {
            WriteMultiNewLine(message);
            Write("Please enter Y or N: ");
            if (!TryRead(out var input))
            {
                return false;
            }

            return input.Equals("Y", StringComparison.CurrentCultureIgnoreCase);
        }

        public static uint QuestionNumberInput(string message, uint min, uint max)
        {
            if (min >= max)
            {
                throw new ArgumentException("Min must be less than max.");
            }

            if (max < min)
            {
                throw new ArgumentException("Max must be greater than min.");
            }

            if (min == 0)
            {
                throw new ArgumentException("Min must be greater than 0.");
            }

            while (true)
            {
                WriteMultiNewLine(message);
                Write($"Please enter a number between {min} and {max}: ");

                if (!TryRead(out var input))
                {
                    return 0;
                }

                if (string.IsNullOrWhiteSpace(input))
                {
                    WriteError("Invalid input. Please try again.");
                    continue;
                }

                if (uint.TryParse(input, out uint number))
                {
                    if (number >= min && number <= max)
                    {
                        return number;
                    }
                }
                WriteError("Invalid input. Please try again.");
            }
        }

        static bool IsNullOrWhiteSpace(this string? input)
        {
            return string.IsNullOrWhiteSpace(input);
        }
    }
}
