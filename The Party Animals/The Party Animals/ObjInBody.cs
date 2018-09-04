using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace The_Party_Animals
{
    public class ObjInBody : Component
    {


        private Texture2D _texture;

        public Rectangle Rectangle;
        public int MoveSpeed;
        public bool isVisible;
        public bool InSpot;

        private bool pressed;

        public ObjInBody(Texture2D texture, Rectangle rectangle)
        {
            _texture = texture;
            Rectangle = rectangle;
        }

        public override void Update(GameTime gameTime)
        {
            if (!InSpot)
            {
                if (Game1.currentMouseState.LeftButton == ButtonState.Pressed && Rectangle.Contains(new Point(Game1.currentMouseState.X, Game1.currentMouseState.Y)))
                {
                    pressed = true;

                }
                if (pressed)
                {
                    Rectangle = new Rectangle(Game1.currentMouseState.X - Rectangle.Width / 2, Game1.currentMouseState.Y - Rectangle.Height / 2, Rectangle.Width, Rectangle.Height);
                }
                if (Game1.currentMouseState.LeftButton == ButtonState.Released)
                    pressed = false;
            }

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (isVisible)
                spriteBatch.Draw(_texture, Rectangle, Game1.supriser);
        }

        public override void Click()
        {
            throw new NotImplementedException();
        }

        public override void Unclick()
        {
            throw new NotImplementedException();
        }

        public override void Hover()
        {
            throw new NotImplementedException();
        }
    }
}
