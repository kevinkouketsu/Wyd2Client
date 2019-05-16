using System.Runtime.InteropServices;

namespace WYD2.Common.GameStructure
{
    /// <summary>
    /// Represents the mob's buffs in the game.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MAffect
    {
        public byte Type;
        public byte Value;
        public ushort Level;
        public uint Time;
    }
}