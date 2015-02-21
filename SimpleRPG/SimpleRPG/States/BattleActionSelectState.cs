using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using SimpleRPG.Windows;
using SimpleRPG.States;

namespace SimpleRPG.States
{
    public class BattleActionSelectState : GameState
    {
        protected ListBox actionsWindow;
        protected BattleState battleState;

        public BattleActionSelectState(Game1 game, GameState parent, StateManager manager, BattleState battle)
            : base(game, parent, manager)
        {
            popOnEscape = false;

            actionsWindow = new ListBox(game, new Point(320 * game.getGraphicsScale(), 0), 100 * game.getGraphicsScale(), 3, new string[] { "Attack", "Skill", "Item" }, "windowskin");
            actionsWindow.setToBottom();
            battleState = battle;
        }

        public override void update()
        {
            base.update();
            actionsWindow.update();

            int index = actionsWindow.getIndex();

            if (Input.isButtonPressed(Controller.ControllerButton.enter))
            {
                Point actionsWindowPosition = actionsWindow.getPosition();
                Point newPosition = new Point(actionsWindowPosition.X + actionsWindow.getWidth(),
                                              actionsWindowPosition.Y);

                // Attack
                if (index == 0)
                {
                    addChildState(new TargetSelectState(gameRef, this, stateManager, battleState.getEnemies()));
                }
                // Skill
                else if (index == 1)
                {

                }
                // Item
                else if (index == 2)
                {
                    addChildState(new InventoryState(gameRef, stateManager, this));
                }
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
            actionsWindow.setOpacity(opacity);
            actionsWindow.draw(spriteBatch);
        }

        public override void passData(GameState sender, object data)
        {
            base.passData(sender, data);

            if (sender is TargetSelectState)
            {
                // Data contains battler that is the target of the selected action
                Battler target = (Battler)data;
                int actionIndex = actionsWindow.getIndex();

                Battler current = battleState.getCurrentBattler();

                // Attack
                if (actionIndex == 0)
                {
                    stateManager.removeState(this);
                    CombatResolver.physicalAttack(current, target, battleState);

                    //battleState.showCombatResult(current, target, damageDealt);
                }
            }
        }

    }
}