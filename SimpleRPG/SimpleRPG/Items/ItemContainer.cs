using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRPG.Items
{
    public class ItemContainer
    {
        protected Dictionary<string, int> itemQuantities;

        public ItemContainer()
        {
            itemQuantities = new Dictionary<string, int>();
        }

        public bool contains(Item item)
        {
            return contains(item.getName());
        }
        public bool contains(string itemName)
        {
            return itemQuantities.ContainsKey(itemName);
        }

        public int numberOfItem(Item item)
        {
            return numberOfItem(item.getName());
        }
        public int numberOfItem(string itemName)
        {
            if (contains(itemName))
                return itemQuantities[itemName];
            else
                return 0;
        }

        public void addItem(Item item)
        {
            addItem(item.getName(), 1);
        }
        public void addItem(string itemName)
        {
            addItem(itemName, 1);
        }
        public void addItem(Item item, int quantity)
        {
            addItem(item.getName(), 1);
        }
        public void addItem(string itemName, int quantity)
        {
            if (contains(itemName))
                itemQuantities[itemName]+= quantity;
            else
                itemQuantities[itemName] = quantity;
        }

        public void removeItem(Item item)
        {
            removeItem(item.getName());
        }
        public void removeItem(string itemName)
        {
            if (contains(itemName))
            {
                itemQuantities[itemName]--;
                if (itemQuantities[itemName] == 0)
                    itemQuantities.Remove(itemName);
            }
        }

        public int count()
        {
            return itemQuantities.Keys.Count;
        }

        public string[] getItemNames()
        {
            string[] itemNames = new string[count()];

            int index = 0;
            foreach (string key in itemQuantities.Keys)
            {
                itemNames[index] = key;
                index++;
            }

            return itemNames;
        }
    }
}
