using System.Linq;
using System.Text;
using NUnit.Framework;
using static ConwayGol.GridPrinter;

namespace ConwayGol
{
    [TestFixture]
    public class GridTests
    {
        [Test]
        public void CanRecallLiveCell()
        {
            var grid = new Grid(new[] {
                new Coordinate(2,2)
            }
            );

            Assert.That(grid[2, 2].IsAlive(), Is.True);
        }

        [Test]
        public void CanRecallDeadCell()
        {
            var grid = new Grid(new[] {
                new Coordinate(2,2)
            }
            );

            Assert.That(grid[1, 1].IsAlive(), Is.False);
        }

        [Test]
        public void PrintGridWithRange()
        {
            var grid = new Grid(new[] { new Coordinate(1, 1), });
            var result = PrintSingleLine(grid, 0, 3, 0, 3);
            var expectedOutput = "---;-X-;---";

            Assert.That(result, Is.EqualTo(expectedOutput));
        }

        [Test]
        public void PrintCreatesBorderOfDeadCells()
        {
            var expected = "---;-X-;-X-;---";
            var seed = new[] {new Coordinate(1, 0), new Coordinate(1, 1),};
            var grid = new Grid(seed);
            Assert.That(PrintSingleLine(grid), Is.EqualTo(expected));

        }

        [Test]
        public void PrintSingleLine_EqualsSeed()
        {
            var seed = "---;-X-;---";
            var grid = new Grid("000;010;000");
            var result = PrintSingleLine(grid);
            Assert.That(result, Is.EqualTo(seed));
        }

        [Test]
        public void CanSeedWithString()
        {
            var seed = "000;010;000";
            var grid = new Grid(seed);
            Assert.IsTrue(grid[1,1].IsAlive());
        }

        [Test]
        public void PrintEmptyGrid_ReturnsEmptyString()
        {
            var grid = new Grid("0");
            Assert.That(Print(grid), Is.Empty);
        }

        [Test]
        public void NextGen_Underpopulation_SingleCellDies()
        {
            var grid = new Grid("1");
            grid.NextGen();
            Assert.IsFalse(grid[0,0].IsAlive());
        }

        [Test]
        public void NextGen_Underpopulation_TwoCellsThatAreNotNeighborsDie()
        {
            var grid = new Grid("010;000;010");
            grid.NextGen();
            Assert.That(grid.IsNoMoreLife, Is.True);
        }

        [Test]
        public void NextGen_Underpopulation_CellSurvivesIfTwoOrMoreNeighbors()
        {
            var seed = "011;010;000";
            var grid = new Grid(seed);
            grid.NextGen();
            Assert.IsTrue(grid[1,1].IsAlive());
        }

        [Test]
        public void NextGen_AnyCellWithMoreThan3NeighborsDies()
        {
            var grid = new Grid("111;011;000");
            grid.NextGen();
            Assert.IsFalse(grid[1,1].IsAlive());

        }

        [Test]
        public void NextGen_AnyDeadCellWithExactly3Neighbors_BecomesAlive()
        {
            var grid = new Grid("111;000");
            grid.NextGen();
            Assert.IsTrue(grid[1,1].IsAlive());
        }

        [Test]
        public void GetNeighbors_FindsSingleNeighbor()
        {
            var grid = new Grid("11");
            var neighborsFound = grid.GetNeighbors(grid[0, 0]);

            Assert.That(neighborsFound.Count, Is.EqualTo(1));
            Assert.That(neighborsFound.First(), Is.EqualTo(new Cell(1,0)));
        }

        [Test]
        public void SeedWithString()
        {
            var grid = new Grid("010;010;000");
            Assert.IsTrue(grid[1, 0].IsAlive());
            Assert.IsTrue(grid[1, 1].IsAlive());
        }

        [Test]
        public void GetNeighbors_FindsTwoNeighbors()
        {
            var grid = new Grid("110;010;000");
            var neighborsFound = grid.GetNeighbors(grid[1, 1]);

            Assert.That(neighborsFound.Count, Is.EqualTo(2));
            Assert.That(neighborsFound, Contains.Item(new Cell(0,0)));
            Assert.That(neighborsFound, Contains.Item(new Cell(1,0)));
        }
    }
}