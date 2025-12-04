namespace AdventOfCode.Business.Year2025
{
    internal class Day3 : IAdventDay
    {
        public void ExecutePart1()
        {
            var day3Input = File.ReadAllText(Path.Combine("Year2025", "Resources", "Day3_Input.txt"));
            var result = SumOfProducedJoltage(day3Input);

            Console.WriteLine($"\r\nThe dial was at position 0 a total of {result} times.\r\n");
        }

        public void ExecutePart2()
        {
            throw new NotImplementedException();
        }

        internal static uint SumOfProducedJoltage(string input)
        {
            return 0;
        }
    }
}
