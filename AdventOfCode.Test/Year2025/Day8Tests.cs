using static AdventOfCode.Business.Year2025.Day8;

namespace AdventOfCode.Test.Year2025
{
    internal class Day8Tests
    {
        [Test]
        public void SolveCircuitMultiplication_ExampleInput_ReturnsExpectedResult()
        {
            // Arrange
            var input = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Example", "Day8_Input.txt"));

            // Act
            var result = SolveCircuitMultiplication(input, 10);

            // Assert
            Assert.That(result, Is.EqualTo(40));
        }

        [Test]
        public void SolveLastConnectionProduct_ExampleInput_ReturnsExpectedResult()
        {
            // Arrange
            var input = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Example", "Day8_Input.txt"));

            // Act
            var result = SolveLastConnectionProduct(input);

            // Assert
            Assert.That(result, Is.EqualTo(25272));
        }

        [Test]
        public void SolveLastConnectionProduct_ValidCoordsList_ReturnsExpectedResult()
        {
            // Arrange
            var lines = new string[]
            {
                "1,2,3",
                "4,5,6",
                "7,8,9",
                "8,9,10",
                "9,10,11",
                "11,12,13",
                "12,13,14",
                "13,14,15",
                "14,15,16",
                "15,16,17",
            };

            // Act
            var result = SolveLastConnectionProduct(lines);

            // Assert
            Assert.That(result, Is.EqualTo(28));
        }

        [Test]
        public void SolveLastConnectionProduct_EmptyPointsList_ReturnsZero()
        {
            // Act
            var result = SolveLastConnectionProduct([]);

            // Assert
            Assert.That(result, Is.Zero);
        }

