using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lecture_CSharpTheradTheory
{
    class ThreadTheory_006_ResetEvent_ManualResetEvent
    {
        class Lock
        {
            // ManualResetEvent는 lock으로 쓰기 좋지 않다.
            // 이유는,
            // WaitOne()과 Reset()을 호출함으로써,
            // 원자성을 위배하기 때문이다.

            // true: 아무나 접근 가능.
            // false: 접근 불가.
            ManualResetEvent _available = new ManualResetEvent(true);

            public void Acquire()
            {
                _available.WaitOne();   // 접근 시도.
                _available.Reset();     // 잠금.
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
