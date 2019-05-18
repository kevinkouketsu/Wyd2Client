using System;
using System.Runtime.InteropServices;

namespace WYD2.Common.GameStructure
{
    /// <summary>
    /// The character class in the game
    /// </summary>
    public enum ECharClass : byte
    {
        /// <summary>
        /// Trans Knight.
        /// </summary>
        TK = 0,
        /// <summary>
        /// Foema.
        /// </summary>
        FM = 1,
        /// <summary>
        /// Beast Master.
        /// </summary>
        BM = 2,
        /// <summary>
        /// Huntress.
        /// </summary>
        HT = 3
    }

    /// <summary>
    /// Represents the points the mob have to distribute.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MMobPointsLeft
    {
        public ushort Status; // Amount of status points the mob have to distribute.
        public ushort Special; // Amount of special points the mob have to distribute.
        public ushort Skill; // Amount of skill points the mob have to distribute.
    }

    /// <summary>
    /// The mob's name.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MMobName
    {
        public const int MAXL_NAME = 16;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXL_NAME)]
        public String Value;

        public string Name { get => Value; }

        public MMobName(string name)
        {
            Value = name;
        }
    }

    /// <summary>
    /// The basic mob structure. Contains the first structure create to represent the mob in the game.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = ProjectBasics.DEFAULT_PACK)]
    public class MMobCore
    {
        public MMobName Name;
        public sbyte Clan; // The clan the mob belongs to.
        public byte Merchant; // The mob's merchant ID.
        public ushort Guild; // The ID of the guild the mob belongs to.
        public ECharClass Class;

        public ushort Rsv; // TODO: unknown type!
        public byte Quest; // Some of the main quests of the game are saved here.

        public int Coin; // The amount of coins the mob has.
        public long Exp; // The amount of experience the mob has.

        public MPosition StellarGemPosition;

        public MScore BaseScore; // The score without outer interference.
        public MScore FinalScore; // The score after all the calculations.

        public MEquip Equip; // The items the mob is wearing.

        public MInventory Inventory; // The items the mob is carrying.

        public uint LearnedSkill; // The skills the mob learned, divided into four categories (00 _ 00 _ 00 _ 00)

        public int Magic; // Magic power increment.

        public MMobPointsLeft PointsLeft;

        public byte Critical; // The chance the mob has to deliver critical hits.

        public byte SaveMana; // TODO: unknown type!

        public byte Skillbar;
        public byte Skillbar1;
        public byte Skillbar2;
        public byte Skillbar3;

        public byte GuildMemberType; // The mob's guuld level, used to define if it's a guild member or leader.

        public ushort HpRegen; // TODO: unknown type!
        public ushort MpRegen; // TODO: unknown type!

        public sbyte ResistFire;
        public sbyte ResistIce;
        public sbyte ResistThunder;
        public sbyte ResistMagic;
    }
}