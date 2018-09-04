using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Party_Animals
{
    public class Button : Component
    {
        #region private
        private bool hovering = false;
        private Texture2D _texture, ON, OFF;
        Color color = Color.White;
        #endregion
        #region Properties
        public Vector2 Position { get; set; }
        public Rectangle Rectangle => new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
        #endregion

        #region Methods

        public Button(Texture2D on, Texture2D off)
        {
            ON = on;
            OFF = off;
            _texture = ON;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color = Game1.suprisee;
            if (hovering)
                color = Color.DarkGray;
            spriteBatch.Draw(_texture, Rectangle, color);
            hovering = false;
        }

        public override void Click()
        {
            if (_texture == ON)
            {
                _texture = OFF;
                Game1.lightOn = false;
                Game1.supriser = Color.Black;
                Game1.suprisee = Color.DarkGray;
            }
            else
            {
                _texture = ON;
                Game1.lightOn = true;
                Game1.supriser = Color.White;
                Game1.suprisee = Color.White;
            }
        }

        public override void Unclick()
        {
            
        }

        public override bool Hover(Rectangle mouseRect)
        {
            if (mouseRect.Intersects(Rectangle))
            {
                hovering = true;
                return true;
            }
            return false;
        }
        #endregion
    }
}
