using AdventOfCode.Business.Year2025;

namespace AdventOfCode.Test.Year2025
{
    public class Day1Test
    {
        [Test]
        public void ParseInstruction_WhenInputIsLeft75_ShouldReturnDirectionLeftNumberOfTurns75()
        {
            // Arrange
            var instruction = "L75";

            // Act
            var (direction, numberOfTurns) = Day1.ParseInstruction(instruction);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(direction, Is.EqualTo(Direction.Left));
                Assert.That(numberOfTurns, Is.EqualTo(75));
            });
        }

        [Test]
        public void TurnDial_WhenCurrentPositionIs20AndTurnLeft30_ShouldReturnCorrectNewPosition()
        {
            // Arrange
            int currentPosition = 20;
            Direction direction = Direction.Left;
            int numberOfTurns = 30;

            // Act
            var newPosition = Day1.TurnDial(currentPosition, direction, numberOfTurns);

            // Assert
            int expectedPosition = 90; // 20 - 30 wraps around to 90
            Assert.That(newPosition, Is.EqualTo(expectedPosition));
        }

        [Test]
        public void IsAtZeroPosition_WhenCurrentPositionIs0_ShouldReturnTrue()
        {
            // Arrange
            int currentPosition = 0;

            // Act
            var result = Day1.IsAtZeroPosition(currentPosition);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void CalculateNumberOfTimesDialIsAt0_WhenInstructionsProvided_ReturnsCorrectNumberOfTimesDialIsAt0()
        {
            // Arrange
            var instructions = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Example", "Day1_Input.txt"));

            // Act
            var result = Day1.CalculateNumberOfTimesDialIsAt0(instructions);

            // Assert
            int expectedCount = 3;
            Assert.That(result, Is.EqualTo(expectedCount));
        }

        [Test]
        public void CalculateNumberOfTimesDialPasses0_WhenInstructionsProvided_ReturnsCorrectNumberOfTimesDialPasses0()
        {
            // Arrange
            var instructions = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Example", "Day1_Input.txt"));

            // Act
            var result = Day1.CalculateNumberOfTimesDialPasses0(instructions);

            // Assert
            int expectedCount = 6;
            Assert.That(result, Is.EqualTo(expectedCount));
        }

        [Test]
        public void CalculateNumberOfTimesDialPasses0_WhenNoPasses_ShouldReturnZero()
        {
            // Arrange
            var instructions = new string[]
            {
                "R10",
                "R20",
                "L15",
            };
            // Act
            var result = Day1.CalculateNumberOfTimesDialPasses0(instructions);
            // Assert
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CalculateNumberOfTimesDialPasses0_WhenInputIsR1000_ShouldReturn10()
        {
            // Arrange
            var instructions = new string[]
            {
                "R1000",
            };

            // Act
            var result = Day1.CalculateNumberOfTimesDialPasses0(instructions);

            // Assert
            Assert.That(result, Is.EqualTo(10));
        }
    }
}
