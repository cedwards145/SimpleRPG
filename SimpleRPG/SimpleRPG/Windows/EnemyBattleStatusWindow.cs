using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleRPG.Windows
{
    public class EnemyBattleStatusWindow : Window
    {
        protected Battler enemy;
        protected Texture2D healthWindow;
        protected Color barColor;

        public EnemyBattleStatusWindow(Game1 game, Point reqPosition, Battler reqEnemy)
            : base(game, reqPosition, 0, 0, "windowskin")
        {
            enemy = reqEnemy;
            healthWindow = game.Content.Load<Texture2D>(@"graphics\health bar");
            barColor = new Color(188, 54, 54);
        }

        public override void update()
        {
            base.update();
            
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            int scale = gameRef.getGraphicsScale();

            spriteBatch.Draw(healthWindow, new Rectangle(location.X, location.Y, healthWindow.Width * scale, healthWindow.Height * scale), Color.White * opacity);

            double healthPerc = enemy.getHP() / (double)enemy.getMaxHP();
            Rectangle destination = new Rectangle(location.X + 16 * scale, location.Y + 4 * scale, (int)(healthPerc * 50 * scale), 3 * scale);

            GraphicsHelper.fillRectangle(spriteBatch, destination, barColor * opacity);
        }
    }
}
