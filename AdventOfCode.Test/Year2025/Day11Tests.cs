using static AdventOfCode.Business.Year2025.Day11;

namespace AdventOfCode.Test.Year2025
{
    internal class Day11Tests
    {
        [Test]
        public void CalculatePathsFromYouToOut_ExampleInput_ReturnsExpectedResult()
        {
            // Act
            var input = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Example", "Day11_Input.txt"));

            List<string> partOne = [];

            foreach (var part in input)
            {
                if (part == "---")
                {
                    break;
                }

                partOne.Add(part);
            }

            // Act
            var result = CalculatePathsFromYouToOut([.. partOne]);

            // Assert
            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public void CalculatePathsThroughDACAndFTT_ExampleInput_ReturnsExpectedResult()
        {
            // Act
            var input = File.ReadAllLines(Path.Combine("Year2025", "Resources", "Example", "Day11_Input.txt"));

            List<string> partTwo = [];
            bool seenSplit = false;
            foreach (var part in input)
            {
                if (seenSplit)
                {
                    partTwo.Add(part);
                    continue;
                }

                if (part == "---")
                {
                    seenSplit = true;
                }
            }

            // Act
            var result = CalculatePathsThroughDACAndFTT([.. partTwo]);

            // Assert
            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void CalculatePathsThroughDACAndFTT_MissingSvrNode_ReturnsZero()
        {
            var input = new[] { "dac: out", "fft: out" };

            var result = CalculatePathsThroughDACAndFTT(input);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CalculatePathsThroughDACAndFTT_MissingOutNode_ReturnsZero()
        {
            var input = new[] { "svr: dac", "dac: fft" };

            var result = CalculatePathsThroughDACAndFTT(input);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CalculatePathsThroughDACAndFTT_MissingDacNode_ReturnsZero()
        {
            var input = new[] { "svr: fft", "fft: out" };

            var result = CalculatePathsThroughDACAndFTT(input);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CalculatePathsThroughDACAndFTT_MissingFftNode_ReturnsZero()
        {
            var input = new[] { "svr: dac", "dac: out" };

            var result = CalculatePathsThroughDACAndFTT(input);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CalculatePathsThroughDACAndFTT_SimplePathThroughBoth_ReturnsOne()
        {
            var input = new[]
            {
                "svr: dac",
                "dac: fft",
                "fft: out"
            };

            var result = CalculatePathsThroughDACAndFTT(input);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void CalculatePathsThroughDACAndFTT_PathMissesDac_ReturnsZero()
        {
            var input = new[]
            {
                "svr: fft",
                "fft: out",
                "dac: out"
            };

            var result = CalculatePathsThroughDACAndFTT(input);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CalculatePathsThroughDACAndFTT_PathMissesFft_ReturnsZero()
        {
            var input = new[]
            {
                "svr: dac",
                "dac: out",
                "fft: out"
            };

            var result = CalculatePathsThroughDACAndFTT(input);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CalculatePathsThroughDACAndFTT_MultipleValidPaths_ReturnsCount()
        {
            var input = new[]
            {
                "svr: a b",
                "a: dac",
                "b: fft",
                "dac: fft",
                "fft: dac",
                "dac: out",
                "fft: out"
            };

            var result = CalculatePathsThroughDACAndFTT(input);

            Assert.That(result, Is.GreaterThan(0));
        }

        [Test]
        public void GetReachableFrom_TargetNotInGraph_ReturnsOnlyTarget()
        {
            var reverseGraph = new Dictionary<string, List<string>>();

            var result = GetReachableFrom("a", reverseGraph);

            Assert.That(result, Is.EquivalentTo(["a"]));
        }

        [Test]
        public void GetReachableFrom_TargetWithNoParents_ReturnsOnlyTarget()
        {
            var reverseGraph = new Dictionary<string, List<string>>
            {
                ["b"] = ["a"]
            };

            var result = GetReachableFrom("a", reverseGraph);

            Assert.That(result, Is.EquivalentTo(["a"]));
        }

        [Test]
        public void GetReachableFrom_SingleParent_ReturnsTargetAndParent()
        {
            var reverseGraph = new Dictionary<string, List<string>>
            {
                ["a"] = ["b"]
            };

            var result = GetReachableFrom("a", reverseGraph);

            Assert.That(result, Is.EquivalentTo(["a", "b"]));
        }

        [Test]
        public void GetReachableFrom_MultipleParents_ReturnsAllReachable()
        {
            var reverseGraph = new Dictionary<string, List<string>>
            {
                ["a"] = ["b", "c"],
                ["b"] = ["d"]
            };

            var result = GetReachableFrom("a", reverseGraph);

            Assert.That(result, Is.EquivalentTo(["a", "b", "c", "d"]));
        }

        [Test]
        public void GetReachableFrom_CircularReferences_HandlesWithoutInfiniteLoop()
        {
            var reverseGraph = new Dictionary<string, List<string>>
            {
                ["a"] = ["b"],
                ["b"] = ["a", "c"]
            };

            var result = GetReachableFrom("a", reverseGraph);

            Assert.That(result, Is.EquivalentTo(["a", "b", "c"]));
        }

        [Test]
        public void CountPaths_NodeCannotReachOut_ReturnsZero()
        {
            var graph = new Dictionary<string, List<string>> { ["a"] = ["b"] };
            var memo = new Dictionary<(string, bool, bool), long>();
            var canReachOut = new HashSet<string>();
            var canReachDac = new HashSet<string> { "a" };
            var canReachFft = new HashSet<string> { "a" };

            var result = CountPaths("a", false, false, graph, memo, canReachOut, canReachDac, canReachFft);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CountPaths_NodeCannotReachDac_ReturnsZero()
        {
            var graph = new Dictionary<string, List<string>> { ["a"] = ["out"] };
            var memo = new Dictionary<(string, bool, bool), long>();
            var canReachOut = new HashSet<string> { "a", "out" };
            var canReachDac = new HashSet<string>();
            var canReachFft = new HashSet<string> { "a" };

            var result = CountPaths("a", false, false, graph, memo, canReachOut, canReachDac, canReachFft);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CountPaths_NodeCannotReachFft_ReturnsZero()
        {
            var graph = new Dictionary<string, List<string>> { ["a"] = ["out"] };
            var memo = new Dictionary<(string, bool, bool), long>();
            var canReachOut = new HashSet<string> { "a", "out" };
            var canReachDac = new HashSet<string> { "a" };
            var canReachFft = new HashSet<string>();

            var result = CountPaths("a", false, false, graph, memo, canReachOut, canReachDac, canReachFft);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CountPaths_ReachesOutWithBothDacAndFft_ReturnsOne()
        {
            var graph = new Dictionary<string, List<string>>();
            var memo = new Dictionary<(string, bool, bool), long>();
            var canReachOut = new HashSet<string> { "out" };
            var canReachDac = new HashSet<string> { "out" };
            var canReachFft = new HashSet<string> { "out" };

            var result = CountPaths("out", true, true, graph, memo, canReachOut, canReachDac, canReachFft);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void CountPaths_ReachesOutWithoutDac_ReturnsZero()
        {
            var graph = new Dictionary<string, List<string>>();
            var memo = new Dictionary<(string, bool, bool), long>();
            var canReachOut = new HashSet<string> { "out" };
            var canReachDac = new HashSet<string> { "out" };
            var canReachFft = new HashSet<string> { "out" };

            var result = CountPaths("out", false, true, graph, memo, canReachOut, canReachDac, canReachFft);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CountPaths_ReachesOutWithoutFft_ReturnsZero()
        {
            var graph = new Dictionary<string, List<string>>();
            var memo = new Dictionary<(string, bool, bool), long>();
            var canReachOut = new HashSet<string> { "out" };
            var canReachDac = new HashSet<string> { "out" };
            var canReachFft = new HashSet<string> { "out" };

            var result = CountPaths("out", true, false, graph, memo, canReachOut, canReachDac, canReachFft);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CountPaths_SimplePathThroughDacAndFft_ReturnsOne()
        {
            var graph = new Dictionary<string, List<string>>
            {
                ["a"] = ["dac"],
                ["dac"] = ["fft"],
                ["fft"] = ["out"]
            };
            var memo = new Dictionary<(string, bool, bool), long>();
            var canReachOut = new HashSet<string> { "a", "dac", "fft", "out" };
            var canReachDac = new HashSet<string> { "a", "dac" };
            var canReachFft = new HashSet<string> { "a", "dac", "fft" };

            var result = CountPaths("a", false, false, graph, memo, canReachOut, canReachDac, canReachFft);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void CountPaths_UsesMemoization_ReturnsCachedValue()
        {
            var graph = new Dictionary<string, List<string>> { ["a"] = ["out"] };
            var memo = new Dictionary<(string, bool, bool), long> { [("a", true, true)] = 42 };
            var canReachOut = new HashSet<string> { "a", "out" };
            var canReachDac = new HashSet<string> { "a" };
            var canReachFft = new HashSet<string> { "a" };

            var result = CountPaths("a", true, true, graph, memo, canReachOut, canReachDac, canReachFft);

            Assert.That(result, Is.EqualTo(42));
        }

        [Test]
        public void CalculatePathsFromYouToOut_NoYouNode_ThrowsException()
        {
            var input = new[] { "aaa: bbb", "bbb: out" };

            Assert.Throws<Exception>(() => CalculatePathsFromYouToOut(input));
        }

        [Test]
        public void CalculatePathsFromYouToOut_NoOutNode_ReturnsZero()
        {
            var input = new[] { "you: aaa", "aaa: bbb" };

            var result = CalculatePathsFromYouToOut(input);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CalculatePathsFromYouToOut_YouIsOut_ReturnsOne()
        {
            var input = new[] { "you: out" };

            var result = CalculatePathsFromYouToOut(input);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void CalculatePathsFromYouToOut_SinglePath_ReturnsOne()
        {
            var input = new[] { "you: aaa", "aaa: out" };

            var result = CalculatePathsFromYouToOut(input);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void CalculatePathsFromYouToOut_MultiplePaths_ReturnsCount()
        {
            var input = new[]
            {
                "you: bbb ccc",
                "bbb: out",
                "ccc: out"
            };

            var result = CalculatePathsFromYouToOut(input);

            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void CountAllPaths_NoPathsToOut_ReturnsZero()
        {
            var start = new Link("A");

            var result = CountAllPaths(start);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CountAllPaths_StartIsOut_ReturnsOne()
        {
            var start = new Link("out");

            var result = CountAllPaths(start);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void CountAllPaths_SinglePathToOut_ReturnsOne()
        {
            var start = new Link("A");
            var end = new Link("out");
            start.Paths.Add(end);

            var result = CountAllPaths(start);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void CountAllPaths_MultiplePathsToOut_ReturnsCount()
        {
            var start = new Link("A");
            var middle1 = new Link("B");
            var middle2 = new Link("C");
            var end = new Link("out");
            start.Paths.Add(middle1);
            start.Paths.Add(middle2);
            middle1.Paths.Add(end);
            middle2.Paths.Add(end);

            var result = CountAllPaths(start);

            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void CountAllPaths_LinkStartIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => CountAllPaths(null!));
        }

        [Test]
        public void CountPathTerminationRecursive_AlreadyVisited_ReturnsZero()
        {
            var link = new Link("A");
            var visited = new HashSet<Link> { link };

            var result = CountPathTerminationRecursive(link, visited);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CountPathTerminationRecursive_CurrentIsOut_ReturnsOne()
        {
            var link = new Link("out");
            var visited = new HashSet<Link>();

            var result = CountPathTerminationRecursive(link, visited);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void CountPathTerminationRecursive_NoPaths_ReturnsZero()
        {
            var link = new Link("A");
            var visited = new HashSet<Link>();

            var result = CountPathTerminationRecursive(link, visited);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CountPathTerminationRecursive_SinglePathToOut_ReturnsOne()
        {
            var start = new Link("A");
            var end = new Link("out");
            start.Paths.Add(end);
            var visited = new HashSet<Link>();

            var result = CountPathTerminationRecursive(start, visited);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void CountPathTerminationRecursive_MultiplePathsToOut_ReturnsCount()
        {
            var start = new Link("A");
            var middle1 = new Link("B");
            var middle2 = new Link("C");
            var end = new Link("out");
            start.Paths.Add(middle1);
            start.Paths.Add(middle2);
            middle1.Paths.Add(end);
            middle2.Paths.Add(end);
            var visited = new HashSet<Link>();

            var result = CountPathTerminationRecursive(start, visited);

            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void GetOrCreate_NewLink_CreatesAndReturnsLink()
        {
            var lookup = new Dictionary<string, Link>();

            var result = GetOrCreate(lookup, "A");

            Assert.Multiple(() =>
            {
                Assert.That(result.Name, Is.EqualTo("A"));
                Assert.That(lookup.ContainsKey("A"), Is.True);
            });
        }

        [Test]
        public void GetOrCreate_ExistingLink_ReturnsSameInstance()
        {
            var lookup = new Dictionary<string, Link>();
            var existing = new Link("A");
            lookup["A"] = existing;

            var result = GetOrCreate(lookup, "A");

            Assert.That(result, Is.SameAs(existing));
        }

        [Test]
        public void GetOrCreate_NewLink_AddsToLookup()
        {
            var lookup = new Dictionary<string, Link>();

            GetOrCreate(lookup, "A");

            Assert.That(lookup, Has.Count.EqualTo(1));
            Assert.That(lookup["A"], Is.SameAs(lookup!["A"]));
        }
    }
}
