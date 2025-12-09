namespace AdventOfCode.Business.Year2025
{
    internal class Day8 : IAdventDay
    {
        string _root = string.Empty;
        public void SetRoot(string path)
        {
            _root = path;
        }

        public void ExecutePart1()
        {
            var day8Input = File.ReadAllLines(Path.Combine(_root, "Day8_Input.txt"));
            var result = SolveCircuitMultiplication(day8Input, 1000);

            Console.WriteLine($"\r\nThe three largest circuit group counts multiplied together equals: '{result}'.\r\n");
        }

        public void ExecutePart2()
        {
            var day8Input = File.ReadAllLines(Path.Combine(_root, "Day8_Input.txt"));
            var result = SolveLastConnectionProduct(day8Input);

            Console.WriteLine($"\r\nMultiplying the last two junction boxes X Coords together equals: '{result}'.\r\n");
        }

        internal static long SolveLastConnectionProduct(string[] lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Length == 0)
            {
                return 0;
            }

            var points = RetrievePoints(lines);
            var result = CalculateLastConnectionProduct(points);

            return result;
        }

        internal static long CalculateLastConnectionProduct(List<Point3D> points)
        {
            ArgumentNullException.ThrowIfNull(points);

            if (points.Count < 2)
            {
                throw new ArgumentException("More than one point is required to create circuits.");
            }

            var edges = GetSortedEdges(points);

            var uf = new UnionFind(points.Count);
            Edge lastConnection = new(-1, -1, 0);

            foreach (var edge in edges)
            {
                if (uf.Union(edge.From, edge.To))
                {
                    lastConnection = edge;

                    if (uf.GetCircuitCount() == 1)
                    {
                        break;
                    }
                }
            }

            return points[lastConnection.From].X * points[lastConnection.To].X;
        }

        internal static long SolveCircuitMultiplication(string[] lines, uint connectionAttempts)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Length == 0)
            {
                return 0;
            }

            var points = RetrievePoints(lines);
            var result = CalculateCircuitMultiplication(points, connectionAttempts);

            return result;
        }

        internal static long CalculateCircuitMultiplication(List<Point3D> points, uint connectionAttempts)
        {
            ArgumentNullException.ThrowIfNull(points);

            if (points.Count == 0)
            {
                return 0;
            }

            var edges = GetSortedEdges(points);

            var uf = new UnionFind(points.Count);
            int connectionsAttempted = 0;

            foreach (var edge in edges)
            {
                if (connectionsAttempted >= connectionAttempts)
                {
                    break;
                }

                uf.Union(edge.From, edge.To);
                connectionsAttempted++;
            }

            var componentSizes = uf.GetCircuitSizes();
            componentSizes.Sort((a, b) => b.CompareTo(a));

            if (componentSizes.Count < 3)
            {
                throw new InvalidOperationException($"Cannot calculate result: only {componentSizes.Count} circuit(s) exist, need at least 3.");
            }

            return (long)componentSizes[0] * componentSizes[1] * componentSizes[2];
        }

        internal static List<Edge> GetSortedEdges(List<Point3D> points)
        {
            ArgumentNullException.ThrowIfNull(points);

            if (points.Count == 0)
            {
                return [];
            }

            var edges = new List<Edge>();
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
                {
                    double dist = points[i].DistanceTo(points[j]);
                    edges.Add(new Edge(i, j, dist));
                }
            }

            edges.Sort((a, b) => a.Distance.CompareTo(b.Distance));

            return edges;
        }

        internal static List<Point3D> RetrievePoints(string[] lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Length == 0)
            {
                throw new ArgumentException("Input needs to have at least one row of x,y,z coords to retrieve Point3Ds list.");
            }

            var points = new List<Point3D>();
            foreach (var line in lines)
            {
                var stringPoints = line.Split(',', StringSplitOptions.TrimEntries);
                if (TryGetPoint(stringPoints, out var point))
                {
                    points.Add(point);
                }
                else
                {
                    throw new FormatException($"Unable to parse line: {line}");
                }
            }

            return points;
        }

        internal static bool TryGetPoint(string[] coordinates, out Point3D point)
        {
            ArgumentNullException.ThrowIfNull(coordinates);

            point = default;

            if (coordinates.Length != 3)
            {
                return false;
            }

            if (!uint.TryParse(coordinates[0], out uint x)
                || !uint.TryParse(coordinates[1], out uint y)
                || !uint.TryParse(coordinates[2], out uint z))
            {
                return false;
            }

            point = new Point3D(x, y, z);
            return true;
        }

        internal struct Point3D(uint x, uint y, uint z)
        {
            public long X = x, Y = y, Z = z;

            public readonly double DistanceTo(Point3D other)
            {
                long dx = X - other.X;
                long dy = Y - other.Y;
                long dz = Z - other.Z;
                return Math.Sqrt(dx * dx + dy * dy + dz * dz);
            }

            public override readonly string ToString() => $"({X},{Y},{Z})";
        }

        internal struct Edge(int from, int to, double distance)
        {
            public int From = from, To = to;
            public double Distance = distance;

            public override readonly string ToString() => $"(From: {From}, To: {To}, Distance: {Distance})";
        }

        private class UnionFind
        {
            private readonly int[] parent;
            private readonly int[] size;

            public UnionFind(int n)
            {
                parent = new int[n];
                size = new int[n];
                for (int i = 0; i < n; i++)
                {
                    parent[i] = i;
                    size[i] = 1;
                }
            }

            public int Find(int x)
            {
                if (parent[x] != x)
                {
                    parent[x] = Find(parent[x]); // Path shortcut.
                }
                return parent[x];
            }

            public bool Union(int x, int y)
            {
                int rootX = Find(x);
                int rootY = Find(y);

                if (rootX == rootY)
                {
                    return false; // Already in the same circuit.
                }

                // Join smaller circuit to the larger circuit.
                if (size[rootX] < size[rootY])
                {
                    parent[rootX] = rootY;
                    size[rootY] += size[rootX];
                }
                else
                {
                    parent[rootY] = rootX;
                    size[rootX] += size[rootY];
                }

                return true;
            }

            public List<int> GetCircuitSizes()
            {
                var sizeMap = new Dictionary<int, int>();
                for (int i = 0; i < parent.Length; i++)
                {
                    int root = Find(i);
                    if (!sizeMap.ContainsKey(root))
                    {
                        sizeMap[root] = size[root];
                    }
                }
                return [.. sizeMap.Values];
            }

            public int GetCircuitCount()
            {
                var roots = new HashSet<int>();
                for (int i = 0; i < parent.Length; i++)
                {
                    roots.Add(Find(i));
                }
                return roots.Count;
            }
        }
    }
}
