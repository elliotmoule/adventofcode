namespace AdventOfCode.Business.Year2025
{
    internal class Day7 : IAdventDay
    {
        public void ExecutePart1()
        {
            var day7Input = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Actual", "Day7_Input.txt"));
            var result = CountSplits(day7Input);

            Console.WriteLine($"\r\nThe beam will be split '{result}' times.\r\n");
        }

        public void ExecutePart2()
        {
            throw new NotImplementedException();
        }

        internal static int CountSplits(string[] grid)
        {
            ArgumentNullException.ThrowIfNull(grid);

            if (grid.Length == 0)
            {
                return 0;
            }

            var startPos = FindStartPosition(grid);
            return startPos == null
                ? throw new ArgumentException("No starting position 'S' found in grid")
                : SimulateBeams(grid, startPos.Value.Row, startPos.Value.Col);
        }

        internal static (int Row, int Col)? FindStartPosition(string[] grid)
        {
            for (int r = 0; r < grid.Length; r++)
            {
                for (int c = 0; c < grid[r].Length; c++)
                {
                    if (grid[r][c] == 'S')
                    {
                        return (r, c);
                    }
                }
            }
            return null;
        }

        internal static int SimulateBeams(string[] grid, int startRow, int startCol)
        {
            int splitCount = 0;
            Queue<Beam> queue = new();
            HashSet<(int, int, int, int)> visited = [];

            queue.Enqueue(new Beam(startRow, startCol, 1, 0));

            int iterations = 0;
            while (queue.Count > 0)
            {
                iterations++;
                var beam = queue.Dequeue();

                var newBeams = ProcessBeam(grid, beam, visited, out bool didSplit);

                if (didSplit)
                {
                    splitCount++;
                }

                foreach (var newBeam in newBeams)
                {
                    queue.Enqueue(newBeam);
                }
            }

            return splitCount;
        }

        internal static List<Beam> ProcessBeam(string[] grid, Beam beam, HashSet<(int, int, int, int)> visited, out bool didSplit)
        {
            didSplit = false;
            var newBeams = new List<Beam>();

            int newRow = beam.Row + beam.DirRow;
            int newCol = beam.Col + beam.DirCol;

            if (!IsInBounds(grid, newRow, newCol))
            {
                return newBeams;
            }

            var state = (newRow, newCol, beam.DirRow, beam.DirCol);
            if (visited.Contains(state))
            {
                return newBeams;
            }

            visited.Add(state);

            char cell = grid[newRow][newCol];

            if (cell == '^')
            {
                didSplit = true;
                newBeams.AddRange(CreateSplitBeams(newRow, newCol));
            }
            else if (cell == '.' || cell == 'S')
            {
                newBeams.Add(new Beam(newRow, newCol, 1, 0));
            }

            return newBeams;
        }

        internal static bool IsInBounds(string[] grid, int row, int col)
        {
            return row >= 0 && row < grid.Length
                && col >= 0 && col < grid[0].Length;
        }

        internal static List<Beam> CreateSplitBeams(int row, int col)
        {
            return
            [
                new(row, col, 1, -1), // Left
                new(row, col, 1, 1)   // Right
            ];
        }

        internal record Beam(int Row, int Col, int DirRow, int DirCol);
    }
}

