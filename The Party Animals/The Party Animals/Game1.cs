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

namespace The_Party_Animals
{
    public class Game1 : Game
    {
        public static int mouseInLeve;
        public static Color supriser = Color.White, suprisee = Color.White;
        public static int GameScene = 0, gameState = 0;
        public static MouseState currentMouseState;
        public static MouseState previousMouseState;
        public static Rectangle ObjectSpace = new Rectangle(247, 219, 893, 286);
        public static int putCount = 0;
        public static bool LevelFinish = false, lightOn;

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

        private TimeSpan SceneStart, IntervalSpan2;

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
        Button lightSwitch;
        BGGraphic spaceBar;

        List<BGGraphic> curtains;

        //public static TaskList taskList;
        public static int isDragging = -1;

        // UI
        List<Dragable> animals;

        List<Component> components;
        List<InteractableObj> interactObjs;

        bool swapper;

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
            sounds["boo"] = Content.Load<SoundEffect>("SFX/boo");
            sounds["comeOn"] = Content.Load<SoundEffect>("SFX/come-on");
            sounds["goGoGo"] = Content.Load<SoundEffect>("SFX/go-go-go");
            sounds["shush"] = Content.Load<SoundEffect>("SFX/shush");
            sounds["stab"] = Content.Load<SoundEffect>("SFX/stab");
            sounds["vanStart"] = Content.Load<SoundEffect>("SFX/vanStart");

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

            swapper = false;

            road = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/road_0") }, new Rectangle(0, 0, 1280, 720))
            {
                RenderOrder = 1
            };
            van = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/van_0"), Content.Load<Texture2D>("Graphics/van_1") }, new Rectangle(0, 0, 1280, 720))
            {
                RenderOrder = 2
            };
            spaceBar = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/space") }, new Rectangle(1079, 564, 165, 132))
            {
                RenderOrder = 3
            };
            components = new List<Component>()
            {
                van, road, spaceBar
            };
            components.Sort((x, y) => x.RenderOrder.CompareTo(y.RenderOrder));
        }

