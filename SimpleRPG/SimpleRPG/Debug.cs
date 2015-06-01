﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace SimpleRPG
{
    public static class Debug
    {
        public static readonly bool DEBUGGING = true;
        private static bool tint = false;

        public static void update()
        {
            if (Input.isKeyPressed(Keys.T) && Input.isKeyDown(Keys.LeftControl))
                toggleTinting();
        }

        public static void toggleTinting()
        {
            tint = !tint;
        }

        public static bool tintTiles()
        {
            return DEBUGGING && tint;
        }
    }
}
