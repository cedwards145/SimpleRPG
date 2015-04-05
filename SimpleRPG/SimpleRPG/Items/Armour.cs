using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRPG.Items
{
    public class Armour : EquippableItem
    {
        protected int baseDefence;

        public Armour(string armourName, string armourDescription, int defence)
            :base(armourName, armourDescription, 0)
        {
            baseDefence = defence;
        }

        public int getBaseDefence()
        {
            return baseDefence;
        }
    }
}
