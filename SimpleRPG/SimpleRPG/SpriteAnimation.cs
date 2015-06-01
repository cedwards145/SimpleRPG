using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleRPG
{
    public class SpriteAnimation
    {
        public enum LoopType { Single, Forever };
        public enum AnimationType { Row, Continuous };

        /// <summary>
        /// Specifies how the frames of animation are layed out in the texture
        /// </summary>
        protected AnimationType animationType;

        /// <summary>
        /// Specifies how the animation should loop
        /// </summary>
        protected LoopType loopType;

        /// <summary>
        /// The spritesheet to animate
        /// </summary>
        protected Texture2D spriteSheet;

        /// <summary>
        /// The number of frames the spritesheet has in each row
        /// </summary>
        protected int framesAcross;

        /// <summary>
        /// The number of frames the spritesheet has in each column
        /// </summary>
        protected int framesDown;

        /// <summary>
        /// The amount of game frames to spend on each frame of animation
        /// </summary>
        protected int framesPerAnimationFrame;

        /// <summary>
        /// Specifies which row of the spritesheet should be animated
        /// </summary>
        protected int rowToAnimate;

        /// <summary>
        /// The current frame of animation;
        /// </summary>
        protected int currentFrame;

        public SpriteAnimation(string spriteSheetName)
            : this(spriteSheetName, 4, 4, 15)
        { }

        public SpriteAnimation(string spriteSheetName, int reqFramesAcross, int reqFramesDown, int reqFramesPerFrame)
        {
            spriteSheet = Utilities.getGameRef().Content.Load<Texture2D>(@"graphics\" + spriteSheetName);
            loopType = LoopType.Single;
            animationType = AnimationType.Continuous;
            rowToAnimate = 0;
            currentFrame = 0;
            framesPerAnimationFrame = reqFramesPerFrame;
            framesAcross = reqFramesAcross;
            framesDown = reqFramesDown;
        }

        public void update()
        {
            // Increase currentFrame if animation loops forever, or animation has not finished
            if (shouldContinue())
                currentFrame++;

            if (Debug.DEBUGGING && Input.isKeyPressed(Microsoft.Xna.Framework.Input.Keys.R))
                resetAnimation();
        }

        protected bool shouldContinue()
        {
            // Continuous animation
            if (animationType == AnimationType.Continuous)
            {
                return currentFrame < (framesPerAnimationFrame * framesAcross * framesDown) - 1;
            }
            // Row animation
            else
            {
                // Either the animation loops forever, and should continue,
                // or the animation loops once and has not yet finished
                return (loopType == LoopType.Forever ||
                        (loopType == LoopType.Single && currentFrame < (framesPerAnimationFrame * framesAcross) - 1));
            }
        }

        public void resetAnimation()
        {
            currentFrame = 0;
        }

        public Rectangle getSourceRectangle()
        {
            int frameHeight = spriteSheet.Height / framesDown;
            int frameWidth = spriteSheet.Width / framesAcross;
            int x, y;

            if (animationType == AnimationType.Row)
            {
                x = frameWidth * ((currentFrame % (framesPerAnimationFrame * framesAcross)) / framesPerAnimationFrame);
                y = frameHeight * rowToAnimate;
            }
            else
            {
                int currentAnimFrame = ((currentFrame % (framesPerAnimationFrame * framesAcross * framesDown)) 
                                       / framesPerAnimationFrame);
                x = (currentAnimFrame % framesAcross) * frameWidth;
                y = (currentAnimFrame / framesAcross) * frameHeight;
            }

            return new Rectangle(x, y, frameWidth, frameHeight);
        }

        public Texture2D getSpriteSheet()
        {
            return spriteSheet;
        }

        public int getWidth()
        {
            return spriteSheet.Width / framesAcross;
        }

        public int getHeight()
        {
            return spriteSheet.Height / framesDown;
        }
    }
}
