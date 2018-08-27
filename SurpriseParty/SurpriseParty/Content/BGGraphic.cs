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


        public bool isVisible;
        public int DisplayingID { get; set; }
        public int ID { get; set; }
        public Rectangle Rectangle { get { return _rectangle; } }

        public BGGraphic(Texture2D[] textures, Rectangle rect)
        {
            _texture = textures;
            _rectangle = rect;
            isVisible = true;
        }



        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (isVisible)
                spriteBatch.Draw(_texture[DisplayingID], _rectangle, Color.White);
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
    }
}
