using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ButtonMash
{
    class Animation
    {
        private readonly Texture2D texture;
        private readonly SpriteFont font;
        private readonly float frameRate;
        private Vector2 position;
        private Rectangle sourceRectangle;
        private Point imageSize, numberOfImages, currentImage;
        private float timeSinceLastFrame;
        private bool loopedOnce;


        public Animation(Texture2D texture, SpriteFont font, Vector2 position, Point numberOfImages)
        {
            this.texture = texture;
            this.font = font;
            this.position = position;
            this.numberOfImages = numberOfImages;

            loopedOnce = false;
            timeSinceLastFrame = 0;

            currentImage = new Point(0, 0);

            imageSize = new Point(texture.Width / numberOfImages.X, texture.Height/ numberOfImages.Y);

            sourceRectangle = new Rectangle(currentImage * imageSize, imageSize);

            frameRate = 1000;
        }


        public void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            if (timeSinceLastFrame >= frameRate)
            {
                currentImage.X++;

                timeSinceLastFrame -= frameRate;
                if (currentImage.X >= numberOfImages.X)
                {
                    currentImage.X = 0;
                    currentImage.Y++;

                    if (currentImage.Y >= numberOfImages.Y)
                    {
                        currentImage.Y = 0;
                        loopedOnce = true;
                    }
                }
            }

            sourceRectangle.Location = currentImage * imageSize;
        }
        


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, Color.White);

            if (loopedOnce)
                spriteBatch.DrawString(font, "Enter to continue", new Vector2(500, 50), Color.Black);
        }
    }
}