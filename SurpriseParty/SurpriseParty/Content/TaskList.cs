using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurpriseParty
{
    public class Task
    {
        string[] tickOrNot = {" ", "☑"};
        string Name;
        public int ID;
        public bool Done;
        public string displayString;

        public Task(string _name, int _ID)
        {
            Name = _name;
            ID = _ID;
            Done = false;
            displayString = tickOrNot[0] + " " + ID.ToString() + ". " + Name +".";
        }
    }

   public class TaskList: Component
    {

        private Texture2D _texture;
        private Rectangle _rectangle;
        private SpriteFont _font;

        public Task[] Tasklist;
        
        public TaskList(Texture2D texture2D, Rectangle rectangle, SpriteFont font)
        {
            _texture = texture2D;
            _rectangle = rectangle;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public void FinishTask(int i)
        {
            if (Tasklist.Length > 0)
            {
                Tasklist[i].Done = true;
            }
        }

        public void UnFinishTask(int i)
        {
            if (Tasklist.Length > 0)
            {
                Tasklist[i].Done = false;
            }
        }
    }
}
