using System.Runtime.InteropServices;

namespace WYD2.Common.GameStructure
{
    /// <summary>
    /// The progress of the quest Mystic Land.
    /// </summary>
    public enum EMysticLandQuest : sbyte
    {
        DontHave = 0,
        DontConcluded = 1,
        Completed = 2
    }

    /// <summary>
    /// The new mob structure. Contains the mob data from the version 7.48 to now.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MMobExtra
    {
        public short ClassMaster;
        public sbyte Citizen;
        public int Fame;
        public sbyte Soul;
        public short MortalFace;

        public MQuestInfo QuestInfo;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public MSavedCelestial[] SavedCelestial; // Represents the celestial and the sub-celestial.

        public long LastNT;
        public int NT;

        public int KefraTicket;
        public long DivineEnd;
        public uint Hold;

        public MDayLog DayLog;

        public MDonateInfo DonateInfo;

        public unsafe fixed int Empty[9];

        #region Sub Structures
        [StructLayout(LayoutKind.Sequential, Pack = ProjectBasics.DEFAULT_PACK)]
        public struct MQuestInfo
        {
            [StructLayout(LayoutKind.Sequential, Pack = ProjectBasics.DEFAULT_PACK)]
            public struct MQuestInfo_Mortal
            {
                public sbyte Newbie; // 00_01_02_03_04  quest com quatro etapas
                public EMysticLandQuest MysticLand;
                public sbyte MolarGargoyle;
                public sbyte OrcPill;
                public unsafe fixed byte Empty[30];
            }

            [StructLayout(LayoutKind.Sequential, Pack = ProjectBasics.DEFAULT_PACK)]
            public struct MQuestInfo_Arch
            {
                public sbyte MortalSlot;
                public sbyte MortalLevel;
                public sbyte Level355;
                public sbyte Level370;
                public sbyte Cristal; //00_01_02_03_04 quest com quatro etapas
                public unsafe fixed byte Empty[30];
            }

            [StructLayout(LayoutKind.Sequential, Pack = ProjectBasics.DEFAULT_PACK)]
            public struct MQuestInfo_Celestial
            {
                public short ArchLevel;
                public short CelestialLevel;
                public short SubCelestialLevel;

                public sbyte Lv40;
                public sbyte Lv90;

                public sbyte Add120;
                public sbyte Add150;
                public sbyte Add180;
                public sbyte Add200;

                public sbyte Arcana;
                public sbyte Reset;

                public unsafe fixed byte Empty[30];
            }

            public MQuestInfo_Mortal Mortal;
            public MQuestInfo_Arch Arch;
            public MQuestInfo_Celestial Celestial;

            public sbyte Circle;
            public unsafe fixed byte Empty[30];
        }

        [StructLayout(LayoutKind.Sequential, Pack = ProjectBasics.DEFAULT_PACK)]
        public struct MSavedCelestial
        {
            public int Class;
            public long Exp;

            public MPosition StellarGemPosition;

            public MScore BaseScore;

            public int LearnedSkill;

            public MMobPointsLeft PointsLeft;

            public unsafe fixed byte SkillBar1[4];
            public unsafe fixed byte SkillBar2[16];

            public sbyte Soul;

            public unsafe fixed byte Empty[30];
        }

        [StructLayout(LayoutKind.Sequential, Pack = ProjectBasics.DEFAULT_PACK)]
        public struct MDayLog
        {
            public long Exp;
            public int YearDay;
        }

        [StructLayout(LayoutKind.Sequential, Pack = ProjectBasics.DEFAULT_PACK)]
        public struct MDonateInfo
        {
            public long LastTime;
            public int Count;
        }
        #endregion
    }
}