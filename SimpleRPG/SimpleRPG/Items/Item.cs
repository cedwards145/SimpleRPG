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
        /// The iconset that contains the item's icon
        /// </summary>
        protected Texture2D iconSet;

        /// <summary>
        /// The index of the item's icon in the iconset
        /// </summary>
        protected int iconIndex;

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="itemName">The new item's name</param>
        /// <param name="itemDescription">The new item's description</param>
        public Item(string itemName, string itemDescription, int itemIconIndex)
        {
            name = itemName;
            description = itemDescription;
            iconIndex = itemIconIndex;

            iconSet = Utilities.getGameRef().Content.Load<Texture2D>(@"graphics\icons");
        }

        /// <summary>
        /// Clone constructor
        /// </summary>
        /// <param name="i">The item to duplicate</param>
        public Item(Item i)
        {
            name = i.name;
            description = i.description;
            iconSet = i.iconSet;
            iconIndex = i.iconIndex;
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

        public Texture2D getIconSet()
        {
            return iconSet;
        }

        public int getIconIndex()
        {
            return iconIndex;
        }
    }
}
