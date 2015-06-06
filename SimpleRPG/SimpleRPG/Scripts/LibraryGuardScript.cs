using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRPG.Scripts
{
    public class LibraryGuardScript : Script
    {
        protected override void main()
        {
            base.main();

            if (playerHasItem("Beer"))
            {
                message("Cheers! Don't get yourself killed, I'll have to clean up after you...");
                takeItem("Beer");
                moveObject(args.callingObject, new MoveRoute(new Moves[] { Moves.MoveRight, Moves.FaceLeft }));
            }
            else
            {
                message("You aren't allowed to go any further down, too dangerous.");
                message("I might momentarily forget to keep guard if I had something to drink, if you catch my drift.");
            }
        }
    }
}
