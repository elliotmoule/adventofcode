using static AdventOfCode.Business.Year2025.Day9;

namespace AdventOfCode.Test.Year2025
{
    internal class Day9Tests
    {
        [Test]
        public void CalculateLargestRectangle_ExampleInput_ReturnsExpectedResult()
        {
            // Arrange
            var input = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Example", "Day9_Input.txt"));

            // Act
            var result = CalculateLargestRectangle(input);

            // Assert
            Assert.That(result, Is.EqualTo(50));
        }

        [Test]
        public void CalculateLargestRectangleAreaWithinPolyon_ExampleInput_ReturnsExpectedResult()
        {
            // Arrange
            var input = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Example", "Day9_Input.txt"));

            // Act
            var result = CalculateLargestRectangleAreaWithinPolyon(input);

            // Assert
            Assert.That(result, Is.EqualTo(24));
        }

        [Test]
        public void FindLargestContainedRectangle_ValidInput_ReturnsExpectedResult()
        {
            // Arrange
            var polygon = new Polygon([new(1, 1), new(5, 1), new(5, 5), new(1, 5)]);
            var largestRectangleInput = new Rectangle(new(1, 1), 5, 5);
            var rectangles = new HashSet<Rectangle>
            {
                largestRectangleInput,
                new(new(2,1), 1,5),
                new(new(3,2), 4,2),
                new(new(4,4), 5,5),
                new(new(2,3), 1,5),
            };

            // Act
            var result = FindLargestContainedRectangle(polygon, rectangles, out var largest);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(largest, Is.EqualTo(largestRectangleInput));
            });
        }

        [Test]
        public void FindLargestContainedRectangle_EmptyPolygon_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => FindLargestContainedRectangle(new Polygon([]), [new(new(3, 4), 1, 2)], out var _));
        }

        [Test]
        public void FindLargestContainedRectangle_EmptyRectangles_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => FindLargestContainedRectangle(new Polygon([new(1, 2), new(2, 3)]), [], out var _));
        }

        [Test]
        public void FindLargestContainedRectangle_NullRectangles_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => FindLargestContainedRectangle(new Polygon([new(1, 2), new(3, 4), new(5, 6)]), null!, out var _));
        }

        [Test]
        public void FindLargestContainedRectangle_NullPolygon_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => FindLargestContainedRectangle(null!, [], out var _));
        }

        [TestCase(true, 1ul, 2ul, 3ul, 4ul, "#.#X\r\n....\r\n#.#.\r\nX..X\r\n")]
        [TestCase(false, 2ul, 2ul, 6ul, 4ul, "X#.X.#\r\n......\r\n.#...#\r\nX..X..\r\n")]
        public void ContainsRectangle_ValidInput_ReturnsExpectedResult(bool expected, ulong x, ulong y, ulong width, ulong height, string gridVisual)
        {
            // Arrange
            var vertices = new Vertex[]
            {
                new(1,1),
                new(4,1),
                new(4,4),
                new(1,4),
            };
            var polygon = new Polygon(vertices);
            var grid = polygon.Visualise([new(x, y), new(x, height), new(width, height), new(width, y)]);

            // Act
            var result = polygon.ContainsRectangle(new(new(x, y), width, height));

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(polygon, Is.Not.Null);
                Assert.That(polygon.Vertices, Is.Not.Null);
                Assert.That(polygon.Vertices, Has.Length.EqualTo(4));
                Assert.That(result, Is.EqualTo(expected));
                Assert.That(grid, Is.EqualTo(gridVisual));
            });
        }

        [Test]
        public void ContainsRectangle_NullInput_ThrowsArgumentNullException()
        {
            // Arrange
            var polygon = new Polygon([new(1, 2), new(2, 3), new(4, 5), new(6, 7)]);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => polygon.ContainsRectangle(null!));
        }

        [TestCase(1ul, 2ul, true)]
        [TestCase(0ul, 2ul, false)]
        [TestCase(3ul, 6ul, true)]
        [TestCase(5ul, 10ul, false)]
        [TestCase(9ul, 5ul, true)]
        public void ContainsVertex_ValidInput_ReturnsExpectedResult(ulong x, ulong y, bool expected)
        {
            // Arrange
            var vertices = new Vertex[]
            {
                new(1,1),
                new(1,9),
                new(9,1),
                new(9,9),
            };
            var polygon = new Polygon(vertices);

            // Act
            var result = polygon.ContainsVertex(new(x, y));

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(polygon, Is.Not.Null);
                Assert.That(polygon.Vertices, Is.Not.Null);
                Assert.That(polygon.Vertices, Has.Length.EqualTo(4));
                Assert.That(result, Is.EqualTo(expected));
            });
        }

        [Test]
        public void CalculateLargestRectangle_ValidInput_ReturnsExpectedResult()
        {
            // Arrange
            var input = new string[]
            {
                "1,2",
                "3,4",
                "3,2",
                "1,5",
                "5,4",
            };

            // Act
            var result = CalculateLargestRectangle(input);

            // Assert
            Assert.That(result, Is.EqualTo(15));
        }

        [Test]
        public void CalculateLargestRectangle_SingleCoordInput_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<Exception>(() => CalculateLargestRectangle(["1,3"]));
        }

        [Test]
        public void CalculateLargestRectangle_EmptyInput_ReturnsZero()
        {
            // Act
            var result = CalculateLargestRectangle([]);

            // Assert
            Assert.That(result, Is.Zero);
        }

        [Test]
        public void CalculateLargestRectangle_NullInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => CalculateLargestRectangle(null!));
        }

        [Test]
        public void CreateRectangles_ValidInput_ReturnsExpectedRectangles()
        {
            // Arrange
            var expected = new HashSet<Rectangle>
            {
                new (new(1,2), 3,4),
                new (new(1,2), 7,3),
                new (new(1,1), 9,2),
                new (new(3,3), 7,4),
                new (new(3,1), 9,4),
                new (new(7,1), 9,3),
            };
            var input = new Vertex[]
            {
                new(1,2),
                new(3,4),
                new(7,3),
                new(9,1),
            };

            // Act
            var result = CreateRectangles(input);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(expected.Count));
                Assert.That(result.FirstOrDefault(), Is.EqualTo(expected.FirstOrDefault()));
                Assert.That(result, Is.EquivalentTo(expected));
            });
        }

        [Test]
        public void CreateRectangles_EmptyInput_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => CreateRectangles([]));
        }

        [Test]
        public void CreateRectangles_NullInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => CreateRectangles(null!));
        }

        [Test]
        public void BuildVerticesFromCoords_ValidCoords_ReturnsCorrectVertices()
        {
            // Arrange
            var expected = new Vertex[]
            {
                new(1,2),
                new(4,8),
                new(6,8),
            };
            var input = new string[]
            {
                "1,2",
                "4,8",
                "6,8",
            };

            // Act
            var result = BuildVerticesFromCoords(input);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Length.EqualTo(3));
                Assert.That(result, Is.EquivalentTo(expected));
            });
        }

        [Test]
        public void BuildVerticesFromCoords_InputCoordsCannotBeParsed_ThrowsInvalidOperationException()
        {
            // Arrange
            var input = new string[]
            {
                "1,2",
                "4,Z",
                "6,8",
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => BuildVerticesFromCoords(input));
        }

        [Test]
        public void BuildVerticesFromCoords_NullInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => BuildVerticesFromCoords(null!));
        }

        [Test]
        public void TryParseCoord_ValidInput_ReturnsExpectedResult()
        {
            // Arrange
            var input = "12,34";

            // Act
            var result = TryParseCoord(input, out var vertex);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(vertex, Is.EqualTo(new Vertex(12, 34)));
            });
        }

        [Test]
        public void TryParseCoord_CoordCannotBeParsed_ThrowsInvalidOperationException()
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => TryParseCoord("12,AB", out var _));
        }

        [Test]
        public void TryParseCoord_CoordIsIncomplete_ThrowsFormatException()
        {
            // Act & Assert
            Assert.Throws<FormatException>(() => TryParseCoord("1234", out var _));
        }

        [TestCase("")]
        [TestCase("    ")]
        public void TryParseCoord_CoordIsEmpty_ThrowsArgumentException(string input)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => TryParseCoord(input, out var _));
        }

        [Test]
        public void TryParseCoord_CoordIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => TryParseCoord(null!, out var _));
        }

        [Test]
        public void RectangleEquals_TwoUnequalRectangles_ReturnsFalse()
        {
            // Arrange
            var rect1 = new Rectangle(new(1, 1), 2, 3);
            var rect2 = new Rectangle(new(1, 2), 2, 3);

            // Act
            var result = rect1.Equals(rect2);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void RectangleEquals_TwoEqualRectangles_ReturnsTrue()
        {
            // Arrange
            var rect1 = new Rectangle(new(1, 1), 2, 3);
            var rect2 = new Rectangle(new(1, 1), 2, 3);

            // Act
            var result = rect1.Equals(rect2);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void RectangleAreaCalculation_ValidInput_ReturnsExpectedResult()
        {
            // Arrange
            var vertex = new Vertex(1, 1);
            ulong height = 5;
            ulong width = 5;

            // Act
            var rect = new Rectangle(vertex, width, height);

            // Assert
            Assert.That(rect.Area, Is.EqualTo(25));
        }

        [Test]
        public void RectangleAreaCalculation_ValidZeroInput_ReturnsExpectedResult()
        {
            // Arrange
            var vertex = new Vertex(0, 0);
            ulong height = 0;
            ulong width = 0;

            // Act
            var rect = new Rectangle(vertex, width, height);

            // Assert
            Assert.That(rect.Area, Is.EqualTo(0));
        }
    }
}
