using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurpriseParty.Content
{
    class UI:Component
    {

        private Texture2D _texture;
        private UI _parent;
        private Rectangle _rectangle;
        private Point _centerPoint;

        public Rectangle Rectangle { get { return _rectangle; } }

        public float alpha;

        public UI (UI parent, Texture2D texture, Point centerPoint)
        {
            _parent = parent;
            _texture = texture;
            _rectangle = new Rectangle(_centerPoint.X - (int)(_texture.Width / 2), _centerPoint.Y - (int)(_rectangle.Height / 2), (_texture.Width / 2), (_texture.Height / 2));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangle, new Color(Color.White, alpha));
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
