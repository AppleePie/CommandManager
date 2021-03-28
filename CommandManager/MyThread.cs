using System.Threading;

namespace CommandManager
{
    public class MyThread
    {
        public bool IsFree { get; private set; }
        public int ThreadId { get; }
        public ThreadStart Task { get; set; }

        public MyThread()
        {
            var thread = new Thread(StartWork);
            ThreadId = thread.ManagedThreadId;
            IsFree = true;
            
            thread.Start();
        }

        private void StartWork()
        {
            while (true)
            {
                if (Task is null)
                {
                    Thread.Sleep(1);
                    continue;
                }

                IsFree = false;
                Task();
                Task = null;
                IsFree = true;
            }
        }
    }
}