        void Load_1()
        {
            #region Scene1_LoadContent

            // UIs
            #region Initialize parameters
            gameState = 0;
            putCount = 0;
            swapper = true;
            // dragable animals
            door = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/door_0"), Content.Load<Texture2D>("Graphics/door_1") }, new Rectangle(811, 130, 143, 230))
            {
                RenderOrder = 0
            };

            cat = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/cat_0"), Content.Load<Texture2D>("Graphics/cat_1"), Content.Load<Texture2D>("Graphics/cat_2"), Content.Load<Texture2D>("Graphics/cat_3") }, new Rectangle(0, 265, 152, 180))
            {
                RenderOrder = 1,
                DisplayingID = 3,
                suprisee = true,
                MoveSpeed = 2
            };

            BGGraphic room = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/room") }, new Rectangle(0, 0, 1280, 720))
            {
                RenderOrder = 2
            };

            curtains = new List<BGGraphic>()
            {
                new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/curtain_0"),  Content.Load<Texture2D>("Graphics/curtain_1") }, new Rectangle(265,58,175,258)){ RenderOrder = 3, ID = 20},
                new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/curtain_0"),  Content.Load<Texture2D>("Graphics/curtain_1") }, new Rectangle(567,58,175,258)){ RenderOrder = 3, ID = 21}

            };

            balloons = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/ballons") }, new Rectangle(480, 100, 143, 230))
            {
                RenderOrder = 4
            };

            boxes = new InteractableObj(new Texture2D[] { Content.Load<Texture2D>("Graphics/box_0"), Content.Load<Texture2D>("Graphics/box_1"), Content.Load<Texture2D>("Graphics/box_2") }, new Rectangle(780, 339, 300, 300), new Point(780 + 150 - 20, 339 + 150), animals)
            {
                ID = 0,
                RenderOrder = 5
            };
            sofa = new InteractableObj(new Texture2D[] { Content.Load<Texture2D>("Graphics/sofa_0"), Content.Load<Texture2D>("Graphics/sofa_1"), Content.Load<Texture2D>("Graphics/sofa_2") }, new Rectangle(420, 189, 300, 300), new Point(420 + 184, 189 + 226), animals)
            {
                ID = 1,
                RenderOrder = 5
            };
            rug = new InteractableObj(new Texture2D[] { Content.Load<Texture2D>("Graphics/rug_0"), Content.Load<Texture2D>("Graphics/rug_1"), Content.Load<Texture2D>("Graphics/rug_2") }, new Rectangle(320, 389, 300, 300), new Point(126 + 320, 389 + 220), animals)
            {
                ID = 2,
                RenderOrder = 5
            };
            lightSwitch = new Button(Content.Load<Texture2D>("Graphics/switch_1"), Content.Load<Texture2D>("Graphics/switch_0"))
            {
                RenderOrder = 5,
                Position = new Vector2(1000, 180)
            };

            animals = new List<Dragable>
            {
                 new Dragable(new Texture2D[] { Content.Load<Texture2D>("Graphics/owl_grey_0"), Content.Load<Texture2D>("Graphics/owl_grey_1") },
                 new Rectangle(rd.Next(ObjectSpace.X,ObjectSpace.X+ObjectSpace.Width), rd.Next(ObjectSpace.Y,ObjectSpace.Y+ObjectSpace.Height), 171, 216))
                {
                RenderOrder = 6,
                ID = 0
                },
                new Dragable(new Texture2D[] { Content.Load<Texture2D>("Graphics/hamster_0"), Content.Load<Texture2D>("Graphics/hamster_1") },
                 new Rectangle(rd.Next(ObjectSpace.X,ObjectSpace.X+ObjectSpace.Width), rd.Next(ObjectSpace.X,ObjectSpace.Y+ObjectSpace.Height), 103, 127))
                {
                RenderOrder = 6,
                ID = 1
                },
                new Dragable(new Texture2D[] { Content.Load<Texture2D>("Graphics/Bear_0"), Content.Load<Texture2D>("Graphics/Bear_1") },
                 new Rectangle(rd.Next(ObjectSpace.X,ObjectSpace.X+ObjectSpace.Width), rd.Next(ObjectSpace.X,ObjectSpace.Y+ObjectSpace.Height), 100, 100))
                {
                RenderOrder = 6,
                ID = 2
                },
            };

            spaceBar = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/space") }, new Rectangle(1079, 564, 165, 132))
            {
                RenderOrder = 7,
                isVisible = false,
                suprisee = true
            };

            confetti = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/confetti") }, new Rectangle(0, 0, ScreenWidth, ScreenHeight))
            {
                RenderOrder = 8,
                isVisible = false
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
                lightSwitch,
                spaceBar,
                confetti,
                curtains[0],
                curtains[1]
            };

            foreach (Dragable animal in animals)
                components.Add(animal);

            foreach(BGGraphic curt in curtains)
                components.Add(curt);


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

            #endregion
        }

        void Load_2()
        {
            vanFrame = SceneStart + FrameRate;

            road = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/road_0"), Content.Load<Texture2D>("Graphics/road_1"), Content.Load<Texture2D>("Graphics/road_2"), Content.Load<Texture2D>("Graphics/road_3"), Content.Load<Texture2D>("Graphics/road_4"), }, new Rectangle(0, 0, 1280, 720))
            {
                RenderOrder = 1
            };
            van = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/van_0"), Content.Load<Texture2D>("Graphics/van_2"), Content.Load<Texture2D>("Graphics/van_3"), Content.Load<Texture2D>("Graphics/van_4") }, new Rectangle(0, 0, 1280, 720))
            {
                RenderOrder = 2
            };
            spaceBar = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/space") }, new Rectangle(1079, 564, 165, 132))
            {
                RenderOrder = 3
            };
            components = new List<Component>()
            {
                van, road, spaceBar
            };
            components.Sort((x, y) => x.RenderOrder.CompareTo(y.RenderOrder));
        }

