using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimpleRPG.Scripts;

namespace SimpleRPG
{
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
        protected string onActionScript;

        // LIGHTING
        protected Texture2D lightTexture;
        protected Color lightColor;
        protected bool emitsLight, lightFlicker;

        #region Constructors

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

        #endregion

        #region Update / Draw

        public virtual void update()
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

        public virtual void draw(SpriteBatch spriteBatch, float opacity, Point mapOffset, int scale)
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

        #endregion

        #region Script Methods

        public void action()
        {
            if (onActionScript != "")
                Script.runScript(onActionScript);
        }

        #endregion

        #region Helper Methods

        public void move(Point moveValue)
        {
            if (!moving)
            {
                facing = pointToFacing(moveValue);
                bool destination = containingMap.getPassability(location.X + moveValue.X, location.Y + moveValue.Y);

                moving = destination;
            }
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

        #endregion

        #region Mutator Methods
        
        public void setOnActionScript(string value)
        {
            onActionScript = value;
        }

        public void face(Point point)
        {
            int xDelta = Math.Abs(location.X - point.X);
            int yDelta = Math.Abs(location.Y - point.Y);

            if (xDelta > yDelta)
                if (location.X - point.X < 0)
                    facing = Facing.Right;
                else
                    facing = Facing.Left;
            else
                if (location.Y - point.Y < 0)
                    facing = Facing.Down;
                else
                    facing = Facing.Up;
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

        public void setSpritesheet(string sheetName)
        {
            spritesheet = Utilities.getGameRef().Content.Load<Texture2D>(@"graphics\" + sheetName);
        }

        #endregion

        #region Accessor Methods

        public TileMap getContainingMap()
        {
            return containingMap;
        }

        public Point getTileFacing()
        {
            Point facingAsPoint = facingToPoint(facing);
            return new Point(location.X + facingAsPoint.X, location.Y + facingAsPoint.Y);
        }

        /// <summary>
        /// Gets the position of a MapObject on its containing Map
        /// </summary>
        /// <returns>Position, in map cells</returns>
        public Point getPosition()
        {
            return location;
        }

        /// <summary>
        /// Gets the position of a MapObject in pixels, including any offsetting done by
        /// animation
        /// </summary>
        /// <returns>Screen position of a MapObject, before any camera transforms are applied</returns>
        public Point getAbsolutePosition()
        {
            int tileSize = 0;
            if (containingMap != null)
                tileSize = containingMap.getTileSize();

            return new Point((int)((location.X + offset.X / 4.0f) * tileSize), (int)((location.Y + offset.Y / 4.0f) * tileSize));
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

        /// <summary>
        /// Gets the dimensions of a MapObject's sprite
        /// </summary>
        /// <returns>Vector containing sprite's width and height</returns>
        public Vector2 getSpriteSize()
        {
            return new Vector2(spritesheet.Width / 4, spritesheet.Height / 4);
        }

        public bool isOnMap(TileMap map)
        {
            return map == containingMap;
        }

        #endregion

        #region Lighting Methods

        public bool givesOffLight()
        {
            return emitsLight;
        }

        public void givesOffLight(string lightName, Color color, bool flicker)
        {
            emitsLight = true;
            lightTexture = Utilities.getGameRef().Content.Load<Texture2D>(@"graphics\" + lightName);
            lightColor = color;
            lightFlicker = flicker;
        }

        public virtual void drawLight(SpriteBatch spriteBatch)
        {
            if (emitsLight)
            {
                Game1 gameRef = Utilities.getGameRef();

                int scale = gameRef.getGraphicsScale();
                int tileSize = containingMap.getTileSize();

                Camera camera = gameRef.getCamera();

                float lightScale = 1.0f;
                if (lightFlicker)
                {
                    lightScale = Utilities.getRandom().Next(90, 100) / 100.0f;
                }

                int lightWidth = (int)(lightTexture.Width * lightScale);
                int lightHeight = (int)(lightTexture.Height * lightScale);

                // Position calculated as: location (in tiles) * tileSize + half of a tile (to center on a tile,
                //                         + any offset from animation, - half the size of the light texture
                Point centerPosition = new Point(location.X * tileSize + (tileSize / 2) + offset.X - (lightWidth / 2),
                                                 location.Y * tileSize + (tileSize / 2) + offset.Y - (lightHeight / 2));

                centerPosition.X *= scale;
                centerPosition.Y *= scale;

                Point transformedPoint = camera.transformPoint(centerPosition);
                spriteBatch.Draw(lightTexture, new Rectangle(transformedPoint.X,
                                                             transformedPoint.Y,
                                                             lightWidth * scale,
                                                             lightHeight * scale), lightColor);
            }
        }

        #endregion
    }
}
