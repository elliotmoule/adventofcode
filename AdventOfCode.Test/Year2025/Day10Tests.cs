using static AdventOfCode.Business.Year2025.Day10;

namespace AdventOfCode.Test.Year2025
{
    internal class Day10Tests
    {
        [Test]
        public void CalculateFewestButtonPressesForMachines_ExampleInput_HasExpectedResult()
        {
            // Arrange
            var input = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Example", "Day10_Input.txt"));

            // Act
            var result = CalculateFewestButtonPressesForConfigurationOne(input);

            // Assert
            Assert.That(result, Is.EqualTo(7));
        }

        [Test]
        public void CalculateFewestButtonPressesForConfigurationTwo_ExampleInput_HasExpectedResult()
        {
            // Act
            var input = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Example", "Day10_Input.txt"));

            // Act
            var result = CalculateFewestButtonPressesForConfigurationTwo(input);

            // Assert
            Assert.That(result, Is.EqualTo(33));
        }

        [Test]
        public void CalculateMachineSolutionsConfigurationTwo_WithSingleMachine_ReturnsMinimumPresses()
        {
            // Arrange
            var machines = new HashSet<MachineConfigurationTwo>
            {
                new([2, 1], [[0, 1], [0]])
            };

            // Act
            var result = CalculateMachineSolutionsConfigurationTwo(machines);

            // Assert
            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void CalculateMachineSolutionsConfigurationTwo_WithMultipleMachines_SumsTotalPresses()
        {
            // Arrange
            var machines = new HashSet<MachineConfigurationTwo>
            {
                new([3, 5, 4, 7], [[3], [1,3], [2], [2,3], [0,2], [0,1]]),
                new([7,5,12,7,2], [[0,2,3,4],[2,3],[0,4],[0,1,2],[1,2,3,4]]),
                new([10,11,11,5,10,5], [[0,1,2,3,4],[0,3,4],[0,1,2,4,5],[1,2]])
            };

            // Act
            var result = CalculateMachineSolutionsConfigurationTwo(machines);

            // Assert
            Assert.That(result, Is.EqualTo(33), "Should sum: 0 + 3 + 2 = 5");
        }

        [Test]
        public void CalculateMachineSolutionsConfigurationTwo_WithAllMachinesAtZero_ReturnsZero()
        {
            // Arrange
            var machines = new HashSet<MachineConfigurationTwo>
            {
                new([0, 0], [[0]]),
                new([0], [[0]])
            };

            // Act
            var result = CalculateMachineSolutionsConfigurationTwo(machines);

            // Assert
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CalculateMachineSolutionsConfigurationTwo_WithUnsolvableMachine_ThrowsInvalidOperationException()
        {
            // Arrange
            var machines = new HashSet<MachineConfigurationTwo>
            {
                new([5, 3], [])
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                CalculateMachineSolutionsConfigurationTwo(machines));
        }

        [Test]
        public void CalculateMachineSolutionsConfigurationTwo_WithEmptyHashSet_ThrowsArgumentException()
        {
            // Arrange
            var machines = new HashSet<MachineConfigurationTwo>();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                CalculateMachineSolutionsConfigurationTwo(machines));
            Assert.That(ex!.Message, Does.Contain("zero machines"));
        }

        [Test]
        public void CalculateMachineSolutionsConfigurationTwo_WithNullHashSet_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                CalculateMachineSolutionsConfigurationTwo(null!));
        }

        [Test]
        public void FindMinimumButtonPresses_WithSimpleSolvableCase_ReturnsMinimumPresses()
        {
            // Arrange
            var targetJoltages = new List<int> { 2, 1 };
            var buttons = new List<List<int>>
            {
                new() { 0, 1 },
                new() { 0 },
            };

            // Act
            var result = FindMinimumButtonPresses(targetJoltages, buttons);

            // Assert
            Assert.That(result, Is.EqualTo(2), "Should press button twice to reach [0, 0]");
        }

        [Test]
        public void FindMinimumButtonPresses_WithMultipleButtons_FindsOptimalSolution()
        {
            // Arrange
            var targetJoltages = new List<int> { 3, 2 };
            var buttons = new List<List<int>>
            {
                new() { 0 },
                new() { 1 },
                new() { 0, 1 }
            };

            // Act
            var result = FindMinimumButtonPresses(targetJoltages, buttons);

            // Assert
            Assert.That(result, Is.LessThanOrEqualTo(5), "Should find an efficient solution");
        }

        [Test]
        public void FindMinimumButtonPresses_WithAlreadyAtZero_ReturnsZero()
        {
            // Arrange
            var targetJoltages = new List<int> { 0, 0 };
            var buttons = new List<List<int>>
            {
                new() { 0, 1 }
            };

            // Act
            var result = FindMinimumButtonPresses(targetJoltages, buttons);

            // Assert
            Assert.That(result, Is.EqualTo(0), "Should return 0 when already at target");
        }

