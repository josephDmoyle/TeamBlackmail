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

namespace Party_Animals
{
    

    public class Game1 : Game
    {


        /// <summary>
        /// Colors of the interior of the room
        /// </summary>
        public static Color supriser = Color.White;
        /// <summary>
        /// Colors of the character which is being suprised
        /// </summary>
        public static Color suprisee = Color.White;
        /// <summary>
        /// Global state of the game
        /// 0 : initial
        /// 1 : door is opened
        /// 2 : cat is waiting
        /// 3 : cat has been suprised
        /// </summary>

        public static int GameScene = 1;
        public static int gameState = 0;
        public static MouseState currentMouseState;
        public static MouseState previousMouseState;
        public static Rectangle[] ObjectMovingRestrictionList = new Rectangle[]
        {
            new Rectangle(247,219,893,286)
        };
        public static int putCount = 0;

        const int ScreenWidth = 1280;
        const int ScreenHeight = 720;

        /// <summary>
        /// Developer variable whether you want to hear the music or not
        /// </summary>
        private readonly bool playMusic = false;

        /// <summary>
        /// How long the door is open until the cat is set
        /// </summary>
        private readonly TimeSpan DoorOpenToComeIn = TimeSpan.FromSeconds(2);
        /// <summary>
        /// How long until the cat arrives
        /// </summary>
        private readonly TimeSpan PlaceObjectTime = TimeSpan.FromSeconds(8);

        GraphicsDeviceManager graphics;
        /// <summary>
        /// Used to draw all sprites to
        /// </summary>
        SpriteBatch spriteBatch;

        #region Parameter_Scene1
        private TimeSpan timeStartToOpenDoor;



        // Player Input - Keyboard



        private SoundEffect beginningSong, supriseSong, scream, comeOn, goGoGo, shush;
        private SoundEffectInstance musicPlayer;

        // Player Input - Mouse

        Vector2 mousePosition;

        // graphic component
        InteractableObj boxes;
        InteractableObj sofa;
        InteractableObj rug;
        BGGraphic balloons;
        BGGraphic confetti;
        BGGraphic door;
        BGGraphic cat;
        Button lightOff;
        BGGraphic spaceBar;

        BGGraphic[] curtains;

        public static TaskList taskList;
        public static int isDragging = -1;

        // UI
        List<Dragable> animals;
        int currentDragID;

        List<Component> components;
        List<InteractableObj> interactObjs;

        bool doorOpened;
        bool doorOpening;
        bool halfTime;

        #endregion
        #region Parameter_LoadingScene
        List<Component> loadingSceneComponts;
        BGGraphic loadingSceneBG;
        #endregion
        #region Parameter_Scene2

        #endregion
        GameTime _gameTime;
        Random rd;
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
            rd = new Random();
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

            LoadContent_Scene1();
            LoadContent_LoadingScene();
        }

