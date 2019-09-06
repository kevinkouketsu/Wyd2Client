using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYD2.Control.System
{
    public class MacroDispatcherHandle
    {
        public int Handle { get; }

        public IMacro Macro { get; }

        public MacroDispatcherHandle(int Handle, IMacro Macro)
        {
            this.Handle = Handle;
            this.Macro = Macro;
        }
    }
}
