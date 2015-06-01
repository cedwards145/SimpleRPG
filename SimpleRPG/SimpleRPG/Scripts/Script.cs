using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleRPG;
using SimpleRPG.States;

namespace SimpleRPG.Scripts
{
    public abstract class Script
    {
        public static void runScript(string scriptName)
        {
            Type scriptType = Type.GetType("SimpleRPG.Scripts." + scriptName);
            Script script = (Script)Activator.CreateInstance(scriptType);
            script.execute();
        }

        public Script()
        { }

        private void execute()
        {
            IdleThread thread = Utilities.getScriptThread();
            thread.doWork(main);
        }

        protected virtual void main()
        { }

        protected void message(string text)
        {
            MessageState state = Utilities.getGameRef().showMessage(text);
            while (!state.isFinished())
                continue;
        }
    }
}
