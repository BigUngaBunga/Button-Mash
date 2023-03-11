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
    class HUD
    {
        //Deklarationer
        Texture2D heartTexture;
        SpriteFont hUDFont;
        Point windowSize;
        int maxLives, timer;

        float scale;

        public HUD(Texture2D heartTexture, SpriteFont hUDFont, Point windowSize)
        {
            this.heartTexture = heartTexture;
            this.hUDFont = hUDFont;
            this.windowSize = windowSize;

            maxLives = 6;
            scale = 0.075f;
        }

        public void Update(GameTime gameTime)
        {
            //Varje sekund, minska tiden med en sekund
            timer += gameTime.ElapsedGameTime.Milliseconds;
            if (timer >= 1000)
            {
                GlobalVariables.Time--;
                timer -= 1000;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Liv
            DrawHearts(spriteBatch);

            //Tid
            spriteBatch.DrawString(hUDFont, "Time: " + GlobalVariables.Time, new Vector2((windowSize.X - hUDFont.Texture.Width) * 0.6f, 0), Color.LightGray);

            //Poäng
            spriteBatch.DrawString(hUDFont, "Score: " + GlobalVariables.Score, Vector2.Zero, Color.LightGray);

            //Combo
            spriteBatch.DrawString(hUDFont, "Combo counter: " + GlobalVariables.Combo, new Vector2(0, hUDFont.Texture.Height * 0.22f), Color.LightGray);

            //Om spelare har Big Slam
            if (GlobalVariables.BigSlam)
            {
                spriteBatch.DrawString(hUDFont, "LMB for Big Slam: ", new Vector2(0, hUDFont.Texture.Height * 0.45f), Color.LightGray);
            }
            
        }

        //Ritar upp liven
        private void DrawHearts(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < GlobalVariables.Lives; i++)
            {
                Vector2 heartPosition = new Vector2(windowSize.X - (maxLives+0.5f) * heartTexture.Width * scale / 2 + heartTexture.Width /2 * scale * i, -500 * scale);
                spriteBatch.Draw(heartTexture, heartPosition, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            
        }
    }
}
