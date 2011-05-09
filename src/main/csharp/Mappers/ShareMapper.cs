using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Model;

namespace Mappers
{
    public class ShareMapper
    {
        private long _stamp;

        private ShareMapper()
        {

        }

        private volatile static ShareMapper _singletonInstance;
        public static ShareMapper Singleton
        {
            get
            {
                if (_singletonInstance == null)
                {
                    Interlocked.CompareExchange(ref _singletonInstance, new ShareMapper(), null);
                }
                return _singletonInstance;

            }
        }

        public void Add(User user, Share share)
        {
            lock (_singletonInstance)
            {
                user.Shares.Add(share);
                share.Stamp = _stamp++;
            }
        }

        public void Remove(User user, Share share)
        {
            lock(_singletonInstance)
            {
                user.Shares.Remove(share);
            }
        }

        public IEnumerable<Share> GetAll(User user)
        {
            return user.Shares;
        }

        public Share Get(User user, long stamp)
        {
            return user.Shares.Where(p => p.Stamp == stamp).SingleOrDefault();
        }

        public IEnumerable<Share> GetAllAfterStamp(User user, long stamp)
        {
            return user.Shares.Where(t => t.Stamp > stamp);
        }

        public IEnumerable<long> GetAllStampMissingBetween(User user, long newestStamp, long oldestStamp)
        {
            IEnumerable<Share> allInBetween = user.Shares.Where(p => p.Stamp <= newestStamp && p.Stamp >= oldestStamp);

            List<long> missingStamps = new List<long>();
            Share shareBefore = null;
            foreach(Share share in allInBetween)
            {
                if(shareBefore == null)
                {
                    shareBefore = share;
                    continue;
                }

                if(shareBefore.Stamp + 1 != share.Stamp)
                {
                    missingStamps.Add(shareBefore.Stamp + 1);
                }
                shareBefore = share;
            }

            return missingStamps;
        }
    }
}
