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

            buttonState = ButtonType.Inert;
        }


        public void Update(GameTime gameTime)
        {
            timeCounter += gameTime.ElapsedGameTime.Milliseconds;

            if (timeCounter >= activationTimer)
            {
                timeCounter -= activationTimer;

                //Ökar spelhastigheten
                if (activationTimer > 1500)
                {
                    activationTimer -= 50;
                }

                if (buttonState == ButtonType.Inert && buttonPosition.Y >= basePosition.Y + buttonBaseBack.Height / 3)
                {
                    ActivateButton();
                }
            }

            buttonTimer -= gameTime.ElapsedGameTime.Milliseconds;
            if (buttonTimer <= 0)
            {
                keepUp = false;
            }

            MoveButton();
            UpdateButton();

            hitBox.Y = (int)buttonPosition.Y;
            hitBox.Height = (int)(basePosition.Y - buttonPosition.Y +30);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Ritar knappbasbakgrunden
            spriteBatch.Draw(buttonBaseBack, basePosition, Color.White);

            //Ritar knappen
            spriteBatch.Draw(buttonTexture, buttonPosition, buttonColour);

            //Gömmer knappens undersida
            spriteBatch.Draw(gameBackground, cover, cover, Color.White);

            //Ritar knappbasförgrunden
            spriteBatch.Draw(buttonBaseFront, basePosition, Color.White);
        }


        //Byter från grå till aktiv
        private void ActivateButton()
        {
            int buttonType = GlobalVariables.Random.Next(0, activationTimer / 30 + 10);


            buttonPosition.Y = basePosition.Y + buttonBaseBack.Height / 3;
            buttonTimer = GlobalVariables.Random.Next(activationTimer - 500, activationTimer + 1000);//Bestämmer tid innan knapp går ned

            if (buttonType <= 20)
            {
                keepUp = true;
                buttonState = ButtonType.Green;
            }
            else if (buttonType <= 33)
            {
                keepUp = true;
                buttonState = ButtonType.Red;
            }
            else if (buttonType <= 40)
            {
                keepUp = true;
                buttonState = ButtonType.Long;
                longButtonHitCount = 3;
            }
            else if (buttonType <= 42)
            {
                keepUp = true;
                buttonState = ButtonType.Strength;
            }
            else if (GlobalVariables.Combo >= 10)//Om 10 lyckade slag på raken
            {
                keepUp = true;
                buttonState = ButtonType.Heart;
                GlobalVariables.Combo = 0;
            }
        }


        //Ändrar knappositionen
        private void MoveButton()
        {
            switch (buttonState)
            {
                case ButtonType.Inert:
                    if (buttonPosition.Y < basePosition.Y + buttonBaseBack.Height / 3)//Flyttar ned tills den når botten
                    {
                        buttonPosition.Y += speed;
                    }
                    break;

                case ButtonType.Long:
                    if (keepUp && buttonPosition.Y > basePosition.Y - buttonTexture.Height / 5 * longButtonHitCount)//Om keepUp, flyttar längre än andra
                    {
                        buttonPosition.Y -= speed;
                    }
                    else if (!keepUp && buttonPosition.Y <= basePosition.Y + buttonBaseBack.Height / 3)
                    {
                        buttonPosition.Y += speed;
                    }
                    break;

                default:
                    if (keepUp && buttonPosition.Y > basePosition.Y - buttonTexture.Height / 6)//Flyttar upp knapp om den är lägre än max
                    {
                        buttonPosition.Y -= speed;
                    }
                    else if (!keepUp && buttonPosition.Y <= basePosition.Y + buttonBaseBack.Height / 3)//Flyttar ned tills den når botten
                    {
                        buttonPosition.Y += speed;
                    }
                    break;
            }
        }


        //Ändrar färg och typ, om missad knapp nollar combo
        private void UpdateButton()
        {
            switch (buttonState)
            {
                case ButtonType.Inert:
                    buttonColour = Color.White;
                    break;

                case ButtonType.Green:
                    buttonColour = Color.Green;
                    if (buttonPosition.Y > basePosition.Y + buttonBaseBack.Height / 3)
                    {
                        GlobalVariables.Lives--;
                        GlobalVariables.Combo = 0;
                        buttonState = ButtonType.Inert;
                    }
                    break;

                case ButtonType.Red:
                    buttonColour = Color.Red;
                    if (buttonPosition.Y > basePosition.Y + buttonBaseBack.Height / 3)
                    {
                        buttonState = ButtonType.Inert;
                    }
                    break;

                case ButtonType.Long:
                    buttonColour = Color.LawnGreen;
                    if (buttonPosition.Y > basePosition.Y + buttonBaseBack.Height / 3)
                    {
                        GlobalVariables.Lives--;
                        GlobalVariables.Combo = 0;
                        buttonState = ButtonType.Inert;
                    }
                    break;

                case ButtonType.Heart:
                    buttonColour = Color.DeepPink;
                    if (buttonPosition.Y > basePosition.Y + buttonBaseBack.Height / 3)
                    {
                        buttonState = ButtonType.Inert;
                    }
                    break;

                case ButtonType.Strength:
                    buttonColour = Color.SaddleBrown;
                    if (buttonPosition.Y > basePosition.Y + buttonBaseBack.Height / 3)
                    {
                        GlobalVariables.Time--;
                        GlobalVariables.Lives--;
                        GlobalVariables.Combo = 0;
                        buttonState = ButtonType.Inert;
                    }
                    break;
            }
        }


        //Om knappen blivit slagen ändras liv, tid och poäng
        public void ButtonHit()
        {
            switch (buttonState)
            {
                case ButtonType.Green://Grön ger tid och poäng
                    GlobalVariables.Time++;
                    GlobalVariables.Score++;
                    GlobalVariables.Combo++;
                    buttonState = ButtonType.Inert;
                    break;

                case ButtonType.Red://Röd tar liv
                    GlobalVariables.Lives--;
                    GlobalVariables.Combo = 0;//Nollar combo
                    buttonState = ButtonType.Inert;
                    break;

                case ButtonType.Long://Lång ger mer tid och poäng
                    longButtonHitCount--;

                    if (buttonPosition.Y < basePosition.Y - buttonTexture.Height / 5 * longButtonHitCount)//Slår ned knappen ett steg
                    {
                        buttonPosition.Y = basePosition.Y - buttonTexture.Height / 5 * longButtonHitCount;
                    }

                    if (longButtonHitCount <= 0)//Om knappen är helt slagen
                    {
                        GlobalVariables.Time += 2;
                        GlobalVariables.Score += 5;
                        GlobalVariables.Combo++;
                        buttonState = ButtonType.Inert;
                    }
                    break;

                case ButtonType.Heart://Rosa ger tid, poäng och liv
                    GlobalVariables.Time++;
                    GlobalVariables.Score++;

                    if (!(GlobalVariables.Lives>=6))
                    {
                        GlobalVariables.Lives++;
                    }

                    buttonState = ButtonType.Inert;
                    break;

                case ButtonType.Strength://Brun tillgång till superslag
                    GlobalVariables.Score++;
                    GlobalVariables.Combo++;
                    GlobalVariables.BigSlam = true;
                    buttonState = ButtonType.Inert;
                    break;
            }
        }


        //Om spelare slår knapp med Big Slam
        public void ButtonSlammed()
        {
            switch (buttonState)
            {
                case ButtonType.Green://Grön ger tid och poäng
                    GlobalVariables.Time++;
                    GlobalVariables.Score++;
                    buttonState = ButtonType.Inert;
                    break;

                case ButtonType.Red://Röd inaktiveras
                    buttonState = ButtonType.Inert;
                    break;

                case ButtonType.Long://Lång ger mer tid och poäng
                    longButtonHitCount = 0;

                    if (buttonPosition.Y < basePosition.Y - buttonTexture.Height / 5 * longButtonHitCount)//Slår ned knappen ett steg
                    {
                        buttonPosition.Y = basePosition.Y - buttonTexture.Height / 5 * longButtonHitCount;
                    }

                    if (longButtonHitCount <= 0)//Om knappen är helt slagen
                    {
                        GlobalVariables.Time += 2;
                        GlobalVariables.Score += 5;
                        buttonState = ButtonType.Inert;
                    }
                    break;

                case ButtonType.Heart://Rosa ger tid, poäng och liv
                    GlobalVariables.Time++;
                    GlobalVariables.Score++;
                    GlobalVariables.Lives++;
                    buttonState = ButtonType.Inert;
                    break;

                case ButtonType.Strength://Brun tillgång till superslag
                    GlobalVariables.Score++;
                    GlobalVariables.BigSlam = true;
                    buttonState = ButtonType.Inert;
                    break;
            }
        }
    }
}
