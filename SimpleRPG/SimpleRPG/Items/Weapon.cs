using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRPG.Items
{
    public class Weapon : EquippableItem
    {
        protected int baseDamage;

        public Weapon(string weaponName, string weaponDescription, int damage)
            :base(weaponName, weaponDescription, 0)
        {
            baseDamage = damage;
        }

        public int getBaseDamage()
        {
            return baseDamage;
        }
    }
}