        [Test]
        public void SolveLastConnectionProduct_NullPoints_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => SolveLastConnectionProduct(null!));
        }

        [Test]
        public void CalculateLastConnectionProduct_InsufficientPointsNumber_ReturnsExpectedResult()
        {
            // Arrange
            var points = new List<Point3D>
            {
                new(1,2,3),
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => CalculateLastConnectionProduct(points));
        }

        [Test]
        public void CalculateLastConnectionProduct_ValidPoints_ReturnsExpectedResult()
        {
            // Arrange
            var points = new List<Point3D>
            {
                new(1,2,3),
                new(4,5,6),
                new(7,8,9),
                new(8,9,10),
                new(9,10,11),
                new(11,12,13),
                new(12,13,14),
                new(13,14,15),
                new(14,15,16),
                new(15,16,17),
            };

            // Act
            var result = CalculateLastConnectionProduct(points);

            // Assert
            Assert.That(result, Is.EqualTo(28));
        }

        [Test]
        public void CalculateLastConnectionProduct_EmptyPointsList_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => CalculateLastConnectionProduct([]));
        }

        [Test]
        public void CalculateLastConnectionProduct_NullPoints_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => CalculateLastConnectionProduct(null!));
        }

        [Test]
        public void SolveCircuitMultiplication_ValidCoordsList_ReturnsExpectedResult()
        {
            // Arrange
            var lines = new string[]
            {
                "1,2,3",
                "4,5,6",
                "7,8,9",
                "8,9,10",
                "9,10,11",
                "11,12,13",
                "12,13,14",
                "13,14,15",
                "14,15,16",
                "15,16,17",
            };

            // Act
            var result = SolveCircuitMultiplication(lines, 5);

            // Assert
            Assert.That(result, Is.EqualTo(10));
        }

        [Test]
        public void SolveCircuitMultiplication_EmptyPointsList_ReturnsZero()
        {
            // Act
            var result = SolveCircuitMultiplication([], 10);

            // Assert
            Assert.That(result, Is.Zero);
        }

        [Test]
        public void SolveCircuitMultiplication_NullPoints_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => SolveCircuitMultiplication(null!, 10));
        }

        [Test]
        public void CalculateCircuitMultiplication_NotEnoughPoints_ThrowsInvalidOperationException()
        {
            // Arrange
            var points = new List<Point3D>
            {
                new(1,2,3),
                new(4,5,6),
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => CalculateCircuitMultiplication(points, 5));
        }

        [TestCase(5u, 10)]
        [TestCase(10u, 8)]
        public void CalculateCircuitMultiplication_ValidPointsWithVaryingConnectionAttempts_ReturnsExpectedResult(uint connectionAttempts, long expectedResult)
        {
            // Arrange
            var points = new List<Point3D>
            {
                new(1,2,3),
                new(4,5,6),
                new(7,8,9),
                new(8,9,10),
                new(9,10,11),
                new(11,12,13),
                new(12,13,14),
                new(13,14,15),
                new(14,15,16),
                new(15,16,17),
            };

            // Act
            var result = CalculateCircuitMultiplication(points, connectionAttempts);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void CalculateCircuitMultiplication_EmptyPointsList_ReturnsZero()
        {
            // Act
            var result = CalculateCircuitMultiplication([], 10);

            // Assert
            Assert.That(result, Is.Zero);
        }

        [Test]
        public void CalculateCircuitMultiplication_NullPoints_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => CalculateCircuitMultiplication(null!, 10));
        }

        [Test]
        public void GetSortedEdges_ValidPoints_ReturnsExpectedList()
        {
            // Arrange
            var expected = new List<Edge>
            {
                new (0, 1, 5.196d),
                new (1, 2, 5.196d),
                new (0, 2, 10.392d),
            };

            var input = new List<Point3D>
            {
                new(1,2,3),
                new(4,5,6),
                new(7,8,9),
            };

            // Act
            var result = GetSortedEdges(input);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(3));
                Assert.That(result[0].Distance, Is.EqualTo(expected[0].Distance).Within(0.01d));
                Assert.That(result[0].From, Is.EqualTo(expected[0].From));
                Assert.That(result[0].To, Is.EqualTo(expected[0].To));
                Assert.That(result[1].Distance, Is.EqualTo(expected[1].Distance).Within(0.01d));
                Assert.That(result[1].From, Is.EqualTo(expected[1].From));
                Assert.That(result[1].To, Is.EqualTo(expected[1].To));
                Assert.That(result[2].Distance, Is.EqualTo(expected[2].Distance).Within(0.01d));
                Assert.That(result[2].From, Is.EqualTo(expected[2].From));
                Assert.That(result[2].To, Is.EqualTo(expected[2].To));
            });
        }

        [Test]
        public void GetSortedEdges_ValidPoints_ReturnsExpectedDistanceSortedList()
        {
            // Arrange
            var expected = new List<Edge>
            {
                new (0, 2, 5.196d),
                new (1, 2, 5.196d),
                new (0, 1, 10.392d),
            };

            var input = new List<Point3D>
            {
                new(1,2,3),
                new(7,8,9),
                new(4,5,6),
            };

            // Act
            var result = GetSortedEdges(input);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(3));
                Assert.That(result[0].Distance, Is.EqualTo(expected[0].Distance).Within(0.01d));
                Assert.That(result[0].From, Is.EqualTo(expected[0].From));
                Assert.That(result[0].To, Is.EqualTo(expected[0].To));
                Assert.That(result[1].Distance, Is.EqualTo(expected[1].Distance).Within(0.01d));
                Assert.That(result[1].From, Is.EqualTo(expected[1].From));
                Assert.That(result[1].To, Is.EqualTo(expected[1].To));
                Assert.That(result[2].Distance, Is.EqualTo(expected[2].Distance).Within(0.01d));
                Assert.That(result[2].From, Is.EqualTo(expected[2].From));
                Assert.That(result[2].To, Is.EqualTo(expected[2].To));
            });
        }

        [Test]
        public void GetSortedEdges_EmptyPoints_ReturnsEmptyEdgeList()
        {
            // Act
            var result = GetSortedEdges([]);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetSortedEdges_NullPoints_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => GetSortedEdges(null!));
        }

        [Test]
        public void RetrievePoints_ValidCoords_ReturnsExpectedPoints()
        {
            // Arrange
            var input = new string[]
            {
                "1,2,3",
                "1,5,6"
            };

            // Act
            var result = RetrievePoints(input);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(2));
                Assert.That(result[0], Is.EqualTo(new Point3D(1, 2, 3)));
                Assert.That(result[1], Is.EqualTo(new Point3D(1, 5, 6)));
            });
        }

        [Test]
        public void RetrievePoints_InvalidCoords_ThrowsFormatException()
        {
            // Arrange
            var input = new string[]
            {
                "1,2,3",
                "1,Y,6"
            };

            // Act & Assert
            Assert.Throws<FormatException>(() => RetrievePoints(input));
        }

        [Test]
        public void RetrievePoints_NotEnoughCoords_ThrowsFormatException()
        {
            // Arrange
            var input = new string[]
            {
                "1,2,3",
                "1,5"
            };

            // Act & Assert
            Assert.Throws<FormatException>(() => RetrievePoints(input));
        }

        [Test]
        public void RetrievePoints_EmptyLinesInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => RetrievePoints([]));
        }

        [Test]
        public void RetrievePoints_NullLinesInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => RetrievePoints(null!));
        }

        [Test]
        public void TryGetPoint_WithThreeCoordinates_ReturnsTrueAndPointCorrect()
        {
            // Arrange
            var coords = new string[]
            {
                "1", "2", "3",
            };
            var expected = new Point3D(1, 2, 3);

            // Act
            var result = TryGetPoint(coords, out var actual);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(actual.X, Is.EqualTo(1));
                Assert.That(actual.Y, Is.EqualTo(2));
                Assert.That(actual.Z, Is.EqualTo(3));
            });
        }

        [Test]
        public void TryGetPoint_InvalidCoordinates_ReturnsFalse()
        {
            // Arrange
            var coords = new string[]
            {
                "1", "Y", "3",
            };
            var expected = new Point3D(1, 2, 3);

            // Act
            var result = TryGetPoint(coords, out var actual);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
            });
        }

        [Test]
        public void TryGetPoint_LessThanThreeCoordinates_ReturnsFalse()
        {
            // Arrange
            var coords = new string[]
            {
                "1", "2"
            };

            // Act
            var result = TryGetPoint(coords, out var _);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void TryGetPoint_EmptyCoordinates_ReturnsFalse()
        {
            // Act
            var result = TryGetPoint([], out var _);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void TryGetPoint_NullCoordinates_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => TryGetPoint(null!, out var point));
        }

        [Test]
        public void DistanceTo_ZeroInput_ReturnsZero()
        {
            // Arrange
            var pointA = new Point3D(0, 0, 0);
            var pointB = new Point3D(0, 0, 0);

            // Act
            var distance = pointA.DistanceTo(pointB);

            // Assert
            Assert.That(distance, Is.Zero);
        }

        [TestCase(1u, 1u, 1u, 1u, 1u, 1u, 0d)]
        [TestCase(1u, 2u, 3u, 4u, 5u, 6u, 5.7d)]
        [TestCase(5u, 6u, 7u, 8u, 9u, 10u, 5.1d)]
        public void DistanceTo_ValidInputs_ReturnsExpectedResult(uint aX, uint aY, uint aZ, uint bX, uint bY, uint bZ, double expectedDistance)
        {
            // Arrange
            var pointA = new Point3D(aX, aY, aZ);
            var pointB = new Point3D(bX, bY, bZ);

            // Act
            var distance = pointA.DistanceTo(pointB);

            // Assert
            Assert.That(distance, Is.EqualTo(expectedDistance).Within(0.51d));
        }
    }
}
