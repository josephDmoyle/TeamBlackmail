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
    public  class Dragable : Component
    {
        #region Fields
        private MouseState _currentState;
        private bool _isHovering;
        private bool _isPressed;
        private MouseState _prevousState;
        private Texture2D[] _textures;
        private Rectangle _defaultPosition;
        private Vector2 _currentPosition;
        private Rectangle _rectangle;
        #endregion
        #region Properties
        public event EventHandler Press;
        public event EventHandler Release;

                public bool isVisible;
        public int DisplayingID { get; set; }
        public bool canPut;
        public bool Clicked { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return _rectangle;
            }
            set
            {
                _rectangle = value;
            }

        }
        #endregion

        #region Methods
        public Dragable(Texture2D[] texture, Rectangle defaultPos)
        {
            _textures = texture;
            _defaultPosition = defaultPos;
            _rectangle = _defaultPosition;
            isVisible = true;
            
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color = Game1.supriser;
            if (_isHovering && (Game1.state == 0))
                color = Color.Gray;
            if(isVisible)
                spriteBatch.Draw(_textures[DisplayingID], Rectangle, color);
        }

        public override void Update(GameTime gameTime)
        {
            _prevousState = _currentState;
            _currentState = Mouse.GetState();

            Rectangle mouseRectangele = new Rectangle(_currentState.X, _currentState.Y, 1, 1);
            if (Game1.state == 0)
            {
                _isHovering = false;
                if (mouseRectangele.Intersects(Rectangle))
                {
                    _isHovering = true;
                }
                if (_currentState.LeftButton == ButtonState.Pressed && !_isPressed)
                {
                    _isPressed = true;
                    Press?.Invoke(this, new EventArgs());
                }
                else if (_currentState.LeftButton == ButtonState.Released && _isPressed)
                {
                    _isPressed = false;
                    Release?.Invoke(this, new EventArgs());
                }
                if (_isPressed)
                {
                    _currentPosition = new Vector2(_currentState.X - _textures[DisplayingID].Width / 2, _currentState.Y - _textures[DisplayingID].Height / 2);
                    Rectangle = new Rectangle((int)_currentPosition.X, (int)_currentPosition.Y, _textures[DisplayingID].Width, _textures[DisplayingID].Height);
                }
            }
        }

        public bool CollidedWithHideSpot(HideSpot hide)
        {
            return Rectangle.Contains(hide.Rectangle);
        }

        public void GoBacktToOrigin()
        {
            if (Game1.state == 0)
                Rectangle = _defaultPosition;
        }

        public void MoveToCenterOfSpotPoint(HideSpot spot)
        {
            if (Game1.state == 0)
                _rectangle = new Rectangle(spot.CenterPoint.X - _textures[DisplayingID].Width/2, spot.CenterPoint.Y - _textures[DisplayingID].Height/2, _textures[DisplayingID].Width, _textures[DisplayingID].Height);
        }

        #endregion
    }
}
