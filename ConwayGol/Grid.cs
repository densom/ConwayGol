using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace ConwayGol
{
    public class Grid
    {
        private readonly HashSet<Cell> _liveCells = new HashSet<Cell>();

        // properties
        public IEnumerable<Coordinate> InitialSeed { get; }
        public int MinX => GetMinX();
        public int MaxX => GetMaxX();
        public int MinY => GetMinY();
        public int MaxY => GetMaxY();

        private int GetMinX()
        {
            if (IsNoMoreLife())
            {
                return 0;
            }

            return _liveCells.Min(c => c.X) - 1;
        }
        private int GetMaxX()
        {
            if (IsNoMoreLife())
            {
                return 0;
            }
            return _liveCells.Max(c => c.X) + 1;
        }
        private int GetMinY()
        {
            if (IsNoMoreLife())
            {
                return 0;
            }
            return _liveCells.Min(c => c.Y) - 1;
        }
        private int GetMaxY()
        {
            if (IsNoMoreLife())
            {
                return 0;
            }
            return _liveCells.Max(c => c.Y) + 1;
        }

        public int Width => MaxX - MinX + 1;
        public int Height => MaxY - MinY + 1;

        // accessors
        public Cell this[int x, int y] => ReturnCell(x, y);

        // constructors
        public Grid(IEnumerable<Coordinate> seed)
        {
            InitialSeed = seed;
            Initialize();
        }
        public Grid(string seed) : this(ToCoordinates(seed))
        {
        }

        // methods
        
        public bool IsNoMoreLife()
        {
            return _liveCells.Count == 0;
        }

        private void Initialize()
        {
            var liveCells = InitialSeed.Select(c => new Cell(c.X, c.Y, true)).ToList();
            liveCells.ForEach(x => _liveCells.Add(x));
        }
        private Cell ReturnCell(int x, int y)
        {
            var cell = _liveCells.FirstOrDefault(c => c.X == x && c.Y == y);
            if (cell == null)
            {
                return new Cell(x, y, false);
            }

            return cell;
        }
        private static IEnumerable<Coordinate> ToCoordinates(string seed)
        {
            var coordinates = new List<Coordinate>();

            var lines = seed.Split(';');
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == '1')
                    {
                        coordinates.Add(new Coordinate(x, y));
                    }

                }
            }

            return coordinates;
        }

        public void NextGen()
        {

            var cellsToKill = new HashSet<Cell>();
            var cellsToBirth = new HashSet<Cell>();

            foreach (var cellUnderInvestigation in _liveCells)
            {
                // Any live cell with fewer than two live neighbours dies, as if caused by under-population 
                if (GetNeighbors(cellUnderInvestigation).Count < 2)
                {
                    // cell has no neighbors
                    cellsToKill.Add(cellUnderInvestigation);
                }

                if (GetNeighbors(cellUnderInvestigation).Count > 3)
                {
                    // cell is overpopulated
                    cellsToKill.Add(cellUnderInvestigation);
                }
            }

            //todo: does not get the correct dead neighbors
            var deadNeighbors = _liveCells.SelectMany(GetAllNeighbors).Where(x => !x.IsAlive());


            foreach (var cell in deadNeighbors)
            {
                if (GetNeighbors(cell).Where(x => x.IsAlive()).Count() == 3)
                {
                    // dead cell has exactly 3 neighbors
                    cellsToBirth.Add(new Cell(cell.X, cell.Y, true));
                }
            }


            // apply final results to population
            _liveCells.ExceptWith(cellsToKill);
            _liveCells.UnionWith(cellsToBirth);
        }

        private HashSet<Cell> GetAllNeighbors(Cell cell)
        {
            //todo:  need to ensure the live cells are actually live
            //throw new NotImplementedException();
            var set = new HashSet<Cell>
            {
                new Cell(cell.X - 1, cell.Y - 1),
                new Cell(cell.X - 1, cell.Y + 1),
                new Cell(cell.X + 1, cell.Y - 1),
                new Cell(cell.X + 1, cell.Y + 1),
                new Cell(cell.X, cell.Y - 1),
                new Cell(cell.X, cell.Y + 1),
                new Cell(cell.X - 1, cell.Y),
                new Cell(cell.X + 1, cell.Y)
            };

            set.ExceptWith(_liveCells);

            return set;
        }

        internal HashSet<Cell> GetNeighbors(Cell cell)
        {
            //todo:  need to include dead cells
            var neighbors = _liveCells.Where(x => IsNeighbor(cell, x)).ToList();
            neighbors.Remove(cell);
            return new HashSet<Cell>(neighbors);
        }

        internal HashSet<Cell> GetNeighbors(IEnumerable<Cell> cells)
        {
            var cellsToReturn = new HashSet<Cell>();
            foreach (var cell in cells)
            {
                foreach (var neighbor in GetNeighbors(cell))
                {
                    cellsToReturn.Add(neighbor);
                }
            }

            return cellsToReturn;

        }

        public override string ToString()
        {
            return GridPrinter.PrintSingleLine(this);
        }

        private bool IsNeighbor(Cell origin, Cell cellInQuestion)
        {
            var xRange = Enumerable.Range(origin.X - 1, 3);
            var yRange = Enumerable.Range(origin.Y - 1, 3);

            return xRange.Contains(cellInQuestion.X) && yRange.Contains(cellInQuestion.Y);
        }
    }
}