        void LoadContent_LoadingScene()
        {
            loadingSceneBG = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/LoadingBackground") }, new Rectangle(0, 0, 1280, 720))
            {
                RenderOrder = 1,
                ID = 0
            };
            loadingSceneComponts = new List<Component>()
            {
                loadingSceneBG
            };
        }
        void LoadContent_Scene1()
        {
            #region Scene1_LoadContent
            beginningSong = Content.Load<SoundEffect>("SFX/beginningSong");
            supriseSong = Content.Load<SoundEffect>("SFX/supriseSong");
            scream = Content.Load<SoundEffect>("SFX/scream");

            comeOn = Content.Load<SoundEffect>("SFX/come-on");
            goGoGo = Content.Load<SoundEffect>("SFX/go-go-go");
            shush = Content.Load<SoundEffect>("SFX/shush");

            musicPlayer = beginningSong.CreateInstance();
            if (playMusic)
                musicPlayer.Play();
            comeOn.Play();
            // UIs
            #region Initialize parameters
            // dragable animals
            animals = new List<Dragable>
            {
                 new Dragable(new Texture2D[] { Content.Load<Texture2D>("Graphics/owl_grey_0"), Content.Load<Texture2D>("Graphics/owl_grey_1") },
                 new Rectangle(rd.Next(ObjectMovingRestrictionList[0].X,ObjectMovingRestrictionList[0].X+ObjectMovingRestrictionList[0].Width), rd.Next(ObjectMovingRestrictionList[0].Y,ObjectMovingRestrictionList[0].Y+ObjectMovingRestrictionList[0].Height), 171, 216))
                {
                RenderOrder = 4,
                ID = 0,
                name = "Snowy"
                },
                new Dragable(new Texture2D[] { Content.Load<Texture2D>("Graphics/hamster_0"), Content.Load<Texture2D>("Graphics/hamster_1") },
                 new Rectangle(rd.Next(ObjectMovingRestrictionList[0].X,ObjectMovingRestrictionList[0].X+ObjectMovingRestrictionList[0].Width), rd.Next(ObjectMovingRestrictionList[0].X,ObjectMovingRestrictionList[0].Y+ObjectMovingRestrictionList[0].Height), 103, 127))
                {
                RenderOrder = 4,
                ID = 1,
                name = "Hammy"
                },
                new Dragable(new Texture2D[] { Content.Load<Texture2D>("Graphics/owl_brown_0"), Content.Load<Texture2D>("Graphics/owl_brown_1") },
                 new Rectangle(rd.Next(ObjectMovingRestrictionList[0].X,ObjectMovingRestrictionList[0].X+ObjectMovingRestrictionList[0].Width), rd.Next(ObjectMovingRestrictionList[0].X,ObjectMovingRestrictionList[0].Y+ObjectMovingRestrictionList[0].Height), 251, 162))
                {
                RenderOrder = 4,
                ID = 2,
                name = "Barny"
                },
            };
            taskList = new TaskList(Content.Load<Texture2D>("Graphics/TaskList"), new Rectangle(29, 20, 147, 200), Content.Load<SpriteFont>("Font/font"))
            {
                RenderOrder = 5,
                taskList = new List<Task>()
            };
            foreach (Dragable animal in animals)
            {
                taskList.taskList.Add(new Task("Hide " + animal.name, animal.ID));
            }
            taskList.taskList.Add(new Task("Turn off the\n         light", animals.Count));
            door = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/door_0"), Content.Load<Texture2D>("Graphics/door_1") }, new Rectangle(811, 130, 143, 230))
            {
                RenderOrder = 0
            };
            confetti = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/confetti") }, new Rectangle(0, 0, ScreenWidth, ScreenHeight))
            {
                RenderOrder = 100,
                isVisible = false
            };
            balloons = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/ballons") }, new Rectangle(480, 100, 143, 230))
            {
                RenderOrder = 1
            };
            cat = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/cat_0"), Content.Load<Texture2D>("Graphics/cat_1"), Content.Load<Texture2D>("Graphics/cat_2") }, new Rectangle(0, 219, 147, 235))
            {
                RenderOrder = -1,
                suprisee = true,
                MoveSpeed = 2
            };
            cat.isVisible = true;

            var room = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/room") }, new Rectangle(0, 0, 1280, 720))
            {
                RenderOrder = 0
            };
            boxes = new InteractableObj(new Texture2D[] { Content.Load<Texture2D>("Graphics/box_0"), Content.Load<Texture2D>("Graphics/box_1"), Content.Load<Texture2D>("Graphics/box_2") }, new Rectangle(780, 339, 300, 300), new Point(780 + 150 - 20, 339 + 150), animals)
            {
                ID = 0,
                RenderOrder = 3
            };

            sofa = new InteractableObj(new Texture2D[] { Content.Load<Texture2D>("Graphics/sofa_0"), Content.Load<Texture2D>("Graphics/sofa_1"), Content.Load<Texture2D>("Graphics/sofa_2") }, new Rectangle(420, 189, 300, 300), new Point(420 + 184, 189 + 226), animals)
            {
                ID = 1,
                RenderOrder = 3
            };

            rug = new InteractableObj(new Texture2D[] { Content.Load<Texture2D>("Graphics/rug_0"), Content.Load<Texture2D>("Graphics/rug_1"), Content.Load<Texture2D>("Graphics/rug_2") }, new Rectangle(320, 389, 300, 300), new Point(126 + 320, 389 + 220), animals)
            {
                ID = 2,
                RenderOrder = 3,
            };
            lightOff = new Button(Content.Load<Texture2D>("Graphics/LightButton"))
            {
                RenderOrder = 5,
                Position = new Vector2(1000, 180)

            };
            lightOff.Click += LightOff_Click;

            spaceBar = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/space") }, new Rectangle(1079, 564, 165, 132))
            {
                RenderOrder = 4,
                isVisible = false,
                suprisee = true
            };

            curtains = new BGGraphic[]
            {
                new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/curtain_0"),  Content.Load<Texture2D>("Graphics/curtain_1") }, new Rectangle(265,58,175,258)){ ID = 20},
                new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/curtain_0"),  Content.Load<Texture2D>("Graphics/curtain_1") }, new Rectangle(567,58,175,258)){ ID = 21}

            };

            foreach (var item in curtains)
            {
                item.OnPress += OnPressCurtain;
            }
            #endregion



            components = new List<Component>()
            {
                door,
                balloons,
                cat,
                room,
                boxes,
                sofa,
                rug,
                lightOff,
                spaceBar,
                taskList,
                confetti,
                curtains[0],
                curtains[1]
            };
            foreach (Dragable animal in animals)
            {
                components.Add(animal);
            }


            // sort the list by the render order
            components.Sort((x, y) => x.RenderOrder.CompareTo(y.RenderOrder));

            #region interaction list and method

            interactObjs = new List<InteractableObj>()
            {
                boxes,
                sofa,
                rug
            };
            #endregion

            cat.MoveTo(new Point(170, 142 - 219));

            #endregion
        }



