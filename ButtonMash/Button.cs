using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ButtonMash
{
    class Button
    {
        public Point arrayIndex;
        public Rectangle hitBox;
        
        private readonly Texture2D buttonTexture, buttonBaseFront, buttonBaseBack, gameBackground;
        private Vector2 basePosition, buttonPosition;
        private ButtonType buttonState;
        private Rectangle cover;
        private Color buttonColour;
        private int longButtonHitCount, activationTimer, timeCounter, buttonTimer, speed;
        private bool keepUp;

        enum ButtonType {
            Inert, Green, Red, Long, Heart, Strength
        };


        public Button(Texture2D buttonTexture, Texture2D buttonBaseFront, Texture2D buttonBaseBack, Texture2D gameBackground, Vector2 basePosition, Point arrayIndex)
        {
            this.buttonTexture = buttonTexture;
            this.buttonBaseFront = buttonBaseFront;
            this.buttonBaseBack = buttonBaseBack;
            this.gameBackground = gameBackground;
            this.basePosition = buttonPosition = basePosition; 
            this.arrayIndex = arrayIndex;
            activationTimer = 3000;
            speed = 3;

            buttonPosition.X += buttonBaseBack.Width/2 - buttonTexture.Width/2 ;
            buttonPosition.Y = basePosition.Y + buttonBaseBack.Height / 3;

            hitBox = new Rectangle((int)buttonPosition.X, (int)buttonPosition.Y, buttonTexture.Width, 0);
            cover = new Rectangle((int)basePosition.X, (int)(basePosition.Y + buttonBaseBack.Height * 0.7f), buttonBaseBack.Width, buttonTexture.Height);

            SetInert();
        }


        public void Update(GameTime gameTime)
        {
            timeCounter += gameTime.ElapsedGameTime.Milliseconds;

            if (timeCounter >= activationTimer)
            {
                timeCounter -= activationTimer;

                if (activationTimer > 1500)
                    activationTimer -= 50;

                if (buttonState == ButtonType.Inert && ReachedBottom())
                    ActivateButton();
            }

            buttonTimer -= gameTime.ElapsedGameTime.Milliseconds;
            if (buttonTimer <= 0)
                keepUp = false;

            MoveButton();
            UpdateButton();

            hitBox.Y = (int)buttonPosition.Y;
            hitBox.Height = (int)(basePosition.Y - buttonPosition.Y +30);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(buttonBaseBack, basePosition, Color.White);
            spriteBatch.Draw(buttonTexture, buttonPosition, buttonColour);
            spriteBatch.Draw(gameBackground, cover, cover, Color.White);
            spriteBatch.Draw(buttonBaseFront, basePosition, Color.White);
        }


        private void ActivateButton()
        {
            int buttonType = GlobalVariables.Random.Next(0, activationTimer / 30 + 10);

            int buttonDurationSpan = 500;
            buttonPosition.Y = basePosition.Y + buttonBaseBack.Height / 3;
            buttonTimer = GlobalVariables.Random.Next(activationTimer - buttonDurationSpan, activationTimer + buttonDurationSpan * 2);

            if (buttonType <= 20)
            {
                keepUp = true;
                buttonState = ButtonType.Green;
                buttonColour = Color.Green;
            }
            else if (buttonType <= 33)
            {
                keepUp = true;
                buttonState = ButtonType.Red;
                buttonColour = Color.Red;
            }
            else if (buttonType <= 40)
            {
                keepUp = true;
                buttonState = ButtonType.Long;
                buttonColour = Color.LawnGreen;
                longButtonHitCount = 3;
            }
            else if (buttonType <= 42)
            {
                keepUp = true;
                buttonState = ButtonType.Strength;
                buttonColour = Color.SaddleBrown;
            }
            else if (GlobalVariables.ComboProcs > 0)
            {
                keepUp = true;
                buttonState = ButtonType.Heart;
                buttonColour = Color.DeepPink;
                --GlobalVariables.ComboProcs;
            }
        }

        private void MoveButton()
        {
            if (!keepUp && !ReachedBottom())
            {
                buttonPosition.Y += speed;
                return;
            }

            switch (buttonState)
            {
                case ButtonType.Long:
                    if (keepUp && buttonPosition.Y > basePosition.Y - buttonTexture.Height / 5 * longButtonHitCount)
                        buttonPosition.Y -= speed;
                    break;

                default:
                    if (keepUp && buttonPosition.Y > basePosition.Y - buttonTexture.Height / 6)
                        buttonPosition.Y -= speed;
                    break;
            }
        }


        private void UpdateButton()
        {
            switch (buttonState)
            {
                case ButtonType.Inert:
                    break;
                case ButtonType.Red:
                    if (ReachedBottom())
                        SetInert();
                    break;
                default:
                    if (ReachedBottom())
                        MissedButton();
                    break;
            }
        }

        public void HitButton()
        {
            switch (buttonState)
            {
                case ButtonType.Green:
                    GlobalVariables.Time++;
                    GlobalVariables.Score++;
                    GlobalVariables.Combo++;
                    SetInert();
                    break;

                case ButtonType.Red:
                    GlobalVariables.Lives--;
                    GlobalVariables.Combo = 0;
                    SetInert();
                    break;

                case ButtonType.Long:
                    longButtonHitCount--;

                    if (buttonPosition.Y < basePosition.Y - buttonTexture.Height / 5 * longButtonHitCount)
                        buttonPosition.Y = basePosition.Y - buttonTexture.Height / 5 * Math.Max(longButtonHitCount, 0);

                    if (longButtonHitCount <= 0)
                    {
                        GlobalVariables.Time += 2;
                        GlobalVariables.Score += 5;
                        GlobalVariables.Combo++;
                        SetInert();
                    }
                    break;

                case ButtonType.Heart:
                    GlobalVariables.Time++;
                    GlobalVariables.Score++;
                    GlobalVariables.Lives++;
                    SetInert();
                    break;

                case ButtonType.Strength:
                    GlobalVariables.Score++;
                    GlobalVariables.Combo++;
                    GlobalVariables.BigSlam = true;
                    SetInert();
                    break;
            }
        }

        public void SlammButton()
        {
            switch (buttonState)
            {
                case ButtonType.Long:
                    longButtonHitCount = 0;
                    HitButton();
                    break;
                case ButtonType.Red:
                    SetInert();
                    break;
                default:
                    HitButton();
                    break;
            }
        }

        private bool ReachedBottom() => buttonPosition.Y >= basePosition.Y + buttonBaseBack.Height / 3;

        private void MissedButton()
        {
            GlobalVariables.Lives--;
            GlobalVariables.Combo = 0;
            SetInert();
        }

        private void SetInert()
        {
            buttonState = ButtonType.Inert;
            buttonColour = Color.White;
            keepUp = false;
        }
    }
}
