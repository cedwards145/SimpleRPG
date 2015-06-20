using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework.Input;

namespace SimpleRPG.Tilemap
{
    public class TileMap : Drawable
    {
        private int width, height, tileSize, tilesPerRow;

        private List<DrawableLayer> layerList;
        private Texture2D tileset, light;
        private ObjectLayer objectLayer;
        protected Game1 gameRef;
        protected Color ambientLight;

        private RenderTarget2D lightRenderTarget;

        #region Constructors / Destructors

        // Read a map PROPERLY from a .tmx XML file
        public TileMap(Game1 game, string filename)
        {
            gameRef = game;
            light = game.Content.Load<Texture2D>(@"graphics\light");

            ambientLight = new Color(255, 255, 255);

            // Set up lighting render target
            lightRenderTarget = new RenderTarget2D(game.GraphicsDevice,
                                                   game.getWidth(),
                                                   game.getHeight());

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
            bool objectEmitsLight = false;
            bool objectLightFlickers = false;
            Color objectLightColor = new Color(0, 0, 0, 0);
            string objectLightTexture = "light";
            string onActionScript = "";
            string onLoadScript = "";
            string objectName = "";

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    // Individually deal with each type of element
                    case XmlNodeType.Element:

                        // Map element
                        if (reader.Name == "map")
                        {
                            lastMajorNode = "map";

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
                            onActionScript = "";
                            onLoadScript = "";

                            // Record new position
                            objectX = (int)float.Parse(reader.GetAttribute("x"));
                            objectY = (int)float.Parse(reader.GetAttribute("y"));
                            objectName = reader.GetAttribute("name");

                        }

                        // Properties elements. Includes tilesets, tiles, objects etc.
                        else if (reader.Name == "property")
                        {
                            if (lastMajorNode == "map")
                            {
                                string propertyName = reader.GetAttribute("name");
                                if (propertyName == "ambientLight")
                                {
                                    ambientLight = Parser.parseColor(reader.GetAttribute("value"));
                                }
                            }
                            // Read tile properties, usually from within a tileset element
                            else if (lastMajorNode == "tile")
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
                                else if (attr == "emitsLight")
                                {
                                    objectEmitsLight = true;
                                    objectLightColor = Parser.parseColor(reader.GetAttribute("value"));
                                }
                                else if (attr == "lightTexture")
                                    objectLightTexture = reader.GetAttribute("value");
                                else if (attr == "lightFlickers")
                                    objectLightFlickers = bool.Parse(reader.GetAttribute("value"));
                                else if (attr == "onAction")
                                    onActionScript = reader.GetAttribute("value");
                                else if (attr == "onLoad")
                                    onLoadScript = reader.GetAttribute("value");
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
                            o.setOnActionScript(onActionScript);
                            o.setOnLoadScript(onLoadScript);
                            o.setName(objectName);
                            ol.addObject(o);

                            if (objectEmitsLight)
                            {
                                o.givesOffLight(objectLightTexture, objectLightColor, objectLightFlickers);
                                objectEmitsLight = false;
                                objectLightTexture = "light";
                                objectLightFlickers = false;
                            }
                        }
                        break;
                    default:
                        break;
                }


            }

            //addObject(new ParticleEmitter(game, "emitter", 3, 5));
        }

        #endregion

        #region Update / Draw

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
            renderLights(spriteBatch, offset);
                        
            foreach (DrawableLayer layer in layerList)
            {
                layer.setOpacity(opacity);
                layer.draw(spriteBatch, firstTile, tilesAcross, tilesDown, offset, scale);
            }

            drawLights(spriteBatch);
        }

        #endregion

        #region Lighting Methods 

        protected void renderLights(SpriteBatch spriteBatch, Point offset)
        {
            // End first draw call
            spriteBatch.End();

            // Render lights to target
            GraphicsDevice graphics = gameRef.GraphicsDevice;

            graphics.SetRenderTarget(lightRenderTarget);
            graphics.Clear(ambientLight);

            // Start lighting draw call
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearWrap, null, null);

            List<MapObject> objects = objectLayer.getObjects();
            foreach (MapObject mapObject in objects)
            {
                if (mapObject.givesOffLight())
                    mapObject.drawLight(spriteBatch, offset);
            }

            spriteBatch.End();

            graphics.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
        }

        protected void drawLights(SpriteBatch spriteBatch)
        {
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, GraphicsHelper.Multiply, SamplerState.PointClamp, null, null);
            spriteBatch.Draw(lightRenderTarget, new Vector2(), Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
        }

        #endregion

        #region Object Management Methods

        public void addObject(MapObject toAdd)
        {
            toAdd.setContainingMap(this);
            objectLayer.addObject(toAdd);
        }

        public void removeObject(MapObject toRemove)
        {
            toRemove.setContainingMap(null);
            objectLayer.removeObject(toRemove);
        }

        #endregion

        #region Accessor Methods

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

        #endregion

        public void tintTile(int x, int y, Color tintColor)
        {
            for (int index = layerList.Count - 1; index >= 0; index--)
            {
                if (layerList[index] is TileLayer)
                { 
                    ((TileLayer)layerList[index]).tintTile(x, y, tintColor);
                    break;
                }
            }
        }

        public List<Point> walk(Point startPoint, int numTiles)
        {
            List<Point> resultantPoints = new List<Point>();
            List<Point> openList = new List<Point>();

            openList.Add(startPoint);

            int tilesAdded = 0;

            while (tilesAdded < numTiles && openList.Count > 0)
            {
                Point p = openList[0];
                addAdjacentPoints(p, openList, resultantPoints);
                openList.Remove(p);
                resultantPoints.Add(p);
                tilesAdded++;
            }

            return resultantPoints;
        }

        private List<Point> temp = new List<Point>();
        private void addAdjacentPoints(Point startPoint, List<Point> openList, List<Point> closedList)
        {
            temp.Clear();
            temp.Add(new Point(startPoint.X - 1, startPoint.Y));
            temp.Add(new Point(startPoint.X, startPoint.Y - 1));
            temp.Add(new Point(startPoint.X + 1, startPoint.Y));
            temp.Add(new Point(startPoint.X, startPoint.Y + 1));

            foreach (Point p in temp)
            {
                // Add p to open list while it is inside the map, passable, and not already in open or closed lists
                if (p.X >= 0 && p.X < width && p.Y >= 0 && p.Y < height && getPassability(p.X, p.Y)
                    && (!openList.Contains(p)) && (!closedList.Contains(p)))
                    openList.Add(p);
            }
        }

    }
}
