using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Party_Animals
{
    public class ObjInBody : Component
    {

        private Rectangle _rectangle;
        private Texture2D _texture;

        public int MoveSpeed;
        public bool isVisible;

        public ObjInBody(Texture2D texture, Rectangle rectangle)
        {
            _texture = texture;
            _rectangle = rectangle;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (isVisible)
                spriteBatch.Draw(_texture, _rectangle, Game1.supriser);
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
