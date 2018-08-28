using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using System;
using System.Collections.Generic;
using System.Collections;
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
        public static Color supriser = Color.White, suprisee = Color.White;
        public static int state = 0;

        const int ScreenWidth = 1280;
        const int ScreenHeight = 720;

        private bool playMusic = true;

        private readonly TimeSpan DoorOpenToComeIn = TimeSpan.FromSeconds(2);
        private readonly TimeSpan PlaceObjectTime = TimeSpan.FromSeconds(10);

        private TimeSpan timeStartToOpenDoor;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Player Input - Keyboard
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        private SoundEffect beginningSong, waitSong, supriseSong, scream;
        private SoundEffectInstance musicPlayer;

        // Player Input - Mouse
        MouseState currentMouseState;
        MouseState previousMouseState;
        Vector2 mousePosition;

        // graphic component
        BGGraphic boxes;
        BGGraphic sofa;
        BGGraphic door;
        BGGraphic cat;
        BGGraphic rug;

        // UI
        Dragable dragFox;
        UI downUI;
        UI[] countDown;
        List<Component> components;
        List<HideSpot> hideSpots;



        // interaction
        List<Interaction> interactions;
        HideSpot tempSpot;

        bool doorOpened;
        bool doorOpening;

        GameTime _gameTime;

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

            beginningSong = Content.Load<SoundEffect>("SFX/beginningSong");
            waitSong = Content.Load<SoundEffect>("SFX/waitSong");
            supriseSong = Content.Load<SoundEffect>("SFX/supriseSong");
            scream = Content.Load<SoundEffect>("SFX/scream");
            musicPlayer = beginningSong.CreateInstance();
            if (playMusic)
                musicPlayer.Play();
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

            door = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/door_0"), Content.Load<Texture2D>("Graphics/door_1") }, new Rectangle(811, 125, 143, 230))
            {
                RenderOrder = 1
            };
            cat = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/cat_0"), Content.Load<Texture2D>("Graphics/cat_1"), Content.Load<Texture2D>("Graphics/cat_2") }, new Rectangle(811, 125, 143, 230))
            {
                RenderOrder = 2,
                suprisee = true

            };
            cat.isVisible = false;

            var room = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/room") }, new Rectangle(0, 0, 1280, 720))
            {
                RenderOrder = 0
            };
            boxes = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/box_0"), Content.Load<Texture2D>("Graphics/box_1"), Content.Load<Texture2D>("Graphics/box_2") }, new Rectangle(780, 339, 300, 300))
            {
                ID = 0,
                RenderOrder = 1
            };
            var spotPoint_box = new HideSpot(Content.Load<Texture2D>("Graphics/TempUI"), new Point(900, 489))
            {
                ID = 0,
                RenderOrder = 4
            };

            sofa = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/sofa_0"), Content.Load<Texture2D>("Graphics/sofa_1"), Content.Load<Texture2D>("Graphics/sofa_2") }, new Rectangle(420, 189, 300, 300))
            {
                ID = 1,
                RenderOrder = 1
            };
            var spotPoint_Sofa = new HideSpot(Content.Load<Texture2D>("Graphics/TempUI"), new Point(550, 339))
            {
                ID = 1,
                RenderOrder = 4
            };

            rug = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/rug_0"), Content.Load<Texture2D>("Graphics/rug_1"), Content.Load<Texture2D>("Graphics/rug_2") }, new Rectangle(320, 389, 300, 300))
            {
                ID = 2,
                RenderOrder = 1
            };
            var spotPoint_Rug = new HideSpot(Content.Load<Texture2D>("Graphics/TempUI"), new Point(450, 539))
            {
                ID = 2,
                RenderOrder = 4
            };

            dragFox = new Dragable(new Texture2D[] { Content.Load<Texture2D>("Graphics/fox_0"), Content.Load<Texture2D>("Graphics/fox_1") }, new Rectangle(485, 486, 171, 216))
            {
                RenderOrder = 6
            };

            dragFox.Press += DragItem_Press;
            dragFox.Release += DragItem_Release;

            // UI Elements
            downUI = new UI(null, Content.Load<Texture2D>("Graphics/Down_UI"), new Point(535, 646))
            {
                RenderOrder = 5,
                UIMoveSpeed = 3
            };
            countDown = new UI[] {
                new UI(null,Content.Load<Texture2D>("Graphics/Back_coutdown_UI"), new Point(273, 27))
                {
                    RenderOrder = 5,
                    UIMoveSpeed = 3
                },
                new UI(null,Content.Load<Texture2D>("Graphics/Top_coutdown_UI"), new Point(273, 27))
                {
                    RenderOrder = 6,
                    UIMoveSpeed =1
                },
            };




            components = new List<Component>()
            {
                door,
                cat,
                room,
                boxes,
                sofa,
                rug,
                spotPoint_box,
                spotPoint_Sofa,
                spotPoint_Rug,
                downUI,
                countDown[0],
                countDown[1],
                dragFox,
                boxButton
            };


            // sort the list by the render order
            components.Sort((x, y) => x.RenderOrder.CompareTo(y.RenderOrder));

            #region interaction list and method
            interactions = new List<Interaction>()
            {
                new Interaction(dragFox, boxes),
                new Interaction(dragFox, sofa),
                new Interaction(dragFox, rug)
            };
            foreach (var item in interactions)
            {
                item.onEnter += OnDrag_Enter;
                item.onExit += OnDrag_Exit;
            }
            #endregion

            hideSpots = new List<HideSpot>()
            {
                spotPoint_box,
                spotPoint_Sofa,
                spotPoint_Rug
            };
            boxButton.Click += OnButton_Click;
            //   exitButton.Click += ExitButton_Click;

            countDown[1].MoveTo(new Point(-546, 0));
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
                    tempSpot = item;
                }

            }
            if (inSpot)
            {
                if (theSpot != null)
                    theSpot.isPutDown = true;


                dragFox.MoveToCenterOfSpotPoint(theSpot);
                interactions[theSpot.ID]._graphic.SetIMG(2);
                dragFox.isVisible = false;
            }
            else
            {
                // go back to UI bar
                dragFox.RenderOrder = downUI.RenderOrder + 1;
                dragFox.GoBacktToOrigin();
                dragFox.isVisible = true;
                tempSpot = null;
            }

        }

        private void DragItem_Press(object sender, EventArgs e)
        {
            dragFox.RenderOrder = downUI.RenderOrder - 1;
            dragFox.isVisible = true;

            if (tempSpot != null)
                interactions[tempSpot.ID]._graphic.SetIMG(1);
        }
        private void OnButton_Click(object sender, EventArgs e)
        {
            // do my stuff here
            // testing for UI
            downUI.MoveTo(new Point(0, 220));
            //  boxes.NextIMG();
        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void OnDrag_Enter(object sender, IntEventArgs e)
        {
            interactions[e.ID]._graphic.SetIMG(1);
        }
        private void OnDrag_Exit(object sender, IntEventArgs e)
        {
            interactions[e.ID]._graphic.SetIMG(0);
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

            _gameTime = gameTime;

            foreach (var item in components)
            {
                item.Update(gameTime);
            }

            foreach (var item in interactions)
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

            if (!doorOpening)
            {
                if (gameTime.TotalGameTime > PlaceObjectTime)
                {
                    // cout down finish
                    doorOpening = true;
                    state = 1;
                    OpenDoor(gameTime);
                    Game1.supriser = Color.Black;
                    Game1.suprisee = Color.DarkGray;
                    if (playMusic)
                    {
                        musicPlayer.Pause();
                        musicPlayer = waitSong.CreateInstance();
                        musicPlayer.Play();
                    }
                }
            }

            if (!doorOpened && doorOpening)
            {
                if (gameTime.TotalGameTime - timeStartToOpenDoor > DoorOpenToComeIn)
                {
                    doorOpened = true;
                    // show rabit
                    ShowNPC();
                }
            }

            if (doorOpened && doorOpening && Keyboard.GetState().IsKeyDown(Keys.Space) && (state == 1))
            {
                state = 2;
                Game1.supriser = Color.White;
                Game1.suprisee = Color.White;
                if (playMusic)
                {
                    musicPlayer.Pause();
                    musicPlayer = supriseSong.CreateInstance();
                    musicPlayer.Play();
                }
                
                scream.Play();
                CheckResult();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {


            GraphicsDevice.Clear(Color.Gray);

            // sort the layer before draw
            components.Sort((x, y) => x.RenderOrder.CompareTo(y.RenderOrder));

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




        private void OpenDoor(GameTime gameTime)
        {
            door.SetIMG(1);
            timeStartToOpenDoor = gameTime.TotalGameTime;
            doorOpening = true;
            // disapear UI
            downUI.MoveTo(new Point(0, 220));
            countDown[0].MoveTo(new Point(0, -55));
        }

        void ShowNPC()
        {
            cat.isVisible = true;
            // start counting the suprise value

        }

        bool ShowResult;

        void CheckResult()
        {
            // according to the placement of objects and friends, to decide the suprise value
            dragFox.isVisible = true;

            if (tempSpot != null)
            {
                interactions[tempSpot.ID]._graphic.SetIMG(0);
                dragFox.DisplayingID = 1;
            }

            ShowResult = true;


        }
    }
}
