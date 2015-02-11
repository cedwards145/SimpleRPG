using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SimpleRPG.Windows
{
    public class UseItemWindow : ListBox
    {
        public UseItemWindow(Game1 game, Point reqPosition)
            : base(game, reqPosition, 75 * game.getGraphicsScale(), 3, new string[] { "Use", "Drop", "Cancel" }, "windowskin")
        {
        }

    }
}
