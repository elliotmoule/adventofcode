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

        internal static Cell[][] CreateMatrix(uint rows, uint columns, uint adjacentPositions, string[] input)
        {
            ArgumentNullException.ThrowIfNull(input);

            if (input.Length != rows || input[0].Length != columns)
            {
                throw new FormatException("Input string is either null, empty, or does not match the expected column size.");
            }

            var matrix = new Cell[rows][];

            for (int i = 0; i < rows; i++)
            {
                if (input[i].Length != columns)
                {
                    throw new FormatException("Input string rows do not match the expected column size.");
                }

                matrix[i] = new Cell[columns];

                for (int j = 0; j < columns; j++)
                {
                    var adjacentCells = GetAdjacentCellVectors(new UVector((uint)i, (uint)j), rows, columns, adjacentPositions);
                    matrix[i][j] = CreateCell(input[i][j], new((uint)i, (uint)j), adjacentCells);
                }
            }

            return matrix;
        }

        internal static List<UVector> GetAdjacentCellVectors(UVector position, uint maxRows, uint maxColumns, uint adjacentPositionDistance)
        {
            ArgumentNullException.ThrowIfNull(position);

            if (adjacentPositionDistance == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(adjacentPositionDistance), "Distance must be greater than zero.");
            }

            List<UVector> adjacentPositions = [];

            for (int i = 0; i < maxRows; i++)
            {
                for (int j = 0; j < maxColumns; j++)
                {
                    var rowDiff = Math.Abs((int)position.Row - i);
                    var colDiff = Math.Abs((int)position.Column - j);
                    if ((rowDiff == adjacentPositionDistance && colDiff == 0) ||
                        (rowDiff == 0 && colDiff == adjacentPositionDistance) ||
                        (rowDiff == adjacentPositionDistance && colDiff == adjacentPositionDistance))
                    {
                        adjacentPositions.Add(new UVector((uint)i, (uint)j));
                    }
                }
            }

            return adjacentPositions;
        }

        internal static Cell CreateCell(char cellChar, UVector vector, List<UVector> adjacentCells)
        {
            var cellType = GetCellType(cellChar);
            return new Cell(cellType, vector, adjacentCells);
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

        internal class Cell(CellType type, UVector vector, List<UVector> adjacentCells)
        {
            public CellType Type { get; set; } = type;
            public UVector Vector { get; set; } = vector;
            public List<UVector> AdjacentCells { get; set; } = adjacentCells;

            public bool HasNAdjacentCellsOfType(Cell[][] matrix, CellType cellType, uint n)
            {
                uint count = 0;
                foreach (var adjacent in AdjacentCells)
                {
                    if (matrix[adjacent.Row][adjacent.Column].Type == cellType)
                    {
                        count++;
                        if (count >= n)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            public override bool Equals(object? obj)
            {
                if (obj is not Cell other)
                {
                    return false;
                }

                return Type == other.Type
                    && Vector.Equals(other.Vector)
                    && AdjacentCells.Count == other.AdjacentCells.Count
                    && !AdjacentCells.Except(other.AdjacentCells).Any();
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Type, Vector, AdjacentCells);
            }
        }

        internal record UVector(uint Row, uint Column);

        internal enum CellType
        {
            None,
            Paper,
        }
    }
}
