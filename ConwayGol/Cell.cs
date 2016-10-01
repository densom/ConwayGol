using System.Diagnostics;

namespace ConwayGol

{
    [DebuggerDisplay("{X},{Y}")]
    public class Cell
    {
        private bool _state;

        public Cell() : this(0, 0)
        {
        }

#pragma warning disable S2360 // Optional parameters should not be used
        public Cell(int x, int y, bool state = false)
#pragma warning restore S2360 // Optional parameters should not be used
        {
            _state = state;
            X = x;
            Y = y;
        }

        // properties

        public int X { get; set; }
        public int Y { get; set; }


        // methods

        public bool IsAlive()
        {
            return _state;
        }

        public void SetAlive()
        {
            _state = true;
        }

        // equality members
        #region Equality Members
        protected bool Equals(Cell other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Cell)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }
        #endregion

    }
}