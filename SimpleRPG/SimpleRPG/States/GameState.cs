using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleRPG.States
{
    public class GameState : Drawable
    {
        protected Game1 gameRef;
        protected StateManager stateManager;
        protected bool popOnEscape = true;
        protected AnimationType inAnimation = AnimationType.Fade, outAnimation = AnimationType.Fade;
        protected bool closing = false, open = false;
        protected GameState parentState;

        public GameState(Game1 game, GameState parent, StateManager manager)
            : base()
        {
            gameRef = game;
            stateManager = manager;
            setOpacity(0);

            parentState = parent;
        }

        public override void update()
        {
            base.update();

            if (!open)
            {
                Animation.animateIn(this, inAnimation);
                open = true;
            }

            if (closing && opacity <= 0.0f)
            {
                close();
            }

            if (Input.isButtonPressed(Controller.ControllerButton.back) && popOnEscape)
                exit();
        }

        // Called as a state begins to exit
        public virtual void exit()
        {
            if (parentState != null)
                parentState.setOpacity(1);
            Animation.animateOut(this, outAnimation);
            closing = true;
        }
        
        // Called when a state actually removes itself
        public virtual void close()
        {
            stateManager.popState();
        }

        public virtual void passData(GameState sender, object data)
        { }

        public void addChildState(GameState state)
        {
            setOpacity(0.5f);
            stateManager.addState(state);
        }
    }
}
