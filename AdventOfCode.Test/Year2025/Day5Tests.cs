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
        public void CalculateHowManyIngredientsAreFresh_ValidInput_ReturnsCorrectResult()
        {
            // Arrange
            var inputLines = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Example", "Day5_Input.txt"));

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
            List<ushort> expectedFreshIngredientIds = [2, 3, 4, 5, 6, 7, 8, 6, 7, 8, 9, 10, 11, 12, 14, 15, 16];
            (List<ushort> ingredientIds, List<ushort> freshIngredientIds) expected = new(expectedIngredientIds, expectedFreshIngredientIds);
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
        public void BuildFreshIngredientDatabase_NullInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => BuildFreshIngredientDatabase(null!));
        }

        [Test]
        public void BuildFreshIngredientDatabase_EmptyInput_ReturnsEmpty()
        {
            // Act
            var result = BuildFreshIngredientDatabase([]);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void BuildFreshIngredientDatabase_WithMixedNumbers_ReturnsOrderedDictionary()
        {
            // Arrange
            var expected = new Dictionary<ushort, Ingredient>
            {
                { 2, new(2, IngredientState.Fresh, false) },
                { 3, new(3, IngredientState.Fresh, false) },
                { 5, new(5, IngredientState.Fresh, false) },
                { 6, new(6, IngredientState.Fresh, false) },
                { 7, new(7, IngredientState.Fresh, false) },
                { 12, new(12, IngredientState.Fresh, false) },
                { 19, new(19, IngredientState.Fresh, false) },
            };
            List<ushort> input = [12, 6, 19, 2, 5, 3, 7];

            // Act
            var result = BuildFreshIngredientDatabase(input);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(expected.Count));
                Assert.That(result, Is.EquivalentTo(expected));
            });
        }

        [Test]
        public void BuildFreshIngredientDatabase_WithDuplicateMixedNumbers_ReturnsDictionaryWithoutDuplicates()
        {
            // Arrange
            var expected = new Dictionary<ushort, Ingredient>
            {
                { 2, new(2, IngredientState.Fresh, false) },
                { 3, new(3, IngredientState.Fresh, true) },
                { 5, new(5, IngredientState.Fresh, false) },
                { 6, new(6, IngredientState.Fresh, true) },
                { 7, new(7, IngredientState.Fresh, false) },
                { 12, new(12, IngredientState.Fresh, false) },
                { 19, new(19, IngredientState.Fresh, false) },
            };
            List<ushort> input = [12, 6, 19, 2, 5, 3, 7, 6, 3];

            // Act
            var result = BuildFreshIngredientDatabase(input);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(expected.Count));
                Assert.That(result, Is.EquivalentTo(expected));
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
            List<ushort> ingredientIds = [];
            Dictionary<ushort, Ingredient> ingredientDatabase = new()
            {
                { 2, new(2, IngredientState.Fresh, false) },
                { 7, new(7, IngredientState.Fresh, true) },
                { 12, new(12, IngredientState.Fresh, false) },
                { 22, new(22, IngredientState.Fresh, false) },
            };

            // Act
            var result = GetFreshIngredientCountFromAvailableIngredientIds(ingredientIds, ingredientDatabase);

            // Assert
            Assert.That(result, Is.EqualTo(0u));
        }

        [Test]
        public void GetFreshIngredientCountFromAvailableIngredientIds_EmptyIngredientDatabase_ReturnsZero()
        {
            // Arrange
            List<ushort> ingredientIds = [2, 6, 8, 12, 16];
            Dictionary<ushort, Ingredient> ingredientDatabase = [];

            // Act
            var result = GetFreshIngredientCountFromAvailableIngredientIds(ingredientIds, ingredientDatabase);

            // Assert
            Assert.That(result, Is.EqualTo(0u));
        }

        [Test]
        public void GetFreshIngredientCountFromAvailableIngredientIds_ValidInput_ReturnsCorrectResult()
        {
            // Arrange
            List<ushort> ingredientIds = [2, 6, 8, 12, 16];
            Dictionary<ushort, Ingredient> ingredientDatabase = new()
            {
                { 2, new(2, IngredientState.Fresh, false) },
                { 7, new(7, IngredientState.Fresh, true) },
                { 12, new(12, IngredientState.Fresh, false) },
                { 22, new(22, IngredientState.Fresh, false) },
            };

            // Act
            var result = GetFreshIngredientCountFromAvailableIngredientIds(ingredientIds, ingredientDatabase);

            // Assert
            Assert.That(result, Is.EqualTo(2u));
        }
    }
}
