using System;
using System.Collections.Generic;
using System.Text;

namespace ThreadTheory
{
    class ThreadTheory_002_Cache
    {
        public void Execute()
        {
            int[,] arr = new int[10000, 10000];

            {
                // y,x로 접근하면 인접 접근하는 공간들을
                // 캐싱하여 바로바로 접근 가능.
                long now = DateTime.Now.Ticks;
                for (int y = 0; y < 10000; ++y)
                    for (int x = 0; x < 10000; ++x)
                        arr[y, x] = 1;
                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(y, x) 순서 걸린 시간 {end - now}");
            }

            {
                // x,y로 접근하면 인접 접근하는 공간들을
                // 캐싱할 시, 접근 실패로 이어짐.
                // ( 인접 공간을 가져왔으나, 인접 공간에 접근하지 않기 때문. )
                // 즉, 캐시를 활용하지 못하는 경우.
                long now = DateTime.Now.Ticks;
                for (int y = 0; y < 10000; ++y)
                    for (int x = 0; x < 10000; ++x)
                        arr[x, y] = 1;
                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(y, x) 순서 걸린 시간 {end - now}");
            }
        }
    }
}
