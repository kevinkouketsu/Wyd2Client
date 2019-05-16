using System.Runtime.InteropServices;

namespace WYD2.Common.GameStructure
{
    /// <summary>
    /// Represent an item in the game.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MItem
    {
        public short Index;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameBasics.MAXL_ITEM_EFFECT)]
        public MItem_Effects[] Effects;
    }

    /// <summary>
    /// The BItem internal structure representing the item effects.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MItem_Effects
    {
        public sbyte Code;
        public sbyte Value;
    }

    /// <summary>
    /// Represents the mob's equips.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MEquip
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameBasics.MAXL_EQUIP)]
        public MItem[] Items;
    }

    /// <summary>
    /// Represents the mob's inventory.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MInventory
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameBasics.MAXL_INVENTORY)]
        public MItem[] Items;
    }
}