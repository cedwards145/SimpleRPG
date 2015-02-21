using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace SimpleRPG
{
    /// <summary>
    /// Simple class to provide an easy way to change keybindings
    /// </summary>
    public class Controller
    {
        public enum ControllerButton { up, down, left, right, enter, back }

        private static Dictionary<ControllerButton, List<Keys>> keyMap = new Dictionary<ControllerButton, List<Keys>>();
        private static Dictionary<ControllerButton, List<Buttons>> buttonMap = new Dictionary<ControllerButton, List<Buttons>>();

        /// <summary>
        /// Sets up the list of controls. Must be called before any Controller related functions can be used
        /// </summary>
        public static void initialize()
        {
            // Up button
            addKey(ControllerButton.up, Keys.W);
            addKey(ControllerButton.up, Keys.Up);
            addButton(ControllerButton.up, Buttons.LeftThumbstickUp);
            addButton(ControllerButton.up, Buttons.DPadUp);
            
            // Down button
            addKey(ControllerButton.down, Keys.S);
            addKey(ControllerButton.down, Keys.Down);
            addButton(ControllerButton.down, Buttons.LeftThumbstickDown);
            addButton(ControllerButton.down, Buttons.DPadDown);

            // Left button
            addKey(ControllerButton.left, Keys.A);
            addKey(ControllerButton.left, Keys.Left);
            addButton(ControllerButton.left, Buttons.LeftThumbstickLeft);
            addButton(ControllerButton.left, Buttons.DPadLeft);

            // Right button
            addKey(ControllerButton.right, Keys.D);
            addKey(ControllerButton.right, Keys.Right);
            addButton(ControllerButton.right, Buttons.LeftThumbstickRight);
            addButton(ControllerButton.right, Buttons.DPadRight);

            // Enter button
            addKey(ControllerButton.enter, Keys.Enter);
            addButton(ControllerButton.enter, Buttons.A);

            // Back button
            addKey(ControllerButton.back, Keys.Escape);
            addButton(ControllerButton.back, Buttons.B);
        }

        private static void addKey(ControllerButton button, Keys key)
        {
            if (!keyMap.ContainsKey(button) || keyMap[button] == null)
                keyMap[button] = new List<Keys>();

            keyMap[button].Add(key);
        }

        private static void addButton(ControllerButton controllerButton, Buttons button)
        {
            if (!buttonMap.ContainsKey(controllerButton) || buttonMap[controllerButton] == null)
                buttonMap[controllerButton] = new List<Buttons>();

            buttonMap[controllerButton].Add(button);
        }

        public static List<Keys> getButtonKeys(ControllerButton button)
        {
            return keyMap[button];
        }

        public static List<Buttons> getButtonButtons(ControllerButton button)
        {
            return buttonMap[button];
        }
    }
}
