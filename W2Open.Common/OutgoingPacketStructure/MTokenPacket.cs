using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common.GameStructure;

namespace WYD2.Common.OutgoingPacketStructure
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MTokenPacket : IGamePacket
    {
        public const ushort Opcode_Incorrect = 0xFDF;
        public const ushort Opcode = 0xFDE;

        public MPacketHeader Header { get; set; }

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
        public string Password;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Unknown;

        public int IsChanging;

        public static MTokenPacket Create(string password, int isChanging) 
            => new MTokenPacket
            {
                Header = new MPacketHeader()
                {
                    Size = (ushort)Marshal.SizeOf(typeof(MTokenPacket)),
                    Opcode = Opcode,
                },

                Password = password,
                IsChanging = isChanging
            };
    }
}
