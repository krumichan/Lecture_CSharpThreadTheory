using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lecture_CSharpTheradTheory
{
    class ThreadTheory_005_SpinLock
    {
        class SpinLock
        {
            // 0: false
            // 1: true
            volatile int _locked = 0;

            public void Acquire()
            {
                while (true)
                {
                    //// ① ///////////////////////////////////
                    /*// get lock.
                    int original = Interlocked.Exchange(ref _locked, 1);

                    // success.
                    if (original == 0)
                    {
                        break;
                    }*/

                    //// ② ///////////////////////////////////
                    int expected = 0;
                    int desired = 1;

                    // CAS Compare-And-Swap
                    // get lock.
                    // 1, 3 인수가 같으면 2 인수를 1인수에 넣는다.
                    if (Interlocked.CompareExchange(ref _locked, desired, expected) == expected)
                    {
                        // success.
                        break;
                    }

                    // 부담을 떨구기 위함.
                    /*Thread.Sleep(1);    // 무조건 휴식 → n millisecond 휴식.
                    Thread.Sleep(0);    // 조건부 양보 → 나보다 우선순위가 낮은 애들한테는 양보 불가 → 우선순위가 나보다 같거나 높은 쓰레드가 없으면 다시 본인에게. ( 기아 현상 발생 가능성 있음. )*/
                    Thread.Yield();     // 관대한 양보 → 관대하게 양보할테니, 지금 실행이 가능한 Thread가 있으면 실행 → 실행 가능한 Thread가 없을 경우, 남은 시간 소진.
                }
            }

            public void Release()
            {
                // release lock.
                _locked = 0;
            }
        }

        // Program.
        int _number = 0;
        SpinLock _lock = new SpinLock();

        void Thread_1()
        {
            for (int i = 0; i < 100000; ++i)
            {
                _lock.Acquire();
                _number++;
                _lock.Release();
            }
        }

        void Thread_2()
        {
            for (int i = 0; i < 100000; ++i)
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

            Console.WriteLine(_number);
        }
    }
}
