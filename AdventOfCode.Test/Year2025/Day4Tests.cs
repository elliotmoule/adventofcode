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
        public void GetGridSize_ValidInput_ReturnsCorrectSize()
        {
            // Act
            var (rows, columns) = AdventOfCode.Business.Year2025.Day4.GetGridSize(TestInput);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(rows, Is.EqualTo(5ul));
                Assert.That(columns, Is.EqualTo(10ul));
            });
        }

        [TestCase("")]
        [TestCase("    ")]
        public void GetGridSize_EmptyOrWhiteSpaceInput_ReturnsZeroSize(string input)
        {
            // Act
            var (rows, columns) = AdventOfCode.Business.Year2025.Day4.GetGridSize(input);

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
            var (rows, columns) = AdventOfCode.Business.Year2025.Day4.GetGridSize("@.@.@.@.@.");
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
            // Act
            var (rows, columns) = AdventOfCode.Business.Year2025.Day4.GetGridSize(
                """
                @
                .
                @
                .
                @
                .
                @
                .
                @
                .
                """);
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
                AdventOfCode.Business.Year2025.Day4.GetGridSize(null!);
            });
        }

        [Test]
        public void CreateMatrix_ValidInput_ReturnsCorrectMatrix()
        {
            // Arrange

            // Act

            // Assert
        }

        [Test]
        public void CreateMatrix_InvalidInput_ThrowsArgumentException()
        {
            // Arrange

            // Act

            // Assert
        }

        [Test]
        public void CreateMatrix_NullInput_ThrowsArgumentException()
        {
            // Arrange

            // Act

            // Assert
        }

        [TestCase("")]
        [TestCase("     ")]
        public void CreateMatrix_EmptyOrWhiteSpace_ThrowsArgumentException(string input)
        {
            // Arrange

            // Act

            // Assert
        }

        [Test]
        public void GetCellType_ValidInputs_ReturnsCorrectCellType()
        {
            // Arrange & Act & Assert
            Assert.Multiple(() =>
            {
                Assert.That(AdventOfCode.Business.Year2025.Day4.GetCellType('@'), Is.EqualTo(AdventOfCode.Business.Year2025.CellType.Paper));
                Assert.That(AdventOfCode.Business.Year2025.Day4.GetCellType('.'), Is.EqualTo(AdventOfCode.Business.Year2025.CellType.None));
            });
        }

        [Test]
        public void GetCellType_InvalidInput_ThrowsFormatException()
        {
            // Arrange & Act & Assert
            Assert.Throws<FormatException>(() =>
            {
                AdventOfCode.Business.Year2025.Day4.GetCellType('#');
            });
        }

        [Test]
        public void CalculateHowManyPaperRollsAreAccessible_ValidInput_ReturnsCorrectCount()
        {
            // Arrange
            var inputLines = TestInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            // Act
            var result = AdventOfCode.Business.Year2025.Day4.CalculateHowManyPaperRollsAreAccessible(inputLines, 8, 4);

            // Assert
            Assert.That(result, Is.EqualTo(10u));
        }

        [Test]
        public void CalculateHowManyPaperRollsAreAccessible_EmptyInput_ReturnsZero()
        {
            // Arrange
            var inputLines = Array.Empty<string>();

            // Act
            var result = AdventOfCode.Business.Year2025.Day4.CalculateHowManyPaperRollsAreAccessible(inputLines, 8, 4);

            // Assert
            Assert.That(result, Is.EqualTo(0u));
        }
    }
}
