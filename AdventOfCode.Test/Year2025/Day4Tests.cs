using AdventOfCode.Business.Year2025;
using static AdventOfCode.Business.Year2025.Day4;

namespace AdventOfCode.Test.Year2025
{
    internal class Day4Tests
    {
        const string TestInput =
            """
            @@@@@@@@@@
            @.@.@.@.@.
            .@.@.@.@.@
            ..@@..@@..
            @@..@@..@@
            .@..@.@@..
            ..@.@..@@.
            .@.@@@..@@
            @.@...@@..
            ..........
            """;

        [Test]
        public void CalculateHowManyPaperRollsCanBeRemoved_ValidInput_ReturnsCorrectCount()
        {
            // Arrange
            var inputLines = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Example", "Day4_Input.txt"));

            // Act
            var result = Day4.CalculateHowManyPaperRollsCanBeRemoved(inputLines, 1, 4);

            // Assert
            Assert.That(result, Is.EqualTo(43u));
        }

        [Test]
        public void CalculateHowManyPaperRollsAreAccessible_ValidInput_ReturnsCorrectCount()
        {
            // Arrange
            var inputLines = TestInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            // Act
            var result = Day4.CalculateHowManyPaperRollsAreAccessible(inputLines, 1, 4);

            // Assert
            Assert.That(result, Is.EqualTo(29u));
        }

        [Test]
        public void CalculateHowManyPaperRollsAreAccessible_ValidInputFromExample_ReturnsCorrectCount()
        {
            // Arrange
            var inputLines = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Example", "Day4_Input.txt"));

            // Act
            var result = Day4.CalculateHowManyPaperRollsAreAccessible(inputLines, 1, 4);

            // Assert
            Assert.That(result, Is.EqualTo(13u));
        }

        [Test]
        public void CalculateHowManyPaperRollsAreAccessible_EmptyInput_ReturnsZero()
        {
            // Arrange
            var inputLines = Array.Empty<string>();

            // Act
            var result = Day4.CalculateHowManyPaperRollsAreAccessible(inputLines, 1, 4);

            // Assert
            Assert.That(result, Is.EqualTo(0u));
        }

