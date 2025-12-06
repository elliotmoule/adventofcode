using static AdventOfCode.Business.Year2025.Day6;

namespace AdventOfCode.Test.Year2025
{
    internal class Day6Tests
    {
        [Test]
        public void CalculateTotalOfAllAnswers_NullInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => CalculateTotalOfAllAnswers(null!));
        }

        [Test]
        public void CalculateTotalOfAllAnswers_InputHasMoreOperationsThanColumnValues_ThrowsFormatException()
        {
            // Arrange
            string[] input =
            [
                "123  456  789  123",
                "456  789  123  456",
                "789  123  456  789",
                "321  654  987  321",
                "-  +  -",
            ];

            // Act & Assert
            Assert.Throws<FormatException>(() => CalculateTotalOfAllAnswers(input));
        }

        [Test]
        public void CalculateTotalOfAllAnswers_InputHasMoreColumnValuesThanOperations_ThrowsFormatException()
        {
            // Arrange
            string[] input =
            [
                "123  456  789",
                "456  789  123",
                "789  123  456",
                "321  654  987",
                "-  +  -  +",
            ];

            // Act & Assert
            Assert.Throws<FormatException>(() => CalculateTotalOfAllAnswers(input));
        }

        [Test]
        public void CalculateTotalOfAllAnswers_ValidInput_ReturnsCorrectSum()
        {
            // Arrange
            Dictionary<uint, List<long>> expectedColumns = new()
            {
                { 0, [123, 456, 789, 123] },
                { 1, [456, 789, 123, 456] },
                { 2, [789, 123, 456, 789] },
                { 3, [321, 654, 987, 321] },
            };
            string[] input =
            [
                "123  456  789  123",
                "456  789  123  456",
                "789  123  456  789",
                "321  654  987  321",
                "-  +  -  +",
            ];

            //var column1 = -1443;
            //var column2 = 2022;
            //var column3 = -777;
            //var column4 = 1689;
            //var sum = 1491;

            // Act
            var actual = CalculateTotalOfAllAnswers(input);

            // Assert
            Assert.That(actual, Is.EqualTo(1491));
        }

        [Test]
        public void CalculateTotalOfAllAnswers_ValidExampleInput_ReturnsCorrectSum()
        {
            var day6Input = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Example", "Day6_Input.txt"));
            var result = CalculateTotalOfAllAnswers(day6Input);

            Assert.That(result, Is.EqualTo(4277556));
        }

        [Test]
        public void CalculateByOperation_OperationIsNotMathematicOperation_ThrowsArgumentException()
        {
            // Act & Arrange
            Assert.Throws<ArgumentNullException>(() => CalculateByOperation('a', 1, 2));
        }

        [Test]
        public void CalculateByOperation_Value1IsZero_ReturnsValue2()
        {
            // Act
            var result = CalculateByOperation('+', 0, 2);

            // Assert
            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void CalculateByOperation_Value2IsZero_ReturnsValue1()
        {
            // Act
            var result = CalculateByOperation('+', 1, 0);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void CalculateByOperation_OperationIsDivision_ReturnsValue1DividedByValue2()
        {
            // Act
            var result = CalculateByOperation('/', 2, 1);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void CalculateByOperation_OperationIsMultiplication_ReturnsValue1MultipliedByValue2()
        {
            // Act
            var result = CalculateByOperation('*', 2, 1);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void CalculateByOperation_OperationIsAddition_ReturnsValue1AddedToByValue2()
        {
            // Act
            var result = CalculateByOperation('+', 2, 1);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void CalculateByOperation_OperationIsSubtraction_ReturnsValue1IsSubtractedFromByValue2()
        {
            // Act
            var result = CalculateByOperation('-', 1, 2);

            // Assert
            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void SumOfColumnValues_ColumnIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => SumOfColumnValues('-', null!));
        }

        [Test]
        public void SumOfColumnValues_OperationIsInvalid_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => SumOfColumnValues('a', [1, 2, 3]));
        }

        [Test]
        public void SumOfColumnValues_ColumnIsEmpty_ReturnsZero()
        {
            // Act
            var result = SumOfColumnValues('+', []);

            // Assert
            Assert.That(result, Is.EqualTo(0));
        }

        [TestCase(new long[] { 1, 2, 2 }, 5)]
        [TestCase(new long[] { 200, 20, 1562 }, 1782)]
        [TestCase(new long[] { 50, 20, 20 }, 90)]
        [TestCase(new long[] { 1231410, 22123, 212412 }, 1465945)]
        public void SumOfColumnValues_IsValidInput_ReturnsCorrectSum(long[] columns, long sum)
        {
            // Act
            var result = SumOfColumnValues('+', [.. columns]);

            // Assert
            Assert.That(result, Is.EqualTo(sum));
        }

        [Test]
        public void RetrieveColumnValues_RowIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => RetrieveColumnValues(null!));
        }

        [TestCase("")]
        [TestCase("    ")]
        public void RetrieveColumnValues_RowIsEmptyOrWhiteSpace_ThrowsArgumentNullException(string input)
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => RetrieveColumnValues(input));
        }

        [Test]
        public void RetrieveColumnValues_OneColumn_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => RetrieveColumnValues("1234"));
        }

        [Test]
        public void RetrieveColumnValues_NonNumbericRow_ThrowsFormatException()
        {
            // Act & Assert
            Assert.Throws<FormatException>(() => RetrieveColumnValues("+   -   +   -"));
        }

        [Test]
        public void RetrieveColumnValues_RowIncludesColumnWithNonNumericValue_ThrowsFormatException()
        {
            // Act & Assert
            Assert.Throws<FormatException>(() => RetrieveColumnValues("123 456 abc 789"));
        }

        [Test]
        public void RetrieveColumnValues_TwoColumnsWorthOfInputInRow_ReturnsTwoListItems()
        {
            // Arrange
            var input = "421  400";

            // Act
            var actual = RetrieveColumnValues(input);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actual, Has.Count.EqualTo(2));
                Assert.That(actual[0], Is.EqualTo(421));
                Assert.That(actual[1], Is.EqualTo(400));
            });
        }

        [Test]
        public void RetrieveColumnValues_25ColumnsWorthOfInputInRow_Returns25ListItems()
        {
            // Arrange
            var input = "421  400  12  45  89  63  78  123  10  745  489  12  50  60  66  99  782  123  321  302  20  477  951  165  98";

            // Act
            var actual = RetrieveColumnValues(input);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actual, Has.Count.EqualTo(25));
                Assert.That(actual[0], Is.EqualTo(421));
                Assert.That(actual[1], Is.EqualTo(400));
                Assert.That(actual[2], Is.EqualTo(12));
                Assert.That(actual[3], Is.EqualTo(45));
                Assert.That(actual[4], Is.EqualTo(89));
                Assert.That(actual[5], Is.EqualTo(63));
                Assert.That(actual[6], Is.EqualTo(78));
                Assert.That(actual[7], Is.EqualTo(123));
                Assert.That(actual[8], Is.EqualTo(10));
                Assert.That(actual[9], Is.EqualTo(745));
                Assert.That(actual[10], Is.EqualTo(489));
                Assert.That(actual[11], Is.EqualTo(12));
                Assert.That(actual[12], Is.EqualTo(50));
                Assert.That(actual[13], Is.EqualTo(60));
                Assert.That(actual[14], Is.EqualTo(66));
                Assert.That(actual[15], Is.EqualTo(99));
                Assert.That(actual[16], Is.EqualTo(782));
                Assert.That(actual[17], Is.EqualTo(123));
                Assert.That(actual[18], Is.EqualTo(321));
                Assert.That(actual[19], Is.EqualTo(302));
                Assert.That(actual[20], Is.EqualTo(20));
                Assert.That(actual[21], Is.EqualTo(477));
                Assert.That(actual[22], Is.EqualTo(951));
                Assert.That(actual[23], Is.EqualTo(165));
                Assert.That(actual[24], Is.EqualTo(98));
            });
        }

        [Test]
        public void RetrieveColumns_InputIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => RetrieveColumns(0, null!));
        }

        [Test]
        public void RetrieveColumns_InputIsEmpty_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => RetrieveColumns(0, []));
        }

        [Test]
        public void RetrieveColumns_OperationsIsLessThanOne_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => RetrieveColumns(0, ["", "", ""]));
        }

        [Test]
        public void RetrieveColumns_ValidInputWithOperationsLine_ReturnsAllColumnsFromAllRowsExceptOperations()
        {
            // Arrange
            Dictionary<uint, List<long>> expectedColumns = new()
            {
                { 0, [123, 456, 789, 123] },
                { 1, [456, 789, 123, 456] },
                { 2, [789, 123, 456, 789] },
                { 3, [321, 654, 987, 321] },
            };
            string[] input =
            [
                "123  456  789  123",
                "456  789  123  456",
                "789  123  456  789",
                "321  654  987  321",
                "-  +  -  +",
            ];

            // Act
            var actual = RetrieveColumns(4, input);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actual, Has.Count.EqualTo(4));
                Assert.That(actual[0][0], Is.EqualTo(expectedColumns[0][0]));
                Assert.That(actual[2][2], Is.EqualTo(expectedColumns[2][2]));
                Assert.That(actual.Last().Value[0], Is.EqualTo(expectedColumns[3][321]));
            });
        }

        [Test]
        public void RetrieveColumnOperations_NullInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => RetrieveColumnOperations(null!));
        }

        [Test]
        public void RetrieveColumnOperations_EmptyInput_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => RetrieveColumnOperations([]));
        }

        [Test]
        public void RetrieveColumnOperations_ValidInputWithOperationsLine_ReturnsAllOperations()
        {
            // Arrange
            Dictionary<uint, List<long>> expectedColumns = new()
            {
                { 0, [123, 456, 789, 123] },
                { 1, [456, 789, 123, 456] },
                { 2, [789, 123, 456, 789] },
                { 3, [321, 654, 987, 321] },
            };
            string[] input =
            [
                "123  456  789  123",
                "456  789  123  456",
                "789  123  456  789",
                "321  654  987  321",
                "-  +  -  +",
            ];

            // Act
            var actual = RetrieveColumnOperations(input);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actual, Has.Count.EqualTo(4));
                Assert.That(actual[0], Is.EqualTo('-'));
                Assert.That(actual[1], Is.EqualTo('+'));
                Assert.That(actual[2], Is.EqualTo('-'));
                Assert.That(actual[3], Is.EqualTo('+'));
            });
        }
    }
}
