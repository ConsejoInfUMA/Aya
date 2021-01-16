using System.Collections.Generic;

namespace Aya.Polls
{
    class VoteValueComparer : IEqualityComparer<Vote>
    {
        public bool Equals(Vote v1, Vote v2)
        {
            if (v1 == null || v2 == null)
            {
                return false;
            }

            return v1.Value == v2.Value;
        }

        public int GetHashCode(Vote vote)
        {
            return vote.GetHashCode() + vote.Value;
        }
    }
}

