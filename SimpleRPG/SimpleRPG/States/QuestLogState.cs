using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleRPG.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleRPG.States
{
    public class QuestLogState : GameState
    {
        protected Window window;

        public QuestLogState(Game1 game, GameState parent, StateManager manager)
            :base(game, parent, manager)
        {
            window = new Window(game, new Point(), 400 * game.getGraphicsScale(), 300 * game.getGraphicsScale(), "windowskin");
            window.centerWindow();
        }

        public override void update()
        {
            base.update();
            window.update();
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
            window.setOpacity(opacity);
            window.draw(spriteBatch);
        }
    }
}
