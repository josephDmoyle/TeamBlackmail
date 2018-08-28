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
        private int _centerLength;
       
        #endregion
        #region Properties
        public event EventHandler<IntEventArgs> Press;
        public event EventHandler<IntEventArgs> Release;

                public bool isVisible;
        public int DisplayingID { get; set; }
        public bool canPut;
        public bool Clicked { get; set; }
        public Rectangle CenterRect {
            get; set;
        }
        public int ID;

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
            CenterRect = new Rectangle((int)(defaultPos.X + defaultPos.Width / 2) - _centerLength, (int)(defaultPos.Y + defaultPos.Height /2) - _centerLength, _centerLength, _centerLength);

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
                if (mouseRectangele.Intersects(Rectangle) && Game1.isDragging == -1)
                {
                    _isHovering = true;
                }
                if (_currentState.LeftButton == ButtonState.Pressed && !_isPressed && _isHovering)
                {
                    _isPressed = true;
                    Game1.isDragging = ID;
                    Press?.Invoke(this, new IntEventArgs(ID));
                }
                else if (_currentState.LeftButton == ButtonState.Released && _isPressed)
                {
                    _isPressed = false;
                    Game1.isDragging = -1;
                    Release?.Invoke(this, new IntEventArgs(ID));
                }
                if (_isPressed  && Game1.isDragging == ID)
                {
                    _currentPosition = new Vector2(_currentState.X - _textures[DisplayingID].Width / 2, _currentState.Y - _textures[DisplayingID].Height / 2);
                    Rectangle = new Rectangle((int)_currentPosition.X, (int)_currentPosition.Y, _textures[DisplayingID].Width, _textures[DisplayingID].Height);
                    CenterRect = new Rectangle((int)(Rectangle.X + Rectangle.Width / 2) - _centerLength, (int)(Rectangle.Y + Rectangle.Height / 2) - _centerLength, _centerLength, _centerLength);

                }
            }
        }

       /* public void GoBacktToOrigin()
        {
            if (Game1.state == 0)
                Rectangle = _defaultPosition;
        }*/

        public void MoveToCenterOfSpotPoint(Point point)
        {
            if (Game1.state == 0)
                _rectangle = new Rectangle(point.X - _textures[DisplayingID].Width/2, point.Y - _textures[DisplayingID].Height/2, _textures[DisplayingID].Width, _textures[DisplayingID].Height);
        }

        #endregion
    }
}
