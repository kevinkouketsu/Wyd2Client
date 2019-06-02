using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyd2.Client.Model;
using WYD2.Common.GameStructure;

namespace Wyd2.Client
{
    public static class Extension
    {
        public static MMob ById(this IList<MMob> mob, int index)
        {
            var result = mob.Where(x => x.Index == index);
            if (result.Count() == 0)
                return null;

            return result.First();
        }
    }
}
