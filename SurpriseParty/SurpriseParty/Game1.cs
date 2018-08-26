using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SurpriseParty
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        const int ScreenWidth = 1280    ;
        const int ScreenHeight = 720;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Player Input - Keyboard
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        private SoundEffect backGroundMusic;

        // Player Input - Mouse
        MouseState currentMouseState;
        MouseState previousMouseState;
        Vector2 mousePosition;

        // graphic component
        BGGraphic boxes;

        // UI
        Dragable dragFox;
        UI downUI;
        List<Component> components;
        List<HideSpot> hideSpots;

        List<Component> interactObj;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = ScreenWidth;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = ScreenHeight;   // set this value to the desired height of your window
            graphics.ApplyChanges();
            base.Initialize();
        }



        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            backGroundMusic = Content.Load<SoundEffect>("SFX/Leopard Print Elevator");
            //backGroundMusic.Play();
            // UIs

            var boxButton = new Button(Content.Load<Texture2D>("Graphics/box1"), Content.Load<SpriteFont>("Font/font"))
            {
                Position = new Vector2(100, 300),
                Text = "Box1",
                RenderOrder = 3
            };
            /*    var exitButton = new Button(Content.Load<Texture2D>("Graphics/drink"), Content.Load<SpriteFont>("Font/font"))
               {
                   Position = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2),
                   Text = "Quit",
               };*/

            var room = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/room") }, new Rectangle(0, 0, 1280, 720))
            {
                RenderOrder = 0
            };
            boxes = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/box_0"), Content.Load<Texture2D>("Graphics/box_1"), Content.Load<Texture2D>("Graphics/box_2") }, new Rectangle(780, 339, 300, 300))
            {
                RenderOrder = 1
            };
            var sofa = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/sofa") }, new Rectangle(462, 260, 165, 165))
            {
                RenderOrder = 2
            };

            var spotPoint = new HideSpot(Content.Load<Texture2D>("Graphics/TempUI"), new Point(529, 394))
            {
                RenderOrder = 4
            };

            dragFox = new Dragable(Content.Load<Texture2D>("Graphics/fox_0"), new Rectangle(485, 586, 112, 147))
            {
                RenderOrder = 6
            };

            dragFox.Press += DragItem_Press;
            dragFox.Release += DragItem_Release;

            // UI Elements
            downUI = new UI(null, Content.Load<Texture2D>("Graphics/Down_UI"), new Point(535, 646))
            {
                RenderOrder = 5
            };
            components = new List<Component>()
            {
                room,
                boxes,
                sofa,
                spotPoint,
                downUI,
                dragFox,
                boxButton
            };

            interactObj = new List<Component>()
            {
                boxes
            };

            // sort the list by the render order
            components.Sort((x, y) => x.RenderOrder.CompareTo(y.RenderOrder));

            hideSpots = new List<HideSpot>()
            {
                spotPoint
            };
            boxButton.Click += OnButton_Click;
         //   exitButton.Click += ExitButton_Click;
        }

        #region EventMethods
        private void DragItem_Release(object sender, EventArgs e)
        {
            bool inSpot = false;
            HideSpot theSpot = null;
            foreach (var item in hideSpots)
            {
                if (dragFox.CollidedWithHideSpot(item))
                {
                    inSpot = true;
                    theSpot = item;
                }

            }
            if (inSpot)
            {
                if (theSpot != null)
                    theSpot.isPutDown = true;


                dragFox.MoveToCenterOfSpotPoint(theSpot);

            }
            else
            {
                // go back to UI bar
                dragFox.GoBacktToOrigin();
            }
        }

        private void DragItem_Press(object sender, EventArgs e)
        {

        }
        private void OnButton_Click(object sender, EventArgs e)
        {
            // do my stuff here
            // testing for UI
            //downUI.MoveTo(new Point(0, 220));
            boxes.NextIMG();
        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            Exit();
        }

        #endregion
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (var item in components)
            {
                item.Update(gameTime);
            }

            // Keyboard staffs
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            // mouse staffs
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            UpdateMouse();

            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {


            GraphicsDevice.Clear(Color.Gray);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            //spriteBatch.Draw(room, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
            foreach (var item in components)
            {
                item.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        void UpdateMouse()
        {

            mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);
            // one click
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {

                //hornPosition = new Point(currentMouseState.X - hornSize.X/2, currentMouseState.Y - hornSize.Y/2);
            }

        }


    }
}
