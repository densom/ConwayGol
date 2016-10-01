using System;
using NUnit.Framework.Interfaces;
using static ConwayGol.GridPrinter;

namespace ConwayGol
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        // [STAThread]
        static void Main()
        {
            var seed = "0000;0111;1110;0000";
            var grid = new Grid(seed);

            OutputResults("Initial Seed", grid);
            
            var numGenerations = 5;
            for (int i = 1; i <= numGenerations; i++)
            {
                grid.NextGen();
                OutputResults($"Iteration: {i}", grid);
            }
            
        }

        private static void OutputResults(string message, Grid grid)
        {
            Console.WriteLine(message);
            Console.WriteLine(Print(grid));
            Console.WriteLine();
        }
    }
}
