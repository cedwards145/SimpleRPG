using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimpleRPG.Items;

namespace SimpleRPG.Windows
{
    public class ItemList : ListBox
    {
        protected ItemContainer itemContainer;

        public ItemList(Game1 game, Point reqPosition, int reqWidth, int optionsInWindow, ItemContainer items)
            :base(game, reqPosition, reqWidth, optionsInWindow, new string[] { }, "windowskin") 
        {
            itemContainer = items;
            string[] itemNames = items.getItemNames();
            setOptions(itemNames);
        }

        public Item getSelectedItem()
        {
            if (options.Count > 0)
                return ItemManager.getItem(options[index]);
            else
                return null;
        }

        protected override void drawOptions(SpriteBatch spriteBatch)
        {
            base.drawOptions(spriteBatch);

            int scale = gameRef.getGraphicsScale();

            int charHeight = (int)font.MeasureString("I").Y;

            for (int optionsIndex = 0; optionsIndex < noOptionsInWindow && optionsIndex < options.Count; optionsIndex++)
            {
                Color color = (optionsIndex + indexOffset == index ? selectedColor : textColor);

                string quantityString = "x" + itemContainer.numberOfItem(options[optionsIndex + indexOffset]);
                float stringWidth = font.MeasureString(quantityString).X;

                spriteBatch.DrawString(font, quantityString,
                                       new Vector2(location.X + width - (10 * scale + stringWidth),
                                                   location.Y + 10 * scale + (charHeight * optionsIndex)),
                                       color * opacity);
            }
        }
    }
}
