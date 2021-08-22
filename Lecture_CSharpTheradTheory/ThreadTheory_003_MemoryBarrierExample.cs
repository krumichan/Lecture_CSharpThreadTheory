using System;
using System.Threading;

namespace Lecture_CSharpTheradTheory
{
    class ThreadTheory_003_MemoryBarrierExample
    {
        int _answer;
        bool _complete;

        void A()
        {
            _answer = 123;
            Thread.MemoryBarrier(); // Barrier 1  : _answer Store에 대한 가시성.
            _complete = true;
            Thread.MemoryBarrier(); // Barrier 2  : _complete Store에 대한 가시성.
        }

        void B()
        {
            Thread.MemoryBarrier(); // Barrier 3  : _complete Load 전에 대한 가시성.
            if (_complete)
            {
                Thread.MemoryBarrier(); // Barrier 4  : _answer Load 전에 대한 가시성.
                Console.WriteLine(_answer);
            }
        }
    }
}
