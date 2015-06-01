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

            message("You aren't allowed to go any further down, too dangerous.");
        }
    }
}
