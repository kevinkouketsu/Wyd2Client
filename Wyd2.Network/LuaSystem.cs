using NLua;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common.GameStructure;

namespace WYD2.Control
{
    public class LuaSystem
    {
        public ClientConnection Connection { get; }
        public ClientControl Control { get; }
        public MMobCore Mob { get; set; }
        public string Folder { get; }
        private Lua Lua { get; }

        public LuaSystem(string folder, ClientConnection connection, ClientControl control, MMobCore mob)
        {
            Connection = connection;
            Control = control;
            Mob = mob;
            Folder = folder;

            Lua = new Lua();
            Lua.LoadCLRPackage();

        }

        public void LoadLua(string fileName)
        {
            try
            {   
                Lua["mob"] = Mob;
                Lua["connection"] = Connection;
                Lua["control"] = Control;

                Lua.DoFile(Path.Combine(Folder, fileName));
            }
            catch(Exception e)
            {

            }
        }
    }
}
