using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
