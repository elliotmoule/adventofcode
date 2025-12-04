namespace AdventOfCode.Test.Year2025
{
    internal class Day3Tests
    {
        [TestCase(2U, 357U)]
        [TestCase(12U, 3121910778619UL)]
        public void SumOfProducedJoltage_WhenInputIsProvided_ShouldReturnExpectedResult(uint digits, ulong expected)
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
            var result = Business.Year2025.Day3.SumOfProducedJoltage(input, digits);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("")]
        [TestCase("     ")]
        public void SumOfProducedJoltage_WhenInputIsEmpty_ShouldThrowArgumentException(string input)
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Business.Year2025.Day3.SumOfProducedJoltage(input, 2));
            Assert.That(ex.ParamName, Is.EqualTo("input"));
        }

        [Test]
        public void SumOfProducedJoltage_WhenInvalidInputIsProvided_ShouldThrowArgumentException()
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
                var ex = Assert.Throws<ArgumentException>(() => Business.Year2025.Day3.SumOfProducedJoltage(input, 2));
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
                1234567891234567891
                9876543219876543219
                1111111111111111111
                """;
            // Act
            var banks = Business.Year2025.Day3.ExtractBatteryBanks(input, 12);

            // Assert
            Assert.That(banks, Has.Count.EqualTo(3));
        }

        [TestCase("")]
        [TestCase("     ")]
        public void ExtractBatteryBanks_WhenInputIsEmpty_ShouldThrowArgumentException(string input)
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Business.Year2025.Day3.ExtractBatteryBanks(input, 2));
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
            Assert.Throws<ArgumentException>(() => Business.Year2025.Day3.ExtractBatteryBanks(input, 2));
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
            List<ulong> numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9];

            // Act
            var result = Business.Year2025.BatteryBank.GetLargestNDigitNumber(numbers, 2);

            // Assert
            Assert.That(result, Is.EquivalentTo([8, 9]));
        }

        [Test]
        public void GetLargestNumbers_WhenInvalidNumberListIsProvided_ShouldThrowArgumentException()
        {
            // Arrange
            List<ulong> numbers = [1];

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Business.Year2025.BatteryBank.GetLargestNDigitNumber(numbers, 2));
            Assert.That(ex.Message, Does.Contain("at least 2 numbers"));
        }

        [Test]
        public void GetLargestNumbers_WhenNullNumberListIsProvided_ShouldThrowArgumentException()
        {
            // Arrange
            List<ulong>? numbers = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Business.Year2025.BatteryBank.GetLargestNDigitNumber(numbers!, 2));
            Assert.That(ex.Message, Does.Contain("at least 2 numbers"));
        }

        [Test]
        public void GetLargestNumbers_WhenMultipleBatteriesAreProvided_ShouldReturnCorrectLargestNumbers()
        {
            // Arrange
            var actualValues = new Dictionary<int, List<ulong>>();

            List<List<ulong>> batteries =
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
                var result = Business.Year2025.BatteryBank.GetLargestNDigitNumber(battery, 2);
                actualValues.Add(actualValues.Count, result);
            }

            // Assert
            var expectedValues = new Dictionary<int, List<ulong>>
            {
                { 0, [9, 9] },
                { 1, [5, 6] },
                { 2, [8, 4] },
                { 3, [9, 2] },
                { 4, [8, 2] },
            };

            Assert.That(actualValues, Has.Count.EqualTo(expectedValues.Count));
            Assert.Multiple(() =>
            {
                foreach (var (index, result) in actualValues)
                {
                    Assert.That(result, Is.EquivalentTo(expectedValues[index]));
                }
            });
        }

        [TestCase(2U, 97UL)]
        [TestCase(12U, 973461365314UL)]
        public void CalculateProducedJoltage_WhenBatteriesAreProvided_ShouldReturnCorrectJoltage(uint digits, ulong expected)
        {
            // Arrange
            var batteries = new List<Business.Year2025.Battery>
            {
                new('9'),
                new('5'),
                new('3'),
                new('7'),
                new('1'),
                new('3'),
                new('4'),
                new('6'),
                new('1'),
                new('3'),
                new('6'),
                new('5'),
                new('3'),
                new('1'),
                new('4'),
            };

            // Act
            var joltage = Business.Year2025.BatteryBank.CalculateProducedJoltage(batteries, digits);

            // Assert
            var expectedJoltage = expected;
            Assert.That(joltage, Is.EqualTo(expectedJoltage));
        }

        [Test]
        public void CalculateProducedJoltage_WhenNoBatteriesAreProvided_ShouldThrowArgumentException()
        {
            // Arrange
            var batteries = new List<Business.Year2025.Battery>();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Business.Year2025.BatteryBank.CalculateProducedJoltage(batteries, 2));
            Assert.That(ex.Message, Does.Contain("at least 2 numbers"));
        }

        [Test]
        public void GetLargestNDigitNumber_With2Digits_ShouldReturnCorrectLargestNumbers()
        {
            // Arrange
            var batteries = new List<ulong>
            {
                2, 4, 6, 8, 1, 3, 5, 7,
            };

            // Act
            var result = Business.Year2025.BatteryBank.GetLargestNDigitNumber(batteries, 2);

            // Assert
            Assert.That(result, Is.EquivalentTo(new List<uint> { 8, 7 }));
        }

        [Test]
        public void GetLargestNDigitNumber_With4Digits_ShouldReturnCorrectLargestNumbers()
        {
            // Arrange
            var batteries = new List<ulong>
            {
                2, 4, 6, 8, 1, 3, 5, 7,
            };

            // Act
            var result = Business.Year2025.BatteryBank.GetLargestNDigitNumber(batteries, 4);

            // Assert
            Assert.That(result, Is.EquivalentTo(new List<uint> { 8, 3, 5, 7 }));
        }

        [Test]
        public void GetLargestNDigitNumber_With8Digits_ShouldReturnCorrectLargestNumbers()
        {
            // Arrange
            var batteries = new List<ulong>
            {
                2, 4, 6, 8, 1, 3, 5, 7, 6, 8, 2, 4, 1, 7, 5, 3,
            };

            // Act
            var result = Business.Year2025.BatteryBank.GetLargestNDigitNumber(batteries, 8);

            // Assert
            Assert.That(result, Is.EquivalentTo(new List<uint> { 8, 8, 2, 4, 1, 7, 5, 3 }));
        }

        [Test]
        public void GetLargestNDigitNumber_With12Digits_ShouldReturnCorrectLargestNumbers()
        {
            // Arrange
            var batteries = new List<ulong>
            {
                2, 4, 6, 8, 1, 3, 5, 7, 6, 8, 2, 4, 1, 7, 5, 3,
            };

            // Act
            var result = Business.Year2025.BatteryBank.GetLargestNDigitNumber(batteries, 8);

            // Assert
            Assert.That(result, Is.EquivalentTo(new List<uint> { 8, 8, 2, 4, 1, 7, 5, 3 }));
        }
    }
}
