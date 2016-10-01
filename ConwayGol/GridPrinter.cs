using System;
using System.IO;

namespace ConwayGol
{
    public static class GridPrinter
    {
        private const string AliveMarker = "X";
        private const string DeadMarker = "-";
        private const string SingleLineSeparator = ";";

        public static string Print(Grid grid)
        {
            return Print(grid, grid.MinX, grid.Width, grid.MinY, grid.Height);
        }
        public static string Print(Grid grid, int startX, int width, int startY, int height)
        {
            if (grid.IsNoMoreLife())
            {
                return string.Empty;
            }
            return PrintSingleLine(grid, startX, width, startY, height).Replace(SingleLineSeparator, Environment.NewLine);
        }
        public static string PrintSingleLine(Grid grid)
        {
            return PrintSingleLine(grid, grid.MinX, grid.Width, grid.MinY, grid.Height);
        }
        public static string PrintSingleLine(Grid grid, int startX, int width, int startY, int height)
        {
            if (grid.IsNoMoreLife())
            {
                return string.Empty;
            }

            var stringWriter = new StringWriter();

            for (int y = startY; y < startY + height; y++)
            {
                for (int x = startX; x < startX + width; x++)
                {
                    stringWriter.Write(grid[x, y].IsAlive() ? AliveMarker : DeadMarker);
                }
                if (y < startY + height - 1)
                {
                    stringWriter.Write(SingleLineSeparator);
                }
            }

            return stringWriter.ToString();
        }
    }
}