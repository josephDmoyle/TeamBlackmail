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
        private int UIMoveSpeed = 2;
        private Texture2D _texture;
        private UI _parent;
        private List<UI> _children;
        private Rectangle _rectangle;
        private Point _centerPoint;
     private   bool moving = false;
        private Point _destination;
        public Rectangle Rectangle { get { return _rectangle; } }

        public float alpha;

        public UI (UI parent, Texture2D texture, Point centerPoint)
        {
            _children = new List<UI>();
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
            if (moving)
            {

                

                if(_centerPoint.X == _destination.X && _centerPoint.Y == _destination.Y)
                {
                    // moving done
                    moving = false;
                }
            }
        }

        public void AddChildUI(UI ui)
        {
            _children.Add(ui);
        }

        public void MoveTo(Point direction)
        {
            moving = true;
            _destination = _centerPoint + direction;
        }
    }
}
