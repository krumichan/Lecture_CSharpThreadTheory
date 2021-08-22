using System;
using System.Threading;

namespace Lecture_CSharpTheradTheory
{
    class ThreadTheory_008_ReaderWirteLockSample
    {
        class Reward
        {

        }

        ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        // 99.99999%
        Reward GetRewardById(int id)
        {
            // 평소에는 누구든지 다 허용한다.
            // 만약 누군가가 WriteLock을 설정하면,
            // 이제 허용 안해준다.
            _lock.EnterReadLock();
            _lock.ExitReadLock();

            return null;
        }

        // 0.00001%
        void  AddReward(Reward reward)
        {
            // 특수 상황.
            _lock.EnterWriteLock();
            _lock.ExitWriteLock();
        }
    }
}
