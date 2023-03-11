using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace ButtonMash
{
    public enum GameState
    {
        Animation, Start, Playing, GameOver
    };
    
    public class ButtonMash : Game
    {
        private SpriteBatch spriteBatch;
        private MouseState mouseState, previousMouseState;
        private bool LmbPressed => mouseState.LeftButton.Equals(ButtonState.Pressed) && !previousMouseState.LeftButton.Equals(ButtonState.Pressed);
        private bool RmbPressed => mouseState.RightButton.Equals(ButtonState.Pressed) && !previousMouseState.RightButton.Equals(ButtonState.Pressed);
        private GameState gameState;
        private Texture2D mainMenuTexture;
        private Texture2D buttonTexture, buttonBaseFront, buttonBaseBack;
        private Texture2D animationBackground, menuBackground, gameBackground;
        private SpriteFont titleFont, mainMenuFont, enterFont;
        private Button[,] buttons;
        private string[] mainMenuText;
        private Rectangle[] mainMenuHitBoxes;
        private float buttonScale;
        private Animation animation;
        private Hand hand;
        private HUD hud;


        public ButtonMash()
        {
            GraphicsDeviceManager graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1080;
            graphics.PreferredBackBufferHeight = 900;
        }


        protected override void Initialize()
        {
            gameState = GameState.Animation;

            mainMenuHitBoxes = new Rectangle[4];
            mainMenuText = new string[] { "Easy", "Normal", "Hard", "Quit" };
            buttonScale = 0.25f;

            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            mainMenuTexture = Content.Load<Texture2D>("Button");
            Texture2D heartTexture = Content.Load<Texture2D>("Helt Hjarta");
            Texture2D fistTexture = Content.Load<Texture2D>("Fist");
            Texture2D treeAnimation = Content.Load<Texture2D>("Tree Animation");

            titleFont = Content.Load<SpriteFont>("Title");
            mainMenuFont = Content.Load<SpriteFont>("Buttons");
            enterFont = Content.Load<SpriteFont>("Hit Enter");
            SpriteFont hudFont = Content.Load<SpriteFont>("HUD");

            buttonBaseBack = Content.Load<Texture2D>("ButtonBackground");
            buttonBaseFront = Content.Load<Texture2D>("ButtonCover");
            buttonTexture = Content.Load<Texture2D>("MashThisButton");

            animationBackground = Content.Load<Texture2D>("Animation Background");
            menuBackground = Content.Load<Texture2D>("Menu Background");
            gameBackground = Content.Load<Texture2D>("Button Background");

            animation = new Animation(treeAnimation, enterFont, new Vector2(100, Window.ClientBounds.Height - treeAnimation.Height/2 - 50), new Point(4, 2));
            hud = new HUD(heartTexture, hudFont, new Point(Window.ClientBounds.Width, Window.ClientBounds.Height));
            hand = new Hand(fistTexture, buttonTexture);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (gameState)
            {
                case GameState.Animation:
                    EnterToContinue();
                    animation.Update(gameTime);
                    break;

                case GameState.Start:
                    mainMenuButtons();
                    break;

                case GameState.GameOver:
                    mainMenuButtons();

                    break;

                case GameState.Playing:
                    IsMouseVisible = false;
                    foreach (Button button in buttons)
                    {
                        button.Update(gameTime);
                    }
                    HitAButton();
                    hand.Update();
                    hud.Update(gameTime);
                    GameOver();
                    break;
            }
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();

            switch (gameState)
            {
                case GameState.Animation:
                    GraphicsDevice.Clear(Color.PapayaWhip);
                    spriteBatch.Draw(animationBackground, new Vector2(0, Window.ClientBounds.Height - animationBackground.Height), Color.White);
                    animation.Draw(spriteBatch);
                    break;

                case GameState.Start:
                    GraphicsDevice.Clear(Color.SaddleBrown);
                    spriteBatch.Draw(menuBackground, Vector2.Zero, Color.White);
                    DrawMainMenuButtons(spriteBatch);
                    spriteBatch.DrawString(titleFont, "Button Mash", new Vector2(Window.ClientBounds.Width / 2 - titleFont.Texture.Width / 2, 0), Color.Goldenrod);
                    break;

                case GameState.GameOver:
                    GraphicsDevice.Clear(Color.DeepSkyBlue);
                    spriteBatch.Draw(menuBackground, Vector2.Zero, Color.White);
                    DrawMainMenuButtons(spriteBatch);
                    spriteBatch.DrawString(enterFont, $"You got: {GlobalVariables.Score} points!", new Vector2(Window.ClientBounds.Width / 2 - titleFont.Texture.Width / 2, 0), Color.Wheat);
                    spriteBatch.DrawString(mainMenuFont, "Want to play again?", new Vector2(Window.ClientBounds.Width / 2 - titleFont.Texture.Width / 2, 100), Color.Wheat);
                    break;

                case GameState.Playing:
                    GraphicsDevice.Clear(Color.MonoGameOrange);
                    spriteBatch.Draw(gameBackground, Vector2.Zero, Color.White);

                    foreach (Button button in buttons)
                    {
                        button.Draw(spriteBatch);
                    }

                    hand.Draw(spriteBatch);
                    hud.Draw(spriteBatch);
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void mainMenuButtons()
        {
            mouseState = Mouse.GetState();

            if (PressedButton(0))//Easy
            {
                LoadMasherButtons(new Point(3, 2));
                ResetGlobalVariables(0);
                gameState = GameState.Playing;
            }
            else if (PressedButton(1))//Medium
            {
                LoadMasherButtons(new Point(3, 3));
                ResetGlobalVariables(1);
                gameState = GameState.Playing;
            }
            else if (PressedButton(2))//Hard
            {
                LoadMasherButtons(new Point(4,3));
                ResetGlobalVariables(2);
                gameState = GameState.Playing;
            }
            else if (PressedButton(3))
                Exit();

            previousMouseState = mouseState;
        }

        private bool PressedButton(int buttonIndex) => mainMenuHitBoxes[buttonIndex].Contains(mouseState.Position) && LmbPressed;

        private void ResetGlobalVariables(int difficulty)
        {
            switch (difficulty) {
                case 0:
                    GlobalVariables.Time = 25;
                    GlobalVariables.Lives = 6;
                    break; 
                case 1:
                    GlobalVariables.Time = 20;
                    GlobalVariables.Lives = 4;
                    break;
                case 2:
                    GlobalVariables.Time = 15;
                    GlobalVariables.Lives = 3;
                    break;
            }
            GlobalVariables.Score = 0;
            GlobalVariables.BigSlam = false;
        }

        private void LoadMainMenuButtons()
        {
            for (int i = 0; i < mainMenuHitBoxes.Length; i++)
            {
                mainMenuHitBoxes[i] = new Rectangle((int)(Window.ClientBounds.Width / 2 - mainMenuTexture.Width * buttonScale / 2),
                                                    (int)(Window.ClientBounds.Height * 0.43f - mainMenuTexture.Height * buttonScale + i * mainMenuTexture.Height * buttonScale * 1.1f),
                                                    (int)(mainMenuTexture.Width * buttonScale), (int)(mainMenuTexture.Height * buttonScale));
            }
        }

        private void DrawMainMenuButtons(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < mainMenuHitBoxes.Length; i++)
            {
                Vector2 position = new Vector2(mainMenuHitBoxes[i].X, mainMenuHitBoxes[i].Y);
                Vector2 stringPosition = new Vector2(position.X + mainMenuTexture.Width * buttonScale / 2 -  mainMenuText[i].Count() * mainMenuFont.Texture.Width / 33,
                                                        position.Y + mainMenuTexture.Height * buttonScale / 2 - mainMenuFont.Texture.Height * buttonScale / 2);

                spriteBatch.Draw(mainMenuTexture, position, null, Color.White, 0f, Vector2.Zero, buttonScale, SpriteEffects.None, 0f);
                spriteBatch.DrawString(mainMenuFont, mainMenuText[i], stringPosition, Color.White);
            }
        }


        private void LoadMasherButtons(Point size)
        {
            buttons = new Button[size.Y, size.X];

            for (int i = 0; i < buttons.GetLength(0); i++)
            {
                for (int j = 0; j < buttons.GetLength(1); j++)
                {
                    Vector2 position = new Vector2((Window.ClientBounds.Width - buttonBaseBack.Width) / size.X * j + Window.ClientBounds.Width * 0.5f/size.X,
                                                   (Window.ClientBounds.Height - buttonTexture.Height) / size.Y + buttonTexture.Height * 1.5f * i);

                    buttons[i, j] = new Button(buttonTexture, buttonBaseFront, buttonBaseBack, gameBackground, position, new Point(j, i));
                }
            }
        }

        private void HitAButton()
        {
            mouseState = Mouse.GetState();

            foreach (Button button in buttons)
            {
                if (GlobalVariables.BigSlam && hand.mashBox.Intersects(button.hitBox) && RmbPressed)
                {
                    for (int i = 0; i < buttons.GetLength(0); i++)
                    {
                        for (int j = 0; j < buttons.GetLength(1); j++)
                        {
                            if (j == button.arrayIndex.X || i == button.arrayIndex.Y)//Vertical && Horizontal
                            {
                                buttons[i, j].SlammButton();
                            }
                            else if (j == button.arrayIndex.Y - (button.arrayIndex.X - i) || j == button.arrayIndex.X - (button.arrayIndex.Y - i))//Diagonal
                            {
                                buttons[i, j].SlammButton();
                            }
                        }
                    }
                    GlobalVariables.BigSlam = false;
                }
                

                else if (hand.mashBox.Intersects(button.hitBox) && (LmbPressed || RmbPressed))
                    button.HitButton();
            }

            previousMouseState = mouseState;
        }

        public void EnterToContinue()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                LoadMainMenuButtons();
                IsMouseVisible = true;
                gameState = GameState.Start;
            }
        }

        private void GameOver()
        {
            if(GlobalVariables.Lives <= 0 || GlobalVariables.Time <= 0)
            {
                IsMouseVisible = true;
                gameState = GameState.GameOver;
            }
        }
    }
}
