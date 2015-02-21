using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SimpleRPG
{   
    public class Tile 
    {
        protected Passability passability;
        protected int tileID;
        protected Color tintColor = new Color(0, 0, 0, 0);

        public Tile(Passability reqPassability, int reqTileID)
        {
            passability = reqPassability;
            tileID = reqTileID;
        }

        public Tile()
            : this(Passability.True, 0)
        {   }

        public static int coodToTile(int x, int y, int tilesPerRow)
        {
            return y * tilesPerRow + x;
        }

        // ACCESSOR METHODS
        public Passability getPassability()
        {
            return passability;
        }

        public int getTileID()
        {
            return tileID;
        }

        public Color getTint()
        {
            if (Debug.tintTiles())
                return tintColor;
            else
                return new Color(0, 0, 0, 0);
        }

        // MUTATOR METHODS
        public void tint(Color newTint)
        {
            tintColor = newTint;
        }
    }
}
