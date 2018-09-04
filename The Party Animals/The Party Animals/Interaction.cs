using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Party_Animals
{
    public class IntEventArgs : EventArgs
    {
        public int ID
        {
            get; set;
        }
        public IntEventArgs(int _id)
        {
            ID = _id;
        }
    }
}
