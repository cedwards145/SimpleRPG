using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SimpleRPG.Windows
{
    public class ListBox : Window
    {
        protected List<string> options;
        protected int index = 0, indexOffset = 0, noOptionsInWindow;
        protected static Color selectedColor = new Color(50, 190, 255);
        protected TextAlign textAlign = TextAlign.Right;

        public ListBox(Game1 game, Point reqPosition, int reqWidth, int optionsInWindow, string[] items, string windowskin)
            : base(game, reqPosition, reqWidth, 0, windowskin)
        {
            noOptionsInWindow = optionsInWindow;
            setOptions(items);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);

            drawOptions(spriteBatch);
        }

        protected virtual void drawOptions(SpriteBatch spriteBatch)
        {
            int charHeight = (int)font.MeasureString("I").Y;
            int scale = gameRef.getGraphicsScale();

            Vector2 drawPosition;

            for (int optionsIndex = 0; optionsIndex < noOptionsInWindow && optionsIndex < options.Count; optionsIndex++)
            {
                if (textAlign == TextAlign.Right)
                    drawPosition = new Vector2(location.X + (15 * scale),
                                               location.Y + 10 * scale + (charHeight * optionsIndex));
                else if (textAlign == TextAlign.Center)
                    drawPosition = new Vector2(GraphicsHelper.centerTextP(font, options[optionsIndex + indexOffset], getSize()).X,
                                               location.Y + 10 * scale + (charHeight * optionsIndex));
                else
                {
                    int stringWidth = (int)(font.MeasureString(options[optionsIndex + indexOffset]).X);
                    drawPosition = new Vector2(location.X + width - (10 * scale + stringWidth),
                                               location.Y + 10 * scale + (charHeight * optionsIndex));
                }

                Color color = (optionsIndex + indexOffset == index ? selectedColor : textColor);
                spriteBatch.DrawString(font, options[optionsIndex + indexOffset],
                                       drawPosition,
                                       color * opacity);
            }
        }

        public override void update()
        {
            base.update();

            if (Input.isKeyPressed(Keys.W))
            {
                if (index - indexOffset == 0)
                    indexOffset--;
                index--;

                if (index < 0)
                {
                    index = options.Count - 1;
                    indexOffset = (int)MathHelper.Clamp(options.Count - noOptionsInWindow, 0, options.Count);
                }
            }
            else if (Input.isKeyPressed(Keys.S))
            {
                if (index - indexOffset == noOptionsInWindow - 1)
                    indexOffset++;
                index++;

                if (index >= options.Count)
                {
                    index = 0;
                    indexOffset = 0;
                }
            }

            if (index >= options.Count)
                index = options.Count - 1;
        }

        public void setOptions(string[] items)
        {
            options = new List<string>(items);

            int windowskinTileSize = skin.Width * gameRef.getGraphicsScale() / 3;

            int itemSize = (int)font.MeasureString("I").Y;
            
            height = (noOptionsInWindow * itemSize) + (windowskinTileSize * 2);
        }

        public int getIndex()
        {
            return index;
        }

        public void setTextAlign(TextAlign newAlignment)
        {
            textAlign = newAlignment;
        }
    }
}
