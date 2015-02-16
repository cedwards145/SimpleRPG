using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleRPG
{
    public enum Facing { Up, Down, Left, Right };

    public class MapObject : IComparable<MapObject>
    {
        private static int nextID = 1;

        protected int id;
        protected Point location, offset;
        protected Texture2D spritesheet;
        
        // Frame - The current frame of animation the character is on.
        // Also used for offsetting sprite
        protected int frame;

        // The number of update frames needed to advance the sprite's animation frame
        protected int gameFramesPerFrame = 3;
        protected int framesSinceAnimation = 0;

        protected Facing facing;
        protected TileMap containingMap;
        protected bool moving = false;
        protected Passability passability;

        private bool smoothMoving = true;

        public MapObject(Game game, string textureName, int xCoord, int yCoord, Facing reqFacing)
            :this(game, textureName, xCoord, yCoord)
        {
            facing = reqFacing;
        }

        public MapObject(Game game, string textureName, int xCoord, int yCoord)
        {
            id = nextID;
            nextID++;

            location = new Point(xCoord, yCoord);
            offset = new Point();
            spritesheet = game.Content.Load<Texture2D>(@"graphics\" + textureName);
            facing = Facing.Down;
        }

        public MapObject(Game game, string textureName, int xCoord, int yCoord, int tileSize)
        {
            id = nextID;
            nextID++;

            offset = new Point();
            spritesheet = game.Content.Load<Texture2D>(@"graphics\" + textureName);

            // Transform coords from Tiled coords to map coords

            // Subtract half of a sprite's width (1/8th of the whole sheet)
            xCoord += (spritesheet.Width / 8);
            // Divide by tilesize
            xCoord /= tileSize;

            // For ycoord, add half tilesize to account for slightly misplaced entities, then divide
            yCoord -= (tileSize / 2);
            yCoord /= tileSize;

            location = new Point(xCoord, yCoord);
            facing = Facing.Down;
        }

        public void update()
        {
            if (containingMap != null)
            {
                Point moveValue = facingToPoint(facing);

                if (moving)
                {
                    framesSinceAnimation++;
                    if (framesSinceAnimation == gameFramesPerFrame)
                    {
                        framesSinceAnimation = 0;
                        frame++;
                        if (frame == 4)
                        {
                            frame = 0;
                            moving = false;
                            location.X += moveValue.X;
                            location.Y += moveValue.Y;
                        }
                    }
                }

                if (smoothMoving)
                {
                    // Smooth moving
                    int stepsToMove = (gameFramesPerFrame + 1) * 4;
                    int offsetPerFrame = containingMap.getTileSize() / stepsToMove;
                    offset.X = ((frame * (gameFramesPerFrame + 1)) + framesSinceAnimation) * offsetPerFrame * moveValue.X;
                    offset.Y = ((frame * (gameFramesPerFrame + 1)) + framesSinceAnimation) * offsetPerFrame * moveValue.Y;
                }
                else
                {
                    // Rough moving
                    offset.X = (facing == Facing.Down || facing == Facing.Up ? 0
                                                                             : frame * (containingMap.getTileSize() / 4)
                                                                               * moveValue.X);
                    offset.Y = (facing == Facing.Left || facing == Facing.Right ? 0
                                                                                : frame * (containingMap.getTileSize() / 4)
                                                                                  * moveValue.Y);
                }
            }
        }

        public void draw(SpriteBatch spriteBatch, float opacity, Point mapOffset, int scale)
        {
            int frameWidth = spritesheet.Width / 4;
            int frameHeight = spritesheet.Height / 4;

            int tileSize = containingMap.getTileSize();

            Rectangle destination = new Rectangle(location.X * tileSize - ((frameWidth - tileSize) / 2) + offset.X - mapOffset.X,  //  
                                                  location.Y * tileSize - frameHeight + tileSize + offset.Y - mapOffset.Y,         //  
                                                  frameWidth, frameHeight);
            destination.X *= scale;
            destination.Y *= scale;
            destination.Width *= scale;
            destination.Height *= scale;

            Rectangle source = new Rectangle(frame * frameWidth, facingToInt(facing) * frameHeight, frameWidth, frameHeight);

            spriteBatch.Draw(spritesheet, destination, source, Color.White * opacity);
        }

        public void move(Point moveValue)
        {
            if (!moving)
            {
                facing = pointToFacing(moveValue);
                bool destination = containingMap.getPassability(location.X + moveValue.X, location.Y + moveValue.Y);

                moving = destination;
            }
        }

        public static Point facingToPoint(Facing face)
        {
            switch (face)
            {
                case Facing.Up: return new Point(0, -1);
                case Facing.Down: return new Point(0, 1);
                case Facing.Left: return new Point(-1, 0);
                case Facing.Right: return new Point(1, 0);
                default: return new Point(0, 1);
            }
        }

        public static Facing pointToFacing(Point point)
        {
            if (point.Equals(new Point(0, -1)))
                return Facing.Up;
            else if (point.Equals(new Point(0, 1)))
                return Facing.Down;
            else if (point.Equals(new Point(-1, 0)))
                return Facing.Left;
            else if (point.Equals(new Point(1, 0)))
                return Facing.Right;
            else
                return Facing.Down;
        }

        public static int facingToInt(Facing face)
        {
            switch (face)
            {
                case Facing.Up: return 3;
                case Facing.Down: return 0;
                case Facing.Left: return 1;
                case Facing.Right: return 2;
                default: return 0;
            }
        }

        public void setContainingMap(TileMap value)
        {
            containingMap = value;
        }

        public void setPassability(Passability pass)
        {
            passability = pass;
        }

        public void setFacing(Facing face)
        {
            facing = face;
        }

        public void setPosition(int x, int y)
        {
            location = new Point(x, y);
        }

        public int CompareTo(MapObject other)
        {
            if (location.Y == other.location.Y)
            {
                if (location.X == other.location.X)
                    return id - other.id;
                else
                    return location.X - other.location.X;
            }
            else
                return location.Y - other.location.Y;
        }

        public TileMap getContainingMap()
        {
            return containingMap;
        }

        // Gets an object's location on a map, in cells
        public Point getPosition()
        {
            return location;
        }

        public Point getAbsolutePosition()
        {
            int tileSize = containingMap.getTileSize();
            
            return new Point((location.X + offset.X) * tileSize, (location.Y + offset.Y) * tileSize);
        }

        // Gets the object's draw location, ie, it's position in cells multiplied
        // by tilesize, including it's frame offset
        public Point getOffset()
        {
            return offset;
        }

        public int getWidth()
        {
            if (spritesheet != null)
                return spritesheet.Width / 4;
            else
                return 0;
        }

        public int getHeight()
        {
            if (spritesheet != null)
                return spritesheet.Height / 4;
            else
                return 0;
        }

        public Passability getPassability()
        {
            return passability;
        }
    }
}
