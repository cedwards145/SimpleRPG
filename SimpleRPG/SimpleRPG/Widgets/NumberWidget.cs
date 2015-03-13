using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleRPG.Widgets
{
    public class NumberWidget : TextWidget
    {
        protected int countTo, percentageToTarget = 0, numberLength;
        protected int timeUntilFade = 60, timeSinceCountCompleted = 0;

        public NumberWidget(SpriteFont reqFont, Color reqColor, Vector2 reqPosition, int targetNumber)
            : base(reqFont, reqColor, reqPosition)
        {
            countTo = targetNumber;
            numberLength = ("" + countTo).Length;
        }

        public override void update()
        {
            base.update();

            double sinValue = Math.Sin(MathHelper.ToRadians(percentageToTarget));
            sinValue *= 10000;
            sinValue = Math.Round(sinValue);
            sinValue /= 10000;

            int value = (int)(sinValue * countTo);

            setText(value.ToString("D" + numberLength));

            if (percentageToTarget < 90)
                percentageToTarget += 2;
            else if (percentageToTarget > 90)
                percentageToTarget = 90;

            if (percentageToTarget == 90)
                timeSinceCountCompleted++;

            removeMe = (timeSinceCountCompleted >= timeUntilFade);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
        }

    }
}