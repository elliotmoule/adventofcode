using System.Text.RegularExpressions;

namespace AdventOfCode.Business.Year2025
{
    internal partial class Day10 : IAdventDay
    {
        string _root = string.Empty;
        public void SetRoot(string path)
        {
            _root = path;
        }

        public void ExecutePart1()
        {
            var day10Input = File.ReadAllLines(Path.Combine(_root, "Day10_Input.txt"));
            var result = CalculateFewestButtonPressesForConfigurationOne(day10Input);

            Console.WriteLine($"\r\nThe fewest button presses required to correctly configure the indicator lights for the light diagrams on all of the machines is: '{result}'.\r\n");
        }

        public void ExecutePart2()
        {
            var day10Input = File.ReadAllLines(Path.Combine(_root, "Day10_Input.txt"));
            var result = CalculateFewestButtonPressesForConfigurationTwo(day10Input);

            Console.WriteLine($"\r\nThe fewest button presses required to correctly configure the indicator lights for the joltage counters on all of the machines is: '{result}'.\r\n");
        }

        internal static int CalculateFewestButtonPressesForConfigurationTwo(string[] lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Length == 0)
            {
                throw new ArgumentException("Machines instructions were empty.");
            }

            var machines = GetMachinesForConfigurationTwo(lines);
            var solution = CalculateMachineSolutionsConfigurationTwo(machines);

            return solution;
        }

        internal static int CalculateFewestButtonPressesForConfigurationOne(string[] lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Length == 0)
            {
                throw new ArgumentException("Machines instructions were empty.");
            }

            HashSet<MachineConfigurationOne> machines = GetMachines(lines);
            var solutions = CalculateMachineSolutions(machines);

            if (solutions == null || solutions.Count == 0)
            {
                throw new Exception("No solutions created from calculation");
            }

