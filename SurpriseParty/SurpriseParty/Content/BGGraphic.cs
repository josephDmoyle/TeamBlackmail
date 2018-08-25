using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace SurpriseParty
{
    class BGGraphic: Component
    {
        // fields
        private Rectangle _rectangle;
        private Texture2D _texture;

        
        public BGGraphic(Texture2D texture, Rectangle rect)
        {
            _texture = texture;
            _rectangle = rect;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangle, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
