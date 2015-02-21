using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SimpleRPG
{
    /// <summary>
    /// Class to simplify handling input from controllers and keyboards
    /// </summary>
    public class Input
    {
        private static KeyboardState lastKState, currentKState;
        private static MouseState lastMState, currentMState;
        private static GamePadState lastCState, currentCState;

        /// <summary>
        /// Must be called from the Game's update loop to refresh keyboard states
        /// </summary>
        public static void update()
        {
            lastKState = currentKState;
            currentKState = Keyboard.GetState();

            lastMState = currentMState;
            currentMState = Mouse.GetState();

            lastCState = currentCState;
            currentCState = GamePad.GetState(PlayerIndex.One);
        }

        /// <summary>
        /// Checks if a given keyboard key is currently held down
        /// </summary>
        /// <param name="key">The key to query</param>
        /// <returns>Returns the key's current "down" state</returns>
        public static bool isKeyDown(Keys key)
        {
            return currentKState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks if a button on the first 360 pad is currently held down
        /// </summary>
        /// <param name="button">The button to query</param>
        /// <returns></returns>
        public static bool isPadButtonDown(Buttons button)
        {
            return currentCState.IsConnected && currentCState.IsButtonDown(button);
        }

        /// <summary>
        /// Checks if a given button defined in Controller is down
        /// </summary>
        /// <param name="button">The button to query</param>
        /// <returns></returns>
        public static bool isButtonDown(Controller.ControllerButton button)
        {
            List<Keys> keys = Controller.getButtonKeys(button);
            bool down = false;
            foreach (Keys key in keys)
                down = down || isKeyDown(key);

            List<Buttons> buttons = Controller.getButtonButtons(button);
            foreach (Buttons padButton in buttons)
                down = down || isPadButtonDown(padButton);

            return down;
        }

        /// <summary>
        /// Checks if a keyboard key has been pressed, meaning that it was not pressed last update,
        /// and now is.
        /// </summary>
        /// <param name="key">The key to query</param>
        /// <returns></returns>
        public static bool isKeyPressed(Keys key)
        {
            return currentKState.IsKeyDown(key) && !lastKState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks if a button on the first 360 pad has been pressed, meaning that it was not pressed last update,
        /// and now is.
        /// </summary>
        /// <param name="button">The button to query</param>
        /// <returns></returns>
        public static bool isPadButtonPressed(Buttons button)
        {
            return currentCState.IsButtonDown(button) && !lastCState.IsButtonDown(button);
        }

        /// <summary>
        /// Checks if a controller button has been pressed, meaning that it was not pressed last update,
        /// and now is.
        /// </summary>
        /// <param name="button">The button to query</param>
        /// <returns></returns>
        public static bool isButtonPressed(Controller.ControllerButton button)
        {
            List<Keys> keys = Controller.getButtonKeys(button);
            bool pressed = false;
            foreach (Keys key in keys)
                pressed = pressed || isKeyPressed(key);

            List<Buttons> buttons = Controller.getButtonButtons(button);
            foreach (Buttons padButton in buttons)
                pressed = pressed || isPadButtonPressed(padButton);

            return pressed;
        }

        /// <summary>
        /// Checks if a keyboard key has just been released
        /// </summary>
        /// <param name="key">The key to query</param>
        /// <returns></returns>
        public static bool isKeyReleased(Keys key)
        {
            return !currentKState.IsKeyDown(key) && lastKState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks if a button on the first 360 pad has just been released
        /// </summary>
        /// <param name="button">The button to query</param>
        /// <returns></returns>
        public static bool isPadButtonReleased(Buttons button)
        {
            return !currentCState.IsButtonDown(button) && lastCState.IsButtonDown(button);
        }

        /// <summary>
        /// Checks if a controller button has just been released
        /// </summary>
        /// <param name="button">The button to query</param>
        /// <returns></returns>
        public static bool isButtonReleased(Controller.ControllerButton button)
        {
            List<Keys> keys = Controller.getButtonKeys(button);
            bool released = false;
            foreach (Keys key in keys)
                released = released || isKeyReleased(key);

            List<Buttons> buttons = Controller.getButtonButtons(button);
            foreach (Buttons padButton in buttons)
                released = released || isPadButtonReleased(padButton);

            return released;
        }

        /// <summary>
        /// Gets the current position of the mouse pointer
        /// </summary>
        /// <returns></returns>
        public static Vector2 getMousePosition()
        {
            return new Vector2(currentMState.X, currentMState.Y);
        }
    }
}
