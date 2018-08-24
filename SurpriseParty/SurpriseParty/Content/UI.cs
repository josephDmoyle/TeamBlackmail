using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurpriseParty
{
    class UI : Component
    {
        private int UIMoveSpeed = 3;
        private Texture2D _texture;
        private UI _parent;
        private List<UI> _children;
        private Rectangle _rectangle;
        private Point _centerPoint;
        private bool moving = false;
        private Point _destination;
        public Rectangle Rectangle { get { return _rectangle; } }

        public float alpha;

        public UI(UI parent, Texture2D texture, Point centerPoint)
        {
            // add this UI to parent UI list
            if (parent != null)
                parent.AddChildUI(this);

            _children = new List<UI>();
            _parent = parent;

            _texture = texture;
            _centerPoint = centerPoint;
            _rectangle = new Rectangle(_centerPoint.X - (int)(_texture.Width / 2), _centerPoint.Y - (int)(_texture.Height / 2), (_texture.Width), (_texture.Height));

            alpha = 1;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangle, new Color(Color.White, alpha));
        }

        public override void Update(GameTime gameTime)
        {
            if (moving)
            {

                Vector2 direction = UIMoveSpeed * Vector2.Normalize( new Vector2(_destination.X - _centerPoint.X, _destination.Y - _centerPoint.Y));
                _centerPoint.X += (int)direction.X;
                _centerPoint.Y += (int)direction.Y;

                if (Math.Abs(_centerPoint.X - _destination.X)<2 && Math.Abs(_centerPoint.Y - _destination.Y) < 2)
                {
                    _centerPoint.X = _destination.X;
                    _centerPoint.Y = _destination.Y;
                    // moving done
                    moving = false;
                }
                _rectangle = new Rectangle(_centerPoint.X - (int)(_texture.Width / 2), _centerPoint.Y - (int)(_texture.Height / 2), (_texture.Width), (_texture.Height));

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
            if (_children.Count > 0)
            {
                foreach (var item in _children)
                {
                    item.MoveTo(direction);
                }
            }

        }
    }
}
