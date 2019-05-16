using System;
using System.Runtime.InteropServices;

namespace WYD2.Common.GameStructure
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MLoginInfo
    {
        public const int MAXL_ACCNAME = 16;
        public const int MAXL_PSW = 12;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXL_ACCNAME)]
        public String AccName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXL_PSW)]
        public String Password;
    }

    [StructLayout(LayoutKind.Sequential, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MAccountInfo
    {
        public const int MAXL_REALNAME = 24;
        public const int MAXL_EMAIL = 48;
        public const int MAXL_TELEPHONE = 16;
        public const int MAXL_ADDRESS = 78;
        public const int MAXL_NUMTOKEN = 6;

        public MLoginInfo LoginInfo;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXL_REALNAME)]
        public String RealName;

        public int SSN1;
        public int SSN2;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXL_EMAIL)]
        public String Email;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXL_TELEPHONE)]
        public String Telephone;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXL_ADDRESS)]
        public String Address;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXL_NUMTOKEN)]
        public String NumericToken;

        public int Year;
        public int YearDay;
    }
}