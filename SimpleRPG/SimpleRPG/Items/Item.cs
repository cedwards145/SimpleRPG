using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimpleRPG.States;

namespace SimpleRPG.Items
{
    public class Item
    {
        /// <summary>
        /// The name of an item
        /// </summary>
        protected string name;

        /// <summary>
        /// A brief description of an item
        /// </summary>
        protected string description;

        /// <summary>
        /// The icon to be displayed with an item
        /// </summary>
        protected Texture2D icon;

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="itemName">The new item's name</param>
        /// <param name="itemDescription">The new item's description</param>
        public Item(string itemName, string itemDescription)
        {
            name = itemName;
            description = itemDescription;
        }

        /// <summary>
        /// Clone constructor
        /// </summary>
        /// <param name="i">The item to duplicate</param>
        public Item(Item i)
        {
            name = i.name;
            description = i.description;
            icon = i.icon;
        }

        /// <summary>
        /// Gets the name of an item
        /// </summary>
        /// <returns>Item's name</returns>
        public string getName()
        {
            return name;
        }

        /// <summary>
        /// Gets the description of an item
        /// </summary>
        /// <returns>Item's description</returns>
        public string getDescription()
        {
            return description;
        }
    }
}
