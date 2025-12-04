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

        [TestCase("")]
        [TestCase("     ")]
        public void SumOfProducedJoltage_WhenInputIsEmpty_ShouldThrowArgumentException(string input)
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Business.Year2025.Day3.SumOfProducedJoltage(input));
            Assert.That(ex.ParamName, Is.EqualTo("input"));
        }

        [Test]
        public void ExtractBatteryBanks_WhenInputIsProvided_ShouldReturnCorrectNumberOfBanks()
        {
            // Arrange
            var input =
                """
                1234567890
                0987654321
                1111111111
                """;
            // Act
            var banks = Business.Year2025.Day3.ExtractBatteryBanks(input);

            // Assert
            Assert.That(banks, Has.Count.EqualTo(3));
        }

        [TestCase("")]
        [TestCase("     ")]
        public void ExtractBatteryBanks_WhenInputIsEmpty_ShouldThrowArgumentException(string input)
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Business.Year2025.Day3.ExtractBatteryBanks(input));
            Assert.That(ex.ParamName, Is.EqualTo("input"));
        }
    }
}
