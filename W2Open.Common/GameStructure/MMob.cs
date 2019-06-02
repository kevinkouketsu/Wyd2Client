using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYD2.Common.GameStructure
{
    public class MMob
    {
        #region Public Properties

        /// <summary>
        /// Index of mob
        /// </summary>
        public ushort Index { get; }

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

        public MMob(string name, ushort index)
        {
            Name = name;
            Index = index;
        }

        #endregion
    }
}
