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

            if (Input.isButtonPressed(Controller.ControllerButton.back))
                stateManager.addState(new PauseState(gameRef, null, stateManager));

            if (player != null)
            {
                if (Input.isButtonDown(Controller.ControllerButton.up))
                {
                    player.move(new Point(0, -1));
                }
                else if (Input.isButtonDown(Controller.ControllerButton.down))
                {
                    player.move(new Point(0, 1));
                }
                else if (Input.isButtonDown(Controller.ControllerButton.left))
                {
                    player.move(new Point(-1, 0));
                }
                else if (Input.isButtonDown(Controller.ControllerButton.right))
                {
                    player.move(new Point(1, 0));
                }
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
