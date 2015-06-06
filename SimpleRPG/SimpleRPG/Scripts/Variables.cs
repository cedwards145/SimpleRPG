using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRPG.Scripts
{
    public class Variables
    {
        private static Dictionary<string, object> variables = new Dictionary<string, object>();

        public static void set(string name, object value)
        {
            variables[name] = value;
        }

        public static object get(string name)
        {
            if (variables.ContainsKey(name))
                return variables[name];
            return null;
        }

        public static int getAsInt(string name)
        {
            return (int)variables[name];
        }

        public static bool getAsBool(string name)
        {
            return (bool)variables[name];
        }

        public static float getAsFloat(string name)
        {
            return (float)variables[name];
        }
    }
}
