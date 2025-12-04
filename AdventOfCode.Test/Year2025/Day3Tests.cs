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
        public void SumOfProducedJoltage_WhenInvalidInputIsProvided_ShouldReturn0()
        {
            // Arrange
            var input =
                """
                987654321111111
                ThisIsInvalidInput
                234234234234278
                818181911112111
                """;

            // Act & Assert
            Assert.Multiple(() =>
            {
                var ex = Assert.Throws<ArgumentException>(() => Business.Year2025.Day3.SumOfProducedJoltage(input));
                Assert.That(ex!.ParamName, Is.EqualTo("rating"));
                Assert.That(ex!.Message, Does.Contain("Battery rating must be a digit character"));
            });
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

        [Test]
        public void ExtractBatteryBanks_WhenInvalidInputIsProvided_ShouldThrowArgumentException()
        {
            // Arrange
            var input =
                """
                1234567890
                InvalidInputHere
                1111111111
                """;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Business.Year2025.Day3.ExtractBatteryBanks(input));
        }

        [Test]
        public void ExtractBatteries_WhenBankStringIsProvided_ShouldReturnCorrectNumberOfBatteries()
        {
            // Arrange
            var bankString = "1234567890";

            // Act
            var batteries = Business.Year2025.BatteryBank.ExtractBatteries(bankString);

            // Assert
            Assert.That(batteries, Has.Count.EqualTo(10));
        }

        [Test]
        public void ExtractBatteries_WhenInvalidBankStringIsProvided_ShouldThrowArgumentException()
        {
            // Arrange
            var bankString = "12A34B6789";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Business.Year2025.BatteryBank.ExtractBatteries(bankString));
        }

        [TestCase("")]
        [TestCase("    ")]
        public void ExtractBatteries_WhenEmptyBankStringIsProvided_ShouldThrowArgumentException(string bankString)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Business.Year2025.BatteryBank.ExtractBatteries(bankString));
        }

        [Test]
        public void GetLargestNumbers_WhenValidNumberListIsProvided_ShouldReturnCorrectFirstAndSecondLargestNumbers()
        {
            // Arrange
            List<uint> numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9];

            // Act
            (uint first, uint second) = Business.Year2025.BatteryBank.GetLargestTwoDigitNumber(numbers);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(first, Is.EqualTo(8));
                Assert.That(second, Is.EqualTo(9));
            });
        }

        [Test]
        public void GetLargestNumbers_WhenInvalidNumberListIsProvided_ShouldThrowArgumentException()
        {
            // Arrange
            List<uint> numbers = [1];

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Business.Year2025.BatteryBank.GetLargestTwoDigitNumber(numbers));
            Assert.That(ex.Message, Does.Contain("at least two numbers"));
        }

        [Test]
        public void GetLargestNumbers_WhenNullNumberListIsProvided_ShouldThrowArgumentException()
        {
            // Arrange
            List<uint>? numbers = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Business.Year2025.BatteryBank.GetLargestTwoDigitNumber(numbers!));
            Assert.That(ex.Message, Does.Contain("at least two numbers"));
        }

        [Test]
        public void GetLargestNumbers_WhenMultipleBatteriesAreProvided_ShouldReturnCorrectLargestNumbers()
        {
            // Arrange
            var actualValues = new Dictionary<int, (uint first, uint second)>();

            List<List<uint>> batteries =
            [
                [4, 9, 7, 5, 9, 3],
                [1, 2, 3, 4, 5, 6],
                [1, 1, 8, 2, 1, 4],
                [5, 4, 3, 6, 9, 2],
                [3, 8, 1, 2, 1, 1],
            ];

            // Act
            foreach (var battery in batteries)
            {
                (uint first, uint second) = Business.Year2025.BatteryBank.GetLargestTwoDigitNumber(battery);
                actualValues.Add(actualValues.Count, (first, second));
            }

            // Assert
            var expectedValues = new Dictionary<int, (uint first, uint second)>
            {
                { 0, (9, 9) },
                { 1, (5, 6) },
                { 2, (8, 4) },
                { 3, (9, 2) },
                { 4, (8, 2) },
            };

            Assert.That(actualValues, Has.Count.EqualTo(expectedValues.Count));
            Assert.Multiple(() =>
            {
                foreach (var (index, (first, second)) in actualValues)
                {
                    Assert.That(first, Is.EqualTo(expectedValues[index].first));
                    Assert.That(second, Is.EqualTo(expectedValues[index].second));
                }
            });
        }

        [Test]
        public void CalculateProducedJoltage_WhenBatteriesAreProvided_ShouldReturnCorrectJoltage()
        {
            // Arrange
            var batteries = new List<Business.Year2025.Battery>
            {
                new('9'),
                new('5'),
                new('3'),
                new('7'),
            };

            // Act
            var joltage = Business.Year2025.BatteryBank.CalculateProducedJoltage(batteries);

            // Assert
            uint expectedJoltage = 97;
            Assert.That(joltage, Is.EqualTo(expectedJoltage));
        }

        [Test]
        public void CalculateProducedJoltage_WhenNoBatteriesAreProvided_ShouldThrowArgumentException()
        {
            // Arrange
            var batteries = new List<Business.Year2025.Battery>();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Business.Year2025.BatteryBank.CalculateProducedJoltage(batteries));
            Assert.That(ex.Message, Does.Contain("at least two numbers"));
        }
    }
}
