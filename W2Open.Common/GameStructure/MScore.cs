using System.Runtime.InteropServices;

namespace WYD2.Common.GameStructure
{
    /// <summary>
    /// Represents a set of information regard to a mob's status.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MScore
    {
        public int Level;

        public int Defense;
        public int Damage;

        public byte Merchant; // TODO: unknown type!

        private byte m_MovementSpeed;
        public byte MovementSpeed
        {
            get { return m_MovementSpeed; }
            set { m_MovementSpeed = (value > GameBasics.MAX_MOB_SPEED) ? GameBasics.MAX_MOB_SPEED : value; }
        }

        public byte Direction; // The direction the mob is facing.
        public byte ChaosRate; // TODO: unknown type!

        public int MaxHp { get; set; }
        public int MaxMp { get; set; }
        public int CurrHp { get; set; }
        public int CurrMp { get; set; }

        public short Str;
        public short Int;
        public short Dex;
        public short Con;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public short[] Special; // The 4 special points ("pontos de aprendizagem").
    }
}