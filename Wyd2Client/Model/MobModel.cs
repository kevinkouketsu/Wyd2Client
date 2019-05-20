using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common.GameStructure;

namespace Wyd2.Client.Model
{
    public class MobModel
    {
        #region Public Properties

        /// <summary>
        /// Index of mob
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Name of mob
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Current position of mob on map
        /// </summary>
        public MPosition Position { get; set; }

        /// <summary>
        /// Current status of mob
        /// </summary>
        public MScore Score { get; set; }

        #endregion

        #region Constructor

        public MobModel(string name, int index)
        {
            Name = name;
            Index = index;
        }

        #endregion
    }
}