            return solutions.Sum(s => s.Steps);
        }

        internal static int CalculateMachineSolutionsConfigurationTwo(HashSet<MachineConfigurationTwo> machines)
        {
            ArgumentNullException.ThrowIfNull(machines);

            if (machines.Count == 0)
            {
                throw new ArgumentException("Unable to calculate solution with zero machines input.");
            }

            int totalPresses = 0;

            foreach (var machine in machines)
            {
                int minPresses = FindMinimumButtonPresses(machine.Joltages, machine.Buttons);
                totalPresses += minPresses;
            }

            return totalPresses;
        }

        internal static int FindMinimumButtonPresses(List<int> targetJoltages, List<List<int>> buttons)
        {
            ArgumentNullException.ThrowIfNull(targetJoltages);
            ArgumentNullException.ThrowIfNull(buttons);

            int bestSolution = int.MaxValue;
            var initialState = targetJoltages.Select(j => (long)j).ToList();

            var searchStack = new Stack<SearchState>();
            searchStack.Push(new SearchState(initialState, 0, buttons));

            while (searchStack.TryPop(out var currentState))
            {
                var status = EvaluateSearchState(currentState, bestSolution);

                if (status == SearchStatus.Invalid)
                {
                    continue;
                }

                if (status == SearchStatus.Complete)
                {
                    bestSolution = Math.Min(bestSolution, currentState.PressCount);
                    continue;
                }

                if (status == SearchStatus.Pruned)
                {
                    continue;
                }

                if (TryFindDeterministicMove(currentState, out var deterministicNextState) && deterministicNextState != null)
                {
                    searchStack.Push(deterministicNextState);
                    continue;
                }

                // Couldn't find a deterministic move. Explore all button options.
                EnqueueAllButtonOptions(searchStack, currentState);
            }

            if (bestSolution == int.MaxValue)
            {
                throw new InvalidOperationException("No valid solution found for machine configuration.");
            }

            return bestSolution;
        }

        internal static SearchStatus EvaluateSearchState(SearchState state, int bestKnownSolution)
        {
            long minJoltage = state.CurrentJoltages.Min();
            long maxJoltage = state.CurrentJoltages.Max();

            if (minJoltage < 0)
            {
                return SearchStatus.Invalid;
            }

            if (maxJoltage == 0)
            {
                return SearchStatus.Complete;
            }

            if (state.PressCount + maxJoltage >= bestKnownSolution)
            {
                return SearchStatus.Pruned;
            }

            return SearchStatus.Continue;
        }

        internal static bool TryFindDeterministicMove(SearchState state, out SearchState? nextState)
        {
            ArgumentNullException.ThrowIfNull(state);

            nextState = null;

            for (int counterA = 0; counterA < state.CurrentJoltages.Count; counterA++)
            {
                for (int counterB = counterA + 1; counterB < state.CurrentJoltages.Count; counterB++)
                {
                    if (state.CurrentJoltages[counterA] == state.CurrentJoltages[counterB])
                    {
                        continue;
                    }

                    int higherCounter = state.CurrentJoltages[counterA] > state.CurrentJoltages[counterB]
                        ? counterA
                        : counterB;
                    int lowerCounter = state.CurrentJoltages[counterA] > state.CurrentJoltages[counterB]
                        ? counterB
                        : counterA;

                    // Find buttons that help reduce the gap (affect higher but not lower).
                    var highCounterButtonIndices = FindButtonsAffectingHigherCounterOnly(
                        state.AvailableButtons,
                        higherCounter,
                        lowerCounter);

                    if (highCounterButtonIndices.Count == 0)
                    {
                        return false;
                    }

                    // Exactly one button which affects the higher counter only, so we must use it.
                    if (highCounterButtonIndices.Count == 1)
                    {
                        int buttonIndex = highCounterButtonIndices[0];
                        var newJoltages = PressButton(state.CurrentJoltages, state.AvailableButtons[buttonIndex]);

                        nextState = new SearchState(
                            newJoltages,
                            state.PressCount + 1,
                            state.AvailableButtons);

                        return true;
                    }
                }
            }

            return false;
        }

        internal static List<int> FindButtonsAffectingHigherCounterOnly(
            IReadOnlyList<List<int>> availableButtons,
            int higherCounter,
            int lowerCounter)
        {
            ArgumentNullException.ThrowIfNull(availableButtons);

            var helpfulIndices = new List<int>();

            for (int i = 0; i < availableButtons.Count; i++)
            {
                var button = availableButtons[i];
                bool affectsHigher = button.Contains(higherCounter);
                bool affectsLower = button.Contains(lowerCounter);

                if (affectsHigher && !affectsLower)
                {
                    helpfulIndices.Add(i);
                }
            }

            return helpfulIndices;
        }

        internal static void EnqueueAllButtonOptions(Stack<SearchState> stack, SearchState currentState)
        {
            ArgumentNullException.ThrowIfNull(stack);
            ArgumentNullException.ThrowIfNull(currentState);

            // Try buttons in reverse order.
            for (int i = currentState.AvailableButtons.Count - 1; i >= 0; i--)
            {
                var button = currentState.AvailableButtons[i];
                var nextJoltages = PressButton(currentState.CurrentJoltages, button);

                // Reduce available buttons (had to add this because blowout).
                var nextAvailableButtons = currentState.AvailableButtons.Skip(i).ToList();

                stack.Push(new SearchState(
                    nextJoltages,
                    currentState.PressCount + 1,
                    nextAvailableButtons));
            }
        }

        internal static List<long> PressButton(List<long> currentJoltages, List<int> button)
        {
            ArgumentNullException.ThrowIfNull(currentJoltages);
            ArgumentNullException.ThrowIfNull(button);

            var nextJoltages = new List<long>(currentJoltages);

            foreach (var counterIndex in button)
            {
                nextJoltages[counterIndex] -= 1;
            }

            return nextJoltages;
        }

        internal static HashSet<MachineConfigurationOneSolution> CalculateMachineSolutions(HashSet<MachineConfigurationOne> machines)
        {
            ArgumentNullException.ThrowIfNull(machines, nameof(machines));

            if (machines.Count == 0)
            {
                throw new ArgumentException("Unable to calculate solution with zero machines input.", nameof(machines));
            }

            HashSet<MachineConfigurationOneSolution> solutions = [];
            foreach (var machine in machines)
            {
                var queue = new Queue<State>();
                var visited = new HashSet<int>();

                queue.Enqueue(new State { CurrentMask = 0 });
                visited.Add(0);

                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();

                    if (current.CurrentMask == machine.Mask)
                    {
                        var solution = new MachineConfigurationOneSolution
                        {
                            Steps = current.History.Count,
                            ButtonSequence = current.History
                        };

                        solutions.Add(solution);
                    }

                    foreach (var (mask, label) in machine.Buttons)
                    {
                        int nextMask = current.CurrentMask ^ mask;

                        if (visited.Add(nextMask))
                        {
                            var newHistory = new List<string>(current.History) { label };

                            queue.Enqueue(new State
                            {
                                CurrentMask = nextMask,
                                History = newHistory
                            });
                        }
                    }
                }
            }

            return solutions;
        }

        internal static HashSet<MachineConfigurationOne> GetMachines(string[] lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Length == 0)
            {
                throw new ArgumentException("Machines instructions were empty.");
            }

            HashSet<MachineConfigurationOne> machines = [];

            foreach (var row in lines)
            {
                if (!TryParseMachineConfigurationOne(row, out var machine) || machine == null)
                {
                    throw new InvalidDataException($"Row wasn't able to be parsed: '{row}'.");
                }

                machines.Add(machine);
            }

            return machines;
        }

        internal static HashSet<MachineConfigurationTwo> GetMachinesForConfigurationTwo(string[] lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Length == 0)
            {
                throw new ArgumentException("Machines instructions were empty.");
            }

            HashSet<MachineConfigurationTwo> machines = [];

            foreach (var row in lines)
            {
                if (!TryParseMachineConfigurationTwo(row, out var machine))
                {
                    throw new InvalidDataException($"Row wasn't able to be parsed: '{row}'.");
                }

                machines.Add(machine);
            }

            return machines;
        }

        internal static bool TryParseMachineConfigurationTwo(string machineData, out MachineConfigurationTwo machine)
        {
            ArgumentNullException.ThrowIfNull(machineData, nameof(machineData));

            if (string.IsNullOrWhiteSpace(machineData))
            {
                throw new ArgumentException("Provided machine data was null or empty.", nameof(machineData));
            }

            var schematicMatches = SchematicMatchRegex().Matches(machineData);
            var joltageMatch = JoltageMatchRegex().Match(machineData);

            if (schematicMatches.Count == 0)
            {
                throw new FormatException("Invalid schematic format. Expecting similar to: (0,1) (1,2) (0)");
            }

            if (!joltageMatch.Success)
            {
                throw new FormatException("Invalid Joltage format. Expecting similar to: {4,2,2,3,8}");
            }

            var targetJoltages = joltageMatch.Groups[1].Value
                .Split([','], StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse);

            var buttonDefinitions = new List<List<int>>();
            foreach (Match match in schematicMatches)
            {
                string content = match.Groups[1].Value;
                if (string.IsNullOrWhiteSpace(content)) continue;

                var indices = content.Split(',').Select(int.Parse).ToList();
                buttonDefinitions.Add(indices);
            }

            machine = new MachineConfigurationTwo([.. targetJoltages], buttonDefinitions);

            return true;
        }

        internal static bool TryParseMachineConfigurationOne(string machineData, out MachineConfigurationOne? machine)
        {
            ArgumentNullException.ThrowIfNull(machineData, nameof(machineData));

            if (string.IsNullOrWhiteSpace(machineData))
            {
                throw new ArgumentException("Provided machine data was null or empty.", nameof(machineData));
            }

            machine = null;

            var diagramMatch = DiagramMatchRegex().Match(machineData);
            var schematicMatches = SchematicMatchRegex().Matches(machineData);

            if (!diagramMatch.Success)
            {
                throw new FormatException("Invalid diagram format. Expecting similar to: [##..]");
            }

            string targetDiagram = diagramMatch.Groups[1].Value;

            int targetMask = 0;
            for (int i = 0; i < targetDiagram.Length; i++)
            {
                if (targetDiagram[i] == '#')
                {
                    targetMask |= (1 << i);
                }
            }

            var buttons = new List<(int mask, string label)>();
            foreach (Match match in schematicMatches)
            {
                string content = match.Groups[1].Value;
                if (string.IsNullOrWhiteSpace(content)) continue;

                var indices = content.Split(',').Select(int.Parse).ToArray();
                int buttonMask = 0;
                foreach (int idx in indices)
                {
                    buttonMask |= (1 << idx);
                }
                buttons.Add((buttonMask, match.Value));
            }

            machine = new MachineConfigurationOne(targetMask, buttons);

            return true;
        }

        internal class State
        {
            public int CurrentMask { get; set; }
            public List<string> History { get; set; } = [];
        }

        internal class MachineConfigurationOneSolution
        {
            public int Steps { get; set; }
            public List<string> ButtonSequence { get; set; } = [];
        }

        internal record MachineConfigurationTwo(List<int> Joltages, List<List<int>> Buttons);

        internal record MachineConfigurationOne(int Mask, List<(int mask, string label)> Buttons);

        internal sealed class SearchState(List<long> currentJoltages, int pressCount, IReadOnlyList<List<int>> availableButtons)
        {
            public List<long> CurrentJoltages { get; } = currentJoltages;
            public int PressCount { get; } = pressCount;
            public IReadOnlyList<List<int>> AvailableButtons { get; } = availableButtons;
        }

        internal enum SearchStatus
        {
            Continue,
            Complete,
            Invalid,
            Pruned
        }

        [GeneratedRegex(@"\[([#.]+)\]")]
        private static partial Regex DiagramMatchRegex();
        [GeneratedRegex(@"\((.*?)\)")]
        private static partial Regex SchematicMatchRegex();
        [GeneratedRegex(@"\{(.*?)\}")]
        private static partial Regex JoltageMatchRegex();
    }
}