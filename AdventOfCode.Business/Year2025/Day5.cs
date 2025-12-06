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

            Dictionary<ushort, Ingredient> ingredientsDatabase = BuildFreshIngredientDatabase(freshIngredientIds);

            var freshCount = GetFreshIngredientCountFromAvailableIngredientIds(ingredientIds, ingredientsDatabase);

            return freshCount;
        }

        internal static (List<ushort> ingredientIds, List<ushort> freshIngredientIds) BuildIngredientLists(string[] input)
        {
            ArgumentNullException.ThrowIfNull(input);

            if (input.Length == 0)
            {
                throw new ArgumentException("Must contain input to build ingredient lists.");
            }

            var isRangeSection = true;
            List<ushort> ingredientIds = [];
            List<ushort> freshIngredientIds = [];

            foreach (var line in input)
            {
                if (isRangeSection)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        isRangeSection = false;
                        continue;
                    }
                    var range = GetRange(line);
                    for (ushort i = range.Start; i <= range.End; i++)
                    {
                        freshIngredientIds.Add(i);
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }
                    var ingredientId = ushort.Parse(line);
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
            var start = ushort.Parse(parts[0]);
            var end = ushort.Parse(parts[1]);

            return new URange(start, end);
        }

        internal static Dictionary<ushort, Ingredient> BuildFreshIngredientDatabase(List<ushort> freshIngredientIds)
        {
            ArgumentNullException.ThrowIfNull(freshIngredientIds, nameof(freshIngredientIds));

            if (freshIngredientIds.Count == 0)
            {
                return [];
            }

            Dictionary<ushort, Ingredient> ingredientsDatabase = [];
            foreach (var fresh in freshIngredientIds.OrderBy(x => x))
            {
                if (ingredientsDatabase.ContainsKey(fresh))
                {
                    ingredientsDatabase[fresh] = new Ingredient(fresh, IngredientState.Fresh, true);
                    continue;
                }

                ingredientsDatabase[fresh] = new Ingredient(fresh, IngredientState.Fresh, false);
            }

            return ingredientsDatabase;
        }

        internal static uint GetFreshIngredientCountFromAvailableIngredientIds(List<ushort> ingredientIds, Dictionary<ushort, Ingredient> ingredientsDatabase)
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
                if (ingredientsDatabase.ContainsKey(ingredientId))
                {
                    freshCount++;
                }
            }

            return freshCount;
        }

        internal record URange(ushort Start, ushort End);

        internal record Ingredient(uint Id, IngredientState IngredientState, bool? Overlaps);

        internal enum IngredientState
        {
            Fresh,
            Spoiled,
        }
    }
}
