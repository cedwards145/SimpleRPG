using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace SimpleRPG.Windows
{
    public class Window : Drawable
    {
        protected int width, height;
        protected Point location;
        protected Texture2D skin;
        protected SpriteFont font;
        protected Game1 gameRef;
        

        public Window(Game1 game, Point reqPosition, int reqWidth, int reqHeight, string windowskin)
        {
            location = reqPosition;
            width = reqWidth;
            height = reqHeight;
            gameRef = game;

            skin = game.Content.Load<Texture2D>(@"graphics\" + windowskin);

            font = game.getFont();
        }

        public override void update()
        {
            base.update();
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            int scale = gameRef.getGraphicsScale();

            int tileSize = skin.Width / 3;
            int vOffset;

            for (int row = 0; row < (height / (tileSize * scale)); row++)
            {
                if (row == 0)
                    vOffset = 0;
                else if (row == (height / (tileSize * scale)) - 1)
                    vOffset = 2;
                else
                    vOffset = 1;

                spriteBatch.Draw(skin, new Rectangle(location.X, location.Y + row * tileSize * scale, tileSize * scale, tileSize * scale),
                                       new Rectangle(0, tileSize * vOffset, tileSize, tileSize), Color.White * opacity);

                // Top, middle and bottom window inner
                for (int x = 1; x < (width - 1) / (tileSize * scale); x++)
                {
                    int currentSliceWidth = tileSize;

                    // If this is the last segment of inner window to be drawn, reduce the width of the slice so that it does not overlap
                    // the slice next to it
                    if (x == (width - 1) / (tileSize * scale) - 1)
                    {
                        // Calculate how much of the width is take up by what's already
                        // been drawn, and the last panel
                        currentSliceWidth = width - (x + 1) * tileSize * scale;

                        // Divide off the scale factor
                        currentSliceWidth /= scale;
                    }

                    Rectangle source = new Rectangle(tileSize, tileSize * vOffset, currentSliceWidth, tileSize);

                    spriteBatch.Draw(skin, new Rectangle(location.X + x * tileSize * scale, location.Y + row * tileSize * scale, currentSliceWidth * scale, tileSize * scale),
                                           source, Color.White * opacity);
                }

                spriteBatch.Draw(skin, new Rectangle(location.X + width - tileSize * scale, location.Y + row * tileSize * scale, tileSize * scale, tileSize * scale),
                                       new Rectangle(tileSize * 2, tileSize * vOffset, tileSize, tileSize), Color.White * opacity);

                //spriteBatch.DrawString(font, "The matter/anti-matter symmetry of the B2 pi pi pi \ndecay mode is 5.4%", new Vector2(location.X + 10, location.Y + 10), Color.MediumAquamarine * opacity);


            }
        }

        protected void drawRepeated()
        {

        }

        public Point getPosition()
        {
            return location;
        }

        public virtual void setPosition(Point newPosition)
        {
            location = newPosition;
        }
        /// <summary>
        /// Aligns the window to the left edge of the screen
        /// </summary>
        public void setToLeft()
        {
            setPosition(new Point(0, location.Y));
        }
        /// <summary>
        /// Aligns the window to the right edge of the screen
        /// </summary>
        public void setToRight()
        {
            setPosition(new Point(gameRef.getWidth() - width, location.Y));
        }
        /// <summary>
        /// Aligns the window to the top edge of the screen
        /// </summary>
        public void setToTop()
        {
            setPosition(new Point(location.X, 0));
        }
        /// <summary>
        /// Aligns the window to the bottom edge of the screen
        /// </summary>
        public void setToBottom()
        {
            setPosition(new Point(location.X, gameRef.getHeight() - height));
        }
        /// <summary>
        /// Center the window on the screen
        /// </summary>
        public void centerWindow()
        {
            setPosition(GraphicsHelper.calculateCenterPositionP(width, height));
        }
        /// <summary>
        /// Center the window horizontally on the screen
        /// </summary>
        public void centerHorizontally()
        {
            setPosition(new Point(GraphicsHelper.calculateCenterPositionP(width, height).X, location.Y));
        }

        public void centerVertically()
        {
            setPosition(new Point(location.X, GraphicsHelper.calculateCenterPositionP(width, height).Y));
        }

        public int getWidth()
        {
            return width;
        }

        public int getHeight()
        {
            return height;
        }

        public Rectangle getSize()
        {
            return new Rectangle(location.X, location.Y, width, height);
        }

        public int left()
        {
            return location.X;
        }
        public int right()
        {
            return location.X + width;
        }
        public int top()
        {
            return location.Y;
        }
        public int bottom()
        {
            return location.Y + height;
        }
    }
}
