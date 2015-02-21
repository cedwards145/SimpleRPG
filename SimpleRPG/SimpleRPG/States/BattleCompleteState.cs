using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleRPG.Windows;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleRPG.States
{
    public class BattleCompleteState : GameState
    {
        protected BattleCompleteWindow window;

        public BattleCompleteState(Game1 game, GameState parent, StateManager manager, BattleState battle)
            : base(game, parent, manager)
        {
            window = new BattleCompleteWindow(game, "windowskin", battle.getTotalExp());
        }

        public override void update()
        {
            base.update();
            window.update();

            if (!closing && Input.isButtonPressed(Controller.ControllerButton.enter))
                exit();
        }

        public override void exit()
        {
            base.exit();

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
