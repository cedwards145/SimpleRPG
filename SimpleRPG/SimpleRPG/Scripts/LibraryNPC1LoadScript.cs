using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRPG.Scripts
{
    public class LibraryNPC1LoadScript : Script
    {
        protected override void main()
        {
            base.main();

            moveObject(args.callingObject,
                       new MoveRoute(new Moves[] { Moves.MoveLeft, Moves.MoveDown, Moves.MoveRight, 
                                                   Moves.MoveRight, Moves.MoveUp, Moves.MoveLeft },
                                     true, false));
        }
    }
}
