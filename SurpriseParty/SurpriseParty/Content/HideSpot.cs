using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SurpriseParty
{
   public class HideSpot : Component
    {
        private Texture2D _texture;
        private int _rectLength;
        private Point _centerPoint;
        private Rectangle _rectangle;

        public bool isPutDown;

        public int ID { get; set; }

      private  bool isVisible;

        public Rectangle Rectangle { get { return _rectangle; } }
        public Point CenterPoint { get {return _centerPoint; } }
        public HideSpot(Texture2D texture, Point centerPoint)
        {
            _texture = texture;
            _rectLength = 20;
            _centerPoint = centerPoint;
            _rectangle = new Rectangle(_centerPoint.X - (int)(_rectLength / 2), _centerPoint.Y - (int)(_rectLength / 2), _rectLength, _rectLength);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            if(isVisible)
            spriteBatch.Draw(_texture, _rectangle, Game1.supriser);
        }

        public override void Update(GameTime gameTime)
        {
            
        }

    }
}
