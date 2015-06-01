using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRPG.Scripts
{
    public class LibraryNPC1Script : Script
    {
        protected override void main()
        {
            base.main();

            Facing prevFacing = args.callingObject.getFacing();
            args.callingObject.face(Player.getPlayerMapObject());

            message("Please don't disturb me, I'm busy working.");
            message("...");
            message("Seriously, go away!");

            args.callingObject.setFacing(prevFacing);
        }
    }
}
