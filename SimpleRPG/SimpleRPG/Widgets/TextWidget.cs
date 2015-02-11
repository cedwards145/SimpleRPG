using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleRPG.Widgets
{
    public class TextWidget : Drawable
    {
        protected string text = "";
        protected SpriteFont font;
        protected Vector2 position;
        protected Color color;

        protected Color flashColor, oldColor, startColor;
        protected int redChange, greenChange, blueChange;
        protected bool flashing;

        public TextWidget(SpriteFont reqFont, Color reqColor, Vector2 reqPosition)
            :this(reqFont, reqColor, reqPosition, "")
        {   }

        public TextWidget(SpriteFont reqFont, Color reqColor, Vector2 reqPosition, string reqText)
        {
            font = reqFont;
            color = reqColor;
            position = reqPosition;
            text = reqText;
        }

        public override void update()
        {
            base.update();

            if (flashing)
                flashUpdate();
        }

        protected void flashUpdate()
        {
            int r = color.R + redChange;
            int g = color.G + greenChange;
            int b = color.B + blueChange;

            if ((redChange > 0 && r > flashColor.R) || (redChange < 0 && r < flashColor.R))
                r = flashColor.R;
            if (greenChange > 0 && g > flashColor.G || (greenChange < 0 && g < flashColor.G))
                g = flashColor.G;
            if (blueChange > 0 && b > flashColor.B || (blueChange < 0 && b < flashColor.B))
                b = flashColor.B;

            color = new Color(r, g, b);

            if (color.Equals(flashColor))
            {
                Color temp = flashColor;
                flashColor = oldColor;
                oldColor = temp;
                redChange *= -1;
                greenChange *= -1;
                blueChange *= -1;
            }
        }

        public void setText(string newText)
        {
            text = newText;
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);

            spriteBatch.DrawString(font, text, position, color * opacity);
        }

        public void flash(Color newColor)
        {
            if (!flashing)
            {
                oldColor = color;
                startColor = color;
                flashColor = newColor;
                flashing = true;

                redChange = (flashColor.R - startColor.R) / 20;
                greenChange = (flashColor.G - startColor.G) / 20;
                blueChange = (flashColor.B - startColor.B) / 20;
            }
        }

        public void stopFlash()
        {
            if (flashing)
            {
                color = startColor;
                flashing = false;
            }
        }

        public void setPosition(Vector2 pos)
        {
            position = pos;
        }

    }
}
