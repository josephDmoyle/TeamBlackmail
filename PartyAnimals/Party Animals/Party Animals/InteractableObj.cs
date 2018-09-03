using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Party_Animals
{
    class InteractableObj : Component
    {
        private Texture2D[] _texture;
        private Point _pivotPoint;
        private List<Dragable> _dragables;
        private int _currentInteractID = -1;
        private bool suprisee;
        private Rectangle _rectangle;


        public bool isVisible = true;
        public int DisplayingID { get; set; }
        public int ID { get; set; }
        public Rectangle Rectangle { get { return _rectangle; } }


        public bool Interacted;
        public float alpha = 1;


        public InteractableObj(Texture2D[] textures, Rectangle rect, Point pivot, List<Dragable> list)
        {
            _dragables = list;
            _texture = textures;
            _rectangle = rect;
            _pivotPoint = pivot;
            DisplayingID = 0;
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color = suprisee ? Game1.suprisee : Game1.supriser;
            color = new Color(color, alpha);

            if (isVisible)
                spriteBatch.Draw(_texture[DisplayingID], _rectangle, color);
        }

        public override void Update(GameTime gameTime)
        {
            if (_dragables.Count > 0 && Game1.gameState == 0)
            {
                if (!Interacted)
                {
                    int count = 0;
                    foreach (var item in _dragables)
                    {
                        if (item.Rectangle.Contains(_pivotPoint))
                        {
                            count++;
                            // if mouse is pressed and image is inside the pivot
                            // set image to 1
                            if (Game1.currentMouseState.LeftButton.Equals(ButtonState.Pressed))
                                DisplayingID = 1;

                            if (Game1.currentMouseState.LeftButton.Equals(ButtonState.Released) &&
                           Game1.previousMouseState.LeftButton.Equals(ButtonState.Pressed) &&
                            item.Rectangle.Contains(Game1.currentMouseState.Position))
                            {
                                // if mouse is released and image is inside the pivot
                                // set image to 2, set as interacted
                                DisplayingID = 2;
                                // item -> stop moving
                                Interacted = true;
                                item.isVisible = false;
                                item.StopMovement();
                                item.InSpot = true;
                                Game1.putCount++;
                                //Game1.taskList.taskList[item.ID].ChangeTaskStatus(true);
                                _currentInteractID = item.ID;
                                break;
                            }
                        }
                    }
                    if (count == 0)
                        DisplayingID = 0;
                }
                else if (Interacted)
                {
                    foreach (var item in _dragables)
                    {
                        if (item.Rectangle.Contains(_pivotPoint)
                            && Game1.currentMouseState.LeftButton.Equals(ButtonState.Pressed)
                            && Dragable.currentDraggingID == _currentInteractID &&
                            item.Rectangle.Contains(Game1.currentMouseState.Position))
                        {
                            // if mouse is pressed and image is inside the pivot
                            // set image to 1
                            DisplayingID = 1;
                            item.isVisible = true;
                            // item -> return to move
                            item.StartMovement();
                            item.InSpot = false;
                            Interacted = false;
                            Game1.putCount--;
                            //Game1.taskList.taskList[item.ID].ChangeTaskStatus(false);

                            _currentInteractID = -1;
                        }
                    }
                }
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
    }

}
