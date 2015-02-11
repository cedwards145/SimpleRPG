using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleRPG.States;

namespace SimpleRPG.Events
{
    public class Event
    {
        protected Boolean finished = false;
        protected Game1 gameRef;
        protected StateManager stateManager;

        public Event(Game1 game, StateManager manager)
        {
            gameRef = game;
            stateManager = manager;
        }

        public virtual void start()
        { }

        public virtual void update()
        { }

        public Boolean isFinished()
        {
            return finished;
        }
    }
}
