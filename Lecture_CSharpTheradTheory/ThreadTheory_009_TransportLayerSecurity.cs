using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lecture_CSharpTheradTheory
{
    class ThreadTheory_009_TransportLayerSecurity
    {
        // Thread 고유의 전역 변수.
        static ThreadLocal<string> ThreadName = new ThreadLocal<string>(() => { return $"My Name Is {Thread.CurrentThread.ManagedThreadId}"; });

        void WhoAmI()
        {
            /*ThreadName.Value = $"My Name Is {Thread.CurrentThread.ManagedThreadId}";
            Thread.Sleep(1000);
            Console.WriteLine(ThreadName.Value);*/

            bool repeat = ThreadName.IsValueCreated;
            if (repeat)
            {
                Console.WriteLine(ThreadName.Value + "(repeat)");
            }
            else
            {
                Console.WriteLine(ThreadName.Value);
            }
        }

        public void Execute()
        {
            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(3, 3);
            Parallel.Invoke(WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI);

            ThreadName.Dispose();
        }
    }
}
