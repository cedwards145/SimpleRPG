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
        protected bool finished = false;

        public MessageState(Game1 game, GameState parent, StateManager manager, string message)
            :base(game, parent, manager)
        {
            window = new MessageWindow(game, message);
            inAnimation = WindowAnimationType.Fade;
            outAnimation = WindowAnimationType.None;
        }

        public override void update()
        {
            base.update();
            window.update();

            if (window.isFinished() && !closing)
                exit();
        }

        public override void exit()
        {
            finished = true;
            base.exit();
        }

        public bool isFinished()
        {
            return finished;
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
            window.setOpacity(opacity);
            window.draw(spriteBatch);
        }
    }
}
