using System.Text;

namespace AdventOfCode.Business.Year2025
{
    internal class Day9 : IAdventDay
    {
        string _root = string.Empty;
        public void SetRoot(string path)
        {
            _root = path;
        }

        public void ExecutePart1()
        {
            var day9Input = File.ReadAllLines(Path.Combine(_root, "Day9_Input.txt"));
            var result = CalculateLargestRectangle(day9Input);

            Console.WriteLine($"\r\nThe largest rectangle which can be made is: '{result}'.\r\n");
        }

        public void ExecutePart2()
        {
            var day9Input = File.ReadAllLines(Path.Combine(_root, "Day9_Input.txt"));
            var result = CalculateLargestRectangleAreaWithinPolyon(day9Input);

            Console.WriteLine($"\r\nThe largest area of any rectangle I can make using only red and green tiles is: '{result}'.\r\n");
        }

        internal static ulong CalculateLargestRectangleAreaWithinPolyon(string[] input)
        {
            ArgumentNullException.ThrowIfNull(input);

            if (input.Length == 0)
            {
                return 0;
            }

            var vertices = BuildVerticesFromCoords(input);
            var rectangles = CreateRectangles(vertices);
            var polygon = new Polygon(vertices);

            return FindLargestContainedRectangle(polygon, rectangles, out var largestRectangle) ? largestRectangle.Area : 0;
        }

        internal static bool FindLargestContainedRectangle(Polygon polygon, HashSet<Rectangle> rectangles, out Rectangle largestRectangle)
        {
            ArgumentNullException.ThrowIfNull(polygon);
            ArgumentNullException.ThrowIfNull(rectangles);

            Rectangle @default = new(new(0, 0), 0, 0);
            largestRectangle = @default;

            if (polygon.Vertices == null || polygon.Vertices.Length == 0)
            {
                throw new ArgumentException("Provided polygon had no vertices.");
            }

            if (rectangles.Count == 0)
            {
                throw new ArgumentException("Provided rectangles collection was empty.");
            }

            ulong maxArea = 0;

            foreach (var rect in rectangles)
            {
                if (!polygon.ContainsRectangle(rect)
                    || rect.Area <= maxArea)
                {
                    continue;
                }

                maxArea = rect.Area;
                largestRectangle = rect;
            }

            return largestRectangle != @default;
        }

        internal static ulong CalculateLargestRectangle(string[] input)
        {
            ArgumentNullException.ThrowIfNull(input);

            if (input.Length == 0)
            {
                return 0;
            }

            var points = BuildVerticesFromCoords(input);
            var rectangles = CreateRectangles(points);

            if (rectangles == null || rectangles.Count == 0)
            {
                throw new Exception("Could not create any rectangles from given input.");
            }

            var biggest = rectangles.OrderByDescending(x => x.Area).FirstOrDefault();
            return biggest!.Area;
        }

        internal static HashSet<Rectangle> CreateRectangles(Vertex[] vertices)
        {
            ArgumentNullException.ThrowIfNull(vertices);

            if (vertices.Length == 0)
            {
                throw new ArgumentException("Input cannot be empty.");
            }

            var rectangles = new HashSet<Rectangle>();

            var observedRectangles = new HashSet<(ulong minX, ulong minY, ulong maxX, ulong maxY)>();

            for (int i = 0; i < vertices.Length; i++)
            {
                var currentPoint = vertices[i];
                for (int j = i + 1; j < vertices.Length; j++)
                {
                    var otherPoint = vertices[j];

                    ulong minX = Math.Min(currentPoint.X, otherPoint.X);
                    ulong maxX = Math.Max(currentPoint.X, otherPoint.X);
                    ulong minY = Math.Min(currentPoint.Y, otherPoint.Y);
                    ulong maxY = Math.Max(currentPoint.Y, otherPoint.Y);

                    var rectKey = (minX, minY, maxX, maxY);

                    if (observedRectangles.Add(rectKey))
                    {
                        rectangles.Add(new Rectangle(new Vertex(minX, minY), maxX, maxY));
                    }
                }
            }

            return rectangles;
        }

        internal static Vertex[] BuildVerticesFromCoords(string[] input)
        {
            ArgumentNullException.ThrowIfNull(input);

            if (input.Length == 0)
            {
                return [];
            }

            var points = new Vertex[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                var row = input[i];
                if (!TryParseCoord(row, out var point))
                {
                    throw new InvalidOperationException($"Failed to parse coord: '{row}'");
                }

                points[i] = point;
            }

            return points;
        }

