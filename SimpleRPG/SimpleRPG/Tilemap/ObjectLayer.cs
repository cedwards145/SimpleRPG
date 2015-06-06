using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleRPG.Tilemap
{
    public class ObjectLayer : DrawableLayer
    {
        private List<MapObject> objects;

        public ObjectLayer()
        {
            objects = new List<MapObject>();
        }

        public ObjectLayer(string reqName)
            :base(reqName)
        {
            objects = new List<MapObject>();
        }

        public void addObject(MapObject toAdd)
        {
            objects.Add(toAdd);
        }

        public void removeObject(MapObject toRemove)
        {
            objects.Remove(toRemove);
        }

        public override void update()
        {
            base.update();
            foreach (MapObject o in objects)
                o.update();

            if (Input.isButtonPressed(ControllerButton.enter))
            {
                Point tilePlayerFacing = Player.getPlayerMapObject().getTileFacing();
                int mapWidth = Player.getCurrentMap().getWidth();
                int mapHeight = Player.getCurrentMap().getHeight();

                if (tilePlayerFacing.X >= 0 && tilePlayerFacing.X < mapWidth &&
                    tilePlayerFacing.Y >= 0 && tilePlayerFacing.Y < mapHeight)
                {
                    foreach (MapObject mapObject in objects)
                    {
                        if (mapObject.getPosition() == tilePlayerFacing)
                        {
                            mapObject.action();
                            break;
                        }
                    }
                }
            }
        }

        public override void draw(SpriteBatch spriteBatch, Point firstTile, int tilesAcross, int tilesDown, Point offset, int scale)
        {
            objects.Sort();
            foreach (MapObject currentObject in objects)
            {
                currentObject.draw(spriteBatch, opacity, offset, scale);
            }
        }

        public override Passability getPassability(int x, int y)
        {
            foreach (MapObject o in objects)
            {
                if (o.getPosition().Equals(new Microsoft.Xna.Framework.Point(x, y)))
                    return o.getPassability();
            }

            return Passability.Ignore;
        }

        public List<MapObject> getObjects()
        {
            return objects;
        }
    }
}
