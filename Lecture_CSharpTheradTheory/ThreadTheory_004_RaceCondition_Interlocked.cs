using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadTheory
{
    class ThreadTheory_004_RaceCondition_Interlocked
    {
        // Race Condition

        // atomic = 원자성.

        volatile int number = 0;

        void Thread_1()
        {
            for (int i = 0; i < 100000; ++i)
            {
                number++;
                // number++ 는 실제 아래와 같다.
                // int temp = number;
                // temp = temp + 1;
                // number = temp;
            }
        }

        void Thread_2()
        {
            for (int i = 0; i < 100000; ++i)
            {
                number--;
                // number-- 는 실제 아래와 같다.
                // int temp = number;
                // temp = temp - 1;
                // number = temp;
            }
        }

        int numberWithInterlocked = 0;

        void ThreadWithInterlocked_1()
        {
            for (int i = 0; i < 100000; ++i)
            {
                // atomic operation을 수행한다.
                // 하지만, cost가 높다. ( 캐시가 쓸모없기 때문. )
                // Do All Or Do Nothing.
                int prev = numberWithInterlocked;
                int after = Interlocked.Increment(ref numberWithInterlocked);
                int next = numberWithInterlocked;

                // 아래는 같지 않다.
                // 이유는 numberWithInterlocked를 다른 Thread에서 수정할 수 있기 때문이다.
                // 이 경우, 정확한 값을 얻으려면 Interlocked.Increment()의 반환값을 받으면 된다.
                // prev != next + 1
            }
        }

        void ThreadWithInterlocked_2()
        {
            for (int i = 0; i < 100000; ++i)
            {
                // atomic operation을 수행한다.
                // 하지만, cost가 높다. ( 캐시가 쓸모없기 때문. )
                // Do All Or Do Nothing.
                Interlocked.Decrement(ref numberWithInterlocked);
            }
        }

        public void Execute(int functionId = 0)
        {
            if (functionId == 1)
            {
                Task t1 = new Task(Thread_1);
                Task t2 = new Task(Thread_2);
                t1.Start();
                t2.Start();

                Task.WaitAll(t1, t2);

                // 0이 아닌 다른 값이 나올 수 있는데,
                // 이는 number의 값을 temp에 가져올 때 생기는 문제이다.
                // ( 원자성으로 동작하지 않는다. )
                Console.WriteLine(number);
            }
            else
            {
                Task t1 = new Task(ThreadWithInterlocked_1);
                Task t2 = new Task(ThreadWithInterlocked_2);
                t1.Start();
                t2.Start();

                Task.WaitAll(t1, t2);

                Console.WriteLine(numberWithInterlocked);
            }
        }
    }
}
