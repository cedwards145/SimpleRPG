using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleRPG.Windows
{
    public class BattleCompleteWindow : Window
    {
        protected string text = "";

        public BattleCompleteWindow(Game1 game, string windowskin, int expEarned)
            : base(game, new Point(), 200 * game.getGraphicsScale(), 50 * game.getGraphicsScale(), windowskin)
        {
            location = GraphicsHelper.calculateCenterPositionP(width, height);
            text = "Battle complete!\nEarned " + expEarned + " EXP!";
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);

            Vector2 size = font.MeasureString(text);

            spriteBatch.DrawString(font, text, GraphicsHelper.centerTextP(font, text, getSize()), 
                                                                          ColorScheme.mainTextColor * opacity);
        }
    }
}
