using System.Collections.Generic;
using System.Linq;
using Model;

namespace Mappers
{
    public class ShareMapper
    {
        private readonly LinkedList<Share> _allShares = new LinkedList<Share>();
        private long _stamp = 0;

        private ShareMapper()
        {
            
        }

        private static ShareMapper _singletonInstance;
        public static ShareMapper Singleton { 
            get { return _singletonInstance ?? (_singletonInstance = new ShareMapper()); }
        }

        public void Add(Share share)
        {
            _allShares.AddFirst(share);
            share.Stamp = _stamp++;
        }

        public void Remove(Share share)
        {
            _allShares.Remove(share);
        }

        public IEnumerable<Share> GetAll()
        {
            return _allShares;
        }

        public IEnumerable<Share> GetAllAfterStamp(long stamp)
        {
            return _allShares.Where(t => t.Stamp > stamp);
        }
    }
}
