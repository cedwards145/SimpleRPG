using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimpleRPG.Windows;
using SimpleRPG.Items;

namespace SimpleRPG.States
{
    public class CraftingState : GameState
    {
        protected CraftingItemList inventory;
        protected LabelWindow helpWindow;
        protected ClickableLabel currentActionWindow;
        protected CraftingWindow craftingWindow;

        protected Texture2D leftArrow, rightArrow;
        protected Point relativeTopLeft;
  

        public CraftingState(Game1 game, GameState parent, StateManager manager)
            : base(game, parent, manager)
        {
            int scale = game.getGraphicsScale();

            leftArrow = game.Content.Load<Texture2D>(@"graphics\left arrow");
            rightArrow = game.Content.Load<Texture2D>(@"graphics\right arrow");

            int screenWidth = game.getWidth();
            int screenHeight = game.getHeight();

            relativeTopLeft = new Point((screenWidth - (416 * scale)) / 2,
                                              (screenHeight - (192 * scale)) / 2);
            

            inventory = new CraftingItemList(game, new Point(relativeTopLeft.X + (208 * scale),
                                                             relativeTopLeft.Y + (32 * scale)), 208 * scale, 10);

            helpWindow = new LabelWindow(game, relativeTopLeft, 416, 32, "Enter: Add item to recipe", "windowskin");
            currentActionWindow = new ClickableLabel(game, 
                                                     new Point(relativeTopLeft.X + (48 * scale),
                                                               relativeTopLeft.Y + (32 * scale)),
                                                     80, 32, "Cook", "windowskin");
            currentActionWindow.setTextAlign(TextAlign.Center);

            craftingWindow = new CraftingWindow(game, new Point(relativeTopLeft.X + (68 * scale),
                                                                relativeTopLeft.Y + (64 * scale)), 3, "windowskin");
        }

        public override void update()
        {
            int oldCraftWindowIndex = craftingWindow.getIndex();

            base.update();
            inventory.update();
            helpWindow.update();
            currentActionWindow.update();
            craftingWindow.update();

            if (Input.isButtonPressed(ControllerButton.left) && inventory.getEnabled())
            {
                inventory.setEnabled(false);
                craftingWindow.setEnabled(true);
            }
            else if (Input.isButtonPressed(ControllerButton.right))
            {
                inventory.setEnabled(true);
                currentActionWindow.setEnabled(false);
                craftingWindow.setEnabled(false);
            }
            else if (Input.isButtonPressed(ControllerButton.up) && craftingWindow.getEnabled() 
                     && oldCraftWindowIndex == 0)
            {
                craftingWindow.setEnabled(false);
                currentActionWindow.setEnabled(true);
            }
            else if (Input.isButtonPressed(ControllerButton.down) && currentActionWindow.getEnabled())
            {
                currentActionWindow.setEnabled(false);
                craftingWindow.setEnabled(true);
            }
            else if (Input.isButtonPressed(ControllerButton.enter))
            {
                // Handle enter on inventory window
                if (inventory.getEnabled() && !craftingWindow.isFull())
                {
                    Item selectedItem = inventory.getSelectedItem();
                    Player.takeItem(selectedItem);
                    craftingWindow.addItem(selectedItem);

                    // If crafting window is now full, enable the craft button
                    if (craftingWindow.isFull())
                    {
                        inventory.setEnabled(false);
                        currentActionWindow.setEnabled(true);
                    }
                }
                // Handle enter on crafting window
                else if (craftingWindow.getEnabled())
                {
                    Item selectedItem = craftingWindow.getItem();
                    craftingWindow.removeItem();
                    if (selectedItem != null)
                        Player.giveItem(selectedItem);
                }
            }

            // Update help window text
            if (inventory.getEnabled())
                helpWindow.setText("Enter: add " + inventory.getSelectedOption() + " to recipe");
            else if (craftingWindow.getEnabled())
                helpWindow.setText("Enter: remove " + craftingWindow.getItemName() + " from recipe");

        }

        public override void draw(SpriteBatch spriteBatch)
        {
            GraphicsHelper.fillRectangle(spriteBatch,
                                         new Rectangle(0, 0, gameRef.getWidth(), gameRef.getHeight()),
                                         Color.Black * 0.4f);
            base.draw(spriteBatch);

            int scale = gameRef.getGraphicsScale();

            inventory.setOpacity(opacity);
            inventory.draw(spriteBatch);

            // Draw windows
            helpWindow.setOpacity(opacity);
            helpWindow.draw(spriteBatch);

            currentActionWindow.setOpacity(opacity); 
            currentActionWindow.draw(spriteBatch);

            craftingWindow.setOpacity(opacity);
            craftingWindow.draw(spriteBatch);

            // Draw arrows
            spriteBatch.Draw(leftArrow, new Rectangle(relativeTopLeft.X + 32 * scale,
                                                      relativeTopLeft.Y + 32 * scale,
                                                      leftArrow.Width * scale, leftArrow.Height * scale), Color.White * opacity);
            spriteBatch.Draw(rightArrow, new Rectangle(relativeTopLeft.X + 128 * scale,
                                                       relativeTopLeft.Y + 32 * scale,
                                                       rightArrow.Width * scale, rightArrow.Height * scale), Color.White * opacity);
        }
    }
}
