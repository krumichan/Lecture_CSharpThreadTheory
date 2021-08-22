using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadTheory
{
    class ThreadTheory_007_Mutex
    {
        // Mutex는 SpinLock보다 느리다.
        // Kernel까지 가기 때문.

        int _number = 0;

        // 잠근 횟수까지 알 수 있다.
        // ThreadId : lock을 한 Thread를 알 수 있다.
        Mutex _lock = new Mutex();

        void Thread_1()
        {
            for (int i = 0; i < 100000; ++i)
            {
                _lock.WaitOne();
                _number++;
                _lock.ReleaseMutex();
            }
        }

        void Thread_2()
        {
            for (int i = 0; i < 100000; ++i)
            {
                _lock.WaitOne();
                _number--;
                _lock.ReleaseMutex();
            }
        }

        public void Execute()
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine($"{_number}");
        }
    }
}
