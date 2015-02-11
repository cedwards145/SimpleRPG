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
            : this(game, parent, new Point(), manager, battlers)
        { }

        public TargetSelectState(Game1 game, GameState parent, Point windowPosition, StateManager manager, List<Battler> battlers)
            : base(game, parent, manager)
        {
            // Get list of targets
            List<string> targetNames = new List<string>();

            enemies = battlers;

            for (int index = 0; index < battlers.Count; index++)
                // if (battlers[index].isAlive())
                    targetNames.Add(battlers[index].getName());

            targets = new ListBox(game, windowPosition, 150 * game.getGraphicsScale(), 3, targetNames.ToArray(), "windowskin");
        }

        public override void update()
        {
            base.update();
            targets.update();

            if (Input.isKeyPressed(Keys.Enter))
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
