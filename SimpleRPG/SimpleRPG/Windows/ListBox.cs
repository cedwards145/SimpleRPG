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
        
        protected TextAlign textAlign = TextAlign.Right;
        protected List<bool> optionStates;

        protected bool enabled = true;

        public ListBox(Game1 game, Point reqPosition, int reqWidth, int optionsInWindow, string[] items, string windowskin)
            : base(game, reqPosition, reqWidth, 0, windowskin)
        {
            noOptionsInWindow = optionsInWindow;
            optionStates = new List<bool>();
            setOptions(items);

            setFirstIndex();
        }

        protected void setFirstIndex()
        {
            // Find first enabled index
            index = -1;
            moveCursorDown();
        }

        protected void setLastIndex()
        {
            index = options.Count;
            moveCursorUp();
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

                Color color = (enabled && (optionsIndex + indexOffset == index) ? ColorScheme.selectedTextColor : ColorScheme.mainTextColor);
                color = (optionStates[optionsIndex + indexOffset] ? color : ColorScheme.disabledColor);

                spriteBatch.DrawString(font, options[optionsIndex + indexOffset],
                                       drawPosition,
                                       color * opacity);
            }
        }

        public override void update()
        {
            base.update();

            if (enabled && Input.isButtonPressed(ControllerButton.up))
            {
                moveCursorUp();
            }
            else if (enabled && Input.isButtonPressed(ControllerButton.down))
            {
                moveCursorDown();
            }

            if (index >= options.Count)
                index = options.Count - 1;
        }

        protected void moveCursorDown()
        {
            // If at least one option is enabled
            if (optionStates.Contains(true))
            {
                do
                {
                    if (index - indexOffset == noOptionsInWindow - 1)
                        indexOffset++;
                    index++;

                    if (index >= options.Count)
                    {
                        index = 0;
                        indexOffset = 0;
                    }
                } while (!optionStates[index + indexOffset]);
            }
        }

        protected void checkCursorLegal()
        {
            if (index < 0)
                setFirstIndex();
            else if (index >= options.Count)
                setLastIndex();
        }

        protected void moveCursorUp()
        {
            if (optionStates.Contains(true))
            {
                do
                {
                    if (index - indexOffset == 0)
                        indexOffset--;
                    index--;

                    if (index < 0)
                    {
                        index = options.Count - 1;
                        indexOffset = (int)MathHelper.Clamp(options.Count - noOptionsInWindow, 0, options.Count);
                    }
                } while (!optionStates[index]);
            }
        }

        public void setOptions(string[] items, bool[] enabled)
        {
            options = new List<string>(items);
            optionStates.Clear();
            // Enable all options by default
            for (int tempIndex = 0; tempIndex < enabled.Count(); tempIndex++)
                optionStates.Add(enabled[tempIndex]);

            int windowskinTileSize = skin.Width * gameRef.getGraphicsScale() / 3;

            int itemSize = (int)font.MeasureString("I").Y;

            height = (noOptionsInWindow * itemSize) + (windowskinTileSize * 2);
            checkCursorLegal();
        }
        public void setOptions(string[] items)
        {
            bool[] enabled = new bool[items.Count()];
            for (int tempIndex = 0; tempIndex < items.Count(); tempIndex++)
                enabled[tempIndex] = true;

            setOptions(items, enabled);
        }

        public void setIndexEnableState(int tempIndex, bool newState)
        {
            optionStates[tempIndex] = newState;
            setFirstIndex();
        }

        public void setEnabledOptions(bool[] enabled)
        {
            optionStates.Clear();
            foreach (bool b in enabled)
                optionStates.Add(b);
            setFirstIndex();
        }

        public int getIndex()
        {
            return index;
        }

        public string getSelectedOption()
        {
            return options[index];
        }

        public bool getEnabled()
        {
            return enabled;
        }

        public void setEnabled(bool value)
        {
            enabled = value;
        }

        public void setTextAlign(TextAlign newAlignment)
        {
            textAlign = newAlignment;
        }
    }
}