        internal static bool TryParseCoord(string coord, out Vertex vertex)
        {
            ArgumentNullException.ThrowIfNull(coord);

            vertex = default;

            if (string.IsNullOrWhiteSpace(coord))
            {
                throw new ArgumentException("Provided coordinate was empty.");
            }

            var split = coord.Split(',');

            if (split.Length != 2)
            {
                throw new FormatException($"Provided coordinate '{coord}' did not match the expected 'x,y' format.");
            }

            if (!ulong.TryParse(split[0], out var x)
                || !ulong.TryParse(split[1], out var y))
            {
                throw new InvalidOperationException($"Provided coordinate '{coord}' could not be parsed as am unsigned long.");
            }

            vertex = new Vertex(x, y);

            return true;
        }

        internal readonly struct Vertex(ulong x, ulong y)
        {
            public readonly ulong X = x, Y = y;

            public override string ToString()
            {
                return $"({X},{Y})";
            }
        }

        internal class Rectangle
        {
            public Vertex Vertex { get; private set; }
            public ulong Width { get; private set; }
            public ulong Height { get; private set; }
            public ulong Area { get; private set; }

            public Rectangle(Vertex vertex, ulong width, ulong height)
            {
                Vertex = vertex;
                Width = width;
                Height = height;

                if (Vertex.X == Width && Vertex.Y == Height)
                {
                    Area = 0;
                    return;
                }

                var correctedWidth = Math.Abs((long)Vertex.X - (long)Width) + 1;    // We're offset by the grid (0 based), so adding 1 to both.
                var correctedHeight = Math.Abs((long)Vertex.Y - (long)Height) + 1;
                Area = (ulong)(correctedWidth * correctedHeight);
            }

            public override bool Equals(object? obj)
            {
                if (obj is not Rectangle other) return false;
                return Vertex.X == other.Vertex.X
                    && Vertex.Y == other.Vertex.Y
                    && Width == other.Width
                    && Height == other.Height;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Vertex.X, Vertex.Y, Width, Height);
            }

            public override string ToString()
            {
                return $"(X:{Vertex.X},Y:{Vertex.Y},W:{Width},H:{Height})";
            }
        }

        internal class Polygon
        {
            public Vertex[] Vertices { get; private set; }

            ulong _minX, _maxX, _minY, _maxY;

            public Polygon(Vertex[] orderedVertices)
            {
                ArgumentNullException.ThrowIfNull(orderedVertices);

                if (orderedVertices.Length < 3)
                {
                    throw new ArgumentException("A polygon must have at least 3 vertices.");
                }

                Vertices = orderedVertices;
                CalculateBounds(orderedVertices);
            }

            public bool ContainsRectangle(Rectangle rect)
            {
                ArgumentNullException.ThrowIfNull(rect);

                ulong minX = Math.Min(rect.Vertex.X, rect.Width);
                ulong maxX = Math.Max(rect.Vertex.X, rect.Width);
                ulong minY = Math.Min(rect.Vertex.Y, rect.Height);
                ulong maxY = Math.Max(rect.Vertex.Y, rect.Height);

                if (minX < _minX || maxX > _maxX || minY < _minY || maxY > _maxY)
                {
                    return false;
                }

                if (!ContainsVertex(new Vertex(minX, minY)) ||
                    !ContainsVertex(new Vertex(maxX, minY)) ||
                    !ContainsVertex(new Vertex(maxX, maxY)) ||
                    !ContainsVertex(new Vertex(minX, maxY)))
                {
                    return false;
                }

                ulong centerX = (minX + maxX) / 2;
                ulong centerY = (minY + maxY) / 2;
                if (!ContainsVertex(new Vertex(centerX, centerY)))    // Check the centre point of the rectangle is within the polygon.
                {
                    return false;
                }

                // We'll now get a scaled cross-section sample of the rectangle.
                ulong width = maxX - minX;
                ulong height = maxY - minY;

                // Min 20 / Max 200 should provide a good enough density.
                int horizontalSamples = Math.Min(
                    Math.Max(20, (int)(width / 500)),
                    200);
                int verticalSamples = Math.Min(
                    Math.Max(20, (int)(height / 500)),
                    200);

                for (int i = 1; i < horizontalSamples; i++)
                {
                    ulong x = minX + (width * (ulong)i / (ulong)horizontalSamples);
                    if (!ContainsVertex(new Vertex(x, minY)) || !ContainsVertex(new Vertex(x, maxY)))
                    {
                        return false;
                    }
                }

                for (int i = 1; i < verticalSamples; i++)
                {
                    ulong y = minY + (height * (ulong)i / (ulong)verticalSamples);
                    if (!ContainsVertex(new Vertex(minX, y)) || !ContainsVertex(new Vertex(maxX, y)))
                    {
                        return false;
                    }
                }

                return true;
            }

