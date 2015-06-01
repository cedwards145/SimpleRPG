using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SimpleRPG.Items;
using SimpleRPG.Crafting;

namespace SimpleRPG.Windows
{
    public class CraftingItemList : ItemList
    {
        public CraftingItemList(Game1 game, Point reqPosition, int reqWidth, int optionsInWindow)
            : base(game, reqPosition, reqWidth, optionsInWindow, Player.getInventory())
        {
            
        }

        public void setFilter(CraftAction action)
        {

        }
    }
}
