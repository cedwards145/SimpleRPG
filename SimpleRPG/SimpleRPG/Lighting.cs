using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SimpleRPG
{
    public class Lighting
    {
        private static int time = -1;
        private static Color lightValue = Color.White;
        private static int evening, night, morning, afternoon;

        private static float r, g, b, rDelta, gDelta, bDelta;

        private static Color[] lights = { new Color(30, 30, 30), new Color(183, 147, 255),
                                          new Color(255, 255, 255), new Color(255, 129, 78) };

        public static void update()
        {
            time = (time + 1) % 3600;

            if (time == 0)
                transitionColor(lights[0], lights[1], 900);
            else if (time == 900)
                transitionColor(lights[1], lights[2], 900);
            else if (time == 1800)
                transitionColor(lights[2], lights[3], 900);
            else if (time == 2700)
                transitionColor(lights[3], lights[0], 900);

            r += rDelta;
            g += gDelta;
            b += bDelta;
            lightValue = new Color((int)r, (int)g, (int)b);
        }

        public static Color getLightValue()
        {
            //return lightValue;
            return lights[2];
        }

        private static void transitionColor(Color from, Color to, int time)
        {
            r = from.R;
            g = from.G;
            b = from.B;

            rDelta = (to.R - from.R) / (float)time;
            gDelta = (to.G - from.G) / (float)time;
            bDelta = (to.B - from.B) / (float)time;
        }
    }
}
