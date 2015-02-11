using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SimpleRPG.Windows
{
    public class EquipItemWindow : ListBox
    {
        public EquipItemWindow(Game1 game, Point reqPosition)
            : base(game, reqPosition, 75 * game.getGraphicsScale(), 3, new string[] { "Equip", "Drop", "Cancel" }, "windowskin")
        {
        }

    }
}
