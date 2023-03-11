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
    class Hand
    {
        private readonly Texture2D handTexture, shadow;
        private Vector2 position;
        private MouseState mouseState;
        private readonly float scale, defaultAngle, rotationSpeed;
        private float rotation;
        public Rectangle mashBox;


        public Hand(Texture2D handTexture, Texture2D shadow)
        {
            this.handTexture = handTexture;
            this.shadow = shadow;

            scale = 0.4f;
            defaultAngle = 0.3f;
            rotationSpeed = 0.1f;

            mashBox = new Rectangle(mouseState.X, mouseState.Y, 150, 50);
        }


        public void Update()
        {
            mouseState = Mouse.GetState();
            position.X = mouseState.X + handTexture.Width * scale * 0.8f; 
            position.Y = mouseState.Y;

            mashBox.X = mouseState.X - mashBox.Width / 2;
            mashBox.Y = mouseState.Y - mashBox.Height / 2;

            SwingHand(mouseState);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(shadow, mashBox, new Color(0, 0, 0, 75));
            spriteBatch.Draw(handTexture, position, null, Color.White, rotation, new Vector2(handTexture.Width, handTexture.Height), scale, SpriteEffects.None, 0f);
        }

        private void SwingHand(MouseState mouseState)
        {
            if (mouseState.LeftButton.Equals(ButtonState.Pressed) || mouseState.RightButton.Equals(ButtonState.Pressed))
            {
                rotation = 0f;
            }
            else if (rotation < defaultAngle)
                rotation += rotationSpeed;
        }
    }
}
