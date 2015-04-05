using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRPG.Items
{
    public class EquippableItem : Item
    {
        public EquippableItem(string itemName, string itemDescription, int itemIconIndex)
            : base(itemName, itemDescription, itemIconIndex)
        { }
        public virtual void equip(Battler target)
        { }
    }
}
