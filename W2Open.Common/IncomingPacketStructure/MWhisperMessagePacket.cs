﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common.GameStructure;

namespace WYD2.Common.IncomingPacketStructure
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MWhisperMessagePacket : IGamePacket
    {
        public const ushort Opcode = 0x334;

        public MPacketHeader Header { get; set; }

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string Command;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 132)]
        public string Message;
    }
}
