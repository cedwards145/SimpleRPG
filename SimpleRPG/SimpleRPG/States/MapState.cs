using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SimpleRPG.States
{
    public class MapState : GameState
    {
        protected TileMap map;
        protected Camera camera;
        protected MapObject player;

        public MapState(Game1 game, GameState parent, StateManager manager, TileMap reqMap, Camera reqCamera, MapObject playerRef)
            : base(game, parent, manager)
        {
            camera = reqCamera;
            map = reqMap;
            player = playerRef;

            camera.setMap(map);

            popOnEscape = false;

            inAnimation = AnimationType.None;
            outAnimation = AnimationType.None;
        }

        public override void update()
        {
            base.update();
            map.update();
            camera.update();

            if (Input.isKeyPressed(Keys.Escape))
                stateManager.addState(new PauseState(gameRef, null, stateManager));

            if (Input.isKeyDown(Keys.W))
            {
                player.move(new Point(0, -1));
            }
            else if (Input.isKeyDown(Keys.S))
            {
                player.move(new Point(0, 1));
            }
            else if (Input.isKeyDown(Keys.A))
            {
                player.move(new Point(-1, 0));
            }
            else if (Input.isKeyDown(Keys.D))
            {
                player.move(new Point(1, 0));
            }

            if (Input.isKeyPressed(Keys.OemMinus))
                camera.decreaseZoom();
            else if (Input.isKeyPressed(Keys.OemPlus))
                camera.increaseZoom();
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
            camera.setOpacity(opacity);
            camera.draw(spriteBatch);
        }

        public TileMap getMap()
        {
            return map;
        }
    }
}
