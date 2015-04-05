using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleRPG.Widgets;
using SimpleRPG.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleRPG.States
{
    public class BattleCompleteState : GameState
    {
        protected BattleCompleteWindow window;
        protected List<TextWidget> nameWidgets;
        protected List<Battler> party;

        public BattleCompleteState(Game1 game, GameState parent, StateManager manager, BattleState battle)
            : base(game, parent, manager)
        {
            window = new BattleCompleteWindow(game, "windowskin", battle.getTotalExp());
            nameWidgets = new List<TextWidget>();

            int scale = gameRef.getGraphicsScale();

            party = Player.getParty();
            for (int index = 0; index < party.Count; index++)
            {
                if (party[index] is PlayerBattler)
                {
                    PlayerBattler battler = (PlayerBattler)party[index];
                    TextWidget text = new TextWidget(game.getFont(), ColorScheme.mainTextColor,
                                                     new Vector2(100, 100 + (index * 50)), 
                                                     battler.getName() + " - L" + battler.getLevel());
                    nameWidgets.Add(text);
                    widgets.Add(text);

                    ExpBarWidget bar = new ExpBarWidget(100, 5, battler, battle.getTotalExp());
                    bar.setPosition(new Vector2(100, 120 + (index * 50)));

                    widgets.Add(bar);
                }
            }
        }

        public override void update()
        {
            base.update();
            window.update();

            if (!closing && Input.isButtonPressed(ControllerButton.enter))
                exit();

            for (int index = 0; index < party.Count; index++)
            {
                PlayerBattler battler = (PlayerBattler)party[index];
                nameWidgets[index].setText(battler.getName() + " - L" + battler.getLevel());
            }
        }

        public override void exit()
        {
            base.exit();

            foreach (Widget widget in widgets)
            {
                if (widget is ExpBarWidget)
                    ((ExpBarWidget)widget).giveRemainingEXP();
            }

            // Leave battle
            Player.exitBattle();

            parentState.exit();
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
            window.setOpacity(opacity);
            window.draw(spriteBatch);
        }
    }
}
