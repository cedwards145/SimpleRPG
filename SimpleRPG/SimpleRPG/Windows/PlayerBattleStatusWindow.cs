using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimpleRPG.Widgets;

namespace SimpleRPG.Windows
{
    public class PlayerBattleStatusWindow : Window
    {
        protected List<TextWidget> widgets;

        public PlayerBattleStatusWindow(Game1 game, Point reqPosition, string windowskin)
            : base(game, reqPosition, 320 * game.getGraphicsScale(), 71 * game.getGraphicsScale(), windowskin)
        {
            widgets = new List<TextWidget>();
        }

        public override void update()
        {
            base.update();
            
            foreach (TextWidget widget in widgets)
                widget.update();

            if (Input.isKeyPressed(Microsoft.Xna.Framework.Input.Keys.K))
                Player.getParty()[0].addHP(-95);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);

            foreach (TextWidget widget in widgets)
            {
                widget.setOpacity(opacity);
                widget.draw(spriteBatch);
            }
        }

        protected void addWidgets()
        {
            int scale = Utilities.getGameRef().getGraphicsScale();

            List<Battler> party = Player.getParty();

            for (int index = 0; index < party.Count(); index++)
            {
                Battler battler = party[index];

                widgets.Add(new TextWidget(font, Color.White,
                                           new Vector2(location.X + 15 * scale, location.Y + (scale * (25 * index + 8))),
                                           battler.getName()));
                widgets.Add(new HPWidget(font, Color.White,
                                         new Vector2(location.X + 115 * scale, location.Y + (scale * (25 * index + 8))),
                                         battler, 170 * scale, 7 * scale));
                widgets.Add(new MPWidget(font, Color.White,
                                         new Vector2(location.X + 115 * scale, location.Y + (scale * (25 * index + 16))),
                                         battler, 170 * scale, 7 * scale));
            }
        }

        public override void setPosition(Point newPosition)
        {
            base.setPosition(newPosition);
            widgets.Clear();
            addWidgets();
        }
    }
}
