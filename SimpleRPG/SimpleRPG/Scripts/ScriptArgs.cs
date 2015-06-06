using SimpleRPG.Tilemap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRPG.Scripts
{
    public class ScriptArgs
    {
        public readonly ScriptActivationType activationType;
        public readonly MapObject callingObject;

        public ScriptArgs(ScriptActivationType type, MapObject caller)
        {
            callingObject = caller;
            activationType = type;
        }

    }
}
