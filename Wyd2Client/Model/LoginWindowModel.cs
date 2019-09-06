using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyd2.Client.Model
{
    public class LoginWindowModel
    {
        public struct ServerInfo
        {
            public string ServerName { get; set; }
            public string IpAddress { get; set; }

            public ServerInfo(string serverName, string ipAddress)
            {
                ServerName = serverName;
                IpAddress = ipAddress;
            }
        }

        public string Login { get; set; }
        public string Token { get; set; }
        public int SelectedServerIndex { get; set; } = -1;
    }
}
