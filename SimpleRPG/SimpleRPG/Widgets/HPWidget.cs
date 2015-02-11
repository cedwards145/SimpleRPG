using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleRPG.Widgets
{
    public class HPWidget : TextWidget
    {
        protected Battler battler;
        protected float flashThreshold = 0.1f;
        protected int barWidth, barHeight;
        protected Color barColor;

        public HPWidget(SpriteFont reqFont, Color reqColor, Vector2 reqPosition, Battler reqBattler, int reqBarWidth, int reqBarHeight)
            :base (reqFont, reqColor, reqPosition)
        {
            battler = reqBattler;
            barWidth = reqBarWidth;
            barHeight = reqBarHeight;

            barColor = new Color(188, 54, 54);
        }

        public override void update()
        {
            base.update();

            if (battler.getHP() / (float)battler.getMaxHP() < flashThreshold)
                flash(Color.Red);

            setText("HP " + battler.getHP() + "/" + battler.getMaxHP());
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            int scale = Utilities.getGameRef().getGraphicsScale();
            int textWidth = (int)font.MeasureString("HP").X + 5 * scale;

            double hpPerc = battler.getHP() / (double)battler.getMaxHP();

            Rectangle shadowRect = new Rectangle((int)(position.X) + textWidth + 1 * scale,
                                                 (int)(position.Y) + 4 * scale,
                                                 barWidth, barHeight);
            GraphicsHelper.fillRectangle(spriteBatch, shadowRect, Color.Black * opacity);

            Rectangle fillRect = new Rectangle((int)(position.X) + textWidth,
                                               (int)(position.Y) + 3 * scale,
                                               (int)(barWidth * hpPerc), barHeight);
            GraphicsHelper.fillRectangle(spriteBatch, fillRect, barColor * opacity);

            base.draw(spriteBatch);
        }
    }
}
