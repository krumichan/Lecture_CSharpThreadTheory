using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadTheory
{
    class ThreadTheory_003_MemoryBarrier
    {
        // Memory Barrier
        // A) 코드 재배치 억제
        // B) 가시성

        // 1) Full Memory Barrier (ASM MFENCE, C# Thread.MomoryBarrier) : Store/Load 둘 다 막는다.
        // 2) Store Memory Barrier (ASM SFENCE) : Store 만 막는다.
        // 3) Load Memory Barrier (ASM LFENCE) : Load 만 막는다.

        int x = 0;
        int y = 0;
        int r1 = 0;
        int r2 = 0;

        void Thread_1()
        {
            y = 1;  // Store y
            r1 = x; // Load x
        }

        void Thread_2()
        {
            x = 1;  // Store x
            r2 = y; // Load y
        }

        void ThreadWithMemoryBarrier_1()
        {
            y = 1;  // Store y

            // ------------
            Thread.MemoryBarrier();

            r1 = x; // Load x
        }

        void ThreadWithMemoryBarrier_2()
        {
            x = 1;  // Store x

            // ------------
            Thread.MemoryBarrier();

            r2 = y; // Load y
        }

        public void Execute(int functionId = 0)
        {
            int count = 0;
            if (functionId == 1)
            {
                while (true)
                {
                    count++;
                    x = y = r1 = r2 = 0;

                    Task t1 = new Task(Thread_1);
                    Task t2 = new Task(Thread_2);
                    t1.Start();
                    t2.Start();

                    Task.WaitAll(t1, t2);

                    // 경우의 수를 따져보면,
                    // 사실상 둘 다 0이 나올 수 없다.
                    // 이유는 Hardware에서 일어나는 최적화 때문인데,
                    // 각 Thread_n() 함수 내의 두 줄 간에 연관성이 없기 때문에,
                    // Hardware 측에서 순서를 바꿔서 실행해 버린 것이다.
                    // 즉, Thread_1()은 y=1; r1=x; → r1=x; y=1; 로,
                    //     Thread_2()은 x=1; r2=y; → r2=y; x=1; 로 바뀌어 버린 것.
                    // Single Thread 환경에서는 위 처럼 순서를 바꾸는 것이 더 효율적이면 바꿔버렸는데,
                    // Multi Thread에 대한 고려가 전혀 되지 않기 때문에 이러한 현상이 발생한다.
                    if (r1 == 0 && r2 == 0)
                        break;
                }
            }
            else
            {
                while (true)
                {
                    count++;
                    x = y = r1 = r2 = 0;

                    Task t1 = new Task(ThreadWithMemoryBarrier_1);
                    Task t2 = new Task(ThreadWithMemoryBarrier_2);
                    t1.Start();
                    t2.Start();

                    Task.WaitAll(t1, t2);

                    if (r1 == 0 && r2 == 0)
                        break;
                }
            }
            

            Console.WriteLine($"{count}번만에 빠져나옴!");
        }
    }
}
