using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lecture_CSharpTheradTheory
{
    class ThreadTheory_006_ResetEvent_AutoResetEvent
    {
        class Lock
        {
            // AutoResetEvent를 쓰면 어마어마하게 느리다.
            // Kernel까지 가기 때문.

            // true: 아무나 접근 가능.
            // false: 접근 불가.
            AutoResetEvent _available = new AutoResetEvent(true);

            public void Acquire()
            {
                _available.WaitOne(); // 접근 시도.
            }

            public void Release()
            {
                // AutoResetEvent를 release.
                // flag → true.
                _available.Set();
            }
        }

        int _number = 0;
        Lock _lock = new Lock();

        void Thread_1()
        {
            for (int i = 0; i < 10000; ++i)
            {
                _lock.Acquire();
                _number++;
                _lock.Release();
            }
        }

        void Thread_2()
        {
            for (int i = 0; i < 10000; ++i)
            {
                _lock.Acquire();
                _number--;
                _lock.Release();
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
