using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Party_Animals
{
    public class Button : Component
    {
        #region private
        private bool _isHovering;
        private Texture2D _texture;
        #endregion
        #region Properties
        public event EventHandler Click;
        public bool Clicked { get; set; }
        public Color PenColor { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        public string Text
        {
            get;
            set;
        }
        #endregion

        #region Methods

        public Button(Texture2D texture)
        {
            _texture = texture;

            PenColor = Color.Black;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color = Game1.suprisee;
            if (_isHovering && Game1.gameState == 0)
                color = Color.Gray;

            spriteBatch.Draw(_texture, Rectangle, color);
        }

        public override void Update(GameTime gameTime)
        {
            Rectangle mouseRectangele = new Rectangle(Game1.currentMouseState.X, Game1.currentMouseState.Y, 1, 1);

            _isHovering = false;
            if (mouseRectangele.Intersects(Rectangle) && Game1.gameState == 0)
            {
                _isHovering = true;
                if (Game1.currentMouseState.LeftButton == ButtonState.Released && Game1.previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }
        #endregion
    }
}
