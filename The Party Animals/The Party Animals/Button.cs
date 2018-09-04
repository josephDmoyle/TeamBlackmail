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
        private bool _isHovering;
        private Texture2D _texture, ON, OFF;
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
            Rectangle mouseRectangele = new Rectangle(Game1.currentMouseState.X, Game1.currentMouseState.Y, 1, 1);

            _isHovering = false;
            if (mouseRectangele.Intersects(Rectangle) && Game1.gameState == 0)
            {
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color = Game1.suprisee;
            if (_isHovering && Game1.gameState == 0)
                color = Color.Gray;
            spriteBatch.Draw(_texture, Rectangle, color);
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
            throw new NotImplementedException();
        }

        public override void Hover()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