        #region Scene1_EventMethods
        bool isLightOff;
        private void LightOff_Click(object sender, EventArgs e)
        {
            if (!isLightOff)
            {
                Game1.taskList.taskList[3].ChangeTaskStatus(true);

                Game1.supriser = Color.Black;
                Game1.suprisee = Color.DarkGray;
                isLightOff = true;
            }
            else
            {
                Game1.supriser = Color.White;
                Game1.suprisee = Color.White;
                Game1.taskList.taskList[3].ChangeTaskStatus(false);
                isLightOff = false;
            }

        }

        private void OnPressCurtain(object sender, IntEventArgs e)
        {
            if (curtains[e.ID - 20].DisplayingID == 0)
            {
                curtains[e.ID - 20].DisplayingID = 1;
            }
            else
            {
                curtains[e.ID - 20].DisplayingID = 0;

            }
        }
        private void DragItem_Press(object sender, IntEventArgs e)
        {
            //  animals[e.ID].RenderOrder = downUI.RenderOrder - 1;
            animals[e.ID].isVisible = true;

            if (animals[e.ID]._interaction != null && animals[e.ID]._interaction._graphic.Interacted)
            {
                animals[e.ID]._interaction._graphic.Interacted = false;
                animals[e.ID]._interaction._graphic._interaction = null;
                animals[e.ID]._interaction = null;
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Exit();
        }

        //private void OnDrag_Enter(object sender, IntEventArgs e)
        //{
        //    if (!interactions[e.ID]._graphic.Interacted)
        //        interactions[e.ID]._graphic.SetIMG(1);
        //}
        //private void OnDrag_Exit(object sender, IntEventArgs e)
        //{

        //    if (!interactions[e.ID]._graphic.Interacted)
        //        interactions[e.ID]._graphic.SetIMG(0);

        //}

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
            switch (GameScene)
            {
                case 0:
                    {
                        Update_Scene1(_gameTime);
                        break;
                    }
                case 1:
                    {
                        Update_Scene2(_gameTime);
                        break;
                    }
            }
            base.Update(gameTime);
        }

