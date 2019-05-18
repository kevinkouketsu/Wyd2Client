using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common.GameStructure;

namespace Wyd2.Client.Model
{
    public class CreateCharacterModel 
    {
        public string Name { get; set; }
        public ECharClass Class { get; set; }
    }
}