        [Test]
        public void GetGridSize_ValidInput_ReturnsCorrectSize()
        {
            // Act
            var (rows, columns) = Day4.GetGridSize(TestInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries));

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(rows, Is.EqualTo(10ul));
                Assert.That(columns, Is.EqualTo(10ul));
            });
        }

        [TestCase("")]
        [TestCase("    ")]
        public void GetGridSize_EmptyOrWhiteSpaceInput_ReturnsZeroSize(string input)
        {
            // Act
            var (rows, columns) = Day4.GetGridSize([input]);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(rows, Is.EqualTo(0ul));
                Assert.That(columns, Is.EqualTo(0ul));
            });
        }

        [Test]
        public void GetGridSize_SingleLineInput_ReturnsCorrectSize()
        {
            // Act
            var (rows, columns) = Day4.GetGridSize(["@.@.@.@.@."]);
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(rows, Is.EqualTo(1ul));
                Assert.That(columns, Is.EqualTo(10ul));
            });
        }

        [Test]
        public void GetGridSize_SingleColumnInput_ReturnsCorrectSize()
        {
            // Arrange
            string[] input = ["@", ".", "@", ".", "@", ".", "@", ".", "@", "."];

            // Act
            var (rows, columns) = Day4.GetGridSize(input);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(rows, Is.EqualTo(10ul));
                Assert.That(columns, Is.EqualTo(1ul));
            });
        }

        [Test]
        public void GetGridSize_NullInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                Day4.GetGridSize(null!);
            });
        }

        [Test]
        public void CreateMatrix_ValidInput_ReturnsCorrectMatrix()
        {
            // Arrange
            var expected = new Cell[][]
            {
                [
                    new Cell(CellType.Paper, new(0,0), [new(0,1), new(1,1), new(1,0)]),
                    new Cell(CellType.Paper, new(0,1), [new(0,0), new(0,2), new(1,0), new(1,1), new(1,2)]),
                    new Cell(CellType.Paper, new(0,2), [new(1,1), new(1,2), new(0,1)]),
                ],
                [
                    new Cell(CellType.None, new(1,0), [new(0,0), new(0,1), new(1,1), new(2,1), new(2,0)]),
                    new Cell(CellType.Paper, new(1,1), [new(0,0), new(0,1), new(0,2), new(1,0), new(1,2), new(2,0), new(2,1), new(2,2)]),
                    new Cell(CellType.None, new(1,2), [new(0,1), new(0,2), new(1,1), new(2,1), new(2,2)]),
                ],
                [
                    new Cell(CellType.Paper, new(2,0), [new(1,0), new(1,1), new(2,1)]),
                    new Cell(CellType.None, new(2,1), [new(1,0), new(1,1), new(1,2), new(2,0), new(2,2)]),
                    new Cell(CellType.Paper, new(2,2), [new(1,1), new(1,2), new(2,1)]),
                ],
            };

            string[] input = ["@@@", ".@.", "@.@"];

            // Act
            var result = Day4.CreateMatrix(3, 3, 1, input);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0][0], Is.EqualTo(expected[0][0]));
                Assert.That(result[0][1], Is.EqualTo(expected[0][1]));
                Assert.That(result[0][2], Is.EqualTo(expected[0][2]));
                Assert.That(result[1][0], Is.EqualTo(expected[1][0]));
                Assert.That(result[1][1], Is.EqualTo(expected[1][1]));
                Assert.That(result[1][2], Is.EqualTo(expected[1][2]));
                Assert.That(result[2][0], Is.EqualTo(expected[2][0]));
                Assert.That(result[2][1], Is.EqualTo(expected[2][1]));
                Assert.That(result[2][2], Is.EqualTo(expected[2][2]));
            });
        }

        [Test]
        public void CreateMatrix_InvalidInput_ThrowsFormatException()
        {
            // Act & Assert
            Assert.Throws<FormatException>(() =>
            {
                Day4.CreateMatrix(3, 3, 8, ["%&#", "@.@", ".@."]);
            });
        }

        [Test]
        public void CreateMatrix_NullInput_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                Day4.CreateMatrix(3, 3, 8, null!);
            });
        }

        [TestCase("")]
        [TestCase("     ")]
        public void CreateMatrix_EmptyOrWhiteSpace_ThrowsFormatException(string input)
        {
            // Act & Assert
            Assert.Throws<FormatException>(() =>
            {
                Day4.CreateMatrix(3, 3, 8, [input]);
            });
        }

        [Test]
        public void GetCellType_ValidInputs_ReturnsCorrectCellType()
        {
            // Arrange & Act & Assert
            Assert.Multiple(() =>
            {
                Assert.That(Day4.GetCellType('@'), Is.EqualTo(CellType.Paper));
                Assert.That(Day4.GetCellType('.'), Is.EqualTo(CellType.None));
            });
        }

        [Test]
        public void GetCellType_InvalidInput_ThrowsFormatException()
        {
            // Arrange & Act & Assert
            Assert.Throws<FormatException>(() =>
            {
                Day4.GetCellType('#');
            });
        }

        [Test]
        public void CreateCell_ValidInput_ReturnsCorrectCell()
        {
            // Arrange
            var position = new UVector(2, 3);
            var adjacentPositions = new List<UVector> { new(1, 2), new(2, 2), new(3, 2) };

            // Act
            var cell = new Cell(CellType.Paper, position, adjacentPositions);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(cell.Type, Is.EqualTo(CellType.Paper));
                Assert.That(cell.Vector, Is.EqualTo(position));
                Assert.That(cell.AdjacentCells, Is.EquivalentTo(adjacentPositions));
            });
        }

        [Test]
        public void CreateCell_InvalidInput_ThrowsFormatException()
        {
            // Arrange
            var position = new UVector(2, 3);
            var adjacentPositions = new List<UVector> { new(1, 2), new(2, 2), new(3, 2) };

            // Act & Assert
            Assert.Throws<FormatException>(() =>
            {
                Day4.CreateCell('%', position, adjacentPositions);
            });
        }

        [Test]
        public void CreateCell_NotEqualCells_ReturnsFalse()
        {
            // Arrange
            var position = new UVector(2, 3);
            var adjacentPositions1 = new List<UVector> { new(1, 2), new(2, 2), new(3, 2) };
            var adjacentPositions2 = new List<UVector> { new(1, 1), new(2, 2), new(3, 2) };

            // Act
            var cell1 = Day4.CreateCell('@', position, adjacentPositions1);
            var cell2 = Day4.CreateCell('@', position, adjacentPositions2);

            // Assert
            Assert.That(cell1, Is.Not.EqualTo(cell2));
        }

        [Test]
        public void GetAdjacentCellVectors_ValidInput_ReturnsCorrectAdjacentPositions()
        {
            // Arrange
            var position = new UVector(1, 1);
            uint maxRows = 3;
            uint maxColumns = 3;
            uint distance = 1;
            var expectedAdjacentPositions = new List<UVector>
            {
                new(0,1),
                new(1,0),
                new(1,2),
                new(2,1),
                new(0,0),
                new(0,2),
                new(2,0),
                new(2,2),
            };

            // Act
            var result = Day4.GetAdjacentCellVectors(position, maxRows, maxColumns, distance);

            // Assert
            Assert.That(result, Is.EquivalentTo(expectedAdjacentPositions));
        }

        [Test]
        public void GetAdjacentCellVectors_ValidInputDistance2_ReturnsCorrectAdjacentPositions()
        {
            // Arrange
            var position = new UVector(3, 4);
            uint maxRows = 10;
            uint maxColumns = 10;
            uint distance = 2;
            var expectedAdjacentPositions = new List<UVector>
            {
                new(1,2),
                new(1,3),
                new(1,4),
                new(1,5),
                new(1,6),

                new(2,2),
                new(2,3),
                new(2,4),
                new(2,5),
                new(2,6),

                new(3,2),
                new(3,3),
                new(3,5),
                new(3,6),

                new(4,2),
                new(4,3),
                new(4,4),
                new(4,5),
                new(4,6),

                new(5,2),
                new(5,3),
                new(5,4),
                new(5,5),
                new(5,6),
            };

            var expectedVectors = string.Empty;
            foreach (var vector in expectedAdjacentPositions)
            {
                expectedVectors += $"({vector.Row},{vector.Column}) ";
            }

            // Act
            var result = Day4.GetAdjacentCellVectors(position, maxRows, maxColumns, distance);

            var vectors = string.Empty;
            foreach (var vector in result)
            {
                vectors += $"({vector.Row},{vector.Column}) ";
            }

            // Assert
            Assert.That(result, Is.EquivalentTo(expectedAdjacentPositions));
        }

        [Test]
        public void GetAdjacentCellVectors_DistanceZero_ReturnsEmptyList()
        {
            // Arrange
            var position = new UVector(1, 1);
            uint maxRows = 3;
            uint maxColumns = 3;
            uint distance = 0;

            // Act
            var cells = Day4.GetAdjacentCellVectors(position, maxRows, maxColumns, distance);

            // Assert
            Assert.That(cells, Is.Empty);
        }

        [Test]
        public void GetAdjacentCellVectors_NullPosition_ThrowsArgumentNullException()
        {
            // Arrange
            UVector? position = null;
            uint maxRows = 3;
            uint maxColumns = 3;
            uint distance = 1;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                Day4.GetAdjacentCellVectors(position!, maxRows, maxColumns, distance);
            });
        }

        [Test]
        public void GetAdjacentCellVectors_NoAdjacentCells_ReturnsEmptyList()
        {
            // Arrange
            var position = new UVector(0, 0);
            uint maxRows = 1;
            uint maxColumns = 1;
            uint distance = 1;

            // Act
            var result = Day4.GetAdjacentCellVectors(position, maxRows, maxColumns, distance);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void HasNAdjacentCellsOfType_ValidInput_ReturnsCorrectResult()
        {
            // Arrange
            var input = TestInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            Cell[][] matrix = Day4.CreateMatrix(10, 10, 1, input);

            var position = new UVector(1, 1);

            // Act
            var cell = matrix[position.Row][position.Column];
            var result = cell.HasNAdjacentCellsOfType(matrix, CellType.Paper, 4);   // Should have 6.

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void HasNAdjacentCellsOfType_NotEnoughAdjacentCells_ReturnsFalse()
        {
            // Arrange
            var input = TestInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            Cell[][] matrix = Day4.CreateMatrix(10, 10, 1, input);
            var position = new UVector(1, 1);

            // Act
            var cell = matrix[position.Row][position.Column];
            var result = cell.HasNAdjacentCellsOfType(matrix, CellType.Paper, 7);   // Should have 6.

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void HasNAdjacentCellsOfType_NoAdjacentCells_ReturnsFalse()
        {
            // Arrange
            var input = TestInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            Cell[][] matrix = Day4.CreateMatrix(10, 10, 1, input);
            var position = new UVector(9, 9);

            // Act
            var cell = matrix[position.Row][position.Column];
            var result = cell.HasNAdjacentCellsOfType(matrix, CellType.Paper, 1);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void HasNAdjacentCellsOfType_NullMatrix_ThrowsArgumentNullException()
        {
            // Arrange
            var position = new UVector(1, 1);
            var cell = new Cell(CellType.Paper, position, [new(0, 1), new(1, 0), new(1, 2), new(2, 1)]);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                cell.HasNAdjacentCellsOfType(null!, CellType.Paper, 1);
            });
        }

        [Test]
        public void HasNAdjacentCellsOfType_WhenNIs0_ReturnsCorrectResult()
        {
            // Arrange
            var input = TestInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            Cell[][] matrix = Day4.CreateMatrix(10, 10, 1, input);
            var position = new UVector(1, 1);

            // Act
            var cell = matrix[position.Row][position.Column];
            var result = cell.HasNAdjacentCellsOfType(matrix, CellType.Paper, 0);

            // Assert
            Assert.That(result, Is.True);
        }
    }
}