        void Load_3()
        {
            gameState = 0;
            putCount = 0;
            swapper = true;

            door = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/door_0"), Content.Load<Texture2D>("Graphics/door_1") }, new Rectangle(811, 130, 143, 230))
            {
                RenderOrder = 0
            };

            cat = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/cat_0"), Content.Load<Texture2D>("Graphics/cat_1"), Content.Load<Texture2D>("Graphics/cat_2"), Content.Load<Texture2D>("Graphics/cat_3") }, new Rectangle(0, 265, 152, 180))
            {
                RenderOrder = 1,
                DisplayingID = 3,
                suprisee = true,
                MoveSpeed = 2
            };

            var room = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/room2") }, new Rectangle(0, 0, 1280, 720))
            {
                RenderOrder = 2
            };

            curtains = new List<BGGraphic>
            {
                new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/curtain_0"),  Content.Load<Texture2D>("Graphics/curtain_1") }, new Rectangle(265,58,175,258)){ RenderOrder = 3, ID = 20},
                new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/curtain_0"),  Content.Load<Texture2D>("Graphics/curtain_1") }, new Rectangle(567,58,175,258)){ RenderOrder = 3, ID = 21}
            };

            lounge = new InteractableObj(new Texture2D[] { Content.Load<Texture2D>("Graphics/Lounge_0"), Content.Load<Texture2D>("Graphics/Lounge_1"), Content.Load<Texture2D>("Graphics/Lounge_2") }, new Rectangle(325, 251, 398, 219), new Point(467, 354), animals)
            {
                ID = 0,
                RenderOrder = 4
            };
            var rug2 = new InteractableObj(new Texture2D[] { Content.Load<Texture2D>("Graphics/rug_0"), Content.Load<Texture2D>("Graphics/rug_1"), Content.Load<Texture2D>("Graphics/rug_2") }, new Rectangle(521, 452, 366, 366), new Point(615, 665), animals)
            {
                ID = 1,
                RenderOrder = 4
            };
            var sofa2 = new InteractableObj(new Texture2D[] { Content.Load<Texture2D>("Graphics/sofa_0"), Content.Load<Texture2D>("Graphics/sofa_1"), Content.Load<Texture2D>("Graphics/sofa_2") }, new Rectangle(100, 360, 352, 352), new Point(258, 548), animals)
            {
                ID = 2,
                RenderOrder = 4
            };
            lightSwitch = new Button(Content.Load<Texture2D>("Graphics/switch_1"), Content.Load<Texture2D>("Graphics/switch_0"))
            {
                RenderOrder = 4,
                Position = new Vector2(1000, 180)

            };


            animals = new List<Dragable>
            {
                 new Dragable(new Texture2D[] { Content.Load<Texture2D>("Graphics/owl_grey_0"), Content.Load<Texture2D>("Graphics/owl_grey_1") },
                 new Rectangle(rd.Next(ObjectSpace.X,ObjectSpace.X+ObjectSpace.Width), rd.Next(ObjectSpace.Y,ObjectSpace.Y+ObjectSpace.Height), 171, 216))
                {
                RenderOrder = 5,
                ID = 0,
                holdPoint = new Point(98,65)
                },
                new Dragable(new Texture2D[] { Content.Load<Texture2D>("Graphics/Fox_Peach_0"), Content.Load<Texture2D>("Graphics/Fox_Peach_1") },
                new Rectangle(rd.Next(ObjectSpace.X,ObjectSpace.X+ObjectSpace.Width), rd.Next(ObjectSpace.X,ObjectSpace.Y+ObjectSpace.Height), 180, 184))
                {
                RenderOrder = 5,
                ID = 1,
                holdPoint = new Point(58,92)
                },
                new Dragable(new Texture2D[] { Content.Load<Texture2D>("Graphics/Bear_0"), Content.Load<Texture2D>("Graphics/Bear_1") },
                new Rectangle(rd.Next(ObjectSpace.X,ObjectSpace.X+ObjectSpace.Width), rd.Next(ObjectSpace.X,ObjectSpace.Y+ObjectSpace.Height), 200, 200))
                {
                RenderOrder = 5,
                ID = 2,
                holdPoint = new Point(49,61)
                },
            };

            horns = new ObjInBody[]{
                new ObjInBody(Content.Load<Texture2D>("Graphics/Horn_0"), new Rectangle(100, 600, 50, 50))
            {
                RenderOrder = 6,
                isVisible = true
            }, new ObjInBody(Content.Load<Texture2D>("Graphics/Horn_1"), new Rectangle(200, 400, 50, 50))
            {
                RenderOrder = 6,
                isVisible = true
            },
                new ObjInBody(Content.Load<Texture2D>("Graphics/Horn_2"), new Rectangle(400, 500, 50, 50))
            {
                RenderOrder = 6,
                isVisible = true
            }
            };

            spaceBar = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/space") }, new Rectangle(1079, 564, 165, 132))
            {
                RenderOrder = 7,
                isVisible = false,
                suprisee = true
            };

            confetti = new BGGraphic(new Texture2D[] { Content.Load<Texture2D>("Graphics/confetti") }, new Rectangle(0, 0, ScreenWidth, ScreenHeight))
            {
                RenderOrder = 8,
                isVisible = false
            };

            foreach (Dragable animal in animals)
            {
                components.Add(animal);
                for (int i = 0; i < horns.Length; i++)
                    animal.objInBodies.Add(horns[i]);
            }

            components = new List<Component>()
            {
                room,
                spaceBar,
                lightSwitch,
                door,
                confetti,
                cat,
                lounge,
                rug2,
                sofa2
            };

            foreach (BGGraphic curt in curtains)
                components.Add(curt);
            foreach (ObjInBody horn in horns)
                components.Add(horn);

            interactableObjs2 = new List<InteractableObj>()
            {
                lounge, rug2, sofa2
            };

            cat.MoveTo(new Point(170, 142 - 219));
        }

