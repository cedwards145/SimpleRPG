using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace SimpleRPG
{
    public class SoundScheme
    {
        static SoundScheme()
        {
            Game1 gameRef = Utilities.getGameRef();
            messageTone = gameRef.Content.Load<SoundEffect>(@"Audio\SE\beep");
        }

        public static readonly SoundEffect messageTone;
    }
}
