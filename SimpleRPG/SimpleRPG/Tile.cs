using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRPG
{   
    public class Tile 
    {
        protected Passability passability;
        protected int tileID;

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
    }
}
