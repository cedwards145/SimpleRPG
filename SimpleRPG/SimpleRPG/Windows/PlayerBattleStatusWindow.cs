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
        protected int selectedBattler = -1;

        // Only used to iterate across party names, widgets are also contained in
        // widgets list
        protected List<TextWidget> nameWidgets;

        public PlayerBattleStatusWindow(Game1 game, Point reqPosition, string windowskin)
            : base(game, reqPosition, 320 * game.getGraphicsScale(), 71 * game.getGraphicsScale(), windowskin)
        {
            widgets = new List<TextWidget>();
            nameWidgets = new List<TextWidget>();
        }

        public override void update()
        {
            base.update();

            for (int index = 0; index < nameWidgets.Count; index++)
            {
                if (index == selectedBattler)
                    nameWidgets[index].flash(ColorScheme.selectedTextColor);
                else
                    nameWidgets[index].stopFlash();
            }

            foreach (TextWidget widget in widgets)
                widget.update();
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

        public void setSelectedBattler(int index)
        {
            selectedBattler = index;
        }

        protected void addWidgets()
        {
            int scale = Utilities.getGameRef().getGraphicsScale();

            List<Battler> party = Player.getParty();

            for (int index = 0; index < party.Count(); index++)
            {
                Battler battler = party[index];

                TextWidget nameWidget;
                nameWidget = new TextWidget(font, ColorScheme.mainTextColor,
                                            new Vector2(location.X + 15 * scale, location.Y + (scale * (25 * index + 8))),
                                            battler.getName());
                widgets.Add(nameWidget);
                nameWidgets.Add(nameWidget);

                widgets.Add(new HPWidget(font, ColorScheme.mainTextColor,
                                         new Vector2(location.X + 115 * scale, location.Y + (scale * (25 * index + 8))),
                                         battler, 170 * scale, 7 * scale));

                widgets.Add(new MPWidget(font, ColorScheme.mainTextColor,
                                         new Vector2(location.X + 115 * scale, location.Y + (scale * (25 * index + 16))),
                                         battler, 170 * scale, 7 * scale));
            }
        }

        public override void setPosition(Point newPosition)
        {
            base.setPosition(newPosition);
            widgets.Clear();
            nameWidgets.Clear();
            addWidgets();
        }
    }
}
