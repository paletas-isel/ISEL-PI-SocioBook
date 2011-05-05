using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace Mappers
{
    public class ShareMapper
    {
        private readonly LinkedList<Share> _allShares = new LinkedList<Share>();
 
        public void Add(Share share)
        {
            _allShares.AddFirst(share);
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
