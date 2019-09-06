using System.Runtime.InteropServices;

namespace WYD2.Common.GameStructure
{
    /// <summary>
    /// Represents a 4-byte bidimensional position in the game.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = ProjectBasics.DEFAULT_PACK)]
    public class MPosition
    {
        public short X { get; set; }
        public short Y { get; set; }

        public MPosition()
        {

        }
        public MPosition(short X, short Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public override string ToString()
        {
            return $"{ X }x { Y }y";
        }
    }
}