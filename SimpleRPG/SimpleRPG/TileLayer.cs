using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleRPG
{
    public class TileLayer : DrawableLayer
    {
        private Tile[,] tiles;
        private Texture2D tileset;
        private int width, height, tileSize, tilesPerRow;

        public TileLayer(Game game, string reqName, Texture2D reqTexture, int reqWidth, int reqHeight, int reqTilesPerRow, int reqTileSize)
            : base(reqName)
        {
            tileset = reqTexture;
            tilesPerRow = reqTilesPerRow;
            tileSize = reqTileSize;
            width = reqWidth;
            height = reqHeight;

            tiles = new Tile[width, height];
        }

        public Tile[,] getTiles()
        {
            return tiles;
        }

        public Tile getTile(int x, int y)
        {
            return tiles[x, y];
        }

        public override Passability getPassability(int x, int y)
        {
            return tiles[x, y].getPassability();
        }

        // Draw rectangular subset of tiles on layer with an offset and a scale
        public override void draw(SpriteBatch spriteBatch, Point firstTile, int tilesAcross, int tilesDown, Point offset, int scale)
        {
            Tile currentTile = null;
            int tileX, tileY;
            
            for (int x = firstTile.X; x < width && x < firstTile.X + tilesAcross; x++)
            {
                for (int y = firstTile.Y; y < height && y < firstTile.Y + tilesDown; y++)
                {
                    currentTile = tiles[x, y];
                    tileX = currentTile.getTileID() % tilesPerRow;
                    tileY = currentTile.getTileID() / tilesPerRow;

                    Rectangle destination = new Rectangle(x * tileSize - offset.X, y * tileSize - offset.Y, tileSize, tileSize);
                    destination.X *= scale;
                    destination.Y *= scale;
                    destination.Width *= scale;
                    destination.Height *= scale;

                    Rectangle source = new Rectangle(tileX * tileSize, tileY * tileSize, tileSize, tileSize);

                    spriteBatch.Draw(tileset, destination, source, Color.White * opacity);
                }
            }
        }
    }
}
