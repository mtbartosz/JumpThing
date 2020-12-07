using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;


namespace jumpthingy
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D backgroundTxr, playerSheetTxr, platformSheetTxr, whiteBox;
        SpriteFont uiTextFont;
        SoundEffect jumpSound, bumpSound, fanfareSound;

        Point screenSize = new Point(800, 450);
        int levelNumber = 0; 

        PlayerSprite playerSprite;
        CoinSprite coinSprite;



        List<List<PlatformSprite>> levels = new List<List<PlatformSprite>>();
        List<Vector2> coins = new List<Vector2>();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = screenSize.X;
            _graphics.PreferredBackBufferHeight = screenSize.Y;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundTxr = Content.Load<Texture2D>("JumpThing_background");
            platformSheetTxr = Content.Load<Texture2D>("JumpThing_spriteSheet2");
            playerSheetTxr = Content.Load<Texture2D>("JumpThing_spriteSheet1");
            uiTextFont = Content.Load<SpriteFont>("UITextFont");
            jumpSound = Content.Load<SoundEffect>("jump");
            bumpSound = Content.Load<SoundEffect>("bump");
            fanfareSound = Content.Load<SoundEffect>("fanfare");


            whiteBox = new Texture2D(GraphicsDevice, 1, 1);
            whiteBox.SetData(new[] { Color.White });

            playerSprite = new PlayerSprite(playerSheetTxr, whiteBox, new Vector2(100, 50), jumpSound, bumpSound);
            coinSprite = new CoinSprite(playerSheetTxr, whiteBox, new Vector2(200, 200));

            BuildLevels();
            
                



        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            playerSprite.Update(gameTime, levels[levelNumber]);

            if (playerSprite.spritePos.Y > screenSize.Y + 50)
            {
                playerSprite.lives--;
                if (playerSprite.lives <= 0)
                {
                    playerSprite.lives = 3;
                    levelNumber = 0;
                }
                playerSprite.ResetPlayer(new Vector2(100, 50));
            }
            if (playerSprite.checkCollision(coinSprite))
            {
                levelNumber++;
                if (levelNumber >= levels.Count) levelNumber = 0;
                coinSprite.spritePos = coins[levelNumber];
                playerSprite.ResetPlayer(new Vector2(100, 50));
                fanfareSound.Play();
            }

            base.Update(gameTime);
            
        }

        protected override void Draw(GameTime gameTime)
        {
            

            _spriteBatch.Begin();

            _spriteBatch.Draw(backgroundTxr, new Rectangle(0, 0, screenSize.X, screenSize.Y), Color.White);

            playerSprite.Draw(_spriteBatch, gameTime);
            coinSprite.Draw(_spriteBatch, gameTime);

            foreach (PlatformSprite platform in levels[levelNumber]) platform.Draw(_spriteBatch, gameTime);

            string livesString = "";

            for(int i = 0; i < playerSprite.lives; i++) livesString += "O";

            

            _spriteBatch.DrawString(uiTextFont, livesString, new Vector2(15, 10), Color.White);


            
            _spriteBatch.DrawString(
                uiTextFont,
                "level" + (levelNumber + 1),
                new Vector2(screenSize.X - 15 - uiTextFont.MeasureString("level " + (levelNumber +1 )).X, 5),
                Color.White);
           
            

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void BuildLevels()
        {
            levels.Add(new List<PlatformSprite>());
            levels[0].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(100, 300)));
            levels[0].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(250, 300)));
            coins.Add(new Vector2(200, 200));

            levels.Add(new List<PlatformSprite>());
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(100, 400)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(250, 350)));
            coins.Add(new Vector2(400, 200));

        }
    }
}
