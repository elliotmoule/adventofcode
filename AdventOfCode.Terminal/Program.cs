using AdventOfCode.Business;
using System.Reflection;

namespace AdventOfCode.Terminal
{
    internal class Program
    {
        public static string NewLine = Environment.NewLine;
        static string _root = string.Empty;

        static void Main(string[] args)
        {
            var availableYears = new Dictionary<string, List<string>>
            {
                { "Exit", [] },
                { "2025", [] },
            };

            var dayClasses = GetAdventDayClasses();
            availableYears["2025"] = [.. dayClasses
                .OrderBy(d => d.Number)
                .Select(s => s.ClassName)];

            MainCore(args, availableYears);
        }

        static void MainCore(string[] args, Dictionary<string, List<string>> availableYears)
        {
            if (args == null || availableYears == null || availableYears.Count == 0)
            {
                return;
            }

            Console.ForegroundColor = ConsoleColor.White;
            while (true)
            {
                Output.SetColorTemporarily(ConsoleColor.Cyan, () =>
                {
                    Output.WriteMultiNewLine("---- Advent of Code ----");
                });

                if (args.Length > 0)
                {
                    var possibleRoot = args[0];
                    Output.WriteImportant($"Given Root: '{possibleRoot}'");
                    if (Directory.Exists(possibleRoot))
                    {
                        var files = Directory.GetFiles(possibleRoot, "Day*_Input.txt");
                        if (files.Length == 0)
                        {
                            Output.WriteError($"No puzzle input files found in provided root: '{possibleRoot}'.\r\n\r\nInput files must be in the format: 'Day*_Input.txt' (where '*' is an available number).");
                            return;
                        }

                        Output.WriteImportant($"Setting directory root to: '{possibleRoot}'");
                        _root = possibleRoot;
                    }
                    else
                    {
                        Output.WriteError($"Provided Root does not exist: '{possibleRoot}'\r\nExitting.");
                        return;
                    }
                }

                var (selectedYear, availableDays) = SelectYear(availableYears);

                if (selectedYear == "0")
                {
                    return;
                }

                SetRoot(selectedYear);

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

                adventDay.SetRoot(_root);

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

        static void SetRoot(string selectedYear)
        {
            if (string.IsNullOrWhiteSpace(selectedYear))
            {
                return;
            }

            if (!Directory.Exists(_root))
            {
                _root = Path.Combine($"Year{selectedYear}", "Resources", "Actual");
            }
        }

        static KeyValuePair<string, List<string>> SelectYear(Dictionary<string, List<string>> availableYears)
        {
            if (availableYears == null || availableYears.Count == 0)
            {
                return new KeyValuePair<string, List<string>>(string.Empty, []);
            }

            while (true)
            {
                var optionsDictionary = new Dictionary<int, string>();
                uint counter = 1;
                var message = $"Select Year. Available options:{NewLine}";

                foreach (var year in availableYears.Keys.OrderByDescending(x => x))
                {
                    optionsDictionary.Add((int)counter, year);
                    message += $"{NewLine}{counter}) {year}";
                    counter++;
                }

                message = $"{NewLine}{message}{NewLine}{NewLine}Enter year selection";
                var selectedYearNumber = Output.QuestionNumberInput(message, 1, counter - 1);

                if (selectedYearNumber == 1)
                {
                    return new KeyValuePair<string, List<string>>("0", []);
                }

                if (optionsDictionary.TryGetValue((int)selectedYearNumber, out var selectedYear))
                {
                    return new KeyValuePair<string, List<string>>(selectedYear, availableYears[selectedYear]);
                }
            }
        }

        static uint SelectDay(List<string> availableDays)
        {
            if (availableDays == null || availableDays.Count == 0)
            {
                return 0;
            }

            while (true)
            {
                var optionsDictionary = new Dictionary<int, string>();
                uint counter = 1;
                int counterPaddingOffset = (availableDays.Count - 1).ToString().Length;
                var message = $"{NewLine}Select Day. Available options:{NewLine}";
                foreach (var day in availableDays)
                {
                    optionsDictionary.Add((int)counter, day);
                    message += $"{NewLine}{counter.ToString().PadLeft(counterPaddingOffset, '0')}) {day}";
                    counter++;
                }

                message += $"{NewLine}{NewLine}Enter day selection";
                var selectDayNumber = Output.QuestionNumberInput(message, 1, counter - 1);

                if (selectDayNumber == 1)
                {
                    return 0;
                }

                if (optionsDictionary.TryGetValue((int)selectDayNumber, out var selectedDay))
                {
                    return uint.Parse(selectedDay.Split(' ')[1]);
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

        static IAdventDay GetAdventDay(string year, uint day)
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "AdventOfCode.Business") ?? throw new InvalidOperationException("AdventOfCode Business Assembly not found.");

            var typeName = $"AdventOfCode.Business.Year{year}.Day{day}";
            var type = assembly.GetType(typeName);

            return type == null
                ? throw new InvalidOperationException($"Type {typeName} not found in assembly {assembly.FullName}.")
                : (IAdventDay)Activator.CreateInstance(type)!;
        }

        internal static IEnumerable<AdventDayName> GetAdventDayClasses()
        {
            var targetNamespace = "AdventOfCode.Business.Year2025";
            var interfaceType = typeof(IAdventDay);

            // Load the referenced assembly by its simple name
            var businessAssembly = Assembly.Load("AdventOfCode.Business");

            var fileNames = businessAssembly
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    t.Namespace == targetNamespace &&
                    interfaceType.IsAssignableFrom(t))
                .Select(type => type.Name);

            return GetAdventDayNames([.. fileNames]);
        }

        internal static IEnumerable<AdventDayName> GetAdventDayNames(List<string> adventDays)
        {
            if (adventDays == null || adventDays.Count == 0)
            {
                return [];
            }

            List<AdventDayName> dayNames = [];
            var dayCount = adventDays.Count.ToString().Length;

            foreach (var day in adventDays)
            {
                var num = day.Replace("Day", string.Empty);
                if (!uint.TryParse(num, out var dayNumber))
                {
                    throw new FormatException($"Class Name in unexpected format: {day}");
                }
                dayNames.Add(new AdventDayName(dayNumber, $"Day {dayNumber.ToString().PadLeft(dayCount, '0')}"));
            }

            return dayNames;
        }

        internal record AdventDayName(uint Number, string ClassName);
    }
}
