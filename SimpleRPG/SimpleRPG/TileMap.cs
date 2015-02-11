using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace SimpleRPG
{
    public class TileMap : Drawable
    {
        private int width, height, tileSize, tilesPerRow;

        private List<DrawableLayer> layerList;
        private Texture2D tileset;
        private ObjectLayer objectLayer;

        // Read a map from a .tmx XML file
        /*
        public TileMap(Game game, string filename)
        {
            // Set up lists
            layerList = new List<DrawableLayer>();

            // Create XML Reader and read to the map element
            XmlReader reader = XmlReader.Create(@"content\" + filename + ".tmx");

            reader.ReadToFollowing("map");

            // Parse the width, height and tilesize from <map>'s attributes
            width = int.Parse(reader.GetAttribute("width"));
            height = int.Parse(reader.GetAttribute("height"));
            tileSize = int.Parse(reader.GetAttribute("tilewidth"));

            Passability[] tilePass = new Passability[1];

            bool objectLayerSeen = false;

            int tileOffset = 0;

            // Parse the remaining elements
            // WARNING! Only supports 1 tileset per map!
            string elementName = "";
            while (reader.Read())
            {
                // Only check for start elements
                if (reader.IsStartElement())
                {
                    // Get element's name
                    elementName = reader.Name;

                    // ==== CASES ====================
                    // Parse a tileset
                    if (elementName.Equals("tileset"))
                    {
                        // WARNING! Uses the tileset name as the name of the asset to load.
                        // Works provided tileset name always set to tileset asset name.
                        tileset = game.Content.Load<Texture2D>(@"graphics\" + reader.GetAttribute("name"));

                        // Initialize tile array
                        int tileCount = (tileset.Width / tileSize) * (tileset.Height / tileSize);
                        tilePass = new Passability[tileCount];

                        for (int index = 0; index < tileCount; index++)
                            tilePass[index] = Passability.Ignore;

                        // Read tileOffset
                        tileOffset = int.Parse(reader.GetAttribute("firstgid"));

                        // Calculate tilesPerRow
                        tilesPerRow = tileset.Width / tileSize;

                        // Read tile properties
                        while (reader.ReadToDescendant("tile") || reader.ReadToNextSibling("tile"))
                        {
                            int id = int.Parse(reader.GetAttribute("id"));

                            // Get property inside properties
                            bool properties = reader.ReadToFollowing("properties");
                            bool property = reader.ReadToFollowing("property");

                            // Set passability
                            tilePass[id] = Parser.parsePassability(reader.GetAttribute("value"));

                            reader.MoveToElement();
                            reader.Skip();
                            reader.MoveToElement();
                            reader.Skip();
                            reader.MoveToElement();
                            reader.Skip();
                            reader.MoveToElement();
                            reader.Skip();
                            reader.MoveToElement();
                            reader.Skip();
                            reader.MoveToElement();
                            //reader.Skip();
                        }
                    }
                    // Parse a list of tiles from a tilelayer
                    else if (elementName.Equals("layer"))
                    {
                        // Get layer name
                        string name = reader.GetAttribute("name");

                        // Set up the new layer
                        TileLayer layer = new TileLayer(game, name, tileset, width, height, tileset.Width / tileSize, tileSize);

                        // Add the new tilelayer to the layerlist
                        layerList.Add(layer);

                        // Get the array of tiles from the layer
                        Tile[,] tiles = layer.getTiles();

                        // Read each tile from the layer
                        for (int count = 0; count < width * height; count++)
                        {
                            reader.ReadToFollowing("tile");
                            int id = int.Parse(reader.GetAttribute("gid")) - tileOffset;
                            //bool pass = id < 0 || tilePass[id];

                            Passability pass = Passability.False;
                            if (id < 0)
                                pass = Passability.Ignore;
                            else
                                pass = tilePass[id];

                            tiles[count % width, count / width] = new Tile(pass, id);
                        }
                    }
                    else if (elementName.Equals("objectgroup"))
                    {
                        objectLayerSeen = true;
                        objectLayer = new ObjectLayer();
                        layerList.Add(objectLayer);
                    }
                }
            }

            if (!objectLayerSeen)
            {
                objectLayer = new ObjectLayer();
                layerList.Add(objectLayer);
            }

            reader.Close();
        }
        */

        // Read a map PROPERLY from a .tmx XML file
        public TileMap(Game game, string filename)
        {
            // List to hold drawable layers
            layerList = new List<DrawableLayer>();

            // Array to hold the passability of each tile
            Passability[] tilePass = new Passability[1];

            // The XML file to read the map from
            XmlTextReader reader = new XmlTextReader(@"content\" + filename + ".tmx");

            // Determines whether the XMLReader is inside of a tileset element or not
            bool insideTileset = false;

            // Used by tiled to offset Tile IDs
            int tileOffset = 0;

            // Used to keep track which element type the reader is inside of
            string lastMajorNode = "";

            // Track the id of the last tile node read
            int lastTileID = 0;

            // The last layer created, used to place new tiles and objects 
            DrawableLayer lastLayer = null;

            // Count how many tiles have been seen. Used to determine the coordinates to place
            // newly read tiles at
            int count = 0;

            // The latest tileset name read. Used to decide which tileset is the main set
            // for the map, indicated by a property 
            string tilesetName = "";
            int lastTileOffset = 0;

            // Used to construct a MapObject in stages, as more information is read from various property elements
            string objectTextureName = "";
            Passability objectPassability = Passability.Ignore;
            int objectX = 0, objectY = 0;
            Facing objectFacing = Facing.Down;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    // Individually deal with each type of element
                    case XmlNodeType.Element:

                        // Map element
                        if (reader.Name == "map")
                        {
                            // Read width, height and size of tile attributes from map
                            width = int.Parse(reader.GetAttribute("width"));
                            height = int.Parse(reader.GetAttribute("height"));
                            tileSize = int.Parse(reader.GetAttribute("tilewidth"));
                        }

                        // Tileset element
                        else if (reader.Name == "tileset")
                        {
                            lastMajorNode = "tileset";
                            insideTileset = true;

                            tilesetName = reader.GetAttribute("name");

                            // Read tileOffset
                            lastTileOffset = int.Parse(reader.GetAttribute("firstgid"));

                        }
                        // Parse a tile element, both in the tileset entry and the layers
                        else if (reader.Name == "tile")
                        {
                            lastMajorNode = "tile";

                            // If tile node appears inside tileset node, has value with name="id"
                            if (insideTileset)
                                lastTileID = int.Parse(reader.GetAttribute("id"));
                            else
                            {
                                // Load a tile and add it in the right place on the current layer

                                // Read the tile's ID and subtract the offset (transform tiled IDs to Game IDs)
                                int id = int.Parse(reader.GetAttribute("gid")) - tileOffset;

                                // Assume tile is impassable
                                Passability pass = Passability.False;

                                // If the tile is before the start of the tileset, ignore it's passability?
                                if (id < 0)
                                    pass = Passability.Ignore;
                                // Otherwise, assign passability based of passabilities read from tile properties
                                else
                                    pass = tilePass[id];

                                // If a tile element has been found, but the reader is not inside of a tileset element,
                                // the last layer must be a tile layer
                                TileLayer tl = (TileLayer)lastLayer;

                                // Get the array of tiles from the layer
                                Tile[,] tiles = tl.getTiles();

                                // Create the new tile
                                tiles[count % width, count / width] = new Tile(pass, id);

                                // Increment the count
                                count++;
                            }
                        }

                        // Read a new object
                        else if (reader.Name == "object")
                        {
                            lastMajorNode = "object";

                            // Clear any information from old objects
                            objectTextureName = "";

                            // Record new position
                            objectX = (int)float.Parse(reader.GetAttribute("x"));
                            objectY = (int)float.Parse(reader.GetAttribute("y"));

                        }

                        // Properties elements. Includes tilesets, tiles, objects etc.
                        else if (reader.Name == "property")
                        {
                            // Read tile properties, usually from within a tileset element
                            if (lastMajorNode == "tile")
                            {
                                // Read attribute with name="pass"
                                // Only one attribute currently, so value can be read directly
                                tilePass[lastTileID] = Parser.parsePassability(reader.GetAttribute("value"));
                            }
                            // Read a tileset's properties
                            else if (lastMajorNode == "tileset")
                            {
                                bool isMain = bool.Parse(reader.GetAttribute("value"));
                                if (isMain)
                                {
                                    tileset = game.Content.Load<Texture2D>(@"graphics\" + tilesetName);

                                    // Set the passabilities for each tile
                                    // Initialize tile array
                                    int tileCount = (tileset.Width / tileSize) * (tileset.Height / tileSize);
                                    tilePass = new Passability[tileCount];

                                    // Set each initial passability to ignore
                                    for (int index = 0; index < tileCount; index++)
                                        tilePass[index] = Passability.Ignore;

                                    // Calculate tilesPerRow
                                    tilesPerRow = tileset.Width / tileSize;

                                    tileOffset = lastTileOffset;
                                }

                            }
                            else if (lastMajorNode == "object")
                            {
                                string attr = reader.GetAttribute("name");
                                if (attr == "img")
                                    objectTextureName = reader.GetAttribute("value");
                                else if (attr == "pass")
                                    objectPassability = Parser.parsePassability(reader.GetAttribute("value"));
                                else if (attr == "face")
                                    objectFacing = Parser.parseFacing(reader.GetAttribute("value"));
                            }
                        }

                        // Parse layers
                        else if (reader.Name == "layer")
                        {
                            // Create a new tile layer
                            TileLayer newLayer = new TileLayer(game, reader.GetAttribute("name"), tileset, width, height, tilesPerRow, tileSize);
                            lastLayer = newLayer;
                            layerList.Add(newLayer);

                            // On seeing a new layer, tile count must be reset
                            count = 0;
                        }

                        // Object layer
                        else if (reader.Name == "objectgroup")
                        {
                            // Create a new object layer
                            ObjectLayer newLayer = new ObjectLayer(reader.GetAttribute("name"));
                            lastLayer = newLayer;
                            layerList.Add(newLayer);

                            objectLayer = newLayer;
                        }
                        break;
                    case XmlNodeType.Attribute:

                        break;

                    case XmlNodeType.EndElement:
                        if (reader.Name == "tileset")
                            insideTileset = false;
                        else if (reader.Name == "object")
                        {
                            // Construct and add the object

                            // If an object is encountered, the last layer must be an object layer
                            ObjectLayer ol = (ObjectLayer)lastLayer;

                            MapObject o = new MapObject(game, objectTextureName, objectX, objectY, tileSize);
                            o.setPassability(objectPassability);
                            o.setContainingMap(this);
                            o.setFacing(objectFacing);
                            ol.addObject(o);
                        }
                        break;
                    default:

                        break;
                }


            }
        }

        public override void update()
        {
            foreach (DrawableLayer layer in layerList)
            {
                layer.update();
            }
        }

        public void draw(SpriteBatch spriteBatch, Point firstTile, int tilesAcross, int tilesDown, Point offset)
        {
            draw(spriteBatch, firstTile, tilesAcross, tilesDown, offset, 1);
        }

        public void draw(SpriteBatch spriteBatch, Point firstTile, int tilesAcross, int tilesDown, Point offset, int scale)
        {
            foreach (DrawableLayer layer in layerList)
            {
                layer.setOpacity(opacity);
                layer.draw(spriteBatch, firstTile, tilesAcross, tilesDown, offset, scale);
            }
        }

        public void addObject(MapObject toAdd)
        {
            toAdd.setContainingMap(this);
            objectLayer.addObject(toAdd);
        }

        // ACCESSOR METHODS
        public int getWidth()
        {
            return width;
        }

        public int getHeight()
        {
            return height;
        }

        public int getTileSize()
        {
            return tileSize;
        }

        public bool getPassability(int x, int y)
        {
            if (x >= width || x < 0 || y >= height || y < 0)
                return false;

            bool value = false;
            foreach (DrawableLayer layer in layerList)
            {
                Passability pass = layer.getPassability(x, y);

                // NECESSARY! Set value to passability UNLESS Passability = ignore, where value
                // is unchanged.
                if (pass == Passability.True)
                    value = true;
                else if (pass == Passability.False)
                    value = false;
            }
            return value;
        }

    }
}
