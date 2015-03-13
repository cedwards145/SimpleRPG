using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleRPG.Widgets
{
    public class BarWidget : Widget
    {
        // Empty, current and full values of the bar
        protected int lowerValue, fillValue, upperValue;
        // Used when setting the value of the bar over a time period
        protected int targetValue;
        
        // Used to hold the value of the bar as a double, so it can be stepped in fractional
        // quantities
        protected double countingValue, step = 0;

        protected Color backColor, barColor;
        protected int width, height;

        /// <summary>
        /// Constructor method, takes max value and initializes lower value and fill value to 0
        /// </summary>
        /// <param name="maxValue">The value of the bar when it is full</param>
        /// <param name="reqWidth">The width of the bar, unscaled</param>
        /// <param name="reqHeight">The height of the bar, unscaled</param>
        /// <param name="unfilledColor">The color of the section of bar that is not filled</param>
        /// <param name="filledColor">The color of the section of bar that is filled</param>
        public BarWidget(int maxValue, int reqWidth, int reqHeight, Color unfilledColor, Color filledColor)
            :this(maxValue, 0, 0, reqWidth, reqHeight, unfilledColor, filledColor)
        {   }

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="maxValue">The value of the bar when it is full</param>
        /// <param name="leastValue">The lower value of the bar, value when bar is empty. Allows for bars that don't start at 0</param>
        /// <param name="startValue">The initial value of the bar</param>
        /// <param name="reqWidth">The width of the bar, unscaled</param>
        /// <param name="reqHeight">The height of the bar, unscaled</param>
        /// <param name="unfilledColor">The color of the section of bar that is not filled</param>
        /// <param name="filledColor">The color of the section of bar that is filled</param>
        public BarWidget(int maxValue, int leastValue, int startValue, int reqWidth, int reqHeight, Color unfilledColor, Color filledColor)
            : base()
        {
            width = reqWidth;
            height = reqHeight;
            lowerValue = leastValue;
            fillValue = startValue;
            upperValue = maxValue;
            backColor = unfilledColor;
            barColor = filledColor;
        }

        public override void update()
        {
            base.update();

            // Indicates that the bar should be incremented towards targetValue
            if (step != 0)
            {
                countingValue += step;
                fillValue = (int)Math.Round(countingValue);
                if (countingValue >= targetValue)
                    step = 0;

            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);

            Game1 gameRef = Utilities.getGameRef();

            GraphicsHelper.fillRectangle(spriteBatch, new Rectangle((int)position.X, (int)position.Y,
                                                                    width * gameRef.getGraphicsScale(), 
                                                                    height * gameRef.getGraphicsScale()), backColor * opacity);

            double fillPercentage = ((double)(fillValue - lowerValue) / (upperValue - lowerValue));

            GraphicsHelper.fillRectangle(spriteBatch, new Rectangle((int)position.X, (int)position.Y,
                                                                    (int)(width * gameRef.getGraphicsScale() * fillPercentage),
                                                                    height * gameRef.getGraphicsScale()), barColor * opacity);
        }

        /// <summary>
        /// Instantly sets the bar's filled value to value
        /// </summary>
        /// <param name="value">The new value for the bar</param>
        public void setValue(int value)
        {
            fillValue = value;
        }

        /// <summary>
        /// Sets the bar's filled value to value over a number of frames
        /// </summary>
        /// <param name="value">The new value for the bar</param>
        /// <param name="frames">The number of frames taken for current value to equal new value</param>
        public void setValue(int value, int frames)
        {
            step = (value - fillValue) / (double)frames;
            countingValue = fillValue;
            targetValue = value;
        }

        public void setUpperValue(int value)
        {
            upperValue = value;
        }

        public void setLowerValue(int value)
        {
            lowerValue = value;
        }

        public int getUpperValue()
        {
            return upperValue;
        }

        public int getLowerValue()
        {
            return lowerValue;
        }

        public int getValue()
        {
            return fillValue;
        }
    }
}
