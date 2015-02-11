using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleRPG.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleRPG.States
{
    public class MessageState : GameState
    {
        protected MessageWindow window;

        public MessageState(Game1 game, GameState parent, StateManager manager, string message)
            :base(game, parent, manager)
        {
            window = new MessageWindow(game, message);
        }

        public override void update()
        {
            base.update();
            window.update();

            if (window.isFinished() && !closing)
                exit();
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
            window.setOpacity(opacity);
            window.draw(spriteBatch);
        }
    }
}
