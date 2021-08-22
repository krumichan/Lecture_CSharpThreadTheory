using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lecture_CSharpTheradTheory
{
    class Program
    {
        // 1. 근성 : 계속 while 하면서 대기.
        // 2. 양보 : 자리에 돌아갔다가 추후에 다시 옴.
        // 3. 갑질 : 대기가 끝났는지 확인하는 녀석을 하나 둠.

        // 내부적으로 Monitor를 사용한다.  (  상호배제 )
        // object _lock = new object();
        // lock (_lock) { /* Do Somthing. */ }

        // SpinLock은 1, 2를 둘 다 수행한다. ( SpinLock은 C#에서 제공하는 Library가 있다. )
        // 어느정도 계속 대기하다가,
        // 답도 없이 느리면 돌아갔다가 나중에 다시 온다.

        static void Main(string[] args)
        {
            /*ThreadTheory_000_ThreadPool test_threadPool = new ThreadTheory_000_ThreadPool();
            test_threadPool.Execute();*/

            /*ThreadTheory_001_Task test_task = new ThreadTheory_001_Task();
            test_task.Execute();*/

            /*ThreadTheory_002_Cache test_cache = new ThreadTheory_002_Cache();
            test_cache.Execute();*/

            /*ThreadTheory_003_MemoryBarrier test_memoryBarrier = new ThreadTheory_003_MemoryBarrier();
            test_memoryBarrier.Execute(1);
            test_memoryBarrier.Execute();*/

            /*ThreadTheory_004_RaceCondition_Interlocked test_interlocked = new ThreadTheory_004_RaceCondition_Interlocked();
            test_interlocked.Execute(1);
            test_interlocked.Execute();*/

            /*ThreadTheory_004_RaceCondition_Monitor test_monitor = new ThreadTheory_004_RaceCondition_Monitor();
            test_monitor.Execute();*/

            /*ThreadTheory_004_RaceCondition_Lock test_lock = new ThreadTheory_004_RaceCondition_Lock();
            test_lock.Execute();*/

            /*ThreadTheory_005_SpinLock test_spinLock = new ThreadTheory_005_SpinLock();
            test_spinLock.Execute();*/

            /*ThreadTheory_006_ResetEvent_AutoResetEvent test_autoResetEvent = new ThreadTheory_006_ResetEvent_AutoResetEvent();
            test_autoResetEvent.Execute();*/

            /*ThreadTheory_006_ResetEvent_ManualResetEvent test_manualResetEvent = new ThreadTheory_006_ResetEvent_ManualResetEvent();
            test_manualResetEvent.Execute();*/

            /*ThreadTheory_007_Mutex test_mutex = new ThreadTheory_007_Mutex();
            test_mutex.Execute();*/

            /*ThreadTheory_008_ReaderWriteLockRecursive_On test_readerWriteLock = new ThreadTheory_008_ReaderWriteLockRecursive_On();
            test_readerWriteLock.Execute();*/

            ThreadTheory_009_TransportLayerSecurity test_transportLayerSecurity = new ThreadTheory_009_TransportLayerSecurity();
            test_transportLayerSecurity.Execute();
        }
    }
}
