namespace AdventOfCode.Business.Year2025
{
    internal class Day12 : IAdventDay
    {
        string _root = string.Empty;
        public void SetRoot(string path)
        {
            _root = path;
        }

        public void ExecutePart1()
        {
            var day12Input = File.ReadAllText(Path.Combine(_root, "Day12_Input.txt"));
            var result = CalculateHowManyPresentsCanFitAllOfThePresents(day12Input);

            Console.WriteLine($"\r\n'{result}' of the regions can fit all of the listed presents.\r\n");
        }

        public void ExecutePart2()
        {
            Console.WriteLine($"\r\nAdvent of Code for 2025 is COMPLETE!");

            Console.WriteLine($"\r\n{AdventCalendar}\r\n");
        }

        internal static int CalculateHowManyPresentsCanFitAllOfThePresents(string input)
        {
            var lines = input.Trim().Split('\n').Select(l => l.Trim()).Where(l => l.Length > 0).ToList();

            // Parse shapes
            var shapes = new List<Shape>();
            int i = 0;
            while (i < lines.Count && lines[i].Contains(':') && !lines[i].Contains('x'))
            {
                int shapeId = int.Parse(lines[i].Split(':')[0]);
                i++;
                var cells = new List<(int x, int y)>();
                int y = 0;
                while (i < lines.Count && !lines[i].Contains(':'))
                {
                    for (int x = 0; x < lines[i].Length; x++)
                    {
                        if (lines[i][x] == '#')
                            cells.Add((x, y));
                    }
                    y++;
                    i++;
                }
                shapes.Add(new Shape(shapeId, cells));
            }

            // Parse regions
            var regions = new List<Region>();
            while (i < lines.Count)
            {
                var parts = lines[i].Split([' '], StringSplitOptions.RemoveEmptyEntries);
                var dimPart = parts[0].TrimEnd(':');
                var dims = dimPart.Split('x');
                int w = int.Parse(dims[0]);
                int h = int.Parse(dims[1]);
                var counts = parts.Skip(1).Select(int.Parse).ToList();
                regions.Add(new Region(w, h, counts));
                i++;
            }

            // Check each region
            int fitCount = 0;
            int regionNum = 0;

            foreach (var region in regions)
            {
                regionNum++;

                // Quick impossibility check
                int totalCells = region.Width * region.Height;
                int cellsPerShape = shapes[0].Cells.Count;
                int totalPresents = region.PresentCounts.Sum();
                int cellsNeeded = totalPresents * cellsPerShape;

                if (cellsNeeded > totalCells)
                {
                    continue;
                }

                bool fits = CanFitPresents(region, shapes);

                if (fits)
                {
                    fitCount++;
                }
            }

            return fitCount;
        }

        internal static bool CanFitPresents(Region region, List<Shape> shapes)
        {
            var presents = new List<(Shape shape, List<List<(int x, int y)>> orientations)>();

            for (int i = 0; i < shapes.Count; i++)
            {
                for (int j = 0; j < region.PresentCounts[i]; j++)
                {
                    var orientations = GetAllOrientations(shapes[i]);
                    orientations = [.. orientations.Where(o =>
                    {
                        if (o.Count == 0) return false;
                        int w = o.Max(c => c.x) + 1;
                        int h = o.Max(c => c.y) + 1;
                        return w <= region.Width && h <= region.Height;
                    })];

                    if (orientations.Count == 0)
                        return false;

                    presents.Add((shapes[i], orientations));
                }
            }

            if (presents.Count == 0)
                return true; // No presents to place

            // Sort by constraint: fewest valid orientations first
            presents.Sort((a, b) => a.orientations.Count.CompareTo(b.orientations.Count));

            var grid = new bool[region.Width, region.Height];
            return TryPlacePresents(presents, 0, grid, region.Width, region.Height);
        }

        internal static bool TryPlacePresents(List<(Shape shape, List<List<(int x, int y)>> orientations)> presents,
                                      int idx, bool[,] grid, int w, int h)
        {
            if (idx == presents.Count)
                return true;
            var (_, orientations) = presents[idx];

            // Find first empty cell - only try placements starting from here
            int startY = 0, startX = 0;
            bool found = false;
            for (int y = 0; y < h && !found; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (!grid[x, y])
                    {
                        startY = y;
                        startX = x;
                        found = true;
                        break;
                    }
                }
            }

