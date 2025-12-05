namespace AdventOfCode.Business.Year2025
{
    internal class Day5 : IAdventDay
    {
        public void ExecutePart1()
        {
            throw new NotImplementedException();
        }

        public void ExecutePart2()
        {
            throw new NotImplementedException();
        }

        internal Dictionary<uint, Ingredient> BuildIngredientDatabase(string database)
        {
            return [];
        }

        internal record Ingredient(IngredientState State, bool Overlaps);

        internal enum IngredientState
        {
            Fresh,
            Spoiled,
        }
    }
}
