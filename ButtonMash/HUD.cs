using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ButtonMash
{
    class HUD
    {
        Texture2D heartTexture;
        SpriteFont hUDFont;
        Point windowSize;
        int timer;

        float scale;

        public HUD(Texture2D heartTexture, SpriteFont hUDFont, Point windowSize)
        {
            this.heartTexture = heartTexture;
            this.hUDFont = hUDFont;
            this.windowSize = windowSize;

            scale = 0.075f;
        }

        public void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;
            if (timer >= 1000)
            {
                GlobalVariables.Time--;
                timer -= 1000;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawHearts(spriteBatch);
            spriteBatch.DrawString(hUDFont, "Time: " + GlobalVariables.Time, new Vector2((windowSize.X - hUDFont.Texture.Width) * 0.6f, 0), Color.LightGray);
            spriteBatch.DrawString(hUDFont, "Score: " + GlobalVariables.Score, Vector2.Zero, Color.LightGray);
            spriteBatch.DrawString(hUDFont, "Combo counter: " + GlobalVariables.Combo, new Vector2(0, hUDFont.Texture.Height * 0.22f), Color.LightGray);
            if (GlobalVariables.BigSlam)
                spriteBatch.DrawString(hUDFont, "LMB for Big Slam: ", new Vector2(0, hUDFont.Texture.Height * 0.45f), Color.LightGray);
        }

        private void DrawHearts(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < GlobalVariables.Lives; i++)
            {
                Vector2 heartPosition = new Vector2(windowSize.X - (GlobalVariables.maxLives+0.5f) * heartTexture.Width * scale / 2 + heartTexture.Width /2 * scale * i, -500 * scale);
                spriteBatch.Draw(heartTexture, heartPosition, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            
        }
    }
}
