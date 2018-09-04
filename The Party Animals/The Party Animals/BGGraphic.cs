using Microsoft.Xna.Framework;
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
        private bool moving = false;
        private Point _destination;

        public bool isVisible, suprisee;
        public int DisplayingID { get; set; }
        public int ID { get; set; }
        public Rectangle Rectangle { get { return _rectangle; } }

        public Point PivotPoint;
        public bool Interacted;

        public Interaction _interaction;

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
                    // moving done
                    moving = false;
                }
            }

            Rectangle mouseRectangle = new Rectangle(Game1.currentMouseState.X, Game1.currentMouseState.Y, 1, 1);


            if (mouseRectangle.Intersects(Rectangle) && Game1.gameState == 0)
            {

                if (Game1.currentMouseState.LeftButton == ButtonState.Released && Game1.previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    OnPress?.Invoke(this, new IntEventArgs(ID));
                }
            }


        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            color = suprisee ? Game1.suprisee : Game1.supriser;
            color = new Color(color, alpha);

            if (isVisible)
                spriteBatch.Draw(_texture[DisplayingID], _rectangle, color);
        }

        public override void Click()
        {
            throw new NotImplementedException();
        }

        public override void Unclick()
        {
            throw new NotImplementedException();
        }

        public override void Hover()
        {
            throw new NotImplementedException();
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
