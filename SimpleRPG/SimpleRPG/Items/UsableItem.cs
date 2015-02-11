using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleRPG.States;

namespace SimpleRPG.Items
{
    public class UsableItem : Item
    {
        /// <summary>
        /// The Battlers this Item can target, defaults to All
        /// </summary>
        protected ValidTargets targets;

        /// <summary>
        /// If set to true, an item of this type is removed from the player's inventory
        /// when it is used
        /// </summary>
        protected bool consumeOnUse = true;

        /// <summary>
        /// Determines whether an item inflicts or heals when used on a target
        /// </summary>
        protected DamageType damageType = DamageType.Healing;

        /// <summary>
        /// Determines how much an item heals / hurts a target
        /// </summary>
        protected int potency = 0;

        /// <summary>
        /// Create an item with default parameters
        ///  - Targets: All
        ///  - Damage Type: Healing
        ///  - Potency: 0
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="itemDescription"></param>
        public UsableItem(string itemName, string itemDescription)
            : this(itemName, itemDescription, ValidTargets.All, DamageType.Healing, 0)
        { }

        public UsableItem(string itemName, string itemDescription, ValidTargets validTargets, DamageType damage, int reqPotency)
            : base(itemName, itemDescription)
        {
            targets = validTargets;
            targets = validTargets;
            damageType = damage;

            potency = reqPotency;
        }

        public virtual int use(Battler target)
        {
            int damage = (damageType == DamageType.Healing ? 1 : -1) * potency;

            target.addHP(damage);

            consume();

            return Math.Abs(damage);
        }

        protected virtual void consume()
        {
            if (consumeOnUse)
            {
                ItemContainer inventory = Player.getInventory();
                inventory.removeItem(this);
            }
        }

        public ValidTargets getValidTargets()
        {
            return targets;
        }

        public DamageType getDamageType()
        {
            return damageType;
        }
    }
}
