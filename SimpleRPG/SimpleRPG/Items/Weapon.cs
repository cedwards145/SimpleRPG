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
            :base(weaponName, weaponDescription)
        {
            baseDamage = damage;
        }

        public int getBaseDamage()
        {
            return baseDamage;
        }
    }
}
