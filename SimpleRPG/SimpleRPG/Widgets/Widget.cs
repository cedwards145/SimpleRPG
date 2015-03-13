using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SimpleRPG.Widgets
{
    public class Widget : Drawable
    {
        protected bool removeMe = false;
        protected Vector2 position;

        public Widget()
        { }

        public void remove()
        {
            removeMe = true;
        }

        public bool removable()
        {
            return removeMe;
        }

        public void setPosition(Vector2 newPosition)
        {
            position = newPosition;
        }
    }
}
