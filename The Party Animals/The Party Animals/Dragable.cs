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
    public class Dragable : Component
    {
        public static int currentDraggingID;

        #region Fields
        private Texture2D[] _textures;
        private Rectangle initialPosition;
        private int _centerLength = 5;
        //private bool flipped;
        private bool move;
        private bool _collided;
        //
        public bool interacted;
        public ObjInBody objinbody;
        public List<ObjInBody> objInBodies;
        public Point holdPoint;

        private Vector2 position;
        Color color = Color.White;
        bool clicking = false, hovering = false;

        public int MoveSpeed;
        #endregion

        #region Properties

        public bool isVisible;
        public int DisplayingID { get; set; }
        public bool canPut;
        public bool Clicked { get; set; }
        public bool InSpot;
        public Rectangle CenterRect
        {
            get; set;
        }
        public int ID;

        public Rectangle Rectangle;
        #endregion

        #region Methods
        /// <summary>
        /// Object that you can move
        /// </summary>
        /// <param name="texture">Array of images that make up the character</param>
        /// <param name="defaultPos"> x, y, width, height </param>
        public Dragable(Texture2D[] texture, Rectangle defaultPos)
        {
            _textures = texture;
            initialPosition = defaultPos;
            Rectangle = initialPosition;
            CenterRect = new Rectangle((int)(defaultPos.X + defaultPos.Width / 2) - _centerLength, (int)(defaultPos.Y + defaultPos.Height / 2) - _centerLength, _centerLength, _centerLength);

            isVisible = true;
            MoveSpeed = Game1.GetRandomNumber(4, 7);
            move = true;
            position = new Vector2(initialPosition.X, initialPosition.Y);
            ChangeDirection();
            objInBodies = new List<ObjInBody>();
        }

        public override void Update(GameTime gameTime)
        {
            if (isVisible && clicking && Game1.gameState == 0)
            {
                move = false;
                position = new Vector2(Game1.currentMouseState.X - _textures[DisplayingID].Width / 2, Game1.currentMouseState.Y - _textures[DisplayingID].Height / 2);
                Rectangle = new Rectangle((int)position.X, (int)position.Y, _textures[DisplayingID].Width, _textures[DisplayingID].Height);
                CenterRect = new Rectangle((int)(Rectangle.X + Rectangle.Width / 2) - _centerLength, (int)(Rectangle.Y + Rectangle.Height / 2) - _centerLength, _centerLength, _centerLength);
            }
            // Movement
            else if (isVisible && move && Game1.gameState == 0)
            {

                position += Vector2.Normalize(new Vector2(currentDirecton.X, currentDirecton.Y)) * MoveSpeed;
                Rectangle = new Rectangle((int)position.X, (int)position.Y, _textures[DisplayingID].Width, _textures[DisplayingID].Height);
                CenterRect = new Rectangle((int)(Rectangle.X + Rectangle.Width / 2) - _centerLength, (int)(Rectangle.Y + Rectangle.Height / 2) - _centerLength, _centerLength, _centerLength);

                if (Game1.GetRandomNumber(0, 240) == 5 && !_collided)
                    ChangeDirection();

                CheckCollision();
                hovering = false;
            }

            // obj in hand
            /*if (!interacted && objInBodies.Count > 0)
            {
                foreach (var item in objInBodies)
                {
                    if (item.Rectangle.Contains(new Rectangle(holdPoint.X + Rectangle.X - 10, holdPoint.Y + Rectangle.Y - 10, 20, 20)))
                    {
                        if (Game1.currentMouseState.LeftButton.Equals(ButtonState.Pressed) &&
                        item.Rectangle.Contains(Game1.currentMouseState.Position))
                        {
                            interacted = true;
                            item.Rectangle = new Rectangle(holdPoint.X - item.Rectangle.Width / 2, holdPoint.Y - item.Rectangle.Height / 2, item.Rectangle.Width, item.Rectangle.Height);
                            objinbody = item;
                            item.InSpot = true;
                            item.RenderOrder = RenderOrder + 1;
                            break;
                        }
                    }
                }
            }*/

            if (interacted)
            {
                objinbody.Rectangle = new Rectangle(Rectangle.X + holdPoint.X - objinbody.Rectangle.Width / 2, Rectangle.Y + holdPoint.Y - objinbody.Rectangle.Height / 2, objinbody.Rectangle.Width, objinbody.Rectangle.Height);
            }
        }
        /// <summary>
        /// Render the object into the scene
        /// </summary>
        /// <param name="gameTime">Time of the game</param>
        /// <param name="spriteBatch">Spritebatch that is rendering this</param>
        /// 
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            color = Game1.supriser;
            if (hovering)
                color = Color.Gray;
            if (isVisible)
                spriteBatch.Draw(_textures[DisplayingID], Rectangle, color);
            hovering = false;
        }

        public override void Click()
        {
            clicking = true;
            move = false;
        }

        public override void Unclick()
        {
            clicking = false;
            move = true;

        }

        public override bool Hover(Rectangle mouseRect)
        {
            if (mouseRect.Intersects(Rectangle) && isVisible)
            {
                hovering = true;
                return true;
            }
            return false;
        }

        public void MoveToCenterOfSpotPoint(Point point)
        {
            if (Game1.gameState == 0)
                Rectangle = new Rectangle(point.X - _textures[DisplayingID].Width / 2, point.Y - _textures[DisplayingID].Height / 2, _textures[DisplayingID].Width, _textures[DisplayingID].Height);
        }

        private Point currentDirecton;
        void ChangeDirection()
        {
            currentDirecton = new Point(Game1.GetRandomNumber(-3, 3), Game1.GetRandomNumber(-3, 3));
        }

        public void StartMovement()
        {
            move = true;
            ChangeDirection();
        }

        public void StopMovement()
        {
            move = false;
        }

        /// <summary>
        /// Sees if there is a collision
        /// </summary>
        /// <returns>0 left, 1 top, 2 right, 3 down</returns>
        int CheckCollision()
        {
            if (Rectangle.Y > Game1.ObjectSpace.Bottom)
            {
                _collided = true;
                currentDirecton.Y *= -1;
                return 3;
            }
            if (Rectangle.X > Game1.ObjectSpace.Right)
            {
                _collided = true;
                currentDirecton.X *= -1;
                return 2;
            }
            if (Rectangle.X < Game1.ObjectSpace.Left)
            {
                _collided = true;
                currentDirecton.X *= -1;
                return 1;
            }
            if (Rectangle.Y < Game1.ObjectSpace.Top)
            {
                _collided = true;
                currentDirecton.Y *= -1;
                return 0;
            }
            _collided = false;
            return -1;
        }


        #endregion
    }
}