        #region Scene1_EventMethods

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
        }

        void Update_0(GameTime gameTime)
        {
            if (swapper)
            {
                if (gameTime.TotalGameTime > goFrame)
                {
                    sounds["stab"].Play();
                    SceneStart = gameTime.TotalGameTime;
                    GameScene = 1;
                    Load_1();
                }
                else if (gameTime.TotalGameTime > vanFrame)
                {
                    vanFrame = gameTime.TotalGameTime + FrameRate;
                    van.NextIMG();
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                sounds["vanStart"].Play();
                spaceBar.isVisible = false;
                goFrame = gameTime.TotalGameTime + sounds["vanStart"].Duration;
                swapper = true;
            }
        }

        void Update_1(GameTime gameTime)
        {
            #region Update_Scene1
            mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);
            foreach (Component item in components)
            {
                item.Update(gameTime);
            }

            // mouse staffs
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            UpdateMouse();

            if(gameState == 0)
            {
                if(gameTime.TotalGameTime < SceneStart + FirstLeg && swapper)
                {
                    sounds["comeOn"].Play();
                    cat.MoveTo(new Point(170, 142 - 219));
                    swapper = false;
                }
                else if (gameTime.TotalGameTime > SceneStart + FirstLeg && !swapper)
                {
                    cat.MoveTo(new Point(800 - 170, 0));
                    sounds["goGoGo"].Play();
                    swapper = true;
                }
                else if (gameTime.TotalGameTime > SceneStart + FirstLeg + SecondLeg && swapper)
                {
                    sounds["shush"].Play();
                    OpenDoor(gameTime);
                    swapper = false;
                    gameState = 1;
                }
            }
            
            if(gameState == 1)
            {
                if (gameTime.TotalGameTime > SceneStart + FirstLeg + SecondLeg + DoorOpenToComeIn && !swapper)
                {
                    spaceBar.isVisible = true;
                    foreach (var item in animals)
                    {
                        item.StopMovement();
                    }
                    swapper = true;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Space)&& swapper)
                {
                    gameState = 2;
                    Game1.supriser = Color.White;
                    Game1.suprisee = Color.White;
                    CheckResult_Scene1();
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
            
            if (gameState == 2)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.N))
                {
                    GameScene = 2;
                    Load_2();
                }
            }
            #endregion
        }

        void Update_2(GameTime gameTime)
        {
            if (gameTime.TotalGameTime > vanFrame)
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
                SceneStart = gameTime.TotalGameTime;
                Load_3();
            }
        }

        void Update_3(GameTime gameTime)
        {
            _gameTime = gameTime;
            mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);
            foreach (var item in components)
            {
                item.Update(gameTime);
            }
            // mouse staffs
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            UpdateMouse();

            if (gameState == 0)
            {
                if (gameTime.TotalGameTime < SceneStart + FirstLeg && swapper)
                {
                    sounds["comeOn"].Play();
                    cat.MoveTo(new Point(170, 142 - 219));
                    swapper = false;
                }
                else if (gameTime.TotalGameTime > SceneStart + FirstLeg && !swapper)
                {
                    cat.MoveTo(new Point(800 - 170, 0));
                    sounds["goGoGo"].Play();
                    swapper = true;
                }
                else if (gameTime.TotalGameTime > SceneStart + FirstLeg + SecondLeg && swapper)
                {
                    sounds["shush"].Play();
                    OpenDoor(gameTime);
                    swapper = false;
                    gameState = 1;
                }
            }

            if (gameState == 1)
            {
                if (gameTime.TotalGameTime > SceneStart + FirstLeg + SecondLeg + DoorOpenToComeIn && !swapper)
                {
                    spaceBar.isVisible = true;
                    foreach (var item in animals)
                    {
                        item.StopMovement();
                    }
                    swapper = true;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Space) && swapper)
                {
                    gameState = 2;
                    Game1.supriser = Color.White;
                    Game1.suprisee = Color.White;
                    CheckResult_Scene2();
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

            if (gameState == 2)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.N))
                {
                    Exit();
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
            foreach (var item in components)
                item.Draw(gameTime, spriteBatch);
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

            foreach (var item in components)
                item.Draw(gameTime, spriteBatch);
        }

        void Draw_3(GameTime gameTime)
        {
            components.Sort((x, y) => x.RenderOrder.CompareTo(y.RenderOrder));
            foreach (var item in components)
            {
                item.Draw(gameTime, spriteBatch);
            }
        }

        private void OpenDoor(GameTime gameTime)
        {
            door.SetIMG(1);
            IntervalSpan2 = SceneStart;
        }

        void UpdateMouse()
        {
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                for (int i = components.Count - 1; i > -1; i++)
                {

                }
            }
            else if(currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
            {

            }
        }

        void CheckResult_Scene1()
        {
            spaceBar.isVisible = false;

            if (putCount == 3 && !lightOn)
            {
                cat.SetIMG(1);
                sounds["scream"].Play();
                confetti.isVisible = true;
            }
            else
            {
                cat.SetIMG(2);
                sounds["boo"].Play();
            }
        }

        void CheckResult_Scene2()
        {
            spaceBar.isVisible = false;

            foreach (InteractableObj spot in interactableObjs2)
                spot.SetIMG(0);
            foreach (ObjInBody horn in horns)
                horn.isVisible = true;

            if (putCount == 3 && !lightOn)
            {
                cat.SetIMG(1);
                sounds["scream"].Play();
                confetti.isVisible = true;
            }
            else
            {
                cat.SetIMG(2);
                sounds["boo"].Play();
            }
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