            public bool ContainsVertex(Vertex vertex)
            {
                long checkingX = (long)vertex.X;
                long checkingY = (long)vertex.Y;

                for (int i = 0; i < Vertices.Length; i++)
                {
                    int j = (i + 1) % Vertices.Length;
                    long xi = (long)Vertices[i].X;
                    long yi = (long)Vertices[i].Y;
                    long xj = (long)Vertices[j].X;
                    long yj = (long)Vertices[j].Y;

                    if (xi == xj)
                    {
                        if (checkingX == xi && checkingY >= Math.Min(yi, yj) && checkingY <= Math.Max(yi, yj))
                        {
                            return true;
                        }
                    }
                    else if (yi == yj)
                    {
                        if (checkingY == yi && checkingX >= Math.Min(xi, xj) && checkingX <= Math.Max(xi, xj))
                        {
                            return true;
                        }
                    }
                }

                bool inside = false;
                for (int i = 0, j = Vertices.Length - 1; i < Vertices.Length; j = i++)
                {
                    long currentX = (long)Vertices[i].X;
                    long currentY = (long)Vertices[i].Y;
                    long previousX = (long)Vertices[j].X;
                    long previousY = (long)Vertices[j].Y;

                    bool currentAboveLine = currentY > checkingY;
                    bool previousAboveLine = previousY > checkingY;

                    bool edgeStraddles = currentAboveLine != previousAboveLine;

                    if (edgeStraddles)
                    {
                        double slope = (double)(previousX - currentX) / (previousY - currentY);
                        double intersectionX = slope * (checkingY - currentY) + currentX;

                        if (checkingX < intersectionX)
                        {
                            inside = !inside;
                        }
                    }
                }

                return inside; // True means we only intersected once (the vertex is within the poly). False means it intersected twice (and is outside the polyogn).
            }

            void CalculateBounds(Vertex[] vertices)
            {
                _minX = vertices[0].X;
                _maxX = vertices[0].X;
                _minY = vertices[0].Y;
                _maxY = vertices[0].Y;

                foreach (var vertex in Vertices)
                {
                    if (vertex.X < _minX) _minX = vertex.X;
                    if (vertex.X > _maxX) _maxX = vertex.X;
                    if (vertex.Y < _minY) _minY = vertex.Y;
                    if (vertex.Y > _maxY) _maxY = vertex.Y;
                }
            }

            // Just for fun.
            public string Visualise(Vertex[]? verticesToVisualise)
            {
                var vertices = Vertices;

                if (vertices == null || vertices.Length == 0)
                {
                    return string.Empty;
                }

                var verticesForGrid = vertices;
                if (verticesToVisualise != null)
                {
                    verticesForGrid = [.. vertices.Union(verticesToVisualise)];
                }

                ulong minX = verticesForGrid.Min(v => v.X);
                ulong maxX = verticesForGrid.Max(v => v.X);
                ulong minY = verticesForGrid.Min(v => v.Y);
                ulong maxY = verticesForGrid.Max(v => v.Y);

                int width = (int)(maxX - minX + 1);
                int height = (int)(maxY - minY + 1);

                char[,] grid = new char[height, width];

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        grid[y, x] = '.';
                    }
                }

                foreach (var v in vertices)
                {
                    int gx = (int)(v.X - minX);
                    int gy = (int)(v.Y - minY);

                    int gyFlipped = height - 1 - gy;

                    grid[gyFlipped, gx] = 'X';
                }

                if (verticesToVisualise is Vertex[] positions)
                {
                    foreach (var pos in positions)
                    {
                        int px = (int)(pos.X - minX);
                        int py = (int)(pos.Y - minY);

                        int pyFlipped = height - 1 - py;

                        grid[pyFlipped, px] = '#';
                    }
                }

                var builder = new StringBuilder();
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        builder.Append(grid[y, x]);
                    }
                    builder.AppendLine();
                }

                return builder.ToString();
            }

            public override string ToString()
            {
                return $"Polygon(Vertices:{Vertices.Length})";
            }
        }
    }
}
