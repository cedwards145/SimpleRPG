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

        public NumberWidget(SpriteFont reqFont, Color reqColor, Vector2 reqPosition, int targetNumber)
            : base(reqFont, reqColor, reqPosition)
        {
            countTo = targetNumber;
            numberLength = ("" + countTo).Length;
        }

        public override void update()
        {
            base.update();

            int value = (int)(Math.Sin(MathHelper.ToRadians(percentageToTarget)) * countTo);
            setText(value.ToString("D" + numberLength));

            if (percentageToTarget < 90)
                percentageToTarget += 2;
            else if (percentageToTarget > 90)
                percentageToTarget = 90;
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
        }

    }
}