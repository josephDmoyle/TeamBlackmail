using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Party_Animals
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


    public class Interaction : Component
    {

        public Dragable _dragable;
        public BGGraphic _graphic;


        public EventHandler<IntEventArgs> onEnter;
        public EventHandler<IntEventArgs> onExit;

        private int _status = 0; // 0: not collided, 1: collided

        public int ID;


        public Interaction(Dragable dragable, BGGraphic bGGraphic)
        {
            _dragable = dragable;
            _graphic = bGGraphic;

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (_graphic.Rectangle.Contains(_dragable.CenterRect))
            {
                if (_status == 0)
                {
                    _status = 1;
                    // on dragable entered the graphic
                    onEnter?.Invoke(this, new IntEventArgs(ID));
                }
            }
            else
            {
                if (_status == 1)
                {
                    _status = 0;

                    // on dragable exit the graphic
                    onExit?.Invoke(this, new IntEventArgs(ID));

                }
            }
        }
    }
}
