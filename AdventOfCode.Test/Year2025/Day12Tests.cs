using static AdventOfCode.Business.Year2025.Day12;

namespace AdventOfCode.Test.Year2025
{
    internal class Day12Tests
    {
        [Test]
        public void CalculateHowManyPresentsCanFitAllOfThePresents_ExampleInput_ReturnsExpectedResult()
        {
            // Arrange
            var input = File.ReadAllText(Path.Combine("Year2025", "Resources", "Example", "Day12_Input.txt"));

            // Act
            var result = CalculateHowManyPresentsCanFitAllOfThePresents(input);

            // Assert
            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void CanFitPresents_WithNoPresents_ReturnsTrue()
        {
            // Arrange
            var region = new Region(5, 5, [0, 0]);
            var shapes = new List<Shape>
            {
                new(1, [(0, 0), (1, 0)]),
                new(2, [(0, 0), (0, 1)])
            };

            // Act
            var result = CanFitPresents(region, shapes);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void CanFitPresents_WithSingleShapeThatFits_ReturnsTrue()
        {
            // Arrange
            var region = new Region(5, 5, [1]);
            var shapes = new List<Shape>
            {
                new(1, [(0, 0), (1, 0)])
            };

            // Act
            var result = CanFitPresents(region, shapes);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void CanFitPresents_WithShapeTooLarge_ReturnsFalse()
        {
            // Arrange
            var region = new Region(3, 3, [1]);
            var shapes = new List<Shape>
            {
                new(1, [(0, 0), (1, 0), (2, 0), (3, 0)])
            };

            // Act
            var result = CanFitPresents(region, shapes);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void CanFitPresents_WithMultipleShapesThatFit_ReturnsTrue()
        {
            // Arrange
            var region = new Region(5, 5, [1, 1]);
            var shapes = new List<Shape>
            {
                new(1, [(0, 0), (1, 0)]),
                new(2, [(0, 0), (0, 1)])
            };

            // Act
            var result = CanFitPresents(region, shapes);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void CanFitPresents_WithMultipleCopiesOfSameShape_ReturnsTrue()
        {
            // Arrange
            var region = new Region(6, 6, [3]);
            var shapes = new List<Shape>
            {
                new(1, [(0, 0), (1, 0)])
            };

            // Act
            var result = CanFitPresents(region, shapes);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void CanFitPresents_WithTooManyShapes_ReturnsFalse()
        {
            // Arrange
            var region = new Region(2, 2, [5]);
            var shapes = new List<Shape>
            {
                new(1, [(0, 0)])
            };

            // Act
            var result = CanFitPresents(region, shapes);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void CanFitPresents_WithShapeThatFitsInOneOrientation_ReturnsTrue()
        {
            // Arrange
            var region = new Region(3, 5, [1]);
            var shapes = new List<Shape>
            {
                new(1, [(0, 0), (0, 1), (0, 2), (0, 3)])
            };

            // Act
            var result = CanFitPresents(region, shapes);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryPlacePresents_WithEmptyList_ReturnsTrue()
        {
            // Arrange
            var presents = new List<(Shape shape, List<List<(int x, int y)>> orientations)>();
            var grid = new bool[5, 5];

            // Act
            var result = TryPlacePresents(presents, 0, grid, 5, 5);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryPlacePresents_WithSingleShapeThatFits_ReturnsTrue()
        {
            // Arrange
            var cells = new List<(int x, int y)> { (0, 0), (1, 0) };
            var orientations = new List<List<(int x, int y)>> { cells };
            var shape = new Shape(1, cells);
            var presents = new List<(Shape, List<List<(int x, int y)>>)> { (shape, orientations) };
            var grid = new bool[5, 5];

            // Act
            var result = TryPlacePresents(presents, 0, grid, 5, 5);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryPlacePresents_WithShapeTooLargeForGrid_ReturnsFalse()
        {
            // Arrange
            var cells = new List<(int x, int y)> { (0, 0), (1, 0), (2, 0), (3, 0) };
            var orientations = new List<List<(int x, int y)>> { cells };
            var shape = new Shape(1, cells);
            var presents = new List<(Shape, List<List<(int x, int y)>>)> { (shape, orientations) };
            var grid = new bool[3, 3];

            // Act
            var result = TryPlacePresents(presents, 0, grid, 3, 3);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void TryPlacePresents_WithMultipleShapesThatFit_ReturnsTrue()
        {
            // Arrange
            var cells1 = new List<(int x, int y)> { (0, 0), (1, 0) };
            var cells2 = new List<(int x, int y)> { (0, 0), (0, 1) };
            var orientations1 = new List<List<(int x, int y)>> { cells1 };
            var orientations2 = new List<List<(int x, int y)>> { cells2 };
            var shape1 = new Shape(1, cells1);
            var shape2 = new Shape(2, cells2);
            var presents = new List<(Shape, List<List<(int x, int y)>>)>
            {
                (shape1, orientations1),
                (shape2, orientations2)
            };
            var grid = new bool[5, 5];

            // Act
            var result = TryPlacePresents(presents, 0, grid, 5, 5);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryPlacePresents_WithPartiallyFilledGrid_SkipsOccupiedCells()
        {
            // Arrange
            var cells = new List<(int x, int y)> { (0, 0), (1, 0) };
            var orientations = new List<List<(int x, int y)>> { cells };
            var shape = new Shape(1, cells);
            var presents = new List<(Shape, List<List<(int x, int y)>>)> { (shape, orientations) };
            var grid = new bool[5, 5];
            grid[0, 0] = true;
            grid[1, 0] = true;

            // Act
            var result = TryPlacePresents(presents, 0, grid, 5, 5);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryPlacePresents_WithNoValidPlacement_ReturnsFalse()
        {
            // Arrange
            var cells = new List<(int x, int y)> { (0, 0), (1, 0), (2, 0) };
            var orientations = new List<List<(int x, int y)>> { cells };
            var shape = new Shape(1, cells);
            var presents = new List<(Shape, List<List<(int x, int y)>>)> { (shape, orientations) };
            var grid = new bool[2, 2];

            // Act
            var result = TryPlacePresents(presents, 0, grid, 2, 2);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void TryPlacePresents_WithNonZeroIndex_StartsFromGivenIndex()
        {
            // Arrange
            var cells1 = new List<(int x, int y)> { (0, 0) };
            var cells2 = new List<(int x, int y)> { (0, 0) };
            var orientations1 = new List<List<(int x, int y)>> { cells1 };
            var orientations2 = new List<List<(int x, int y)>> { cells2 };
            var shape1 = new Shape(1, cells1);
            var shape2 = new Shape(2, cells2);
            var presents = new List<(Shape, List<List<(int x, int y)>>)>
            {
                (shape1, orientations1),
                (shape2, orientations2)
            };
            var grid = new bool[5, 5];

            // Act
            var result = TryPlacePresents(presents, 1, grid, 5, 5);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void GetAllOrientations_WithSquareShape_ReturnsOneOrientation()
        {
            // Arrange
            var cells = new List<(int x, int y)> { (0, 0), (1, 0), (0, 1), (1, 1) };
            var shape = new Shape(1, cells);

            // Act
            var result = GetAllOrientations(shape);

            // Assert
            Assert.That(result, Has.Count.EqualTo(1));
        }

        [Test]
        public void GetAllOrientations_WithLShape_ReturnsEightOrientations()
        {
            // Arrange
            var cells = new List<(int x, int y)> { (0, 0), (0, 1), (0, 2), (1, 2) };
            var shape = new Shape(1, cells);

            // Act
            var result = GetAllOrientations(shape);

            // Assert
            Assert.That(result, Has.Count.EqualTo(8));
        }

        [Test]
        public void GetAllOrientations_WithLineShape_ReturnsTwoOrientations()
        {
            // Arrange
            var cells = new List<(int x, int y)> { (0, 0), (1, 0), (2, 0) };
            var shape = new Shape(1, cells);

            // Act
            var result = GetAllOrientations(shape);

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
        }

        [Test]
        public void GetAllOrientations_WithTShape_ReturnsFourOrientations()
        {
            // Arrange
            var cells = new List<(int x, int y)> { (0, 0), (1, 0), (2, 0), (1, 1) };
            var shape = new Shape(1, cells);

            // Act
            var result = GetAllOrientations(shape);

            // Assert
            Assert.That(result, Has.Count.EqualTo(4));
        }

        [Test]
        public void GetAllOrientations_AllOrientationsAreNormalized()
        {
            // Arrange
            var cells = new List<(int x, int y)> { (0, 0), (0, 1), (0, 2), (1, 2) };
            var shape = new Shape(1, cells);

            // Act
            var result = GetAllOrientations(shape);

            // Assert
            Assert.Multiple(() =>
            {
                foreach (var orientation in result)
                {
                    var minX = orientation.Min(c => c.x);
                    var minY = orientation.Min(c => c.y);
                    Assert.That(minX, Is.EqualTo(0));
                    Assert.That(minY, Is.EqualTo(0));
                }
            });
        }

        [Test]
        public void GetAllOrientations_WithSingleCell_ReturnsOneOrientation()
        {
            // Arrange
            var cells = new List<(int x, int y)> { (0, 0) };
            var shape = new Shape(1, cells);

            // Act
            var result = GetAllOrientations(shape);

            // Assert
            Assert.That(result, Has.Count.EqualTo(1));
        }

        [Test]
        public void Normalize_WithAlreadyNormalizedCells_ReturnsUnchanged()
        {
            // Arrange
            var cells = new List<(int x, int y)> { (0, 0), (1, 0), (0, 1) };

            // Act
            var result = Normalize(cells);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(3));
                Assert.That(result[0], Is.EqualTo((0, 0)));
                Assert.That(result[1], Is.EqualTo((1, 0)));
                Assert.That(result[2], Is.EqualTo((0, 1)));
            });
        }

        [Test]
        public void Normalize_WithOffsetCells_ShiftsToOrigin()
        {
            // Arrange
            var cells = new List<(int x, int y)> { (2, 3), (3, 3), (2, 4) };

            // Act
            var result = Normalize(cells);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(3));
                Assert.That(result[0], Is.EqualTo((0, 0)));
                Assert.That(result[1], Is.EqualTo((1, 0)));
                Assert.That(result[2], Is.EqualTo((0, 1)));
            });
        }

        [Test]
        public void Normalize_WithNegativeCoordinates_ShiftsToOrigin()
        {
            // Arrange
            var cells = new List<(int x, int y)> { (-2, -1), (-1, -1), (-2, 0) };

            // Act
            var result = Normalize(cells);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(3));
                Assert.That(result[0], Is.EqualTo((0, 0)));
                Assert.That(result[1], Is.EqualTo((1, 0)));
                Assert.That(result[2], Is.EqualTo((0, 1)));
            });
        }

        [Test]
        public void Normalize_WithEmptyCells_ReturnsEmpty()
        {
            // Arrange
            var cells = new List<(int x, int y)>();

            // Act
            var result = Normalize(cells);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Normalize_WithSingleCell_ReturnsOrigin()
        {
            // Arrange
            var cells = new List<(int x, int y)> { (5, 7) };

            // Act
            var result = Normalize(cells);

            // Assert
            Assert.That(result[0], Is.EqualTo((0, 0)));
        }

        [Test]
        public void CanPlace_WithValidEmptySpace_ReturnsTrue()
        {
            // Arrange
            var cells = new List<(int x, int y)> { (0, 0), (1, 0), (0, 1) };
            var grid = new bool[5, 5];

            // Act
            var result = CanPlace(cells, 1, 1, grid, 5, 5);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void CanPlace_WithOccupiedCell_ReturnsFalse()
        {
            // Arrange
            var cells = new List<(int x, int y)> { (0, 0), (1, 0) };
            var grid = new bool[5, 5];
            grid[2, 1] = true;

            // Act
            var result = CanPlace(cells, 1, 1, grid, 5, 5);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void CanPlace_WithOutOfBoundsNegative_ReturnsFalse()
        {
            // Arrange
            var cells = new List<(int x, int y)> { (0, 0), (1, 0) };
            var grid = new bool[5, 5];

            // Act
            var result = CanPlace(cells, -1, 0, grid, 5, 5);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void CanPlace_WithOutOfBoundsExceedsWidth_ReturnsFalse()
        {
            // Arrange
            var cells = new List<(int x, int y)> { (0, 0), (1, 0) };
            var grid = new bool[5, 5];

            // Act
            var result = CanPlace(cells, 4, 0, grid, 5, 5);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void CanPlace_WithOutOfBoundsExceedsHeight_ReturnsFalse()
        {
            // Arrange
            var cells = new List<(int x, int y)> { (0, 0), (0, 1) };
            var grid = new bool[5, 5];

            // Act
            var result = CanPlace(cells, 0, 4, grid, 5, 5);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void CanPlace_WithEmptyCells_ReturnsTrue()
        {
            // Arrange
            var cells = new List<(int x, int y)>();
            var grid = new bool[5, 5];

            // Act
            var result = CanPlace(cells, 0, 0, grid, 5, 5);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Place_WithPlaceTrue_SetsGridCellsToTrue()
        {
            // Arrange
            var cells = new List<(int x, int y)> { (0, 0), (1, 0), (0, 1) };
            var grid = new bool[3, 3];

            // Act
            Place(cells, 0, 0, grid, true);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(grid[0, 0], Is.True);
                Assert.That(grid[1, 0], Is.True);
                Assert.That(grid[0, 1], Is.True);
            });
        }

        [Test]
        public void Place_WithPlaceFalse_SetsGridCellsToFalse()
        {
            // Arrange
            var cells = new List<(int x, int y)> { (0, 0), (1, 0) };
            var grid = new bool[3, 3];
            grid[0, 0] = true;
            grid[1, 0] = true;

            // Act
            Place(cells, 0, 0, grid, false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(grid[0, 0], Is.False);
                Assert.That(grid[1, 0], Is.False);
            });
        }

        [Test]
        public void Place_WithOffset_AppliesOffsetToCoordinates()
        {
            // Arrange
            var cells = new List<(int x, int y)> { (0, 0), (1, 0) };
            var grid = new bool[4, 4];

            // Act
            Place(cells, 2, 1, grid, true);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(grid[2, 1], Is.True);
                Assert.That(grid[3, 1], Is.True);
            });
        }

        [Test]
        public void Place_WithEmptyCells_DoesNotModifyGrid()
        {
            // Arrange
            var cells = new List<(int x, int y)>();
            var grid = new bool[3, 3];

            // Act
            Place(cells, 0, 0, grid, true);

            // Assert
            Assert.That(grid[0, 0], Is.False);
        }
    }
}
