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
    public class InventoryState : GameState
    {
        protected ItemContainer playerInventory;
        protected ItemList itemList;
        protected ItemDescriptionWindow descriptionWindow;

        public InventoryState(Game1 game, StateManager manager, GameState parent)
            : base(game, parent, manager)
        {
            playerInventory = Player.getInventory();
            itemList = new ItemList(game, new Point(), 300 * game.getGraphicsScale(), 7, playerInventory);
            itemList.centerWindow();

            descriptionWindow = new ItemDescriptionWindow(game, new Point(itemList.left(), itemList.bottom()), 300 * game.getGraphicsScale(), 32 * game.getGraphicsScale());
        }

        public override void update()
        {
            base.update();
            // Update the items in the listbox
            itemList.setOptions(Player.getInventory().getItemNames());

            itemList.update();

            Item selectedItem = itemList.getSelectedItem();
            string selectedDescription = (selectedItem == null ? "" : selectedItem.getDescription());
            descriptionWindow.setDescription(selectedDescription);
            descriptionWindow.update();

            if (Input.isKeyPressed(Keys.Enter) && selectedItem != null)
            {
                Point windowPos = itemList.getPosition();
                if (selectedItem is UsableItem)
                    addChildState(new UseItemState(gameRef, this, stateManager, new Point(windowPos.X + 10, windowPos.Y + 10), itemList.getSelectedItem()));
                else if (selectedItem is EquippableItem)
                    addChildState(new EquipItemState(gameRef, this, stateManager, new Point(windowPos.X + 10, windowPos.Y + 10), itemList.getSelectedItem()));
            }
        }

        public override void exit()
        {
            base.exit();
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
            itemList.setOpacity(opacity);
            itemList.draw(spriteBatch);

            descriptionWindow.setOpacity(opacity);
            descriptionWindow.draw(spriteBatch);
        }
    }
}
