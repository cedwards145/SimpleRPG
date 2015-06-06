using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleRPG;
using SimpleRPG.States;
using SimpleRPG.Items; 
using System.Threading;
using SimpleRPG.Tilemap; 

namespace SimpleRPG.Scripts
{
    public abstract class Script
    {
        protected ScriptBehaviour behaviour = ScriptBehaviour.Default;
        protected ScriptArgs args;

        protected static Script lookupScript(string scriptName)
        {
            Type scriptType = Type.GetType("SimpleRPG.Scripts." + scriptName);
            Script script = (Script)Activator.CreateInstance(scriptType);
            return script;
        }

        public static void runScriptSync(string scriptName, ScriptArgs args)
        {
            Script script = lookupScript(scriptName);
            script.args = args;
            script.executeSync();
        }

        public static void runScriptAsync(string scriptName, ScriptArgs args)
        {
            Script script = lookupScript(scriptName);
            script.args = args;
            script.executeAsync();
        }

        public Script()
        { }

        private void executeAsync()
        {
            IdleThread thread = Utilities.getScriptThread();
            thread.doWork(start);
        }

        private void executeSync()
        {
            start();
        }

        private bool canAccessMenu, canMove;

        private void start()
        {
            canAccessMenu = Player.canAccessMenu();
            canMove = Player.canMove();
            
            main();

            Player.setCanAccessMenu(canAccessMenu);
            Player.setCanMove(canMove);
        }

        private void end()
        {
            switch (behaviour)
            {
                case ScriptBehaviour.Default:
                    enableMenu();
                    enablePlayerMovement();
                    break;
                default:
                    break;
            }
        }

        protected virtual void main()
        { 
            switch (behaviour)
            {
                case ScriptBehaviour.Default:
                    disableMenu();
                    disablePlayerMovement();
                    break;
                case ScriptBehaviour.NonBlocking:
                    enableMenu();
                    enablePlayerMovement();
                    break;
            }
        }

        protected void message(string text)
        {
            MessageState state = Utilities.getGameRef().showMessage(text);
            while (!state.isFinished())
                continue;
        }

        protected void waitSeconds(int secondsToWait)
        {
            Thread.Sleep(secondsToWait * 1000);
        }

        protected void waitFrames(int framesToWait)
        {
            Game1 gameRef = Utilities.getGameRef();
            long currentTime = gameRef.getElapsedGameTime();
            long finishTime = currentTime + framesToWait;

            while (gameRef.getElapsedGameTime() != finishTime)
                continue;
        }

        protected void enableMenu()
        {
            Player.setCanAccessMenu(true);
        }

        protected void disableMenu()
        {
            Player.setCanAccessMenu(false);
        }

        protected void enablePlayerMovement()
        {
            Player.getPlayerMapObject().setCanMove(true);
        }

        protected void disablePlayerMovement()
        {
            Player.getPlayerMapObject().setCanMove(false);
        }

        protected void callScript(string scriptName)
        {
            Script script = lookupScript(scriptName);
            script.args = args;
            script.start();
        }

        protected void moveObject(MapObject toMove, MoveRoute route)
        {
            toMove.setMoveRoute(route);
        }

        protected bool playerHasItem(string itemName)
        {
            return Player.getInventory().contains(itemName);
        }

        protected int countItem(string itemName)
        {
            return Player.getInventory().countItem(itemName);
        }

        protected void giveItem(string itemName)
        {
            Player.giveItem(itemName);
        }

        protected void takeItem(string itemName)
        {
            Player.takeItem(itemName);
        }
    }
}
