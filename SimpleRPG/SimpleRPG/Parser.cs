using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SimpleRPG
{
    public static class Parser
    {
        public static Facing parseFacing(string s)
        {
            s = s.ToLower();
            switch (s)
            {
                case "up":
                    return Facing.Up;
                case "down":
                    return Facing.Down;
                case "left":
                    return Facing.Left;
                case "right":
                    return Facing.Right;
                default:
                    return Facing.Down;
            }
        }

        public static Passability parsePassability(string value)
        {
            if (value.ToLower().Equals("true"))
                return Passability.True;
            else if (value.ToLower().Equals("false"))
                return Passability.False;
            else
                return Passability.Ignore;
        }

        public static Color parseColor(string value)
        {
            string[] components = value.Split(',');
            return new Color(int.Parse(components[0]),
                             int.Parse(components[1]),
                             int.Parse(components[2]));
        }
    }
}
