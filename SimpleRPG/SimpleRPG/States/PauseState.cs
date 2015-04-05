using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimpleRPG.Windows;

namespace SimpleRPG.States
{
    public class PauseState : GameState
    {
        protected ListBox pauseWindow;

        public PauseState(Game1 game, GameState parent, StateManager manager)
            : base(game, parent, manager)
        {
            pauseWindow = new PauseWindow(game);
            popOnEscape = true;

            outAnimation = WindowAnimationType.None;
        }

        public override void update()
        {
            base.update();
            pauseWindow.update();

            int menuIndex = pauseWindow.getIndex();
            string menuItem = pauseWindow.getSelectedOption();

            if (Input.isButtonPressed(ControllerButton.enter))
            {
                Point menuPosition = pauseWindow.getPosition();
                
                // Items
                if (menuItem == "Items")
                {
                    addChildState(new InventoryState(gameRef, stateManager, this));
                }
                else if (menuItem == "Potions")
                {
                    addChildState(new CraftingState(gameRef, this, stateManager));
                }
                // Resume
                else if (menuItem == "Resume")
                    exit();
                // Exit Game
                else if (menuItem == "Exit")
                    gameRef.Exit();
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
            pauseWindow.setOpacity(opacity);
            pauseWindow.draw(spriteBatch);
        }

    }
}
