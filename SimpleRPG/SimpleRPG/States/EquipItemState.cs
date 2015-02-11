using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SimpleRPG.Items;
using SimpleRPG.Windows;

namespace SimpleRPG.States
{
    public class EquipItemState : GameState
    {
        protected EquipItemWindow itemWindow;
        protected EquippableItem toUse;

        public EquipItemState(Game1 game, GameState parent, StateManager manager, Point windowPosition, Item item)
            : base(game, parent, manager)
        {
            toUse = (EquippableItem)item;
            itemWindow = new EquipItemWindow(game, windowPosition);
        }

        public override void update()
        {
            base.update();
            itemWindow.update();

            if (Input.isKeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                int selectedIndex = itemWindow.getIndex();

                // Equip
                if (selectedIndex == 0)
                {
                    if (Player.isInBattle())
                        addChildState(new MessageState(gameRef, this, stateManager, "You can't do that in combat!"));
                }
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

        public override void draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
            itemWindow.setOpacity(opacity);
            itemWindow.draw(spriteBatch);
        }

        public void setWindowPosition(Point newPos)
        {
            itemWindow.setPosition(newPos);
        }
    }
}
