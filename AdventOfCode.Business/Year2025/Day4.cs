namespace AdventOfCode.Business.Year2025
{
    internal class Day4 : IAdventDay
    {
        public void ExecutePart1()
        {
            var day4Input = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Actual", "Day4_Input.txt"));
            var result = CalculateHowManyPaperRollsAreAccessible(day4Input, 8, 4);

            Console.WriteLine($"\r\nCan access {result} rolls of paper with the forklift.\r\n");
        }

        public void ExecutePart2()
        {
            throw new NotImplementedException();
        }

        internal static uint CalculateHowManyPaperRollsAreAccessible(string[] input, uint adjacentPositions, uint maxAdjacentRolls)
        {
            return 0;
        }

        internal static (uint rows, uint columns) GetGridSize(string[] input)
        {
            ArgumentNullException.ThrowIfNull(input);

            if (input.Length == 0 || string.IsNullOrWhiteSpace(input[0]))
            {
                return (0, 0);
            }

            var rows = (uint)input.Length;
            var columns = (uint)input[0].Length;

            return (rows, columns);
        }

        internal static uint[][] CreateMatrix(uint row, uint columns, string input)
        {
            return [];
        }

        internal static CellType GetCellType(char cellChar)
        {
            return cellChar switch
            {
                '@' => CellType.Paper,
                '.' => CellType.None,
                _ => throw new FormatException($"Unexpected character for {nameof(CellType)}"),
            };
        }
    }

    internal enum CellType
    {
        None,
        Paper,
    }
}
