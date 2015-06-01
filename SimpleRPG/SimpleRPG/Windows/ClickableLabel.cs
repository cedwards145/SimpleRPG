using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SimpleRPG.Windows
{
    public class ClickableLabel : LabelWindow
    {
        protected bool enabled;

        public ClickableLabel(Game1 game, Point reqPosition, int reqWidth, int reqHeight, string text, string windowskin)
            : base(game, reqPosition, reqWidth, reqHeight, text, windowskin)
        {

        }

        public void setEnabled(bool value)
        {
            enabled = value;
        }

        public bool getEnabled()
        {
            return enabled;
        }

        public override void draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);

            int scale = gameRef.getGraphicsScale();

            Rectangle buttonRectangle = new Rectangle(location.X + (16 * scale), location.Y + (8 * scale),
                                                      width - (32 * scale), height - (18 * scale));

            if (enabled)
                GraphicsHelper.fillRectangle(spriteBatch, buttonRectangle, ColorScheme.selectedTextColor);

            GraphicsHelper.drawRectangle(spriteBatch, buttonRectangle, Color.Black);

            // Redraw text
            spriteBatch.DrawString(font, text, textPos, textColor * opacity);
        }
    }
}
