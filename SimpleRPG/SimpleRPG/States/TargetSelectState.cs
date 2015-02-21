using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleRPG.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SimpleRPG.States
{
    public class TargetSelectState : GameState
    {
        protected ListBox targets;
        protected List<Battler> enemies;

        public TargetSelectState(Game1 game, GameState parent, StateManager manager, List<Battler> battlers)
            : base(game, parent, manager)
        {
            // Get list of targets
            List<string> targetNames = new List<string>();
            bool[] battlersAlive = new bool[battlers.Count];

            enemies = battlers;

            for (int index = 0; index < battlers.Count; index++)
            {
                targetNames.Add(battlers[index].getName());
                battlersAlive[index] = battlers[index].isAlive();
            }

            targets = new ListBox(game, new Point(420 * game.getGraphicsScale(), 0), 150 * game.getGraphicsScale(), 3, targetNames.ToArray(), "windowskin");
            targets.setEnabledOptions(battlersAlive);

            targets.setToBottom();
        }

        public override void update()
        {
            base.update();
            targets.update();

            if (Input.isButtonPressed(Controller.ControllerButton.enter))
            {
                int index = targets.getIndex();
                parentState.passData(this, enemies[index]);
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
            targets.setOpacity(opacity);
            targets.draw(spriteBatch);
        }
    }
}
