﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Party_Animals
{
    public class BGGraphic : Component
    {
        // fields
        private Rectangle _rectangle;
        private Texture2D[] _texture;
        private bool moving = false, hovering = false, clicking = false;
        private Point _destination;

        public bool isVisible, suprisee;
        public int DisplayingID { get; set; }
        public int ID { get; set; }
        public Rectangle Rectangle { get { return _rectangle; } }

        public Point PivotPoint;
        public bool Interacted;

        public EventHandler<IntEventArgs> OnPress;

        public BGGraphic(Texture2D[] textures, Rectangle rect)
        {
            _texture = textures;
            _rectangle = rect;
            isVisible = true;
        }

        public float alpha = 1;
        public Color color;
        public int MoveSpeed;


        public override void Update(GameTime gameTime)
        {
            if (moving)
            {

                Vector2 direction = MoveSpeed * Vector2.Normalize(new Vector2(_destination.X - _rectangle.X, _destination.Y - _rectangle.Y));
                _rectangle.X += (int)direction.X;
                _rectangle.Y += (int)direction.Y;

                if (Math.Abs(_rectangle.X - _destination.X) < 2 && Math.Abs(_rectangle.Y - _destination.Y) < 2)
                {
                    _rectangle.X = _destination.X;
                    _rectangle.Y = _destination.Y;
                    moving = false;
                }
            }

            Rectangle mouseRectangle = new Rectangle(Game1.currentMouseState.X, Game1.currentMouseState.Y, 1, 1);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            color = suprisee ? Game1.suprisee : Game1.supriser;
            if (hovering)
            {
                color = Color.Gray;
            }
            color = new Color(color, alpha);

            if (isVisible)
                spriteBatch.Draw(_texture[DisplayingID], _rectangle, color);
            hovering = false;
        }

        public override void Click()
        {
            NextIMG();
        }

        public override void Unclick()
        {
        }

        public override bool Hover(Rectangle mouseRect)
        {
            if (mouseRect.Intersects(Rectangle))
            {
                hovering = true;
                return true;
            }
            else
            {
                hovering = false;
                return false;
            }
        }

        public void NextIMG()
        {
            if (DisplayingID < _texture.Length - 1)
                DisplayingID++;
            else
                DisplayingID = 0;
        }

        public void PreviousIMG()
        {
            if (DisplayingID > 0)
                DisplayingID--;
            else
                DisplayingID = _texture.Length - 1;
        }

        public void SetIMG(int _idx)
        {
            if (_idx >= 0 && _idx < _texture.Length)
                DisplayingID = _idx;
        }

        public void MoveTo(Point direction)
        {
            moving = true;
            _destination = new Point(_rectangle.X, _rectangle.Y) + direction;
        }

    }
}
