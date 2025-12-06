namespace AdventOfCode.Business.Year2025
{
    internal class Day5 : IAdventDay
    {
        public void ExecutePart1()
        {
            var day5Input = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Actual", "Day5_Input.txt"));
            var result = CalculateHowManyIngredientsAreFresh(day5Input);

            Console.WriteLine($"\r\nThere are {result} fresh ingredient IDs.\r\n");
        }

        public void ExecutePart2()
        {
            throw new NotImplementedException();
        }

        internal static uint CalculateHowManyIngredientsAreFresh(string[] input)
        {
            ArgumentNullException.ThrowIfNull(input);

            if (input.Length == 0)
            {
                return 0u;
            }

            var (ingredientIds, freshIngredientIds) = BuildIngredientLists(input);

            var freshCount = GetFreshIngredientCountFromAvailableIngredientIds(ingredientIds, freshIngredientIds);

            return freshCount;
        }

        internal static (List<ulong> ingredientIds, List<URange> freshIngredientIds) BuildIngredientLists(string[] input)
        {
            ArgumentNullException.ThrowIfNull(input);

            if (input.Length == 0)
            {
                throw new ArgumentException("Must contain input to build ingredient lists.");
            }

            var isRangeSection = true;
            List<ulong> ingredientIds = [];
            List<URange> freshIngredientIds = [];

            foreach (var line in input)
            {
                if (isRangeSection)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        isRangeSection = false;
                        continue;
                    }
                    freshIngredientIds.Add(GetRange(line));
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }
                    var ingredientId = ulong.Parse(line);
                    ingredientIds.Add(ingredientId);
                }
            }
            return (ingredientIds, freshIngredientIds);
        }

        internal static URange GetRange(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                throw new ArgumentException("Line cannot be null or whitespace", nameof(line));
            }

            var parts = line.Split('-', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                throw new FormatException("Line is not in the correct format");
            }
            var start = ulong.Parse(parts[0]);
            var end = ulong.Parse(parts[1]);

            return new URange(start, end);
        }

        internal static uint GetFreshIngredientCountFromAvailableIngredientIds(List<ulong> ingredientIds, List<URange> ingredientsDatabase)
        {
            ArgumentNullException.ThrowIfNull(ingredientIds);
            ArgumentNullException.ThrowIfNull(ingredientsDatabase);

            if (ingredientIds.Count == 0 || ingredientsDatabase.Count == 0)
            {
                return 0;
            }

            var freshCount = 0u;
            foreach (var ingredientId in ingredientIds.OrderBy(x => x))
            {
                if (ExistsWithinAnyRange(ingredientsDatabase, ingredientId))
                {
                    freshCount++;
                }
            }

            return freshCount;
        }

        internal static bool ExistsWithinAnyRange(List<URange> ranges, ulong number)
        {
            ArgumentNullException.ThrowIfNull(ranges);

            if (number == 0)
            {
                return false;
            }

            if (ranges.Count == 0)
            {
                return false;
            }

            foreach (var range in ranges)
            {
                if (ExistsWithinRange(range, number))
                {
                    return true;
                }
            }

            return false;
        }

        internal static bool ExistsWithinRange(URange range, ulong number)
        {
            ArgumentNullException.ThrowIfNull(range);

            if (number == 0)
            {
                return false;
            }

            return number >= range.Start && number <= range.End;
        }

        internal record URange(ulong Start, ulong End);

        internal record Ingredient(ulong Id, IngredientState IngredientState, bool? Overlaps);

        internal enum IngredientState
        {
            Fresh,
            Spoiled,
        }
    }
}
