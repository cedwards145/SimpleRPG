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
            popOnEscape = false;

            outAnimation = AnimationType.None;
        }

        public override void update()
        {
            base.update();
            pauseWindow.update();

            int menuIndex = pauseWindow.getIndex();
            if (Input.isKeyPressed(Keys.Enter))
            {
                Point menuPosition = pauseWindow.getPosition();


                // Items
                if (menuIndex == 2)
                {
                    addChildState(new InventoryState(gameRef, stateManager, this));
                }
                // Quests
                else if (menuIndex == 3)
                {
                    addChildState(new QuestLogState(gameRef, this, stateManager));
                }
                // Resume
                else if (menuIndex == 4)
                    exit();
                // Exit Game
                else if (menuIndex == 3)
                    gameRef.Exit();
            }
            else if (Input.isKeyPressed(Keys.Escape))
                exit();
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
            pauseWindow.setOpacity(opacity);
            pauseWindow.draw(spriteBatch);
        }

    }
}
