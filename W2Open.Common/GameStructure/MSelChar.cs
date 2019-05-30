using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace WYD2.Common.GameStructure
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct MobName
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string Name;

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Contains data representing the character selection scene. Used in some game packets.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MSelChar
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameBasics.MAXL_ACC_MOB)]
        public short[] SPosX;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameBasics.MAXL_ACC_MOB)]
        public short[] SPosY;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameBasics.MAXL_ACC_MOB)]
        public MobName[] Name;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameBasics.MAXL_ACC_MOB)]
        public MScore[] Score;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameBasics.MAXL_ACC_MOB)]
        public MEquip[] Equip;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameBasics.MAXL_ACC_MOB)]
        public ushort[] Guild;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameBasics.MAXL_ACC_MOB)]
        public int[] Coin;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameBasics.MAXL_ACC_MOB)]
        public long[] Exp;

        public IList<MobName> Names
        {
            get
            {
                if (Name == null)
                    return null;

                return Name.ToList();
            }
        }

    }
}