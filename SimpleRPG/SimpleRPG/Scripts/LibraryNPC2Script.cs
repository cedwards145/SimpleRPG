using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRPG.Scripts
{
    public class LibraryNPC2Script : Script
    {
        protected override void main()
        {
            base.main();
            if (Variables.get("beerTaken") != null && Variables.getAsBool("beerTaken"))
            {
                message("Woah woah, I can't just give you all my beer!");
            }
            else
            {
                message("Grab a beer, don't cost nothin!");
                giveItem("Beer");
                Variables.set("beerTaken", true);
            }
        }
    }
}
