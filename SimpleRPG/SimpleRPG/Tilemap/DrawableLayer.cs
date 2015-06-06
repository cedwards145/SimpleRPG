using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleRPG.Tilemap
{
    public class DrawableLayer
    {
        protected float opacity;
        protected string name;

        // Prevent instantiation by other classes
        protected DrawableLayer()
        { }
        protected DrawableLayer(string reqName)
        {
            name = reqName;
        }

        public virtual void update()
        { }

        public virtual void draw(SpriteBatch spriteBatch, Point firstTile, int tilesAcross, int tilesDown, Point offset)
        {
            draw(spriteBatch, firstTile, tilesAcross, tilesDown, offset, 1);
        }

        public virtual void draw(SpriteBatch spriteBatch, Point firstTile, int tilesAcross, int tilesDown, Point offset, int scale)
        { }

        public void setOpacity(float value)
        {
            opacity = value;
        }

        public void addOpacity(float value)
        {
            opacity = MathHelper.Clamp(opacity + value, 0, 1);
        }

        public float getOpacity()
        {
            return opacity;
        }

        public virtual Passability getPassability(int x, int y)
        {
            return Passability.Ignore;
        }
    }
}
