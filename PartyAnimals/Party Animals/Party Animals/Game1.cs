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

        public static int mouseInLeve;
        public static Color supriser = Color.White, suprisee = Color.White;
        public static int GameScene = 0, gameState = 0;
        public static MouseState currentMouseState;
        public static MouseState previousMouseState;
        public static Rectangle[] ObjectMovingRestrictionList = new Rectangle[]
        {
            new Rectangle(247,219,893,286)
        };
        public static int putCount = 0;
        public static bool LevelFinish = false;

        private const int ScreenHeight = 720, ScreenWidth = 1280;

        /// <summary>
        /// Developer variable whether you want to hear the music or not
        /// </summary>
        private readonly bool playMusic = true;

        /// <summary>
        /// How long the door is open until the cat is set
        /// </summary>
        private readonly TimeSpan DoorOpenToComeIn = TimeSpan.FromSeconds(2);

        /// <summary>
        /// How long until the cat arrives
        /// </summary>
        private readonly TimeSpan FirstLeg = TimeSpan.FromSeconds(4), SecondLeg = TimeSpan.FromSeconds(4);

        private readonly TimeSpan FrameRate = TimeSpan.FromMilliseconds(100);

        GraphicsDeviceManager graphics;

        /// <summary>
        /// Used to draw all sprites to
        /// </summary>
        SpriteBatch spriteBatch;

        private TimeSpan SceneStart, IntervalSpan, IntervalSpan2;

        private Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

        private SoundEffectInstance musicPlayer;

        // Player Input - Mouse
        Vector2 mousePosition;

        #region Parameter_Scene1

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

        //public static TaskList taskList;
        public static int isDragging = -1;

        // UI
        List<Dragable> animals;

        List<Component> components;
        List<InteractableObj> interactObjs;

        bool doorOpened, doorOpening, halfTime;

        #endregion

        #region Parameter_LoadingScene
        BGGraphic van, road;
        TimeSpan vanFrame, goFrame;
        #endregion

        #region Parameter_Scene2
        InteractableObj lounge;
        #endregion

        GameTime _gameTime;
        Random rd;
        List<InteractableObj> interactableObjs2;
        ObjInBody[] horns;
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
            sounds["beginningSong"] = Content.Load<SoundEffect>("SFX/beginningSong");
            sounds["supriseSong"] = Content.Load<SoundEffect>("SFX/supriseSong");
            sounds["scream"] = Content.Load<SoundEffect>("SFX/scream");
            sounds["comeOn"] = Content.Load<SoundEffect>("SFX/come-on");
            sounds["goGoGo"] = Content.Load<SoundEffect>("SFX/go-go-go");
            sounds["shush"] = Content.Load<SoundEffect>("SFX/shush");
            sounds["stab"] = Content.Load<SoundEffect>("SFX/stab");
            sounds["vanStart"] = Content.Load<SoundEffect>("SFX/vanStart");

            // TODO: use this.Content to load your game content here

            switch (GameScene)
            {
                case 0:
                    {
                        Load_0();
                        break;
                    }
                case 1:
                    {
                        Load_1();
                        break;
                    }
                case 2:
                    {
                        Load_2();
                        break;
                    }
                case 3:
                    {
                        Load_3();
                        break;

                    }
                default:
                    break;
            }

        }

        void Load_0()
        {
            musicPlayer = sounds["beginningSong"].CreateInstance();
            if (playMusic)
                musicPlayer.Play();
            SceneStart = new TimeSpan();
            vanFrame = SceneStart + FrameRate;
            road = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/road_0") }, new Rectangle(0, 0, 1280, 720))
            {
                RenderOrder = 2
            };
            van = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/van_0"), Content.Load<Texture2D>("Graphics/van_1")}, new Rectangle(0, 0, 1280, 720))
            {
                RenderOrder = 3,
                ID = 0
            };
            spaceBar = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/space") }, new Rectangle(1079, 564, 165, 132))
            {
                RenderOrder = 4
            };
            components = new List<Component>()
            {
                van, road, spaceBar
            };
        }

        void Load_1()
        {
            #region Scene1_LoadContent

            sounds["comeOn"].Play();
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
                new Dragable(new Texture2D[] { Content.Load<Texture2D>("Graphics/Bear_0"), Content.Load<Texture2D>("Graphics/Bear_1") },
                 new Rectangle(rd.Next(ObjectMovingRestrictionList[0].X,ObjectMovingRestrictionList[0].X+ObjectMovingRestrictionList[0].Width), rd.Next(ObjectMovingRestrictionList[0].X,ObjectMovingRestrictionList[0].Y+ObjectMovingRestrictionList[0].Height), 100, 100))
                {
                RenderOrder = 4,
                ID = 2,
                name = "Bear"
                },
            };
            /*
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
            */
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
            cat = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/cat_0"), Content.Load<Texture2D>("Graphics/cat_1"), Content.Load<Texture2D>("Graphics/cat_2"), Content.Load<Texture2D>("Graphics/cat_3") }, new Rectangle(0, 265, 152, 180))
            {
                RenderOrder = -1,
                DisplayingID = 3,
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

        void Load_2()
        {
            vanFrame = SceneStart + FrameRate;
            road = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/road_0"), Content.Load<Texture2D>("Graphics/road_1"), Content.Load<Texture2D>("Graphics/road_2"), Content.Load<Texture2D>("Graphics/road_3"), Content.Load<Texture2D>("Graphics/road_4"), }, new Rectangle(0, 0, 1280, 720))
            {
                RenderOrder = 2
            };
            van = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/van_0"), Content.Load<Texture2D>("Graphics/van_2"), Content.Load<Texture2D>("Graphics/van_3"), Content.Load<Texture2D>("Graphics/van_4") }, new Rectangle(0, 0, 1280, 720))
            {
                RenderOrder = 3,
                ID = 0
            };
            spaceBar = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/space") }, new Rectangle(1079, 564, 165, 132))
            {
                RenderOrder = 4,
            };
            components = new List<Component>()
            {
                van, road, spaceBar
            };
        }

        void Load_3()
        {
            doorOpened = false;
            doorOpening = false;
            LevelFinish = false;
            gameState = 0;
            putCount = 0;
            animals = new List<Dragable>
            {
                 new Dragable(new Texture2D[] { Content.Load<Texture2D>("Graphics/owl_grey_0"), Content.Load<Texture2D>("Graphics/owl_grey_1") },
                 new Rectangle(rd.Next(ObjectMovingRestrictionList[0].X,ObjectMovingRestrictionList[0].X+ObjectMovingRestrictionList[0].Width), rd.Next(ObjectMovingRestrictionList[0].Y,ObjectMovingRestrictionList[0].Y+ObjectMovingRestrictionList[0].Height), 171, 216))
                {
                RenderOrder = 4,
                ID = 0,
                name = "Snowy",
                holdPoint = new Point(98,65)

                },
                new Dragable(new Texture2D[] { Content.Load<Texture2D>("Graphics/Fox_Peach_0"), Content.Load<Texture2D>("Graphics/Fox_Peach_1") },
                 new Rectangle(rd.Next(ObjectMovingRestrictionList[0].X,ObjectMovingRestrictionList[0].X+ObjectMovingRestrictionList[0].Width), rd.Next(ObjectMovingRestrictionList[0].X,ObjectMovingRestrictionList[0].Y+ObjectMovingRestrictionList[0].Height), 180, 184))
                {
                RenderOrder = 4,
                ID = 1,
                name = "Hammy",
                holdPoint = new Point(58,92)

                },
                new Dragable(new Texture2D[] { Content.Load<Texture2D>("Graphics/Bear_0"), Content.Load<Texture2D>("Graphics/Bear_1") },
                 new Rectangle(rd.Next(ObjectMovingRestrictionList[0].X,ObjectMovingRestrictionList[0].X+ObjectMovingRestrictionList[0].Width), rd.Next(ObjectMovingRestrictionList[0].X,ObjectMovingRestrictionList[0].Y+ObjectMovingRestrictionList[0].Height), 200, 200))
                {
                RenderOrder = 4,
                ID = 2,
                name = "Barny",
                holdPoint = new Point(49,61)
                },
            };

            /*
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
            */

            var room = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/room2") }, new Rectangle(0, 0, 1280, 720))
            {
                RenderOrder = 0
            };

            curtains = new BGGraphic[]
            {
                new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/curtain_0"),  Content.Load<Texture2D>("Graphics/curtain_1") }, new Rectangle(265,58,175,258)){ ID = 20},
                new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/curtain_0"),  Content.Load<Texture2D>("Graphics/curtain_1") }, new Rectangle(567,58,175,258)){ ID = 21}

            };
            spaceBar = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/space") }, new Rectangle(1079, 564, 165, 132))
            {
                RenderOrder = 4,
                isVisible = false,
                suprisee = true
            };
            lightOff = new Button(Content.Load<Texture2D>("Graphics/LightButton"))
            {
                RenderOrder = 5,
                Position = new Vector2(1000, 180)

            };
            door = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/door_0"), Content.Load<Texture2D>("Graphics/door_1") }, new Rectangle(811, 130, 143, 230))
            {
                RenderOrder = 0
            };
            confetti = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/confetti") }, new Rectangle(0, 0, ScreenWidth, ScreenHeight))
            {
                RenderOrder = 100,
                isVisible = false
            };
            cat = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/cat_0"), Content.Load<Texture2D>("Graphics/cat_1"), Content.Load<Texture2D>("Graphics/cat_2"), Content.Load<Texture2D>("Graphics/cat_3") }, new Rectangle(0, 265, 152, 180))
            {
                RenderOrder = -1,
                DisplayingID = 3,
                suprisee = true,
                MoveSpeed = 2
            };

            lounge = new InteractableObj(new Texture2D[] { Content.Load<Texture2D>("Graphics/Lounge_0"), Content.Load<Texture2D>("Graphics/Lounge_1"), Content.Load<Texture2D>("Graphics/Lounge_2") }, new Rectangle(325, 251, 398, 219), new Point(467 , 354), animals)
            {
                ID = 0,
                RenderOrder = 3
            };
          var  rug2 = new InteractableObj(new Texture2D[] { Content.Load<Texture2D>("Graphics/rug_0"), Content.Load<Texture2D>("Graphics/rug_1"), Content.Load<Texture2D>("Graphics/rug_2") }, new Rectangle(521, 452, 366, 366), new Point(615, 665), animals)
            {
                ID = 1,
                RenderOrder = 3
            };
            var sofa2 = new InteractableObj(new Texture2D[] { Content.Load<Texture2D>("Graphics/sofa_0"), Content.Load<Texture2D>("Graphics/sofa_1"), Content.Load<Texture2D>("Graphics/sofa_2") }, new Rectangle(100, 360, 352, 352), new Point(258, 548), animals)
            {
                ID = 2,
                RenderOrder = 3
            };

             horns = new ObjInBody[]{
                new ObjInBody(Content.Load<Texture2D>("Graphics/Horn_0"), new Rectangle(100, 600, 50, 50))
            {
                RenderOrder = 3,
                isVisible = true
            }, new ObjInBody(Content.Load<Texture2D>("Graphics/Horn_1"), new Rectangle(200, 400, 50, 50))
            {
                RenderOrder = 3,
                isVisible = true
            },
                new ObjInBody(Content.Load<Texture2D>("Graphics/Horn_2"), new Rectangle(400, 500, 50, 50))
            {
                RenderOrder = 3,
                isVisible = true
            }
            };

            foreach (var item in animals)
            {
                for (int i = 0; i < horns.Length; i++)
                {
                    item.objInBodies.Add(horns[i]);
                }
               
            }

            components = new List<Component>()
            {
                animals[0],
                animals[1],
                animals[2],
                room,
                curtains[0],
                curtains[1],
                spaceBar,
                lightOff,
                door,
                confetti,
                cat,
                horns[0],
                horns[1],
                horns[2],
                lounge,
                rug2,
                sofa2
            };
            interactableObjs2 = new List<InteractableObj>();
            interactableObjs2.Add(lounge);
            interactableObjs2.Add(rug2);
            interactableObjs2.Add(sofa2);

            cat.MoveTo(new Point(170, 142 - 219));
            SceneStart = _gameTime.TotalGameTime;
        }


        #region Scene1_EventMethods

        bool isLightOff;

        private void LightOff_Click(object sender, EventArgs e)
        {
            if (!isLightOff)
            {
                //Game1.taskList.taskList[3].ChangeTaskStatus(true);

                Game1.supriser = Color.Black;
                Game1.suprisee = Color.DarkGray;
                isLightOff = true;
            }
            else
            {
                Game1.supriser = Color.White;
                Game1.suprisee = Color.White;
                //Game1.taskList.taskList[3].ChangeTaskStatus(false);
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

            _gameTime = gameTime;
            switch (GameScene)
            {
                case 0:
                    {
                        Update_0(_gameTime);
                        break;
                    }
                case 1:
                    {
                        Update_1(_gameTime);
                        break;
                    }
                case 2:
                    {
                        Update_2(_gameTime);
                        break;
                    }
                case 3:
                    {
                        Update_3(_gameTime);
                        break;
                    }
            }
            base.Update(gameTime);
        }

        void Update_0(GameTime gameTime)
        {
            if (LevelFinish)
            {
                if (LevelFinish == true && gameTime.TotalGameTime > goFrame)
                {
                    sounds["stab"].Play();
                    SceneStart = gameTime.TotalGameTime;
                    GameScene = 1;
                    LevelFinish = false;
                    Load_1();
                }
                else if (gameTime.TotalGameTime > vanFrame)
                {
                    vanFrame = gameTime.TotalGameTime + FrameRate;
                    van.NextIMG();
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Space) && LevelFinish == false)
            {
                sounds["vanStart"].Play();
                spaceBar.isVisible = false;
                goFrame = gameTime.TotalGameTime + sounds["vanStart"].Duration;
                LevelFinish = true;
            }
        }

        void Update_1(GameTime gameTime)
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
                
                if (gameTime.TotalGameTime > SceneStart + FirstLeg && !halfTime)
                {
                    cat.MoveTo(new Point(800 - 170, 0));
                    sounds["goGoGo"].Play();
                    halfTime = true;
                }
                else if (gameTime.TotalGameTime > SceneStart + FirstLeg + SecondLeg)
                {
                    // cout down finish
                    doorOpening = true;
                    gameState = 1;
                    OpenDoor(gameTime);
                    sounds["shush"].Play();
                    halfTime = false;
                }

            }

            if (!doorOpened && doorOpening)
            {
                if (gameTime.TotalGameTime - IntervalSpan > DoorOpenToComeIn)
                {
                    doorOpened = true;
                    spaceBar.isVisible = true;
                    door.RenderOrder = -2;
                    foreach (var item in animals)
                    {
                        item.StopMovement();
                    }
                    // show rabit
                    cat.isVisible = true;
                }
            }

            if (doorOpened && doorOpening && Keyboard.GetState().IsKeyDown(Keys.Space) && (gameState == 1))
            {
                gameState = 2;
                Game1.supriser = Color.White;
                Game1.suprisee = Color.White;
                CheckResult_Scene1();
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

            if (LevelFinish)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.N))
                {

                    GameScene = 2;
                    Load_2();
                    LevelFinish = false;
                }

            }

            #endregion
        }

        void Update_2(GameTime gameTime)
        {
            if(gameTime.TotalGameTime > vanFrame)
            {
                vanFrame = gameTime.TotalGameTime + FrameRate;
                van.NextIMG();
                road.NextIMG();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                sounds["stab"].Play();
                spaceBar.isVisible = false;
                GameScene = 3;
                Load_3();
            }
        }

        void Update_3(GameTime gameTime)
        {
            _gameTime = gameTime;
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
                if (gameTime.TotalGameTime > SceneStart + FirstLeg && !halfTime)
                {
                    cat.MoveTo(new Point(800 - 170, 0));
                    sounds["goGoGo"].Play();
                    halfTime = true;
                }
                else if (gameTime.TotalGameTime > SceneStart + FirstLeg + SecondLeg)
                {
                    // cout down finish
                    doorOpening = true;
                    gameState = 1;
                    OpenDoor(gameTime);
                    sounds["shush"].Play();
                    halfTime = false;
                }
            }

            if (!doorOpened && doorOpening)
            {
                if ((gameTime.TotalGameTime -  SceneStart) > DoorOpenToComeIn)
                {
                    doorOpened = true;
                    spaceBar.isVisible = true;
                    door.RenderOrder = -2;
                    foreach (var item in animals)
                    {
                        item.StopMovement();
                    }
                    cat.SetIMG(0);

                }
            }
            if (doorOpened && doorOpening && Keyboard.GetState().IsKeyDown(Keys.Space) && (gameState == 1))
            {
                gameState = 2;
                Game1.supriser = Color.White;
                Game1.suprisee = Color.White;


                  CheckResult_Scene2();
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

        }

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
                        Draw_0(gameTime);
                        break;
                    }
                case 1:
                    {
                        Draw_1(gameTime);
                        break;
                    }
                case 2:
                    {
                        Draw_2(gameTime);
                        break;
                    }
                case 3:
                    {
                        Draw_3(gameTime);
                        break;
                    }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        void Draw_0(GameTime gameTime)
        {
            components.Sort((x, y) => x.RenderOrder.CompareTo(y.RenderOrder));
            foreach (var item in components)
            {
                item.Draw(gameTime, spriteBatch);
            }
        }

        void Draw_1(GameTime gameTime)
        {
            components.Sort((x, y) => x.RenderOrder.CompareTo(y.RenderOrder));
            foreach (var item in components)
            {
                item.Draw(gameTime, spriteBatch);
            }
        }

        void Draw_2(GameTime gameTime)
        {

            components.Sort((x, y) => x.RenderOrder.CompareTo(y.RenderOrder));
            foreach (var item in components)
            {
                item.Draw(gameTime, spriteBatch);
            }
        }

        void Draw_3(GameTime gameTime)
        {
            components.Sort((x, y) => x.RenderOrder.CompareTo(y.RenderOrder));
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
            cat.DisplayingID = 0;
            IntervalSpan2 = SceneStart;
            doorOpening = true;

        }

        bool ShowResult;

        void CheckResult_Scene1()
        {
            spaceBar.isVisible = false;
            ShowResult = true;

            if (putCount == 3 && isLightOff)
            {
                cat.SetIMG(1);
                sounds["scream"].Play();
                confetti.isVisible = true;
            }

            else
                cat.SetIMG(2);

            if (!LevelFinish)
                LevelFinish = true;

        }
        void CheckResult_Scene2()
        {
            spaceBar.isVisible = false;
            ShowResult = true;
            foreach (var item in interactableObjs2)
            {
                item.SetIMG(0);
            }
            foreach (var item in horns)
            {
                item.isVisible = true;
            }

            if (putCount == 3 && isLightOff)
            {
                cat.SetIMG(1);
                sounds["scream"].Play();
                confetti.isVisible = true;
            }

            else
                cat.SetIMG(2);

            if (!LevelFinish)
                LevelFinish = true;

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
