using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SimpleRPG
{
    public class DrawableRectangle
    {
        private Rectangle rect;
        private Texture2D tex;

        public DrawableRectangle(Rectangle newRect, Game game)
        {
            rect = newRect;
            tex = new Texture2D(game.GraphicsDevice, 1, 1);
            tex.SetData(new Color[1] { Color.White });
        }

        public void setRectangle(Rectangle newRect)
        {
            rect = newRect;
        }

        public void setColor(Color newColor)
        {
            Color[] colors = new Color[1];
            colors[0] = newColor;
            tex.SetData<Color>(colors);
        }

        public void draw(SpriteBatch sb, Color color, int width)
        {
            sb.Draw(tex, new Rectangle(rect.X - (width / 2), rect.Y - (width / 2), rect.Width + width, width), color);
            sb.Draw(tex, new Rectangle(rect.X - (width / 2), rect.Y + rect.Height - (width / 2), rect.Width + width, width), color);
            sb.Draw(tex, new Rectangle(rect.X - (width / 2), rect.Y - (width / 2), width, rect.Height + width), color);
            sb.Draw(tex, new Rectangle(rect.X + rect.Width - (width / 2), rect.Y - (width / 2), width, rect.Height + width), color);
        }

        public void fill(SpriteBatch sb, Color color)
        {
            sb.Draw(tex, rect, color);
        }
    }
}
