using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadTheory
{
    class ThreadTheory_000_ThreadPool
    {
        void MainThread()
        {
            while (true)
                Console.WriteLine("Hello Thread!");
        }

        void LawCostThread(object state)
        {
            for (int i = 0; i < 5; ++i)
                Console.WriteLine("Hello Thread!");
        }

        public void Execute()
        {
            // Thread 생성은 Cost가 높음( 직원 고용 ).
            Thread t = new Thread(MainThread);
            t.Name = "Test Thread";
            t.IsBackground = true;
            t.Start();
            Console.WriteLine("Waiting for thread!");

            t.Join();

             // Cost가 낮은 Thread 실행( 단기 알바 ).
             // ① new Thread가 아닌,
             //   이미 비활성화된 Thread들이 대기중인 상태이다.
             //   필요 시, 활성화해서 사용하고,
             //   작업이 끝나면, 비활성화 시켜서 다시 대기 시킨다.
             // ② 자신이 돌릴 수 있는 Thread의 최대수가 한정되어 있기 때문에,
             //   무작정 1000개의 Thread를 생성해서 동시 실행시키는 것보다,
             //   좀 더 효율적으로 일을 수행할 수 있다.
             //   ( 손이 빈 Thread들에게 작업을 줌으로써,
             //     최대한 Thread를 활용할 수 있다. )
             /*ThreadPool.QueueUserWorkItem(LawCostThread);*/

             // Thread Pool의 한계치를 초과할 경우,
             // 실행되지 않고 먹통이 된다.
            /*ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(5, 5);
            for (int i = 0; i < 5; ++i)
                ThreadPool.QueueUserWorkItem((obj) => { while (true) { } });
            ThreadPool.QueueUserWorkItem(LawCostThread);*/

             // Task - Thread가 할 일감 단위를 조절한다.
            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(5, 5);
            for (int i = 0; i < 5; ++i)
            {
                // LongRunning Option은 ThreadPool에서 Thread를 꺼내는 것이 아닌,
                // 별도 처리를 위한 Thread를 사용한다.
                // ⇒ Thread와 ThreadPool의 장점만을 뽑아 사용.
                // LongRunning Option을 쓰지 않으면, ThreadPool에서 꺼내다가 쓴다.
                /*Task task = new Task(() => { while (true) { } }, TaskCreationOptions.LongRunning);*/
                Task task = new Task(() => { while (true) { } });
                task.Start();
            }
            ThreadPool.QueueUserWorkItem(LawCostThread);

            while (true)
            {

            }

            Console.WriteLine("Hello World!");
        }
    }
}
