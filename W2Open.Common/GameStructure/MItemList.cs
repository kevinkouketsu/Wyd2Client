using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace WYD2.Common.GameStructure
{
    public struct MItemEffect
    {
        public ushort Index;
        public ushort Value;
    }

    public struct MItemList
    {
        public String Name;

        public short Mesh1;
        public int Mesh2;

        public short Level;
        public short Str;
        public short Int;
        public short Dex;
        public short Con;

        public IList<MItemEffect> Effect;

        public int Price;
        public short Unique;
        public short Pos;
        public short Extreme;
        public short Grade;
    }
}
