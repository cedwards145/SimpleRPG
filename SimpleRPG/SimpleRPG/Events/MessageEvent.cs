using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleRPG.States;

namespace SimpleRPG.Events
{
    public class MessageEvent : Event
    {
        protected string message;

        public MessageEvent(Game1 game, StateManager manager, string reqMessage)
            :base(game, manager)
        {
            message = reqMessage;
        }

        public override void start()
        {
            base.start();

        }
    }
}
