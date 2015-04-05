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
    public class TitleState : GameState
    {
        private ListBox listbox;

        string actionSelected = "";

        public TitleState(Game1 game, StateManager manager)
            : base(game, null, manager)
        {
            inAnimation = WindowAnimationType.Fade;
            outAnimation = WindowAnimationType.FadeSlow;

            popOnEscape = false;

            listbox = new ListBox(game, new Point(), 100 * game.getGraphicsScale(), 4, new string[] {"New Game", "Load Game", "Options", "Exit" }, "windowskin");
            listbox.setPosition(GraphicsHelper.calculateCenterPositionP(listbox.getWidth(), listbox.getHeight()));
            listbox.setTextAlign(TextAlign.Center);
        }

        public override void update()
        {
            base.update();
            listbox.update();

            if (Input.isButtonPressed(ControllerButton.enter))
            {
                int index = listbox.getIndex();

                // New Game
                if (index == 0)
                {
                    actionSelected = "new";
                    exit();
                }
                // Load Game
                else if (index == 1)
                    ;
                // Options
                else if (index == 2)
                    ;
                // Exit
                else if (index == 3)
                    gameRef.Exit();
            }
        }

        public override void close()
        {
            base.close();
            if (actionSelected == "new")
                stateManager.addState(gameRef.getFirstGameState());
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);

            listbox.setOpacity(opacity);
            listbox.draw(spriteBatch);
        }
    }
}
