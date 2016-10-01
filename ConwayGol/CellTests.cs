using System.Collections.Generic;
using NUnit.Framework;

namespace ConwayGol
{
    [TestFixture]
    public class CellTests
    {
        [Test]
        public void CellsAreDeadByDefault()
        {
            var cell = new Cell();
            Assert.That(cell.IsAlive(), Is.EqualTo(false));
        }

        [Test]
        public void Equality_AreEqual()
        {
            var cell1 = new Cell(1, 1, true);
            var cell2 = new Cell(1, 1, true);

            Assert.AreEqual(cell1, cell2);
        }
        
    }

    public enum CellStates
    {
        Alive,
        Dead,
    }
}