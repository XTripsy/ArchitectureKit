using System.Collections.Generic;

namespace Namespace_PlayerService
{
    internal sealed class PlayerIdService : IPlayerIdService
    {
        private int _nextId;
        private readonly Stack<int> _freeIds = new();

        public int INewID()
        {
            if (_freeIds.Count > 0) return _freeIds.Pop();

            int id = _nextId;
            _nextId++;
            return id;
        }

        public void IRemoveID(int id)
        {
            _freeIds.Push(id);
        }
    }
}
