using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SimpleRPG.Scripts
{
    public class IdleThread
    {
        public delegate void Work();


        private Thread thread;
        private bool alive;
        private Work work = null;

        public IdleThread()
        {
            thread = new Thread(loop);
            thread.Start();
            alive = true;
        }

        public void kill()
        {
            alive = false;
            thread.Abort();
        }

        private void loop()
        {
            while (alive)
            {
                if (work != null)
                {
                    work();
                    work = null;
                }
            }
        }

        public void doWork(Work toDo)
        {
            work = toDo;
        }
    }
}
