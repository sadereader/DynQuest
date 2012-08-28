using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;


namespace XNA_test_2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Exiting += new EventHandler<EventArgs>(Game1_Exiting);
        }

        void Game1_Exiting(object sender, EventArgs e)
        {
            enginethread.Abort();
            convertbitmaps = false;
            BitmapConversionThread.Abort();
            
        }
        Thread enginethread;
        Thread BitmapConversionThread;


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            Window.AllowUserResizing = true;
            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 1100;
            graphics.SynchronizeWithVerticalRetrace = false;
            this.IsFixedTimeStep = false;
            graphics.ApplyChanges();
            
            crec = Window.ClientBounds;
            a = new System.Drawing.Bitmap(Window.ClientBounds.Width, Window.ClientBounds.Height);
            base.Initialize();
            enginethread = new Thread(new ThreadStart(Engine_logic));
            enginethread.Start();
            BitmapConversionThread = new Thread(new ThreadStart(Bitmap_conversion));
            BitmapConversionThread.Start();
        }
        bool convertbitmaps = true;
        void Bitmap_conversion()
        {
            while (convertbitmaps)
                kaas = GetTexture(GraphicsDevice, a); //this is a really heavy call, hence seperate thread.
        }

        int animc = 0;
        bool increasing = true;
        long Engine_tickrate;
        volatile System.Drawing.Bitmap a;
        Rectangle crec;
        Texture2D kaas;
        void Engine_logic()
        {
            System.Diagnostics.Stopwatch Eng_timer = new System.Diagnostics.Stopwatch();

            System.Drawing.Bitmap screen_bitmap;
            System.Drawing.SolidBrush b = new System.Drawing.SolidBrush(System.Drawing.Color.Lime);
            System.Drawing.Pen P = new System.Drawing.Pen(b);
            System.Drawing.Image l = System.Drawing.Bitmap.FromFile("spikefucks.bmp");
            P.Width = 3;

            //define rectangles to draw
            List<System.Drawing.Rectangle> RectanglesToDraw;
            bool[,] drawfield = new bool[crec.Width, crec.Height];

            while (true)
            {
                Eng_timer.Start();
                screen_bitmap = new System.Drawing.Bitmap(crec.Width, crec.Height);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(screen_bitmap);
                UpdateSprite();
                //Don't do stuff before here
                
                //mouse stuff
                MouseState m = Mouse.GetState();
                switch (m.LeftButton)
                {
                    case ButtonState.Pressed:
                        g.DrawEllipse(P, m.X, m.Y, 10, 10);
                        break;
                    case ButtonState.Released:
                        g.DrawEllipse(P, m.X, m.Y, 3, 3);
                        break;
                }




                //define rectangles to draw
                RectanglesToDraw = new List<System.Drawing.Rectangle>();
                RectanglesToDraw.Add(new System.Drawing.Rectangle(5, 5, (crec.Width / 3) - 10, (crec.Height / 2) - 10)); //top down map
                RectanglesToDraw.Add(new System.Drawing.Rectangle((crec.Width / 3) , 5, (crec.Width / 3) - 10, (crec.Height / 2) - 10)); //image content window
                RectanglesToDraw.Add(new System.Drawing.Rectangle((crec.Width / 3) *2, 5, (crec.Width / 3) - 10, (crec.Height-5) )); //dynamic content scroller
                RectanglesToDraw.Add(new System.Drawing.Rectangle(5, crec.Height - (crec.Height / 8) * 2 +5, (crec.Width/3)*2-10, (crec.Height / 8) * 2 -10)); // input window


                //g.DrawImage(l, new System.Drawing.Point(0, 0));
                g.DrawRectangles(P, RectanglesToDraw.ToArray());
                g.DrawRectangle(P, new System.Drawing.Rectangle(0, 0, crec.Width - 1, crec.Height - 1));
                //g.DrawLine(P, new System.Drawing.Point(100, 300), new System.Drawing.Point(100, animc + 200));
                if (increasing)
                    animc++;
                else
                    animc--;
                if (increasing && animc > 100)
                    increasing = false;
                else if (!increasing && animc < 0)
                    increasing = true;


                a = new System.Drawing.Bitmap(crec.Width, crec.Height);
                a = screen_bitmap;
                
                //don't do stuff after this
                Thread.Sleep(4);
                Eng_timer.Stop();
                Engine_tickrate = Eng_timer.Elapsed.Milliseconds;
                Eng_timer.Reset();
            }
        }

        // This is a texture we can render.
        Texture2D myTexture;

        // Set the coordinates to draw the sprite at.
        Vector2 spritePosition = Vector2.Zero;

        // Store some information about the sprite's motion.
        Vector2 spriteSpeed = new Vector2(50.0f, 50.0f);

        void UpdateSprite()
        {
            // Move the sprite by speed, scaled by elapsed time.
            spritePosition +=
                spriteSpeed * 0.01f;//(float)gameTime.ElapsedGameTime.TotalSeconds;

            int MaxX =
                graphics.GraphicsDevice.Viewport.Width - myTexture.Width;
            int MinX = 0;
            int MaxY =
                graphics.GraphicsDevice.Viewport.Height - myTexture.Height;
            int MinY = 0;

            // Check for bounce.
            if (spritePosition.X > MaxX)
            {
                spriteSpeed.X *= -1;
                spritePosition.X = MaxX;
            }

            else if (spritePosition.X < MinX)
            {
                spriteSpeed.X *= -1;
                spritePosition.X = MinX;
            }

            if (spritePosition.Y > MaxY)
            {
                spriteSpeed.Y *= -1;
                spritePosition.Y = MaxY;
            }

            else if (spritePosition.Y < MinY)
            {
                spriteSpeed.Y *= -1;
                spritePosition.Y = MinY;
            }
        }

        //content objects
        SpriteFont Font1;
        //System.Drawing.Bitmap Spikefucks;
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Font1 = Content.Load<SpriteFont>("SpriteFont1");
            myTexture = Content.Load<Texture2D>("spikefucks");
            //Spikefucks = Content.Load<System.Drawing.Bitmap>("spikefucks");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }
        private Texture2D GetTexture(GraphicsDevice dev, System.Drawing.Bitmap bmp)
        {
            int[] imgData = new int[bmp.Width * bmp.Height];
            Texture2D texture = new Texture2D(dev, bmp.Width, bmp.Height);

            unsafe
            {
                // lock bitmap
                System.Drawing.Imaging.BitmapData origdata =
                bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);

                uint* byteData = (uint*)origdata.Scan0;

                // Switch bgra -> rgba
                for (int i = 0; i < imgData.Length; i++)
                {
                    byteData[i] = (byteData[i] & 0x000000ff) << 16 | (byteData[i] & 0x0000FF00) | (byteData[i] & 0x00FF0000) >> 16 | (byteData[i] & 0xFF000000);
                }

                // copy data
                System.Runtime.InteropServices.Marshal.Copy(origdata.Scan0, imgData, 0, bmp.Width * bmp.Height);

                byteData = null;

                // unlock bitmap
                bmp.UnlockBits(origdata);
            }

            texture.SetData(imgData);

            return texture;
        }

        long lastdraw = 0;

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            System.Diagnostics.Stopwatch swatch = new System.Diagnostics.Stopwatch();
            swatch.Start();
            spriteBatch.Begin();



            swatch.Stop();

            spriteBatch.Draw(kaas, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
            spriteBatch.Draw(myTexture, spritePosition, Color.White);

            System.Text.StringBuilder sbuilder = new System.Text.StringBuilder();
            sbuilder.AppendFormat("Last graphics tick took: {0}ms", gameTime.ElapsedGameTime.Milliseconds);
            sbuilder.AppendFormat("\nLast engine tick took: {0}ms", Engine_tickrate);
            sbuilder.AppendFormat("\nLast do stuff tick took: {0}ms", lastdraw);
            spriteBatch.DrawString(Font1, sbuilder, new Vector2(10, 10), Color.Lime, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);



            spriteBatch.End();
            lastdraw = swatch.ElapsedMilliseconds;
            swatch.Reset();
            base.Draw(gameTime);

        }

    }
}
