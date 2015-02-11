using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SimpleRPG.Items
{
    public class ItemManager
    {
        private static Dictionary<string, Item> items = new Dictionary<string,Item>();

        public static void addNewItem(Item item)
        {
            items[item.getName()] = item;
        }

        public static Item getItem(string itemName)
        {
            if (items.ContainsKey(itemName))
                return items[itemName];
            else
                return null;
        }

        public static void initialize()
        {
            addNewItem(new UsableItem("Small Potion", "Restores a bit of HP", ValidTargets.Friends, DamageType.Healing, 10));
            addNewItem(new UsableItem("Medium Potion", "Restores a lot of HP", ValidTargets.Friends, DamageType.Healing, 50));
            addNewItem(new UsableItem("Large Potion", "Fully restores HP", ValidTargets.Friends, DamageType.Healing, 99999));
            addNewItem(new UsableItem("Small Elixir", "Restores a bit of MP"));
            addNewItem(new UsableItem("Medium Elixir", "Restores a lot of MP"));
            addNewItem(new UsableItem("Large Elixir", "Fully restores MP"));
            addNewItem(new EquippableItem("Carved Wood Staff", "A hand-carved mage's weapon"));
            addNewItem(new EquippableItem("Worn Axe", "An old looking two-handed axe"));
            addNewItem(new UsableItem("Throwing Knife", "A small dagger to be thrown", ValidTargets.Enemies, DamageType.Harming, 50));
        }
    }
}
