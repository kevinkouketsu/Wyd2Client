using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyd2.Client.Model;

namespace Wyd2.Client
{
    public static class Extension
    {
        public static MobModel ById(this IList<MobModel> mob, int index)
        {
            var result = mob.Where(x => x.Index == index);
            if (result.Count() == 0)
                return null;

            return result.First();
        }
    }
}
