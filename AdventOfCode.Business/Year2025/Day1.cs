namespace AdventOfCode.Business.Year2025
{
    public class Day1 : IAdventDay
    {
        const int maxPosition = 99;
        const int startingPosition = 50;

        public void ExecutePart1()
        {
            var day1Input = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Actual", "Day1_Input.txt"));
            var result = CalculateNumberOfTimesDialIsAt0(day1Input);

            Console.WriteLine($"\r\nThe dial was at position 0 a total of {result} times.\r\n");
        }

        public void ExecutePart2()
        {
            var day1Input = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Actual", "Day1_Input.txt"));
            var result = CalculateNumberOfTimesDialPasses0(day1Input);

            Console.WriteLine($"\r\nThe dial passed position 0 a total of {result} times.\r\n");
        }

        internal static int CalculateNumberOfTimesDialIsAt0(string[] dialInstructions)
        {
            var currentPosition = startingPosition;
            var timesAtZero = 0;

            foreach (var instruction in dialInstructions)
            {
                var (direction, numberOfTurns) = ParseInstruction(instruction);
                currentPosition = TurnDial(currentPosition, direction, numberOfTurns);

                if (IsAtZeroPosition(currentPosition))
                {
                    timesAtZero++;
                }
            }

            return timesAtZero;
        }

        internal static int CalculateNumberOfTimesDialPasses0(string[] dialInstructions)
        {
            var currentPosition = startingPosition;
            var timesPassedZero = 0;
            foreach (var instruction in dialInstructions)
            {
                var (direction, numberOfTurns) = ParseInstruction(instruction);
                for (int i = 0; i < numberOfTurns; i++)
                {
                    currentPosition = TurnDial(currentPosition, direction, 1);

                    if (IsAtZeroPosition(currentPosition))
                    {
                        timesPassedZero++;
                    }
                }
            }
            return timesPassedZero;
        }

        internal static (Direction direction, int numberOfTurns) ParseInstruction(string instruction)
        {
            var direction = instruction[0] == 'L' ? Direction.Left : Direction.Right;
            var numberOfTurns = int.Parse(instruction[1..]);
            return (direction, numberOfTurns);
        }

        internal static int TurnDial(int currentPosition, Direction direction, int numberOfTurns)
        {
            for (int i = 0; i < numberOfTurns; i++)
            {
                if (direction == Direction.Left)
                {
                    // This formula ensures that when we decrement the currentPosition, if it goes below 0,
                    // it wraps around to maxPosition. The addition of (maxPosition + 1) before taking the modulus ensures that the result is always non-negative.
                    currentPosition = (currentPosition - 1 + maxPosition + 1) % (maxPosition + 1);
                }
                else
                {
                    // Incrementing the currentPosition and using modulus to wrap around if it exceeds maxPosition.
                    currentPosition = (currentPosition + 1) % (maxPosition + 1);
                }
            }

            return currentPosition;
        }

        internal static bool IsAtZeroPosition(int currentPosition) => currentPosition == 0;
    }

    internal enum Direction
    {
        Left,
        Right
    }
}
