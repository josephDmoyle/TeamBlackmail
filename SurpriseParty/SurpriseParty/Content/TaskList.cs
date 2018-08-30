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
        string[] tickOrNot = {"X", "O"};
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

        public void ChangeTaskStatus(bool finished)
        {
            displayString = tickOrNot[finished?1:0] + " " + ID.ToString() + ". " + Name +".";
            Done = finished;

        }
    }

   public class TaskList: Component
    {

        private Texture2D _texture;
        private Rectangle _rectangle;
        private SpriteFont _font;

        private int leftPad = 13;
        private int lineSpace = 30;

        public Task[] taskList;
        
        public TaskList(Texture2D texture2D, Rectangle rectangle, SpriteFont font)
        {
            _texture = texture2D;
            _rectangle = rectangle;
            _font = font;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangle, Game1.suprisee);

            for (int i = 0; i < taskList.Length; i++)
            {
                if (!string.IsNullOrEmpty(taskList[i].displayString))
                {
                    float x = _rectangle.X + leftPad;
                    float y = _rectangle.Y + 50 + lineSpace * i;
                    spriteBatch.DrawString(_font, taskList[i].displayString, new Vector2(x, y), Color.Black);
                }
            }
        }

        public void FinishTask(int i)
        {
            if (taskList.Length > 0)
            {
                taskList[i].ChangeTaskStatus(true);
            }
        }

        public void UnFinishTask(int i)
        {
            if (taskList.Length > 0)
            {
                taskList[i].ChangeTaskStatus(false);

            }
        }
    }
}
