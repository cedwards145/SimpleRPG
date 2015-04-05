using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleRPG.States
{
    public class SplashScreenState : GameState
    {
        protected Texture2D texture;
        protected Vector2 drawPosition;
        protected GameState followingState;

        protected int counter, framesToDisplay = 300;

        public SplashScreenState(Game1 game, StateManager manager, string imageName, GameState nextState)
            : base(game, null, manager)
        {
            texture = game.Content.Load<Texture2D>(@"graphics\" + imageName);
            followingState = nextState;

            inAnimation = WindowAnimationType.FadeSlow;
            outAnimation = WindowAnimationType.FadeSlow;
        }

        public override void update()
        {
            base.update();

            // If state has completely faded in, start counting
            if (opacity >= 1)
                counter++;

            // If counter has completed, fade out
            if (counter == framesToDisplay)
            {
                exit();

                // Stops exit being called every frame
                counter++;
            }

            if (Input.isButtonPressed(ControllerButton.back)
                || Input.isButtonPressed(ControllerButton.enter))
                exit();
        }

        public override void close()
        {
            base.close();
            stateManager.addState(followingState);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);

            int width = gameRef.getWidth(), height = gameRef.getHeight();

            // Decide whether to scale splash screen based on width, or height
            // Calculate what the height of the image would be if drawn at screen width,
            // and what the width of the image would be if drawn at screen height
            float heightByWidth = texture.Height * ((float)width / texture.Width);
            float widthByHeight = texture.Width * ((float)height / texture.Height);

            Rectangle destination;

            // Check if the image would fit at screen width
            if (heightByWidth <= height)
                destination = new Rectangle(0, (int)((height - heightByWidth) / 2f), width, (int)heightByWidth);
            else
                destination = new Rectangle((int)((width - widthByHeight) / 2f), 0, width, (int)heightByWidth);

            // SamplerState set to pointclamp to correctly render pixel art
            // Set to linearwrap to render splash screen
            gameRef.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            spriteBatch.Draw(texture, destination, Color.White * opacity);
            gameRef.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        }
    }
}
