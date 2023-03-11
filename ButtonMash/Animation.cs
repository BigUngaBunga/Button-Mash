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
    class Animation
    {
        //Deklarationer
        Texture2D texture;
        Vector2 position;
        SpriteFont font;
        Rectangle sourceRectangle;
        Point imageSize, numberOfImages, currentImage;
        float frameRate, timeSinceLastFrame;
        bool loopedOnce;


        public Animation(Texture2D texture, SpriteFont font, Vector2 position, Point numberOfImages)
        {
            this.texture = texture;
            this.font = font;
            this.position = position;
            this.numberOfImages = numberOfImages;

            loopedOnce = false;
            timeSinceLastFrame = 0;

            currentImage = new Point(0, 0);

            imageSize = new Point(texture.Width / numberOfImages.X, texture.Height/ numberOfImages.Y);//Storlek på animationsbilderna

            sourceRectangle = new Rectangle(currentImage * imageSize, imageSize);//Plats och storlek på 

            frameRate = 1000;
        }


        public void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            //Loopar igenom spriteSheet
            if (timeSinceLastFrame >= frameRate)
            {
                currentImage.X++;//Flyttar till höger

                timeSinceLastFrame -= frameRate;
                if (currentImage.X >= numberOfImages.X)
                {
                    currentImage.X = 0;
                    currentImage.Y++;//Flyttar till nästa rad

                    if (currentImage.Y >= numberOfImages.Y)
                    {
                        currentImage.Y = 0;//Börjar på nytt
                        loopedOnce = true;
                    }
                }
            }

            sourceRectangle.Location = currentImage * imageSize;//Uppdaterar bild
        }
        


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, Color.White);//Ritar animationen new Vector2(300, 645)

            if (loopedOnce)
            {
                spriteBatch.DrawString(font, "Hit enter", new Vector2(500, 50), Color.Black);//Om loopen har körts en gång, ritar sträng
            }
        }
    }
}