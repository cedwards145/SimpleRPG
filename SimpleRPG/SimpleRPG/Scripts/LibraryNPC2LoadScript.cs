using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRPG.Scripts
{
    public class LibraryNPC2LoadScript : Script
    {
        protected override void main()
        {
            base.main();

            moveObject(args.callingObject,
                       new MoveRoute(new Moves[] { Moves.MoveLeft, Moves.MoveLeft, Moves.MoveLeft, Moves.MoveLeft, Moves.MoveLeft, 
                                                   Moves.MoveRight, Moves.MoveRight, Moves.MoveRight, Moves.MoveRight, Moves.MoveRight },
                                     true, false));
        }
    }
}
