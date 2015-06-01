using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleRPG.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimpleRPG.Items;

namespace SimpleRPG.States
{
    public class UseItemState : GameState
    {
        protected UseItemWindow itemWindow;
        protected UsableItem toUse;

        public UseItemState(Game1 game, GameState parent, StateManager manager, Point windowPosition, Item item)
            : base(game, parent, manager)
        {
            itemWindow = new UseItemWindow(game, windowPosition);
            toUse = (UsableItem)item;
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
            itemWindow.setOpacity(opacity);
            itemWindow.draw(spriteBatch);
        }

        public override void update()
        {
            base.update();
            itemWindow.update();

            int selectedIndex = itemWindow.getIndex();

            if (Input.isButtonPressed(Controller.ControllerButton.enter))
            {
                // Use
                if (selectedIndex == 0)
                    useItem();
                // Drop
                else if (selectedIndex == 1)
                {
                    Player.getInventory().removeItem(toUse);
                    exit();
                }
                // Cancel
                else if (selectedIndex == 2)
                    exit();
            }
        }

        /// <summary>
        /// Called when the selected item is to be used
        /// </summary>
        protected virtual void useItem()
        {
            ValidTargets targets = toUse.getValidTargets();

            // Item can only target party if the item only targets friends, or if the item targets all
            // but the player is not in a battle
            if (targets == ValidTargets.Friends || (targets == ValidTargets.All && !Player.isInBattle()))
            {
                Point point = new Point(itemWindow.getPosition().X + 10 * gameRef.getGraphicsScale(),
                                        itemWindow.getPosition().Y + 10 * gameRef.getGraphicsScale());

                addChildState(new TargetSelectState(gameRef, this, stateManager, Player.getParty()));
            }
            // Item can target both enemies and friends if the player is in a battle and the item
            // targets all 
            else if (targets == ValidTargets.All && Player.isInBattle())
            {
                // Calculate location for the new window
                Point point = new Point(330 * gameRef.getGraphicsScale(), 0);

                // Player is in battle, so the battle state can be accessed
                BattleState battle = Player.getBattle();

                // Create the new state
                TargetSelectState state = new TargetSelectState(gameRef, this, stateManager, battle.getAllCombatants());
            }
            // Item can only target enemies if the player is in battle and the item targets enemies
            else if (targets == ValidTargets.Enemies && Player.isInBattle())
            {
                // Calculate location for the new window
                Point point = new Point(itemWindow.getPosition().X + 10 * gameRef.getGraphicsScale(),
                                        itemWindow.getPosition().Y + 10 * gameRef.getGraphicsScale());

                // Player is in battle, so the battle state can be accessed
                BattleState battle = Player.getBattle();

                // Create the new state
                addChildState(new TargetSelectState(gameRef, this, stateManager, battle.getEnemies()));
            }
            // Item is not usable if it only targets enemies and the player is not in battle
            else
            {
                stateManager.addState(new MessageState(gameRef, this, stateManager, "You can't use that now."));
            }
        }

        public override void passData(GameState sender, object data)
        {
            Battler target = (Battler)data;
            
            CombatResolver.useItemOn(target, toUse, (Player.isInBattle() ? Player.getBattle() : null));
        }

        public void setWindowPosition(Point newPos)
        {
            itemWindow.setPosition(newPos);
        }
    }
}
