﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SurpriseParty
{
    class Dragable : Component
    {
        #region Feilds
        private MouseState _currentState;
        private bool _isHovering;
        private bool _isPressed;
        private MouseState _prevousState;
        private Texture2D _texture;
        private Vector2 _defaultPosition;
        private Vector2 _currentPosition;
        private Rectangle _rectangle;
        #endregion
        #region Properties
        public event EventHandler Press;
        public event EventHandler Release;


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
        public Dragable(Texture2D texture, Vector2 defaultPos)
        {
            _texture = texture;
            _defaultPosition = defaultPos;
            _rectangle = new Rectangle((int)_defaultPosition.X, (int)_defaultPosition.Y, _texture.Width, _texture.Height);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            if (_isHovering)
                color = Color.Gray;
            spriteBatch.Draw(_texture, Rectangle, color);
        }

        public override void Update(GameTime gameTime)
        {
            _prevousState = _currentState;
            _currentState = Mouse.GetState();

            Rectangle mouseRectangele = new Rectangle(_currentState.X, _currentState.Y, 1, 1);

            _isHovering = false;
            if (mouseRectangele.Intersects(Rectangle))
            {
                _isHovering = true;
                if (_currentState.LeftButton == ButtonState.Pressed )
                {
                    _isPressed = true;
                }
                if (_isPressed)
                {
                    _currentPosition = new Vector2(_currentState.X - _texture.Width/2, _currentState.Y - _texture.Height/2);
                    Rectangle = new Rectangle((int)_currentPosition.X, (int)_currentPosition.Y, _texture.Width, _texture.Height);

                }

                if (_currentState.LeftButton == ButtonState.Released && _prevousState.LeftButton == ButtonState.Pressed)
                {
                    _isPressed = false;
                    Release?.Invoke(this, new EventArgs());
                }
            }


        }

        public void GoBacktToOrigin()
        {
            Rectangle = new Rectangle((int)_defaultPosition.X, (int)_defaultPosition.Y, _texture.Width, _texture.Height);
        }

        #endregion
    }
}
