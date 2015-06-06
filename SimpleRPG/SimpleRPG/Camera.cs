using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimpleRPG.Tilemap;

namespace SimpleRPG
{
    public class Camera : Drawable
    {
        protected int width, height;
        protected Point position;
        protected TileMap map;
        protected MapObject following;
        protected int scale = 3;

        public Camera(int reqWidth, int reqHeight)
            : base()
        {
            width = reqWidth;
            height = reqHeight;
        }

        public override void update()
        {
            // If following an object, center camera on it
            if (following != null)
                follow();
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            if (map != null)
            {
                int tileSize = map.getTileSize() * scale;

                // Tiles across is the number of tiles to fill the screen
                // Usually width / tilesize, but is (width / tilesize) + 1 if width does not exactly 
                // divide tilesize
                // Then add an extra tile so that when camera is panning, there is no empty space
                int tilesAcross = (width / tileSize) + 2;

                // Same for tiles down
                int tilesDown = (height / tileSize) + 2;

                Point topLeftTile = new Point();
                topLeftTile.X = (int)MathHelper.Clamp(position.X / tileSize, 0, map.getWidth() - tilesAcross);
                topLeftTile.Y = (int)MathHelper.Clamp(position.Y / tileSize, 0, map.getHeight() - tilesDown);

                // Stop the map drawing 
                Point offset = new Point((int)MathHelper.Clamp(position.X, 0, map.getWidth() * tileSize - width),
                                         (int)MathHelper.Clamp(position.Y, 0, map.getHeight() * tileSize - height));

                offset.X /= scale;
                offset.Y /= scale;

                map.setOpacity(opacity);
                map.draw(spriteBatch, topLeftTile, tilesAcross, tilesDown, offset, scale);
            }
        }

        public void setMap(TileMap newMap)
        {
            map = newMap;
        }

        public void setFollowing(MapObject toFollow)
        {
            following = toFollow;
        }

        public void follow()
        {
            // Get object's position
            Point objectCenter = following.getPosition();
            int tileSize = map.getTileSize() * scale;

            // Multiply the object's location by tilesize
            objectCenter.X *= tileSize;
            objectCenter.Y *= tileSize;

            // Add the object's offset to the calculation
            Point spriteOffset = following.getOffset();
            objectCenter.X += (spriteOffset.X * scale);
            objectCenter.Y += (spriteOffset.Y * scale);

            // Shift the location over by half a tile to get the center of the object
            objectCenter.X += (tileSize / 2);
            objectCenter.Y += (tileSize / 2);

            // Adjust camera's position so that the object center is in the center of the screen
            position.X = (objectCenter.X - (width / 2));
            position.Y = (objectCenter.Y - (height / 2));

            // EXPERIMENTAL
            position.X = (int)MathHelper.Clamp(position.X, 0, (map.getWidth() * tileSize * scale) - width);
            position.Y = (int)MathHelper.Clamp(position.Y, 0, (map.getHeight() * tileSize * scale) - height);
        }

        public void move(Point value)
        {
            int speed = 4;

            position.X += (value.X * speed);
            position.Y += (value.Y * speed);
        }

        public void increaseZoom()
        {
            scale++;
        }

        public void decreaseZoom()
        {
            scale = (int)MathHelper.Clamp(scale - 1, 1, scale);
        }

        public void setZoom(int value)
        {
            scale = (int)MathHelper.Clamp(value, 1, 5);
        }

        public Point transformPoint(Point toTransform)
        {
            return new Point(toTransform.X - position.X, toTransform.Y - position.Y);
        }

        public Vector2 transformVector(Vector2 toTransform)
        {
            return new Vector2(toTransform.X - position.X, toTransform.Y - position.Y);
        }
    }
}
