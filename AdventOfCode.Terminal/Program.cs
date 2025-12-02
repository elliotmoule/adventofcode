using AdventOfCode.Business;

namespace AdventOfCode.Terminal
{
    internal class Program
    {
        public static string NewLine = Environment.NewLine;

        static void Main(string[] args)
        {
            var availableYears = new Dictionary<string, List<string>>
            {
                { "Exit", [] },
                { "2025", [ "Day 1", "Day 2" ] },
            };

            MainCore(availableYears);
        }

        static void MainCore(Dictionary<string, List<string>> availableYears)
        {
            while (true)
            {
                Output.WriteMultiNewLine("---- Advent of Code ----");

                var (selectedYear, availableDays) = SelectYear(availableYears);

                if (selectedYear == "0")
                {
                    return;
                }

                if (!availableDays.Contains("Exit"))
                {
                    availableDays.Insert(0, "Exit");
                }

                var selectedDay = SelectDay(availableDays);

                if (selectedDay == 0)
                {
                    return;
                }

                var selectedPart = SelectPart();

                Output.WriteImportant($"You selected Year: {selectedYear}, Day: {selectedDay}, Part: {selectedPart}");

                if (!Output.QuestionYesNo("Is this correct?"))
                {
                    continue;
                }

                var adventDay = GetAdventDay(selectedYear, selectedDay);

                Action? execute;

                if (selectedPart == 1)
                {
                    execute = adventDay.ExecutePart1;
                }
                else
                {
                    execute = adventDay.ExecutePart2;
                }

                Output.SetColorTemporarily(ConsoleColor.Green, () => execute.Invoke());

                Output.WriteImportant("Execution complete.");
                if (!Output.QuestionYesNo("Would you like to run another?"))
                {
                    break;
                }
            }
        }

        static KeyValuePair<string, List<string>> SelectYear(Dictionary<string, List<string>> availableYears)
        {
            while (true)
            {
                var optionsDictionary = new Dictionary<int, string>();
                uint counter = 0;
                var message = $"Select Year. Available options:{NewLine}";

                foreach (var year in availableYears.Keys.OrderByDescending(x => x))
                {
                    counter++;
                    optionsDictionary.Add((int)counter, year);
                    message += $"{NewLine}{counter}) {year}";
                }

                message = $"{NewLine}{message}{NewLine}{NewLine}Enter year selection";
                var selectedYearNumber = Output.QuestionNumberInput(message, 1, counter);

                if (optionsDictionary.TryGetValue((int)selectedYearNumber, out var selectedYear))
                {
                    if (selectedYear == "Exit")
                    {
                        return new KeyValuePair<string, List<string>>("0", []);
                    }
                    return new KeyValuePair<string, List<string>>(selectedYear, availableYears[selectedYear]);
                }
            }
        }

        static int SelectDay(List<string> availableDays)
        {
            while (true)
            {
                var optionsDictionary = new Dictionary<int, string>();
                uint counter = 0;
                var message = $"{NewLine}Select Day. Available options:{NewLine}";
                foreach (var day in availableDays)
                {
                    counter++;
                    optionsDictionary.Add((int)counter, day);
                    message += $"{NewLine}{counter}) {day}";
                }

                message += $"{NewLine}{NewLine}Enter day selection";
                var selectDayNumber = Output.QuestionNumberInput(message, 1, counter);

                if (optionsDictionary.TryGetValue((int)selectDayNumber, out var selectedDay))
                {
                    if (selectedDay == "Exit")
                    {
                        return 0;
                    }
                    return int.Parse(selectedDay.Split(' ')[1]);
                }
            }
        }

        static uint SelectPart()
        {
            while (true)
            {
                if (Output.QuestionNumberInput("Select Part (1 or 2)", 1, 2) is uint selectedPart && (selectedPart == 1 || selectedPart == 2))
                {
                    return selectedPart;
                }
                Output.WriteError("Invalid part selected.");
            }
        }

        static IAdventDay GetAdventDay(string year, int day)
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "AdventOfCode.Business") ?? throw new InvalidOperationException("AdventOfCode Business Assembly not found.");

            var typeName = $"AdventOfCode.Business.Year{year}.Day{day}";
            var type = assembly.GetType(typeName);

            return type == null
                ? throw new InvalidOperationException($"Type {typeName} not found in assembly {assembly.FullName}.")
                : (IAdventDay)Activator.CreateInstance(type)!;
        }
    }
}
