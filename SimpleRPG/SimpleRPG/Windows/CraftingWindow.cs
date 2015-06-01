using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimpleRPG.Items;

namespace SimpleRPG.Windows
{
    public class CraftingWindow : Window
    {
        protected int capacity, filledSoFar = 0;
        protected Item[] ingredients;
        protected bool enabled;
        protected int index = 0;

        public CraftingWindow(Game1 game, Point reqPosition, int reqCapacity, string windowskin)
            : base(game, reqPosition, 
                   40 * game.getGraphicsScale(), 
                   (reqCapacity * 24 + 32) * game.getGraphicsScale(), 
                   windowskin)
        {
            capacity = reqCapacity;
            ingredients = new Item[capacity];
        }

        public override void update()
        {
            base.update();

            if (enabled)
            {
                if (Input.isButtonPressed(ControllerButton.up))
                {
                    index = (int)MathHelper.Clamp(index - 1, 0, capacity - 1);
                }
                else if (Input.isButtonPressed(ControllerButton.down))
                {
                    index = (int)MathHelper.Clamp(index + 1, 0, capacity - 1);
                }
            }
        }

        public override void draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
            int scale = gameRef.getGraphicsScale();

            for (int itemIndex = 0; itemIndex < capacity; itemIndex++)
            {
                if (enabled && (itemIndex == index))
                {
                    GraphicsHelper.fillRectangle(spriteBatch,
                                             new Rectangle(location.X + (10 * scale),
                                                           location.Y + (14 + (24 * itemIndex)) * scale,
                                                           20 * scale, 20 * scale),
                                             ColorScheme.selectedTextColor * opacity);
                }

                GraphicsHelper.drawRectangle(spriteBatch,
                                             new Rectangle(location.X + (10 * scale),
                                                           location.Y + (14 + (24 * itemIndex)) * scale,
                                                           20 * scale, 20 * scale), 
                                             Color.Black * opacity, scale);

                if (ingredients[itemIndex] != null)
                {
                    Texture2D iconSet = ingredients[itemIndex].getIconSet();
                    int iconIndex = ingredients[itemIndex].getIconIndex();

                    int x = iconIndex % (iconSet.Width / 16);
                    int y = iconIndex / (iconSet.Width / 16);
                    Rectangle source = new Rectangle(x * 16, y * 16, 16, 16);

                    spriteBatch.Draw(iconSet, new Rectangle(location.X + (12 * scale),
                                                           location.Y + (16 + (24 * itemIndex)) * scale,
                                                           16 * scale, 16 * scale), 
                                                           source, Color.White * opacity);
                }
            }
        }

        public void setEnabled(bool value)
        {
            enabled = value;
        }

        public bool getEnabled()
        {
            return enabled;
        }

        public Item getItem()
        {
            return ingredients[index];
        }

        public int getIndex()
        {
            return index;
        }

        public string getItemName()
        {
            if (ingredients[index] == null)
                return "item";
            return ingredients[index].getName();
        }

        public void resize(int newCapacity)
        {
            capacity = newCapacity;
            ingredients = new Item[capacity];
            height = (capacity * 24 + 32) * gameRef.getGraphicsScale();
            index = 0;
        }

        public bool isFull()
        {
            return (filledSoFar >= capacity);
        }

        public void addItem(Item toAdd)
        {
            if (!isFull())
            {
                for (int itemIndex = 0; itemIndex < capacity; itemIndex++)
                {
                    if (ingredients[itemIndex] == null)
                    {
                        ingredients[itemIndex] = toAdd;
                        break;
                    }
                }

                filledSoFar++;
            }
        }

        public void removeItem()
        {
            if (ingredients[index] != null)
                filledSoFar--;

            ingredients[index] = null;
        }
    }
}
