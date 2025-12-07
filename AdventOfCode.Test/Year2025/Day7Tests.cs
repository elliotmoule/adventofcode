using static AdventOfCode.Business.Year2025.Day7;

namespace AdventOfCode.Test.Year2025
{
    internal class Day7Tests
    {
        [Test]
        public void CountPaths_WithSimpleGrid_Counts4Paths()
        {
            // Arrange
            var simplePathGrid = new[]
            {
                ".......S.......",
                ".......|.......",
                "......|^|......",
                "...............",
                "......^.^......",
                "..............."
            };

            // Act
            var pathCount = CountPaths(simplePathGrid);

            // Assert
            Assert.That(pathCount, Is.EqualTo(4));
        }

        [Test]
        public void CountPaths_WithExampleGrid_Counts40Paths()
        {
            // Arrange
            var input = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Example", "Day7_Input.txt"));

            // Act
            var pathCount = CountPaths(input);

            // Assert
            Assert.That(pathCount, Is.EqualTo(40));
        }

        [Test]
        public void CountCaretCharacters_NullInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => CountSplits(null!));
        }

        [Test]
        public void CountCaretCharacters_EmptyInput_ReturnsZero()
        {
            // Act
            var result = CountSplits([]);

            // Assert
            Assert.That(result, Is.Zero);
        }

        readonly string[] _simpleGrid =
        [
            "...S...",
            ".......",
            "...^...",
            "......."
        ];

        [Test]
        public void FindStartPosition_WithValidGrid_ReturnsCorrectPosition()
        {
            // Act
            var result = FindStartPosition(_simpleGrid);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result!.Value.Row, Is.EqualTo(0));
                Assert.That(result.Value.Col, Is.EqualTo(3));
            });
        }

        [Test]
        public void FindStartPosition_WithExampleGrid_ReturnsCorrectPosition()
        {
            // Arrange
            var input = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Example", "Day7_Input.txt"));

            // Act
            var result = FindStartPosition(input);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result!.Value.Row, Is.EqualTo(0));
                Assert.That(result.Value.Col, Is.EqualTo(7));
            });
        }

        [Test]
        public void FindStartPosition_WithNoStart_ReturnsNull()
        {
            // Arrange
            var gridWithoutStart = new[]
            {
                ".......",
                "...^...",
                "......."
            };

            // Act
            var result = FindStartPosition(gridWithoutStart);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void IsInBounds_WithValidCoordinates_ReturnsTrue()
        {
            // Act
            var result = IsInBounds(_simpleGrid, 1, 3);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsInBounds_WithNegativeRow_ReturnsFalse()
        {
            // Act
            var result = IsInBounds(_simpleGrid, -1, 3);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsInBounds_WithNegativeColumn_ReturnsFalse()
        {
            // Act
            var result = IsInBounds(_simpleGrid, 1, -1);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsInBounds_WithRowTooLarge_ReturnsFalse()
        {
            // Act
            var result = IsInBounds(_simpleGrid, 10, 3);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsInBounds_WithColumnTooLarge_ReturnsFalse()
        {
            // Act
            var result = IsInBounds(_simpleGrid, 1, 20);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void CreateSplitBeams_CreatesLeftAndRightBeams()
        {
            // Act
            var beams = CreateSplitBeams(5, 10);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(beams, Has.Count.EqualTo(2));

                Assert.That(beams[0].Row, Is.EqualTo(5));
                Assert.That(beams[0].Col, Is.EqualTo(10));
                Assert.That(beams[0].DirRow, Is.EqualTo(1));
                Assert.That(beams[0].DirCol, Is.EqualTo(-1));

                Assert.That(beams[1].Row, Is.EqualTo(5));
                Assert.That(beams[1].Col, Is.EqualTo(10));
                Assert.That(beams[1].DirRow, Is.EqualTo(1));
                Assert.That(beams[1].DirCol, Is.EqualTo(1));
            });
        }

        [Test]
        public void ProcessBeam_WithEmptySpace_ContinuesInSameDirection()
        {
            // Arrange
            var beam = new Beam(0, 3, 1, 0);
            var visited = new HashSet<(int, int, int, int)>();

            // Act
            var newBeams = ProcessBeam(_simpleGrid, beam, visited, out bool didSplit);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(didSplit, Is.False);
                Assert.That(newBeams, Has.Count.EqualTo(1));
                Assert.That(newBeams[0].Row, Is.EqualTo(1));
                Assert.That(newBeams[0].Col, Is.EqualTo(3));
                Assert.That(newBeams[0].DirRow, Is.EqualTo(1));
                Assert.That(newBeams[0].DirCol, Is.EqualTo(0));
            });
        }

        [Test]
        public void ProcessBeam_WithSplitter_CreatesTwoBeams()
        {
            // Arrange
            var beam = new Beam(1, 3, 1, 0); // Moving down toward splitter
            var visited = new HashSet<(int, int, int, int)>();

            // Act
            var newBeams = ProcessBeam(_simpleGrid, beam, visited, out bool didSplit);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(didSplit, Is.True);
                Assert.That(newBeams, Has.Count.EqualTo(2));
            });
        }

        [Test]
        public void ProcessBeam_OutOfBounds_ReturnsNoBeams()
        {
            // Arrange
            var beam = new Beam(3, 3, 1, 0); // At bottom, moving down
            var visited = new HashSet<(int, int, int, int)>();

            // Act
            var newBeams = ProcessBeam(_simpleGrid, beam, visited, out bool didSplit);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(didSplit, Is.False);
                Assert.That(newBeams, Is.Empty);
            });
        }

        [Test]
        public void ProcessBeam_AlreadyVisited_ReturnsNoBeams()
        {
            // Arrange
            var beam = new Beam(0, 3, 1, 0);
            var visited = new HashSet<(int, int, int, int)> { (1, 3, 1, 0) };

            // Act
            var newBeams = ProcessBeam(_simpleGrid, beam, visited, out bool didSplit);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(didSplit, Is.False);
                Assert.That(newBeams, Is.Empty);
            });
        }

        [Test]
        public void SimulateBeams_WithSimpleGrid_CountsOneSplit()
        {
            // Arrange & Act
            var splitCount = SimulateBeams(_simpleGrid, 0, 3);

            // Assert
            Assert.That(splitCount, Is.EqualTo(1));
        }

        [Test]
        public void SimulateBeams_WithExampleGrid_Counts21Splits()
        {
            // Arrange
            var input = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Example", "Day7_Input.txt"));

            // Act
            var splitCount = SimulateBeams(input, 0, 7);

            // Assert
            Assert.That(splitCount, Is.EqualTo(21));
        }

        [Test]
        public void CountSplits_WithSimpleGrid_CountsOneSplit()
        {
            // Arrange & Act
            var splitCount = CountSplits(_simpleGrid);

            // Assert
            Assert.That(splitCount, Is.EqualTo(1));
        }

        [Test]
        public void CountSplits_WithExampleGrid_Counts21Splits()
        {
            // Arrange
            var input = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Example", "Day7_Input.txt"));

            // Act
            var splitCount = CountSplits(input);

            // Assert
            Assert.That(splitCount, Is.EqualTo(21));
        }

        [Test]
        public void CountSplits_WithNoStartPosition_ThrowsException()
        {
            // Arrange
            var gridWithoutStart = new[]
            {
                ".......",
                "...^...",
                "......."
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => CountSplits(gridWithoutStart));
        }

        [Test]
        public void CountPathsWithPositionCaching_NullCacheInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => CountPathsWithPositionCaching([], 0, 0, 1, 0, null!));
        }

        [Test]
        public void CountPathsWithPositionCaching_NullGridInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => CountPathsWithPositionCaching(null!, 0, 0, 1, 0, []));
        }

        [Test]
        public void CountPathsWithPositionCaching_EmptyGridInput_ReturnsZero()
        {
            // Act
            var result = CountPathsWithPositionCaching([], 0, 0, 1, 0, []);

            // Assert
            Assert.That(result, Is.Zero);
        }

        [Test]
        public void CountPathsWithPositionCaching_ValidGridInput_ReturnsExpectedResult()
        {
            // Arrange
            var simplePathGrid = new[]
            {
                ".......S.......",
                ".......|.......",
                "......|^|......",
                "...............",
                "......^.^......",
                "...............",
                ".........^.....",
                "...............",
                ".....^.........",
                "...............",
            };

            // Act
            var pathCount = CountPathsWithPositionCaching(simplePathGrid, 0, 7, 1, 0, []);

            // Assert
            Assert.That(pathCount, Is.EqualTo(6));
        }
    }
}
