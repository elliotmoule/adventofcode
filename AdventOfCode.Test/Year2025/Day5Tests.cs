using AdventOfCode.Business.Year2025;
using static AdventOfCode.Business.Year2025.Day5;

namespace AdventOfCode.Test.Year2025
{
    internal class Day5Tests
    {
        [Test]
        public void CalculateHowManyIngredientsAreFresh_NullInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => { CalculateHowManyIngredientsAreFresh(null!); });
        }

        [Test]
        public void CalculateHowManyIngredientsAreFresh_EmptyInput_ReturnsZero()
        {
            // Act
            var result = CalculateHowManyIngredientsAreFresh([]);

            // Assert
            Assert.That(result, Is.EqualTo(0u));
        }

        [Test]
        public void CalculateHowManyIngredientsAreFresh_ValidShortInput_ReturnsCorrectResult()
        {
            // Arrange
            var inputLines = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Example", "Day5_Input.txt"));

            // Act
            var result = Day5.CalculateHowManyIngredientsAreFresh(inputLines);

            // Assert
            Assert.That(result, Is.EqualTo(3u));
        }

        [Test]
        public void CalculateHowManyIngredientsAreFresh_ValidLongInput_ReturnsCorrectResult()
        {
            // Arrange
            string[] inputLines =
            [
                "155212176866261-163127841533232",
                "124646211327227-148172726118633",
                "",
                "155346879709739",
                "416106857700518",
                "142630980526821",
                "426134376523642",
                "136311093050199",
                "555976078480392",
            ];

            // Act
            var result = Day5.CalculateHowManyIngredientsAreFresh(inputLines);

            // Assert
            Assert.That(result, Is.EqualTo(3u));
        }

        [Test]
        public void BuildIngredientLists_NullInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => BuildIngredientLists(null!));
        }

        [Test]
        public void BuildIngredientLists_EmptyInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => BuildIngredientLists([]));
        }

        [Test]
        public void BuildIngredientLists_ValidInput_ReturnsCorrectResult()
        {
            // Arrange
            List<ushort> expectedIngredientIds = [1, 2, 8, 12, 15, 19];
            List<URange> expectedFreshIngredientIds = [new(2, 8), new(6, 12), new(14, 16)];
            (List<ushort> ingredientIds, List<URange> freshIngredientIds) expected = new(expectedIngredientIds, expectedFreshIngredientIds);
            string[] input =
                [
                    "2-8",
                    "6-12",
                    "14-16",
                    "",
                    "1",
                    "2",
                    "8",
                    "12",
                    "15",
                    "19",
                ];

            // Act
            var (ingredientIds, freshIngredientIds) = BuildIngredientLists(input);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(ingredientIds, Is.EquivalentTo(expected.ingredientIds));
                Assert.That(freshIngredientIds, Is.EquivalentTo(expected.freshIngredientIds));
            });
        }

        [Test]
        public void GetRange_MissingRangeDividerInput_ThrowsFormatException()
        {
            // Arrange
            var input = "14 17";

            // Act & Assert
            Assert.Throws<FormatException>(() => GetRange(input));
        }

        [Test]
        public void GetRange_NonNumericInputForMin_ThrowsFormatException()
        {
            // Arrange
            var input = "Z 17";

            // Act & Assert
            Assert.Throws<FormatException>(() => GetRange(input));
        }

        [Test]
        public void GetRange_NonNumericInputForMax_ThrowsFormatException()
        {
            // Arrange
            var input = "8 Z";

            // Act & Assert
            Assert.Throws<FormatException>(() => GetRange(input));
        }

        [Test]
        public void GetRange_ValidInput_ReturnsCorrectResult()
        {
            // Arrange
            var expected = new URange(12, 26);
            var input = "12-26";

            // Act
            var result = GetRange(input);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Start, Is.EqualTo(12u));
                Assert.That(result.End, Is.EqualTo(26u));
                Assert.That(result, Is.EqualTo(expected));
            });
        }

        [Test]
        public void GetFreshIngredientCountFromAvailableIngredientIds_NullIngredientIdsInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => GetFreshIngredientCountFromAvailableIngredientIds(null!, []));
        }

        [Test]
        public void GetFreshIngredientCountFromAvailableIngredientIds_NullFreshIngredientIdsInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => GetFreshIngredientCountFromAvailableIngredientIds([], null!));
        }

        [Test]
        public void GetFreshIngredientCountFromAvailableIngredientIds_NullAllInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => GetFreshIngredientCountFromAvailableIngredientIds(null!, null!));
        }

        [Test]
        public void GetFreshIngredientCountFromAvailableIngredientIds_EmptyIngredientIdsList_ReturnsZero()
        {
            // Arrange
            List<ulong> ingredientIds = [];
            List<URange> ingredientDatabase =
            [
                new(6,10),
                new(8,12),
                new(14,15),
                new(16,20),
            ];

            // Act
            var result = GetFreshIngredientCountFromAvailableIngredientIds(ingredientIds, ingredientDatabase);

            // Assert
            Assert.That(result, Is.EqualTo(0u));
        }

        [Test]
        public void GetFreshIngredientCountFromAvailableIngredientIds_EmptyIngredientDatabase_ReturnsZero()
        {
            // Arrange
            List<ulong> ingredientIds = [2, 6, 8, 12, 16];
            List<URange> ingredientDatabase = [];

            // Act
            var result = GetFreshIngredientCountFromAvailableIngredientIds(ingredientIds, ingredientDatabase);

            // Assert
            Assert.That(result, Is.EqualTo(0u));
        }

        [Test]
        public void GetFreshIngredientCountFromAvailableIngredientIds_ValidInput_ReturnsCorrectResult()
        {
            // Arrange
            List<ulong> ingredientIds = [2, 6, 8, 11, 13, 16, 21];
            List<URange> ingredientDatabase =
            [
                new(6,10),
                new(8,12),
                new(14,15),
                new(16,20),
            ];

            // Act
            var result = GetFreshIngredientCountFromAvailableIngredientIds(ingredientIds, ingredientDatabase);

            // Assert
            Assert.That(result, Is.EqualTo(4u));
        }

        [Test]
        public void ExistsWithinRange_NullRange_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ExistsWithinRange(null!, 5));
        }

        [Test]
        public void ExistsWithinRange_NumberIsZero_ReturnsFalse()
        {
            // Arrange
            URange range = new(6, 9);

            // Act
            var actual = ExistsWithinRange(range, 0);

            // Assert
            Assert.That(actual, Is.False);
        }

        [Test]
        public void ExistsWithinRange_NumberIsOutsideRange_ReturnsFalse()
        {
            // Arrange
            URange range = new(6, 9);

            // Act
            var actual = ExistsWithinRange(range, 10);

            // Assert
            Assert.That(actual, Is.False);
        }

        [Test]
        public void ExistsWithinRange_NumberIsInsideRange_ReturnsTrue()
        {
            // Arrange
            URange range = new(6, 9);

            // Act
            var actual = ExistsWithinRange(range, 7);

            // Assert
            Assert.That(actual, Is.True);
        }

        [Test]
        public void ExistsWithinAnyRange_NumberZero_ReturnsFalse()
        {
            // Arrange
            List<URange> ranges = [new(6, 9), new(6, 12), new(14, 18)];

            // Act
            var actual = ExistsWithinAnyRange(ranges, 0);

            // Assert
            Assert.That(actual, Is.False);
        }

        [Test]
        public void ExistsWithinAnyRange_RangeIsEmpty_ReturnsFalse()
        {
            // Arrange
            List<URange> ranges = [];

            // Act
            var actual = ExistsWithinAnyRange(ranges, 0);

            // Assert
            Assert.That(actual, Is.False);
        }

        [Test]
        public void ExistsWithinAnyRange_RangeIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ExistsWithinAnyRange(null!, 6));
        }

        [Test]
        public void ExistsWithinAnyRange_NumberIsOutsideRanges_ReturnsFalse()
        {
            // Arrange
            List<URange> ranges = [new(6, 9), new(6, 12), new(14, 18)];

            // Act
            var actual = ExistsWithinAnyRange(ranges, 20);

            // Assert
            Assert.That(actual, Is.False);
        }

        [Test]
        public void ExistsWithinAnyRange_NumberIsWithinRanges_ReturnsTrue()
        {
            // Arrange
            List<URange> ranges = [new(6, 9), new(6, 12), new(14, 18)];

            // Act
            var actual = ExistsWithinAnyRange(ranges, 17);

            // Assert
            Assert.That(actual, Is.True);
        }
    }
}
