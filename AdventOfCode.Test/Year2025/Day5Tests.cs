using AdventOfCode.Business.Year2025;
using static AdventOfCode.Business.Year2025.Day5;

namespace AdventOfCode.Test.Year2025
{
    internal class Day5Tests
    {
        [Test]
        public void BuildIngredientDatabase_ShouldReturnCorrectDictionary()
        {
            // Arrange
            var day5 = new Day5();
            var rawDatabase = File.ReadAllText(Path.Combine("Year2025", "Resources", "Example", "Day5_Input.txt"));

            // Act
            var result = day5.BuildIngredientDatabase(rawDatabase);

            // Assert
            Assert.That(result, Has.Count.EqualTo(5));
            Assert.That(result.LastOrDefault().Value, Is.Not.Null);
            Assert.That(result.LastOrDefault().Value.State, Is.EqualTo(IngredientState.Spoiled));
        }
    }
}
