using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleRPG.Windows
{
    public class ItemDescriptionWindow : Window
    {
        protected string description = "";

        public ItemDescriptionWindow(Game1 game, Point reqPosition, int reqWidth, int reqHeight)
            : base(game, reqPosition, reqWidth, reqHeight, "windowskin")
        { }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
            spriteBatch.DrawString(font, description, new Vector2(location.X + 15 * gameRef.getGraphicsScale(),
                                                                  GraphicsHelper.centerTextP(font, description, getSize()).Y), Color.White * opacity);
        }

        public void setDescription(string newDescription)
        {
            description = newDescription;
        }

    }
}
