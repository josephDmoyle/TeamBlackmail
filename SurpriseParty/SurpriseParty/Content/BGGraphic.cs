using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace SurpriseParty
{
   public class BGGraphic : Component
    {
        // fields
        private Rectangle _rectangle;
        private Texture2D[] _texture;


        public bool isVisible, suprisee;
        public int DisplayingID { get; set; }
        public int ID { get; set; }
        public Rectangle Rectangle { get { return _rectangle; } }

        public BGGraphic(Texture2D[] textures, Rectangle rect)
        {
            _texture = textures;
            _rectangle = rect;
            isVisible = true;
        }

        public float alpha = 1;
        public Color color;



        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            color = suprisee ? Game1.suprisee : Game1.supriser;
            color = new Color(color, alpha);

            if (isVisible)
                spriteBatch.Draw(_texture[DisplayingID], _rectangle, color);
        }

        public override void Update(GameTime gameTime)
        {
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

        public void ClipImage(float value, GraphicsDevice graphicsDevice)
        {
            // Get your texture

            // Calculate the cropped boundary
            /*  Rectangle newBounds = _texture[DisplayingID].Bounds;

             // newBounds.Y = (int)(value * _texture[DisplayingID].Bounds.Height);
              newBounds.Height = (int)(value * _texture[DisplayingID].Bounds.Height);


              // Create a new texture of the desired size
              Texture2D croppedTexture = new Texture2D(graphicsDevice, newBounds.Width, newBounds.Height);

              // Copy the data from the cropped region into a buffer, then into the new texture
              Color[] data = new Color[newBounds.Width * newBounds.Height];
              _texture[DisplayingID].GetData(0, newBounds, data, 0, newBounds.Width * newBounds.Height);
              croppedTexture.SetData(data);
              */
            Texture2D croppedData = new Texture2D(graphicsDevice, _rectangle.Width, (int)(_rectangle.Height * value));



            _texture[DisplayingID] = croppedData;


        }
    }
}