        void Update_Scene1(GameTime gameTime)
        {
            #region Update_Scene1
            foreach (var item in components)
            {
                item.Update(gameTime);
            }

            // mouse staffs
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            UpdateMouse();

            if (!doorOpening)
            {
                if (!halfTime && gameTime.TotalGameTime > TimeSpan.FromSeconds(PlaceObjectTime.Seconds / 2))
                {
                    halfTime = true;
                    cat.MoveTo(new Point(800 - 170, 0));
                    goGoGo.Play();
                }

                if (gameTime.TotalGameTime > PlaceObjectTime)
                {
                    // cout down finish
                    doorOpening = true;
                    gameState = 1;
                    OpenDoor(gameTime);

                    shush.Play();
                }
            }

            if (!doorOpened && doorOpening)
            {
                if (gameTime.TotalGameTime - timeStartToOpenDoor > DoorOpenToComeIn)
                {
                    doorOpened = true;
                    spaceBar.isVisible = true;
                    door.RenderOrder = -2;
                    foreach (var item in animals)
                    {
                        item.StopMovement();
                    }
                    // show rabit
                    ShowNPC();
                }
            }

            if (doorOpened && doorOpening && Keyboard.GetState().IsKeyDown(Keys.Space) && (gameState == 1))
            {
                gameState = 2;
                Game1.supriser = Color.White;
                Game1.suprisee = Color.White;

                if (playMusic)
                {
                    musicPlayer.Pause();
                    musicPlayer = supriseSong.CreateInstance();
                    musicPlayer.Play();

                }


                CheckResult();
            }

            if (gameState == 2)
            {
                gameState = 3;

                foreach (var item in animals)
                {
                    if (item.InSpot)
                    {
                        item.DisplayingID = 1;
                    }
                    item.isVisible = true;
                }
                foreach (var item in interactObjs)
                {
                    item.SetIMG(0);
                }
            }
            #endregion
        }
        void Update_Scene2(GameTime gameTime) { }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {


            GraphicsDevice.Clear(Color.Gray);

            // sort the layer before draw


            spriteBatch.Begin();
            switch (GameScene)
            {
                case 0:
                    {
                        Draw_Scene1(gameTime);
                        break;
                    }
                case 1:
                    {
                        Draw_LoadingScene(gameTime);
                        break;
                    }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        void Draw_Scene1(GameTime gameTime)
        {
            components.Sort((x, y) => x.RenderOrder.CompareTo(y.RenderOrder));
            foreach (var item in components)
            {
                item.Draw(gameTime, spriteBatch);
            }
        }
        void Draw_Scene2(GameTime gameTime) { }
        void Draw_LoadingScene(GameTime gameTime)
        {
            loadingSceneComponts.Sort((x, y) => x.RenderOrder.CompareTo(y.RenderOrder));
            foreach (var item in components)
            {
                item.Draw(gameTime, spriteBatch);
            }
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
        }

        void ShowNPC()
        {
            cat.isVisible = true;
            // start counting the suprise value

        }

        bool ShowResult;

        void CheckResult()
        {
            spaceBar.isVisible = false;
            ShowResult = true;

            if (putCount == 3 && isLightOff)
            {
                cat.SetIMG(1);
                scream.Play();
                confetti.isVisible = true;
            }

            else
                cat.SetIMG(2);

        }

        private static readonly Random getrandom = new Random();

        public static int GetRandomNumber(int min, int max)
        {
            lock (getrandom) // synchronize
            {
                return getrandom.Next(min, max);
            }
        }
    }
}
