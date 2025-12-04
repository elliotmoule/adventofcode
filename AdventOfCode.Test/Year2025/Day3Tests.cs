namespace AdventOfCode.Test.Year2025
{
    internal class Day3Tests
    {
        [Test]
        public void SumOfProducedJoltage_WhenInputIsProvided_ShouldReturnExpectedResult()
        {
            // Arrange
            var input =
                """
                987654321111111
                811111111111119
                234234234234278
                818181911112111
                """;

            // Act
            var result = Business.Year2025.Day3.SumOfProducedJoltage(input);

            // Assert
            uint expected = 357;
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