        [Test]
        public void FindMinimumButtonPresses_WithNoValidSolution_ThrowsInvalidOperationException()
        {
            // Arrange
            var targetJoltages = new List<int> { 5, 3 };
            var buttons = new List<List<int>>
            {
                new() { 2 }
            };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                FindMinimumButtonPresses(targetJoltages, buttons));
        }

        [Test]
        public void FindMinimumButtonPresses_WithNoButtons_ThrowsInvalidOperationException()
        {
            // Arrange
            var targetJoltages = new List<int> { 5, 3 };
            var buttons = new List<List<int>>();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                FindMinimumButtonPresses(targetJoltages, buttons));
        }

        [Test]
        public void FindMinimumButtonPresses_WithDeterministicPath_FindsSolution()
        {
            // Arrange
            var targetJoltages = new List<int> { 5, 2 };
            var buttons = new List<List<int>>
            {
                new() { 0 },     // deterministic
                new() { 0, 1 }
            };

            // Act
            var result = FindMinimumButtonPresses(targetJoltages, buttons);

            // Assert
            Assert.That(result, Is.GreaterThan(0).And.LessThan(int.MaxValue));
        }

        [Test]
        public void FindMinimumButtonPresses_WithNullTargetJoltages_ThrowsArgumentNullException()
        {
            // Arrange
            var buttons = new List<List<int>> { new() { 0 } };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                FindMinimumButtonPresses(null!, buttons));
        }

        [Test]
        public void FindMinimumButtonPresses_WithNullButtons_ThrowsArgumentNullException()
        {
            // Arrange
            var targetJoltages = new List<int> { 5, 3 };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                FindMinimumButtonPresses(targetJoltages, null!));
        }

        [Test]
        public void FindMinimumButtonPresses_WithSingleCounter_FindsSolution()
        {
            // Arrange
            var targetJoltages = new List<int> { 3 };
            var buttons = new List<List<int>>
            {
                new() { 0 }
            };

            // Act
            var result = FindMinimumButtonPresses(targetJoltages, buttons);

            // Assert
            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public void EvaluateSearchState_WithAllZeroJoltages_ReturnsComplete()
        {
            // Arrange
            var state = new SearchState(
                [0, 0, 0],
                10,
                []);

            // Act
            var result = EvaluateSearchState(state, 100);

            // Assert
            Assert.That(result, Is.EqualTo(SearchStatus.Complete));
        }

        [Test]
        public void EvaluateSearchState_WithMaxJoltageZeroAndOthersNegative_ReturnsInvalid()
        {
            // Arrange - min is negative, so should return Invalid before checking max
            var state = new SearchState(
                [0, -1, -2],
                5,
                []);

            // Act
            var result = EvaluateSearchState(state, 100);

            // Assert
            Assert.That(result, Is.EqualTo(SearchStatus.Invalid),
                "Should return Invalid due to negative value, not Complete");
        }

        [Test]
        public void EvaluateSearchState_WhenPressCountPlusMaxExceedsBestSolution_ReturnsPruned()
        {
            // Arrange
            var state = new SearchState(
                [5, 10, 3],
                15,
                []);
            int bestKnownSolution = 20;

            // Act
            var result = EvaluateSearchState(state, bestKnownSolution);

            // Assert
            Assert.That(result, Is.EqualTo(SearchStatus.Pruned));
        }

        [Test]
        public void EvaluateSearchState_WhenPressCountPlusMaxEqualsBestSolution_ReturnsPruned()
        {
            // Arrange
            var state = new SearchState(
                [5, 10, 3],
                15,
                []);
            int bestKnownSolution = 25;

            // Act
            var result = EvaluateSearchState(state, bestKnownSolution);

            // Assert
            Assert.That(result, Is.EqualTo(SearchStatus.Pruned),
                "Should prune when equal to best solution");
        }

        [Test]
        public void EvaluateSearchState_WhenPressCountPlusMaxBelowBestSolution_ReturnsContinue()
        {
            // Arrange
            var state = new SearchState(
                [5, 10, 3],
                10,
                []);
            int bestKnownSolution = 25;

            // Act
            var result = EvaluateSearchState(state, bestKnownSolution);

            // Assert
            Assert.That(result, Is.EqualTo(SearchStatus.Continue));
        }

        [Test]
        public void EvaluateSearchState_WithPositiveJoltagesAndRoomToImprove_ReturnsContinue()
        {
            // Arrange
            var state = new SearchState(
                [2, 4, 6],
                5,
                []);
            int bestKnownSolution = 50;

            // Act
            var result = EvaluateSearchState(state, bestKnownSolution);

            // Assert
            Assert.That(result, Is.EqualTo(SearchStatus.Continue));
        }

        [Test]
        public void EvaluateSearchState_WithSingleJoltage_WorksCorrectly()
        {
            // Arrange
            var state = new SearchState(
                [5],
                2,
                []);
            int bestKnownSolution = 10;

            // Act
            var result = EvaluateSearchState(state, bestKnownSolution);

            // Assert
            Assert.That(result, Is.EqualTo(SearchStatus.Continue));
        }

        [Test]
        public void EvaluateSearchState_ChecksPriorityOrder_InvalidBeforeComplete()
        {
            // Arrange
            var state = new SearchState(
                [0, -1],
                5,
                []);

            // Act
            var result = EvaluateSearchState(state, 100);

            // Assert
            Assert.That(result, Is.EqualTo(SearchStatus.Invalid),
                "Should check for invalid (negative) before checking for complete");
        }

        [Test]
        public void EvaluateSearchState_WithZeroPressCount_WorksCorrectly()
        {
            // Arrange
            var state = new SearchState(
                [10, 20],
                0,
                []);
            int bestKnownSolution = 15;

            // Act
            var result = EvaluateSearchState(state, bestKnownSolution);

            // Assert
            Assert.That(result, Is.EqualTo(SearchStatus.Pruned));
        }

        [Test]
        public void EvaluateSearchState_WithLargeValues_HandlesCorrectly()
        {
            // Arrange
            var state = new SearchState(
                [1000000, 2000000],
                500000,
                []);
            int bestKnownSolution = 3000000;

            // Act
            var result = EvaluateSearchState(state, bestKnownSolution);

            // Assert
            Assert.That(result, Is.EqualTo(SearchStatus.Continue));
        }

        [Test]
        public void TryFindDeterministicMove_WithOneButtonAffectingHigherOnly_ReturnsTrueAndCreatesNextState()
        {
            // Arrange
            var currentJoltages = new List<long> { 7, 3, 5 };
            var availableButtons = new List<List<int>>
                {
                    new() { 0, 1 },
                    new() { 0, 2 },
                    new() { 0 }
                };
            var state = new SearchState(currentJoltages, 5, availableButtons);

            // Act
            bool result = TryFindDeterministicMove(state, out SearchState? nextState);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(nextState, Is.Not.Null);
                Assert.That(nextState!.CurrentJoltages[0], Is.EqualTo(6), "Higher counter should be decremented");
                Assert.That(nextState.CurrentJoltages[1], Is.EqualTo(3), "Lower counter should remain unchanged");
                Assert.That(nextState.PressCount, Is.EqualTo(6), "Press count should increment");
                Assert.That(nextState.AvailableButtons, Is.SameAs(availableButtons), "Available buttons should remain the same");
            });
        }

        [Test]
        public void TryFindDeterministicMove_WithMultipleButtonsAffectingHigherOnly_ReturnsFalse()
        {
            // Arrange
            var currentJoltages = new List<long> { 5, 3 };
            var availableButtons = new List<List<int>>
                {
                    new() { 0 },
                    new() { 0, 2 },
                    new() { 0 }
                };
            var state = new SearchState(currentJoltages, 0, availableButtons);

            // Act
            bool result = TryFindDeterministicMove(state, out SearchState? nextState);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False, "Should return false when multiple buttons affect higher only");
                Assert.That(nextState, Is.Null);
            });
        }

        [Test]
        public void TryFindDeterministicMove_WithNoButtonsAffectingHigherOnly_ReturnsFalse()
        {
            // Arrange
            var currentJoltages = new List<long> { 5, 3 };
            var availableButtons = new List<List<int>>
                {
                    new() { 0, 1 },
                    new() { 1 },
                    new() { 2, 3 }
                };
            var state = new SearchState(currentJoltages, 0, availableButtons);

            // Act
            bool result = TryFindDeterministicMove(state, out SearchState? nextState);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
                Assert.That(nextState, Is.Null);
            });
        }

        [Test]
        public void TryFindDeterministicMove_WithEqualJoltages_SkipsPairAndContinues()
        {
            // Arrange
            var currentJoltages = new List<long> { 5, 5, 3 };
            var availableButtons = new List<List<int>>
                {
                    new() { 0 }
                };
            var state = new SearchState(currentJoltages, 0, availableButtons);

            // Act
            bool result = TryFindDeterministicMove(state, out SearchState? nextState);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True, "Should skip equal pair and find deterministic move for other pairs");
                Assert.That(nextState, Is.Not.Null);
            });
        }

        [Test]
        public void TryFindDeterministicMove_WithAllEqualJoltages_ReturnsFalse()
        {
            // Arrange
            var currentJoltages = new List<long> { 3, 3, 3 };
            var availableButtons = new List<List<int>>
                {
                    new() { 0 }
                };
            var state = new SearchState(currentJoltages, 0, availableButtons);

            // Act
            bool result = TryFindDeterministicMove(state, out SearchState? nextState);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False, "Should return false when all joltages are equal");
                Assert.That(nextState, Is.Null);
            });
        }

        [Test]
        public void TryFindDeterministicMove_WithSingleJoltage_ReturnsFalse()
        {
            // Arrange
            var currentJoltages = new List<long> { 3 };
            var availableButtons = new List<List<int>>
                {
                    new() { 0 }
                };
            var state = new SearchState(currentJoltages, 0, availableButtons);

            // Act
            bool result = TryFindDeterministicMove(state, out SearchState? nextState);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False, "Should return false with only one counter");
                Assert.That(nextState, Is.Null);
            });
        }

        [Test]
        public void TryFindDeterministicMove_ChecksLowerCounterCorrectly()
        {
            // Arrange
            var currentJoltages = new List<long> { 3, 5 };
            var availableButtons = new List<List<int>>
                {
                    new() { 1 }
                };
            var state = new SearchState(currentJoltages, 0, availableButtons);

            // Act
            bool result = TryFindDeterministicMove(state, out SearchState? nextState);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(nextState!.CurrentJoltages[0], Is.EqualTo(3), "Lower counter unchanged");
                Assert.That(nextState.CurrentJoltages[1], Is.EqualTo(4), "Higher counter decremented");
            });
        }

        [Test]
        public void TryFindDeterministicMove_WithNullState_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                TryFindDeterministicMove(null!, out _));
        }

        [Test]
        public void TryFindDeterministicMove_WithMultipleCounterPairs_FindsFirstDeterministicMove()
        {
            // Arrange
            var currentJoltages = new List<long> { 3, 5, 4 };
            var availableButtons = new List<List<int>>
                {
                    new() { 0 },
                    new() { 1, 2 }
                };
            var state = new SearchState(currentJoltages, 0, availableButtons);

            // Act
            bool result = TryFindDeterministicMove(state, out SearchState? nextState);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True, "Should find deterministic move for first suitable pair");
                Assert.That(nextState!.CurrentJoltages[0], Is.EqualTo(3));
            });
        }

        [Test]
        public void TryFindDeterministicMove_WithEmptyAvailableButtons_ReturnsFalse()
        {
            // Arrange
            var currentJoltages = new List<long> { 3, 5 };
            var availableButtons = new List<List<int>>();
            var state = new SearchState(currentJoltages, 0, availableButtons);

            // Act
            bool result = TryFindDeterministicMove(state, out SearchState? nextState);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
                Assert.That(nextState, Is.Null);
            });
        }

        [Test]
        public void FindButtonsAffectingHigherCounterOnly_WithButtonAffectingOnlyHigher_ReturnsButtonIndex()
        {
            // Arrange
            var availableButtons = new List<List<int>>
            {
                new() { 0, 1, 2 },
                new() { 3, 4 },
                new() { 5, 6 }
            };
            int higherCounter = 3;
            int lowerCounter = 1;

            // Act
            var result = FindButtonsAffectingHigherCounterOnly(availableButtons, higherCounter, lowerCounter);

            // Assert
            Assert.That(result, Is.EqualTo(new List<int> { 1 }));
        }

        [Test]
        public void FindButtonsAffectingHigherCounterOnly_WithMultipleMatchingButtons_ReturnsAllIndices()
        {
            // Arrange
            var availableButtons = new List<List<int>>
            {
                new() { 5, 7 },
                new() { 2, 3 },
                new() { 5, 8 },
                new() { 5, 3 }
            };
            int higherCounter = 5;
            int lowerCounter = 3;

            // Act
            var result = FindButtonsAffectingHigherCounterOnly(availableButtons, higherCounter, lowerCounter);

            // Assert
            Assert.That(result, Is.EqualTo(new List<int> { 0, 2 }));
        }

        [Test]
        public void FindButtonsAffectingHigherCounterOnly_WithButtonAffectingBoth_ExcludesButton()
        {
            // Arrange
            var availableButtons = new List<List<int>>
            {
                new() { 2, 4, 6 }
            };
            int higherCounter = 4;
            int lowerCounter = 2;

            // Act
            var result = FindButtonsAffectingHigherCounterOnly(availableButtons, higherCounter, lowerCounter);

            // Assert
            Assert.That(result, Is.Empty, "Should not include buttons that affect both counters");
        }

        [Test]
        public void FindButtonsAffectingHigherCounterOnly_WithButtonAffectingOnlyLower_ExcludesButton()
        {
            // Arrange
            var availableButtons = new List<List<int>>
            {
                new() { 1, 3, 5 }
            };
            int higherCounter = 7;
            int lowerCounter = 3;

            // Act
            var result = FindButtonsAffectingHigherCounterOnly(availableButtons, higherCounter, lowerCounter);

            // Assert
            Assert.That(result, Is.Empty, "Should not include buttons that only affect lower counter");
        }

        [Test]
        public void FindButtonsAffectingHigherCounterOnly_WithButtonAffectingNeither_ExcludesButton()
        {
            // Arrange
            var availableButtons = new List<List<int>>
            {
                new() { 0, 1, 2 }
            };
            int higherCounter = 5;
            int lowerCounter = 3;

            // Act
            var result = FindButtonsAffectingHigherCounterOnly(availableButtons, higherCounter, lowerCounter);

            // Assert
            Assert.That(result, Is.Empty, "Should not include buttons that affect neither counter");
        }

        [Test]
        public void FindButtonsAffectingHigherCounterOnly_WithEmptyButtonList_ReturnsEmptyList()
        {
            // Arrange
            var availableButtons = new List<List<int>>();
            int higherCounter = 5;
            int lowerCounter = 3;

            // Act
            var result = FindButtonsAffectingHigherCounterOnly(availableButtons, higherCounter, lowerCounter);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void FindButtonsAffectingHigherCounterOnly_WithEmptyButtons_ExcludesThem()
        {
            // Arrange
            var availableButtons = new List<List<int>>
            {
                new(),
                new() { 5 },
                new()
            };
            int higherCounter = 5;
            int lowerCounter = 3;

            // Act
            var result = FindButtonsAffectingHigherCounterOnly(availableButtons, higherCounter, lowerCounter);

            // Assert
            Assert.That(result, Is.EqualTo(new List<int> { 1 }));
        }

        [Test]
        public void FindButtonsAffectingHigherCounterOnly_WithSingleElementButton_ReturnsHelpfulIndice()
        {
            // Arrange
            var availableButtons = new List<List<int>>
            {
                new() { 0 },
                new() { 1 },
                new() { 2 }
            };
            int higherCounter = 2;
            int lowerCounter = 0;

            // Act
            var result = FindButtonsAffectingHigherCounterOnly(availableButtons, higherCounter, lowerCounter);

            // Assert
            Assert.That(result, Is.EqualTo(new List<int> { 2 }));
        }

        [Test]
        public void FindButtonsAffectingHigherCounterOnly_WithSameHigherAndLowerCounter_ReturnsEmpty()
        {
            // Arrange
            var availableButtons = new List<List<int>>
            {
                new() { 5, 7 },
                new() { 3, 5, 9 }
            };
            int higherCounter = 5;
            int lowerCounter = 5;

            // Act
            var result = FindButtonsAffectingHigherCounterOnly(availableButtons, higherCounter, lowerCounter);

            // Assert
            Assert.That(result, Is.Empty, "When counters are the same, no button can affect only higher");
        }

        [Test]
        public void FindButtonsAffectingHigherCounterOnly_WithMultipleOccurrencesInButton_StillWorks()
        {
            // Arrange
            var availableButtons = new List<List<int>>
            {
                new() { 5, 5, 5, 7 },
                new() { 3, 3 }
            };
            int higherCounter = 5;
            int lowerCounter = 3;

            // Act
            var result = FindButtonsAffectingHigherCounterOnly(availableButtons, higherCounter, lowerCounter);

            // Assert
            Assert.That(result, Is.EqualTo(new List<int> { 0 }));
        }

        [Test]
        public void FindButtonsAffectingHigherCounterOnly_WithAllButtonsMatching_ReturnsAllIndices()
        {
            // Arrange
            var availableButtons = new List<List<int>>
            {
                new() { 10 },
                new() { 10, 11 },
                new() { 10, 12, 13 }
            };
            int higherCounter = 10;
            int lowerCounter = 5;

            // Act
            var result = FindButtonsAffectingHigherCounterOnly(availableButtons, higherCounter, lowerCounter);

            // Assert
            Assert.That(result, Is.EqualTo(new List<int> { 0, 1, 2 }));
        }

        [Test]
        public void FindButtonsAffectingHigherCounterOnly_WithNullAvailableButtons_ThrowsArgumentNullException()
        {
            // Arrange
            int higherCounter = 5;
            int lowerCounter = 3;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                FindButtonsAffectingHigherCounterOnly(null!, higherCounter, lowerCounter));
        }

        [Test]
        public void FindButtonsAffectingHigherCounterOnly_ReturnsIndicesInOrder()
        {
            // Arrange
            var availableButtons = new List<List<int>>
            {
                new () { 10 },
                new () { 5 },
                new () { 10, 11 },
                new () { 3 },
                new () { 10, 12 }
            };
            int higherCounter = 10;
            int lowerCounter = 5;

            // Act
            var result = FindButtonsAffectingHigherCounterOnly(availableButtons, higherCounter, lowerCounter);

            // Assert
            Assert.That(result, Is.EqualTo(new List<int> { 0, 2, 4 }),
                "Indices should be returned in ascending order");
        }


        [Test]
        public void PressButton_WithSingleCounterIndex_DecrementsCorrectJoltage()
        {
            // Arrange
            var currentJoltages = new List<long> { 3, 5, 4, 7 };
            var button = new List<int> { 1 };

            // Act
            var result = PressButton(currentJoltages, button);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0], Is.EqualTo(3), "Index 0 should remain unchanged");
                Assert.That(result[1], Is.EqualTo(4), "Index 1 should be decremented by 1");
                Assert.That(result[2], Is.EqualTo(4), "Index 2 should remain unchanged");
                Assert.That(result[3], Is.EqualTo(7), "Index 3 should remain unchanged");
            });
        }

        [Test]
        public void PressButton_WithMultipleCounterIndices_DecrementsAllSpecifiedJoltages()
        {
            // Arrange
            var currentJoltages = new List<long> { 3, 5, 4, 7 };
            var button = new List<int> { 0, 2, 3 };

            // Act
            var result = PressButton(currentJoltages, button);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0], Is.EqualTo(2), "Index 0 should be decremented");
                Assert.That(result[1], Is.EqualTo(5), "Index 1 should remain unchanged");
                Assert.That(result[2], Is.EqualTo(3), "Index 2 should be decremented");
                Assert.That(result[3], Is.EqualTo(6), "Index 3 should be decremented");
            });
        }

        [Test]
        public void PressButton_WithDuplicateIndices_DecrementsSameJoltageMultipleTimes()
        {
            // Arrange
            var currentJoltages = new List<long> { 3, 5, 4, 7 };
            var button = new List<int> { 1, 1, 1 };

            // Act
            var result = PressButton(currentJoltages, button);

            // Assert
            Assert.That(result[1], Is.EqualTo(2), "Index 1 should be decremented 3 times");
        }

        [Test]
        public void PressButton_WithEmptyButton_ReturnsUnchangedJoltages()
        {
            // Arrange
            var currentJoltages = new List<long> { 3, 5, 4, 7 };
            var button = new List<int>();

            // Act
            var result = PressButton(currentJoltages, button);

            // Assert
            Assert.That(result, Is.EqualTo(currentJoltages));
            Assert.That(result, Is.Not.SameAs(currentJoltages), "Should return a new list instance");
        }

        [Test]
        public void PressButton_DoesNotModifyOriginalList()
        {
            // Arrange
            var currentJoltages = new List<long> { 3, 5, 4, 7 };
            var originalValues = new List<long>(currentJoltages);
            var button = new List<int> { 0, 1, 2 };

            // Act
            var result = PressButton(currentJoltages, button);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(currentJoltages, Is.EqualTo(originalValues), "Original list should not be modified");
                Assert.That(result, Is.Not.SameAs(currentJoltages), "Should return a different list instance");
            });
        }

        [Test]
        public void PressButton_WithZeroJoltage_CanGoNegative()
        {
            // Arrange
            var currentJoltages = new List<long> { 0, 5, 4, 7 };
            var button = new List<int> { 0, 1 };

            // Act
            var result = PressButton(currentJoltages, button);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0], Is.EqualTo(-1), "Should allow joltage to go negative");
                Assert.That(result[1], Is.EqualTo(4));
            });
        }

        [Test]
        public void PressButton_WithAllIndices_DecrementsAllJoltages()
        {
            // Arrange
            var currentJoltages = new List<long> { 3, 5, 4, 7 };
            var button = new List<int> { 0, 1, 2, 3 };

            // Act
            var result = PressButton(currentJoltages, button);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0], Is.EqualTo(2));
                Assert.That(result[1], Is.EqualTo(4));
                Assert.That(result[2], Is.EqualTo(3));
                Assert.That(result[3], Is.EqualTo(6));
            });
        }

        [Test]
        public void PressButton_WithNullCurrentJoltages_ThrowsArgumentNullException()
        {
            // Arrange
            var button = new List<int> { 0 };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => PressButton(null!, button));
        }

        [Test]
        public void PressButton_WithNullButton_ThrowsArgumentNullException()
        {
            // Arrange
            var currentJoltages = new List<long> { 3, 5, 4, 7 };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => PressButton(currentJoltages, null!));
        }

        [Test]
        public void PressButton_WithSingleJoltageAndSingleButton_Works()
        {
            // Arrange
            var currentJoltages = new List<long> { 3 };
            var button = new List<int> { 0 };

            // Act
            var result = PressButton(currentJoltages, button);

            // Assert
            Assert.That(result[0], Is.EqualTo(2));
        }

        [Test]
        public void PressButton_WithLargeJoltageValues_HandlesCorrectly()
        {
            // Arrange
            var currentJoltages = new List<long> { long.MaxValue - 10, 1000000000L };
            var button = new List<int> { 0, 1 };

            // Act
            var result = PressButton(currentJoltages, button);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0], Is.EqualTo(long.MaxValue - 11));
                Assert.That(result[1], Is.EqualTo(999999999L));
            });
        }

        [Test]
        public void EnqueueAllButtonOptions_WithMultipleButtons_EnqueuesAllInReverseOrder()
        {
            // Arrange
            var stack = new Stack<SearchState>();
            var currentJoltages = new List<long> { 3, 5, 4, 7 };
            var availableButtons = new List<List<int>>
            {
                new() { 3 },
                new() { 1, 3 },
                new() { 2 },
                new() { 2, 3 },
                new() { 0, 2 },
                new() { 0, 1 },
            };

            var currentState = new SearchState(currentJoltages, 5, availableButtons);

            // Act
            EnqueueAllButtonOptions(stack, currentState);

            // Assert
            Assert.That(stack, Has.Count.EqualTo(6), "Should enqueue one state per available button");

            // Verify states are pushed in reverse order (last button first, first button last)
            var firstPopped = stack.Pop();
            Assert.Multiple(() =>
            {
                Assert.That(firstPopped.PressCount, Is.EqualTo(6), "Press count should increment by 1");
                Assert.That(firstPopped.AvailableButtons, Has.Count.EqualTo(6), "Should have 6 remaining button (buttons from index 0 onward)");
            });

            var secondPopped = stack.Pop();
            Assert.Multiple(() =>
            {
                Assert.That(secondPopped.PressCount, Is.EqualTo(6));
                Assert.That(secondPopped.AvailableButtons, Has.Count.EqualTo(5), "Should have 5 remaining buttons (buttons from index 1 onward)");
            });

            var thirdPopped = stack.Pop();
            Assert.Multiple(() =>
            {
                Assert.That(thirdPopped.PressCount, Is.EqualTo(6));
                Assert.That(thirdPopped.AvailableButtons, Has.Count.EqualTo(4), "Should have 4 remaining buttons (buttons from index 2 onward)");
            });
        }

        [Test]
        public void EnqueueAllButtonOptions_WithEmptyButtons_DoesNotEnqueueAnything()
        {
            // Arrange
            var stack = new Stack<SearchState>();
            var currentJoltages = new List<long> { 100 };
            var availableButtons = new List<List<int>>();
            var currentState = new SearchState(currentJoltages, 3, availableButtons);

            // Act
            EnqueueAllButtonOptions(stack, currentState);

            // Assert
            Assert.That(stack, Is.Empty, "Should not enqueue any states when no buttons available");
        }

        [Test]
        public void EnqueueAllButtonOptions_WithNullStack_ThrowsArgumentNullException()
        {
            // Arrange
            var currentState = new SearchState([1], 0, []);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => EnqueueAllButtonOptions(null!, currentState));
        }

        [Test]
        public void EnqueueAllButtonOptions_WithNullCurrentState_ThrowsArgumentNullException()
        {
            // Arrange
            var stack = new Stack<SearchState>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => EnqueueAllButtonOptions(stack, null!));
        }

        [Test]
        public void GetMachinesForConfigurationTwo_OneValidMachine_ReturnsExpectedConfiguration()
        {
            // Arrange
            var input = "[.##...] (4) (2,4) (3) (3,4) (1,3) (1,2) {38,30,29,22,13,25}";

            // Act
            var result = GetMachinesForConfigurationTwo([input]);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Has.Count.EqualTo(1));
                Assert.That(result.FirstOrDefault()!.Joltages, Is.EquivalentTo([38, 30, 29, 22, 13, 25]));
                Assert.That(result.FirstOrDefault()!.Buttons, Is.EquivalentTo(
                new List<List<int>>
                {
                    new() { 4 },
                    new() { 2,4 },
                    new() { 3 },
                    new() { 3,4 },
                    new() { 1,3 },
                    new() { 1,2 },
                }));
            });
        }

        [Test]
        public void GetMachinesForConfigurationTwo_InvalidInput_ThrowsFormatException()
        {
            // Act & Assert
            Assert.Throws<FormatException>(() => GetMachinesForConfigurationTwo(["[.##...] {38,30,29,22,13,25}"]));
        }

        [Test]
        public void GetMachinesForConfigurationTwo_EmptyInput_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => GetMachinesForConfigurationTwo([]));
        }

        [Test]
        public void GetMachinesForConfigurationTwo_NullInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => GetMachinesForConfigurationTwo(null!));
        }

        [Test]
        public void TryParseMachineConfigurationTwo_ValidMachine_ReturnsExpectedConfiguration()
        {
            // Arrange
            var input = "[.##...] (4) (2,4) (3) (3,4) (1,3) (1,2) {38,30,29,22,13,25}";

            // Act
            var result = TryParseMachineConfigurationTwo(input, out var machineConfiguration);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(machineConfiguration!.Joltages, Is.EquivalentTo([38, 30, 29, 22, 13, 25]));
                Assert.That(machineConfiguration!.Buttons, Is.EquivalentTo(
                new List<List<int>>
                {
                    new() { 4 },
                    new() { 2,4 },
                    new() { 3 },
                    new() { 3,4 },
                    new() { 1,3 },
                    new() { 1,2 },
                }));
            });
        }

        [Test]
        public void TryParseMachineConfigurationTwo_NoSchematicInput_ThrowsFormatException()
        {
            // Act & Assert
            Assert.Throws<FormatException>(() => TryParseMachineConfigurationTwo("[.##...] {38,30,29,22,13,25}", out var _));
        }

        [Test]
        public void TryParseMachineConfigurationTwo_NoJoltagesInput_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<FormatException>(() => TryParseMachineConfigurationTwo("[.##.] (4) (2,4) (3) (3,4) (1,3) (1,2)", out var _));
        }

        [Test]
        public void TryParseMachineConfigurationTwo_EmptyInput_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => TryParseMachineConfigurationTwo(string.Empty, out var _));
        }

        [Test]
        public void TryParseMachineConfigurationTwo_NullInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => TryParseMachineConfigurationTwo(null!, out var _));
        }

        [Test]
        public void CalculateMachineSolutions_OneValidMachine_ReturnsExpectedResult()
        {
            // Arrange
            var machines = new HashSet<MachineConfigurationOne>
            {
                new(Mask: 6,
                    Buttons:
                    [
                        (16, "(4)"),
                        (20, "(2,4)"),
                        (8, "(3)"),
                        (24, "(3,4)"),
                        (10, "(1,3)"),
                        (6, "(1,2)"),
                    ])
            };

            // Act
            var result = CalculateMachineSolutions(machines);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(1));
                Assert.That(result.Sum(s => s.Steps), Is.EqualTo(1));
            });
        }

        [Test]
        public void CalculateMachineSolutions_EmptyInput_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => CalculateMachineSolutions([]));
        }

        [Test]
        public void CalculateMachineSolutions_NullInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => CalculateMachineSolutions(null!));
        }

        [Test]
        public void GetMachines_InvalidLine_ThrowsFormatException()
        {
            // Arrange
            string[] input =
            [
                "[.##.] (4) (2,4) (3) (3,4) (1,3) (A,2)",
            ];

            // Act & Assert
            Assert.Throws<FormatException>(() => GetMachines(input));
        }

        [Test]
        public void GetMachines_OneValidLine_ReturnsOneMachine()
        {
            // Arrange
            string[] input =
            [
                "[.##.] (4) (2,4) (3) (3,4) (1,3) (1,2)",
            ];

            // Act
            var result = GetMachines(input);

            // Assert
            Assert.That(result, Has.Count.EqualTo(1));
        }

        [Test]
        public void GetMachines_TwoValidLines_ReturnsTwoMachines()
        {
            // Arrange
            string[] input =
            [
                "[.##.] (4) (2,4) (3) (3,4) (1,3) (1,2)",
                "[##..] (1) (1,2) (2) (2,3) (3) (3,4)",
            ];

            // Act
            var result = GetMachines(input);

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
        }

        [Test]
        public void GetMachines_EmptyInput_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => GetMachines([]));
        }

        [Test]
        public void GetMachines_NullInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => GetMachines(null!));
        }

        [Test]
        public void TryParseMachineConfigurationOne_CorrectInput_ReturnsExpectedResult()
        {
            // Arrange
            var input = "[.##.] (4) (2,4) (3) (3,4) (1,3) (1,2)";

            // Act
            var result = TryParseMachineConfigurationOne(input, out var machineConfiguration);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(machineConfiguration!.Mask, Is.EqualTo(6));
                Assert.That(machineConfiguration!.Buttons, Is.EquivalentTo(
                [
                    (16, "(4)"),
                    (20, "(2,4)"),
                    (8, "(3)"),
                    (24, "(3,4)"),
                    (10, "(1,3)"),
                    (6, "(1,2)"),
                ]));
            });
        }

        [Test]
        public void TryParseMachineConfigurationOne_NullInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => TryParseMachineConfigurationOne(null!, out var _));
        }

        [Test]
        public void TryParseMachineConfigurationOne_EmptyInput_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => TryParseMachineConfigurationOne(string.Empty, out var _));
        }

        [Test]
        public void TryParseMachineConfigurationOne_InvalidDiagramFormatInput_ThrowsFormatException()
        {
            // Act & Assert
            Assert.Throws<FormatException>(() => TryParseMachineConfigurationOne("[.#&.] (4) (2,4) (3) (3,4) (1,3) (1,2)", out var _));
        }

        [Test]
        public void TryParseMachineConfigurationOne_InvalidSchematicFormatInput_ThrowsFormatException()
        {
            // Act & Assert
            Assert.Throws<FormatException>(() => TryParseMachineConfigurationOne("[.##.] (4) (2,4) (3) (3,4) (1,3) (A,2)", out var _));
        }
    }
}
