using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadTheory
{
    // 재귀적 Lock을 허용할지 설정. (Yes)
    // → Write를 수행한 Thread가 또 다시 Write를 하는 것을 허용할 것인가.
    //   WriteLock -> WriteLock (O)
    //   WriteLock -> ReadLock (O)
    //   ReadLock -> WriteLock (X)

    // SpinLock 정책 ( 5000번 → Yield )
    class ThreadTheory_008_ReaderWriteLockRecursive_On
    {
        const int EMPTY_FLAG = 0x00000000;
        const int WRITE_MASK = 0x7FFF0000;
        const int READ_MASK = 0x0000FFFF;
        const int MAX_SPIN_COUNT = 5000;

        // [Unused(1)] [WriteThreadId(15)] [ReadCount(16)]
        // WriteThreadId : Lock을 흭득한 Thread의 ID.
        // ReadCount : 현재 Read 중인 Thread 수.
        int _flag = EMPTY_FLAG;
        int _writeCount = 0;

        public void WriteLock()
        {
            // 동일 Thread가 WriteLock을 흭득하고 있는지 확인.
            int _lockThreadId = (_flag & WRITE_MASK) >> 16;
            if (Thread.CurrentThread.ManagedThreadId == _lockThreadId)
            {
                _writeCount++;
                return;
            }

            int desired = (Thread.CurrentThread.ManagedThreadId << 16) & WRITE_MASK;

            // 아무도 WriteLock 또는 ReadLock을 흭득하고 있지 않을 때, 경합하여 소유권을 흭득.
            while (true)
            {
                for (int i = 0; i < MAX_SPIN_COUNT; ++i)
                {
                    // 시도 성공 시 return.
                    // 이전 값이 EMPTY_FLAG → 아무도 Write/Read Lock을 흭득하고 있지 않음.
                    if (Interlocked.CompareExchange(ref _flag, desired, EMPTY_FLAG) == EMPTY_FLAG)
                    {
                        _writeCount = 1;
                        return;
                    }
                }

                Thread.Yield();
            }
        }

        public void WriteUnlock()
        {
            int lockCount = --_writeCount;
            if (lockCount == 0)
            {
                Interlocked.Exchange(ref _flag, EMPTY_FLAG);
            }
        }

        public void ReadLock()
        {
            // 동일 Thread가 WriteLock을 흭득하고 있는지 확인.
            int _lockThreadId = (_flag & WRITE_MASK) >> 16;
            if (Thread.CurrentThread.ManagedThreadId == _lockThreadId)
            {
                Interlocked.Increment(ref _flag);
                return;
            }

            // 아무도 WriteLock 을 흭득하고 있지 않으면, ReadCount를 1 늘린다.
            while (true)
            {
                for (int i = 0; i < MAX_SPIN_COUNT; ++i)
                {
                    // 만약, 여러 Thread가 동시 다발적으로 들어와서 expected의 값이 같아져도,
                    // 아래의 CompareExchange에서 성공하는 Therad는 하나만 존재하기 때문에,
                    // 문제없다.
                    // ( 성공한 Thread 외의 Thread는 전부 _flag와 expected 값이 달라져버리기 때문. )
                    int expected = (_flag & READ_MASK);

                    // WriteLock을 누군가가 잡고 있다면,
                    // _flag와 expected는 같아질 수 없다.
                    // _flag의 상위 15bit는 Thread Id가 들어가 있기 때문.
                    if (Interlocked.CompareExchange(ref _flag, expected + 1, expected) == expected)
                    {
                        return;
                    }
                }

                Thread.Yield();
            }
        }

        public void ReadUnlock()
        {
            Interlocked.Decrement(ref _flag);
        }

        volatile int count = 0;

        public void Thread_1()
        {
            for (int i = 0; i < 100000; ++i)
            {
                WriteLock();
                WriteLock();
                count++;
                WriteUnlock();
                WriteUnlock();
            }
        }

        public void Thread_2()
        {
            for (int i = 0; i < 100000; ++i)
            {
                WriteLock();
                count--;
                WriteUnlock();
            }
        }

        public void Execute()
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(count);
        }
    }
}
