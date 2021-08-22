using System;
using System.Threading.Tasks;

namespace Lecture_CSharpTheradTheory
{
    class ThreadTheory_004_RaceCondition_Lock
    {
        int number = 0;
        object _obj = new object();

        void Thread_1()
        {
            for (int i = 0; i < 100000; ++i)
            {
                // 상호배제 Mutual Exclusive
                // Window OS : CriticalSection
                // C++ : std::mutex

                // ② (추천!) lock을 사용.
                lock (_obj)
                 {
                     number++;
                 }
            }
        }

        void Thread_2()
        {
            for (int i = 0; i < 100000; ++i)
            {
                lock (_obj)
                {
                    number--;
                }
            }
        }

        public void Execute()
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
    }
}
