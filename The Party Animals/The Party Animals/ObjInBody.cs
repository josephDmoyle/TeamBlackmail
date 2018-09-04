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

        private bool pressed, attached, hovering, clicking;

        public ObjInBody(Texture2D texture, Rectangle rectangle)
        {
            _texture = texture;
            Rectangle = rectangle;
        }

        public override void Update(GameTime gameTime)
        {
            if (!InSpot)
            {
                if (clicking && Game1.gameState == 0)
                {
                    Rectangle = new Rectangle(Game1.currentMouseState.X - Rectangle.Width / 2, Game1.currentMouseState.Y - Rectangle.Height / 2, Rectangle.Width, Rectangle.Height);
                }
            }

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color = Game1.supriser;
            if (hovering)
                color = Color.Gray; ;
            if (isVisible)
                spriteBatch.Draw(_texture, Rectangle, color);
            hovering = false;
        }

        public override void Click()
        {
            if (!attached)
            {
                clicking = true;

            }
        }

        public override void Unclick()
        {
            clicking = false;
        }

        public override bool Hover(Rectangle mouseRect)
        {
            if (mouseRect.Intersects(Rectangle) && !attached)
            {
                hovering = true;
                return true;
            }
            return false;
        }
    }
}
