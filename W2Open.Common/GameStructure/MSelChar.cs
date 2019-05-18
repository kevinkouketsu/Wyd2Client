using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace WYD2.Common.GameStructure
{
    /// <summary>
    /// Contains data representing the character selection scene. Used in some game packets.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MSelChar
    {
        public unsafe fixed short SPosX[GameBasics.MAXL_ACC_MOB];
        public unsafe fixed short SPosY[GameBasics.MAXL_ACC_MOB];

        public IList<MMobName> Names
        {
            get
            {
                if (Name == null)
                    return null;

                return Name.ToList();
            }
        }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameBasics.MAXL_ACC_MOB)]
        public MMobName[] Name;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameBasics.MAXL_ACC_MOB)]
        public MScore[] Score;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameBasics.MAXL_ACC_MOB)]
        public MEquip[] Equip;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameBasics.MAXL_ACC_MOB)]
        public ushort[] Guild;

        public unsafe fixed int Coin[GameBasics.MAXL_ACC_MOB];
        public unsafe fixed long Exp[GameBasics.MAXL_ACC_MOB];
    }
}