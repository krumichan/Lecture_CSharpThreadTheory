using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lecture_CSharpTheradTheory
{
    class ThreadTheory_001_Task
    {
        /*bool _stop = false;*/
        volatile bool _stop = false;

        void ThreadMain()
        {
            Console.WriteLine("Start Thread");

            // Release에서는 아래와 같이 최적화 해버린다.
            /*if (_stop == false)
            {
                while (true)
                {

                }
            }
            */

            while (_stop == false)
            {
                // 누군가 stop 신호를 해주기를 기다린다.
            }

            Console.WriteLine("End Thread");
        }

        public void Execute()
        {
            Task t = new Task(ThreadMain);
            t.Start();

            Thread.Sleep(1000);

            _stop = true;

            Console.WriteLine("Stop 호출");
            Console.WriteLine("종료 대기중");

            t.Wait();

            Console.WriteLine("종료 성공");
        }
    }
}
