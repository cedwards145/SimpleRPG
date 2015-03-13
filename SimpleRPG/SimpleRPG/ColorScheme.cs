using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SimpleRPG
{
    /// <summary>
    /// Class to hold define a set of colors for use in the rest of the game
    /// </summary>
    public class ColorScheme
    {
        /// <summary>
        /// The main color to use for text. Default text color
        /// </summary>
        public static readonly Color mainTextColor = Color.White;

        /// <summary>
        /// The standard color used for highlighted text, eg the selected item in a listbox
        /// </summary>
        public static readonly Color selectedTextColor = new Color(50, 190, 255);

        /// <summary>
        /// The standard color used for disabled text
        /// </summary>
        public static readonly Color disabledColor = new Color(100, 100, 100);


    }
}
