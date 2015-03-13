using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using SimpleRPG.Widgets;

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
        protected List<Widget> widgets;
        private List<Widget> widgetsToRemove;

        public GameState(Game1 game, GameState parent, StateManager manager)
            : base()
        {
            gameRef = game;
            stateManager = manager;

            widgets = new List<Widget>();
            widgetsToRemove = new List<Widget>();

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

            widgetsToRemove.Clear();
            // Update all widgets
            foreach (Widget widget in widgets)
            {
                widget.update();
                if (widget.removable())
                    widgetsToRemove.Add(widget);
            }

            // Remove un-needed widgets
            foreach (Widget widget in widgetsToRemove)
                widgets.Remove(widget);

            if (closing && opacity <= 0.0f)
            {
                close();
            }

            if (Input.isButtonPressed(Controller.ControllerButton.back) && popOnEscape)
                exit();
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);

            foreach (Widget widget in widgets)
            {
                widget.setOpacity(opacity);
                widget.draw(spriteBatch);
            }
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
