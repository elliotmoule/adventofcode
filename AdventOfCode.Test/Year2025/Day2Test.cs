using AdventOfCode.Business.Year2025;

namespace AdventOfCode.Test.Year2025
{
    public class Day2Test
    {
        [Test]
        public void SumOfBasicRepeatedSequenceProductsInRanges_WithMultipleRangesFromString_ShouldReturnCorrectSum()
        {
            // Arrange
            var productRanges = "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124";

            var ranges = Day2.CommaDelimitedList(productRanges)
                .Select(Day2.ParseProductRange);

            // Act
            var result = Day2.SumOfBasicRepeatedSequenceProductsInRanges(ranges);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo((11UL + 22UL) + (99UL) + (1010UL) + (1188511885UL) + (222222UL) + (0) + (446446UL) + (38593859UL) + (0) + (0) + (0)));
                Assert.That(result, Is.EqualTo(1227775554UL));
            });
        }

        [Test]
        public void SumOfBasicRepeatedSequenceProductsInRanges_WhenRangeIsVeryLarge_ShouldReturnCorrectSum()
        {
            // Arrange
            var ranges = new List<ProductRange>
            {
                new(1188511880UL, 1188511890UL)
            };

            // Act
            var result = Day2.SumOfBasicRepeatedSequenceProductsInRanges(ranges);

            // Assert
            Assert.That(result, Is.EqualTo(1188511885));
        }

        [Test]
        public void SumOfBasicRepeatedSequenceProductsInRanges_WhenRangeHasNoRepeatedSequenceProducts_ShouldReturnZero()
        {
            // Arrange
            var ranges = new List<ProductRange>
            {
                new(123UL, 130UL)
            };

            // Act
            var result = Day2.SumOfBasicRepeatedSequenceProductsInRanges(ranges);

            // Assert
            Assert.That(result, Is.EqualTo(0UL));
        }

        [Test]
        public void SumOfBasicRepeatedSequenceProductsInRanges_WhenSingleRangeProvided_ShouldReturnCorrectSum()
        {
            // Arrange
            var ranges = new List<ProductRange>
            {
                new(10UL, 30UL)
            };
            // Act
            var result = Day2.SumOfBasicRepeatedSequenceProductsInRanges(ranges);
            // Assert
            Assert.That(result, Is.EqualTo(11UL + 22UL));
        }

        [Test]
        public void SumOfBasicRepeatedSequenceProductsInRanges_WhenNoRangesProvided_ShouldReturnZero()
        {
            // Arrange
            var ranges = new List<ProductRange>();

            // Act
            var result = Day2.SumOfBasicRepeatedSequenceProductsInRanges(ranges);

            // Assert
            Assert.That(result, Is.EqualTo(0UL));
        }

        [Test]
        public void SumOfBasicRepeatedSequenceProductsInRanges_WhenRangesIsNull_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Day2.SumOfBasicRepeatedSequenceProductsInRanges(null!));
        }

        [Test]
        public void SumOfBasicRepeatedSequenceProductsInRanges_WhenMultipleRangesProvided_ShouldReturnCorrectSum()
        {
            // Arrange
            var ranges = new List<ProductRange>
            {
                new(10UL, 30UL),
                new(50UL, 70UL)
            };

            // Act
            var result = Day2.SumOfBasicRepeatedSequenceProductsInRanges(ranges);

            // Assert
            Assert.That(result, Is.EqualTo((11UL + 22UL) + (55UL + 66UL)));
        }

        [Test]
        public void SumOfBasicRepeatedSequenceProductsInRange_WhenRangeIsNull_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Day2.SumOfBasicRepeatedSequenceProductsInRange(null!));
        }

        [Test]
        public void SumOfBasicRepeatedSequenceProductsInRange__WhenRangeStartIsGreaterThanEnd_ShouldThrowArgumentException()
        {
            // Arrange
            var range = new ProductRange(30UL, 10UL);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Day2.SumOfBasicRepeatedSequenceProductsInRange(range));
        }

        [Test]
        public void SumOfBasicRepeatedSequenceProductsInRange_WhenRangeHasBasicRepeatedSequenceProducts_ShouldReturnCorrectSum()
        {
            // Arrange
            var range = new ProductRange(10UL, 30UL);

            // Act
            var result = Day2.SumOfBasicRepeatedSequenceProductsInRange(range);

            // Assert
            Assert.That(result, Is.EqualTo(11UL + 22UL));
        }

        [Test]
        public void SumOfBasicRepeatedSequenceProductsInRange_WhenRangeHasNoRepeatedSequenceProducts_ShouldReturnZero()
        {
            // Arrange
            var range = new ProductRange(123UL, 130UL);
            // Act
            var result = Day2.SumOfBasicRepeatedSequenceProductsInRange(range);
            // Assert
            Assert.That(result, Is.EqualTo(0UL));
        }

        [Test]
        public void SumOfBasicRepeatedSequenceProductsInRange_WhenRangeIsSingleNumberWithRepeatedSequence_ShouldReturnThatNumber()
        {
            // Arrange
            var range = new ProductRange(99UL, 99UL);

            // Act
            var result = Day2.SumOfBasicRepeatedSequenceProductsInRange(range);

            // Assert
            Assert.That(result, Is.EqualTo(99UL));
        }

        [TestCase(99UL)]
        [TestCase(11UL)]
        [TestCase(222222UL)]
        [TestCase(1010UL)]
        public void HasBasicRepeatedSequence_WhenNumberHasBasicRepeatedSequence_ShouldReturnTrue(ulong number)
        {
            // Act
            var result = Day2.HasBasicRepeatedSequence(number);

            // Assert
            Assert.That(result, Is.True);
        }

        [TestCase(112345UL)]
        [TestCase(123456UL)]
        [TestCase(123124UL)]
        public void HasBasicRepeatedSequence_WhenNumberHasNoRepeatedSequence_ShouldReturnFalse(ulong number)
        {
            // Act
            var result = Day2.HasBasicRepeatedSequence(number);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsOddProductNumberLength_WhenNumberIsZero_ShouldReturnFalse()
        {
            // Arrange
            ulong number = 0; // 1 digit, odd length

            // Act
            var result = Day2.IsOddProductNumberLength(number);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsOddProductNumberLength_WhenNumberHasEvenLength_ShouldReturnFalse()
        {
            // Arrange
            ulong number = 1234; // 4 digits, even length
            // Act
            var result = Day2.IsOddProductNumberLength(number);
            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsOddProductNumberLength_WhenNumberHasOddLength_ShouldReturnTrue()
        {
            // Arrange
            ulong number = 12345; // 5 digits, odd length

            // Act
            var result = Day2.IsOddProductNumberLength(number);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void ParseProductRange_WhenInputIsTooShort_ShouldThrowFormatException()
        {
            // Arrange
            var input = "1-";
            // Act & Assert
            Assert.Throws<FormatException>(() => Day2.ParseProductRange(input));
        }

        [TestCase("1188511880-1188511890", 1188511880UL, 1188511890UL)]
        [TestCase("2121212118-2121212124", 2121212118UL, 2121212124UL)]
        public void ParseProductRange_WhenInputIsValidButVeryLong_ShouldReturnCorrectProductRange(string input, ulong expectedStart, ulong expectedEnd)
        {
            // Act
            var result = Day2.ParseProductRange(input);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Start, Is.EqualTo(expectedStart));
                Assert.That(result.End, Is.EqualTo(expectedEnd));
            });
        }

        [TestCase("1-2", 1UL, 2UL)]
        [TestCase("11-22", 11UL, 22UL)]
        public void ParseProductRange_WhenInputIsValid_ShouldReturnCorrectProductRange(string input, ulong expectedStart, ulong expectedEnd)
        {
            // Act
            var result = Day2.ParseProductRange(input);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Start, Is.EqualTo(expectedStart));
                Assert.That(result.End, Is.EqualTo(expectedEnd));
            });
        }

        [Test]
        public void ParseProductRange_WhenInputEndsWithDash_ShouldThrowFormatException()
        {
            // Arrange
            var input = "1122-";

            // Act & Assert
            Assert.Throws<FormatException>(() => Day2.ParseProductRange(input));
        }

        [Test]
        public void ParseProductRange_WhenInputStartsWithDash_ShouldThrowFormatException()
        {
            // Arrange
            var input = "-11-22";

            // Act & Assert
            Assert.Throws<FormatException>(() => Day2.ParseProductRange(input));
        }

        [Test]
        public void ParseProductRange_WhenInputContainsTooManyDashes_ShouldThrowFormatException()
        {
            // Arrange
            var input = "11-22-33";

            // Act & Assert
            Assert.Throws<FormatException>(() => Day2.ParseProductRange(input));
        }

        [Test]
        public void CommaDelimitedList_WhenInputIsMalformed_ShouldThrowFormatException()
        {
            // Arrange
            var input = "11-22;95-115;998-1012";

            // Act & Assert
            Assert.Throws<FormatException>(() => Day2.CommaDelimitedList(input).ToList());
        }

        [Test]
        public void CommaDelimitedList_WhenInputIsEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var input = "";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Day2.CommaDelimitedList(input));
        }

        [Test]
        public void CommaDelimitedList_WhenInputIsValid_ShouldReturnCorrectList()
        {
            // Arrange
            var input = "11-22,95-115,998-1012";

            // Act
            var result = Day2.CommaDelimitedList(input).ToList();

            // Assert
            var expected = new List<string> { "11-22", "95-115", "998-1012" };
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
