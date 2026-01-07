namespace AdventOfCode.Business.Year2025
{
    internal class Day11 : IAdventDay
    {
        string _root = string.Empty;
        public void SetRoot(string path)
        {
            _root = path;
        }

        public void ExecutePart1()
        {
            var day11Input = File.ReadAllLines(Path.Combine(_root, "Day11_Input.txt"));
            var result = CalculatePathsFromYouToOut(day11Input);

            Console.WriteLine($"\r\nThere are '{result}' different paths.\r\n");
        }

        public void ExecutePart2()
        {
            var day11Input = File.ReadAllLines(Path.Combine(_root, "Day11_Input.txt"));
            var result = CalculatePathsThroughDACAndFTT(day11Input);

            Console.WriteLine($"\r\nThere are '{result}' paths which visit both DAC & FFT.\r\n");
        }

        internal static long CalculatePathsThroughDACAndFTT(string[] input)
        {
            var graph = new Dictionary<string, List<string>>();
            var reverseGraph = new Dictionary<string, List<string>>();

            // Build adjacency lists
            foreach (var line in input)
            {
                var parts = line.Split(": ", StringSplitOptions.TrimEntries);
                if (parts.Length < 2) continue;

                var source = parts[0];
                var targets = parts[1].Split(' ', StringSplitOptions.TrimEntries);

                if (!graph.ContainsKey(source))
                    graph[source] = [];

                foreach (var target in targets)
                {
                    graph[source].Add(target);

                    if (!reverseGraph.ContainsKey(target))
                        reverseGraph[target] = [];
                    reverseGraph[target].Add(source);

                    if (!graph.ContainsKey(target))
                        graph[target] = [];
                }
            }

            if (!graph.ContainsKey("svr") || !graph.ContainsKey("out") ||
                !graph.ContainsKey("dac") || !graph.ContainsKey("fft"))
                return 0;

            // Pruning optimization
            var canReachOut = GetReachableFrom("out", reverseGraph);
            var canReachDac = GetReachableFrom("dac", reverseGraph);
            var canReachFft = GetReachableFrom("fft", reverseGraph);

            var memo = new Dictionary<(string, bool, bool), long>();

            return CountPaths("svr", false, false, graph, memo, canReachOut, canReachDac, canReachFft);
        }

        internal static HashSet<string> GetReachableFrom(string target, Dictionary<string, List<string>> reverseGraph)
        {
            var reachable = new HashSet<string> { target };
            var queue = new Queue<string>();
            queue.Enqueue(target);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (reverseGraph.TryGetValue(current, out var parents))
                {
                    foreach (var parent in parents)
                    {
                        if (reachable.Add(parent))
                            queue.Enqueue(parent);
                    }
                }
            }
            return reachable;
        }

        internal static long CountPaths(
            string node,
            bool seenDac,
            bool seenFft,
            Dictionary<string, List<string>> graph,
            Dictionary<(string, bool, bool), long> memo,
            HashSet<string> canReachOut,
            HashSet<string> canReachDac,
            HashSet<string> canReachFft,
            HashSet<string>? visitedInPath = null)
        {
            visitedInPath ??= [];

            if (visitedInPath.Contains(node)) return 0; // Cycle detected, no valid path

            if (!canReachOut.Contains(node)) return 0;
            if (!seenDac && !canReachDac.Contains(node)) return 0;
            if (!seenFft && !canReachFft.Contains(node)) return 0;

            if (node == "dac") seenDac = true;
            if (node == "fft") seenFft = true;

            if (node == "out")
                return (seenDac && seenFft) ? 1 : 0;

            var state = (node, seenDac, seenFft);
            if (memo.TryGetValue(state, out var cached))
                return cached;

            visitedInPath.Add(node);

            long totalPaths = 0;
            if (graph.TryGetValue(node, out var neighbours))
            {
                foreach (var neighbour in neighbours)
                {
                    totalPaths += CountPaths(neighbour, seenDac, seenFft, graph, memo,
                        canReachOut, canReachDac, canReachFft, visitedInPath);
                }
            }

            visitedInPath.Remove(node); // backtrack
            memo[state] = totalPaths;
            return totalPaths;
        }

        internal static int CalculatePathsFromYouToOut(string[] input)
        {
            var nodeLookup = new Dictionary<string, Link>();

            foreach (var line in input)
            {
                var parts = line.Split(": ", StringSplitOptions.TrimEntries);
                if (parts.Length < 0)
                {
                    continue;
                }

                var sourceName = parts[0];
                var targetNames = parts[1].Split(' ', StringSplitOptions.TrimEntries);

                var sourceLink = GetOrCreate(nodeLookup, sourceName);

                foreach (var targetName in targetNames)
                {
                    var targetLink = GetOrCreate(nodeLookup, targetName);
                    sourceLink.Paths.Add(targetLink);
                }
            }

            if (!nodeLookup.TryGetValue("you", out var startNode))
            {
                throw new Exception("Start node 'you' not found in input.");
            }

            if (!nodeLookup.ContainsKey("out"))
            {
                return 0;
            }

            return CountAllPaths(startNode);
        }

        internal static Link GetOrCreate(Dictionary<string, Link> nodeLookup, string name)
        {
            if (!nodeLookup.TryGetValue(name, out var link))
            {
                link = new Link(name);
                nodeLookup[name] = link;
            }

            return link;
        }

        internal static int CountAllPaths(Link start)
        {
            ArgumentNullException.ThrowIfNull(start, nameof(start));

            var visited = new HashSet<Link>();
            return CountPathTerminationRecursive(start, visited);
        }

        internal static int CountPathTerminationRecursive(Link current, HashSet<Link> visited)
        {
            if (visited.Contains(current))
            {
                return 0;
            }

            if (current.Name == "out")
            {
                return 1;
            }

            visited.Add(current);

            int validPathsFound = 0;

            foreach (var nextLink in current.Paths)
            {
                validPathsFound += CountPathTerminationRecursive(nextLink, visited);
            }

            visited.Remove(current);

            return validPathsFound;
        }

        internal class Link(string name)
        {
            public string Name { get; } = name;
            public HashSet<Link> Paths { get; } = [];
        }
    }
}
