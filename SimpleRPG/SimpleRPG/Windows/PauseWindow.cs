using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SimpleRPG.Windows
{
    public class PauseWindow : ListBox
    {
        public PauseWindow(Game1 game)
            : base(game, default(Point), 100 * game.getGraphicsScale(), 5, new string[] { "Equipment", "Skills", "Items", "Resume", "Exit" }, "windowskin")
        {
            location = new Point((game.getWidth() - width) / 2, (game.getHeight() - height) / 2);
        }

    }
}
