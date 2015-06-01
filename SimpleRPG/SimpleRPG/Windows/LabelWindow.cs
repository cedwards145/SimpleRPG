using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleRPG.Windows
{
    public class LabelWindow : Window
    {
        protected string text;
        protected TextAlign align;
        protected Vector2 textPos;
        protected Color textColor;

        public LabelWindow(Game1 game, Point reqPosition, int reqWidth, int reqHeight, string text, string windowskin)
            :base(game, reqPosition, reqWidth, reqHeight, windowskin)
        {
            font = game.getFont();
            align = TextAlign.Left;
            textColor = ColorScheme.mainTextColor;

            setText(text);
        }

        public void setText(string value)
        {
            text = value;

            float x, y;

            switch (align)
            {
                case TextAlign.Center:
                    textPos = GraphicsHelper.centerTextP(font, text, new Rectangle(location.X, location.Y, width, height));
                    break;
                case TextAlign.Left:
                    y = GraphicsHelper.centerTextP(font, text, new Rectangle(location.X, location.Y, width, height)).Y;
                    x = location.X + (20 * gameRef.getGraphicsScale());
                    textPos = new Vector2(x, y);
                    break;
                case TextAlign.Right:
                    y = GraphicsHelper.centerTextP(font, text, new Rectangle(location.X, location.Y, width, height)).Y;
                    x = location.X + width - (20 * gameRef.getGraphicsScale()) - font.MeasureString(text).X;
                    textPos = new Vector2(x, y);
                    break;
            }
        }

        public void setTextColor(Color value)
        {
            textColor = value;
        }

        public void setTextAlign(TextAlign value)
        {
            align = value;
            setText(text);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);

            spriteBatch.DrawString(font, text, textPos, textColor * opacity);
        }

    }
}
