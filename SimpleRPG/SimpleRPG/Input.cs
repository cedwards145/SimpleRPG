using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SimpleRPG
{
    public class Input
    {
        private static KeyboardState lastKState, currentKState;
        private static MouseState lastMState, currentMState;

        public static void update()
        {
            lastKState = currentKState;
            currentKState = Keyboard.GetState();

            lastMState = currentMState;
            currentMState = Mouse.GetState();
        }

        public static bool isKeyDown(Keys key)
        {
            return currentKState.IsKeyDown(key);
        }

        public static bool isKeyPressed(Keys key)
        {
            return currentKState.IsKeyDown(key) && !lastKState.IsKeyDown(key);
        }

        public static bool isKeyReleased(Keys key)
        {
            return !currentKState.IsKeyDown(key) && lastKState.IsKeyDown(key);
        }

        public static Vector2 getMousePosition()
        {
            return new Vector2(currentMState.X, currentMState.Y);
        }
    }
}