            // Try all positions from first empty cell onwards
            for (int y = startY; y < h; y++)
            {
                int xStart = (y == startY) ? startX : 0;
                for (int x = xStart; x < w; x++)
                {
                    foreach (var oriented in orientations)
                    {
                        if (CanPlace(oriented, x, y, grid, w, h))
                        {
                            Place(oriented, x, y, grid, true);
                            if (TryPlacePresents(presents, idx + 1, grid, w, h))
                                return true;
                            Place(oriented, x, y, grid, false);
                        }
                    }
                }
            }

            return false;
        }

        internal static List<List<(int x, int y)>> GetAllOrientations(Shape shape)
        {
            var orientations = new HashSet<string>();
            var result = new List<List<(int x, int y)>>();

            var current = shape.Cells.ToList();
            for (int rotation = 0; rotation < 4; rotation++)
            {
                // Rotation
                var normalized = Normalize(current);
                var key = string.Join(";", normalized.OrderBy(c => c.y).ThenBy(c => c.x).Select(c => $"{c.x},{c.y}"));
                if (!orientations.Contains(key))
                {
                    orientations.Add(key);
                    result.Add(normalized);
                }

                // Horizontal flip
                var flipped = Normalize([.. current.Select(c => (-c.x, c.y))]);
                key = string.Join(";", flipped.OrderBy(c => c.y).ThenBy(c => c.x).Select(c => $"{c.x},{c.y}"));
                if (!orientations.Contains(key))
                {
                    orientations.Add(key);
                    result.Add(flipped);
                }

                current = [.. current.Select(c => (-c.y, c.x))];
            }

            return result;
        }

        internal static List<(int x, int y)> Normalize(List<(int x, int y)> cells)
        {
            if (cells.Count == 0) return cells;
            int minX = cells.Min(c => c.x);
            int minY = cells.Min(c => c.y);
            return [.. cells.Select(c => (c.x - minX, c.y - minY))];
        }

        internal static bool CanPlace(List<(int x, int y)> cells, int ox, int oy, bool[,] grid, int w, int h)
        {
            foreach (var (x, y) in cells)
            {
                int nx = ox + x;
                int ny = oy + y;
                if (nx < 0 || nx >= w || ny < 0 || ny >= h || grid[nx, ny])
                    return false;
            }
            return true;
        }

        internal static void Place(List<(int x, int y)> cells, int ox, int oy, bool[,] grid, bool place)
        {
            foreach (var (x, y) in cells)
            {
                grid[ox + x, oy + y] = place;
            }
        }

        internal class Shape(int id, List<(int x, int y)> cells)
        {
            public int Id = id;
            public List<(int x, int y)> Cells = cells;
        }

        internal class Region(int w, int h, List<int> counts)
        {
            public int Width = w;
            public int Height = h;
            public List<int> PresentCounts = counts;
        }

        internal const string AdventCalendar =
            """
                ' .' ____ ''.       *    .. ' '   '  <o . .
            ________/O___\__________|_________________0______  1
               _______||_________
               | _@__ || _o_  '.|_ _________________________   2
               |_&_%__||_OO__^=_[ \|..'  _    .. .. ..     |
                                 \_]__--|_|___[]_[]_[]__//_|   3
                                           ____________//___
            __________________________  ...| \ '''''' // @@|   4
            |_  ___ | .--.  ()   ()  |.' ..__[#]_@@__//_@@@|
            |_\_|^|_]_|==|_T_T_T_T_T_...'                      5
             ||   ____________    _______________________
            _||__/'...' '...' \_  |.     |~    .''.    .|      6
            |^@ |   1  2  3 () |  | '..'/ \'..'    '..' |____
            |&%;]__[]_[]_[]__<>|  |    |H_/|\   \\\\\\  | | |  7
                              '...|<>__|H__|_\__|_____|_[_0_|
             __________________________                   |    8
            /'....'______'...'...'__'.|  _________________0__
            [%  @ |(__) [&  ;  o  ] \ |__|  [  ]  ''''''  | |  9
            o=====|_____o=========o_|_[__]_____-/_-/_-/___|_|
            _________||______ ______________________________  10
            |  ___       '..| |'..''..''..''..''..''..''..'|
            |_|..'|_(:::::)_| |   *  ()  *  ()  *  ()  *   |  11
                |      .--.   |  <o>    <^o    <o>    <o^> |
                '______'  '___#_<<^o>__<o^>>__<<^>o__<<^o>_|  12
            """;
    }
}
