using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRPG.Scripts
{
    public class HelloWorldScript : Script
    {
        protected override void main()
        {
            base.main();
            message("Hello World!");
        }
    }
}
