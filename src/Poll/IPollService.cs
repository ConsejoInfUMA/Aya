using System.Collections.Generic;

namespace Aya.Polls
{
    public interface IPollService
    {
        Poll ActivePoll { get; }

        public delegate void OnPollUpdatedDelegate(Poll poll);
        event OnPollUpdatedDelegate OnPollUpdated;
        event OnPollUpdatedDelegate OnPollStateUpdated;
        Poll StartPoll(string title, ulong messageId, ulong id, IEnumerable<ulong> voters);
        void AddCandidate(ulong id, string name);
        bool RemoveCandidate(ulong id);
        bool TryGetCandidate(ulong result, out Candidate candidate);
        void NextState();
        void StartVoting();
        void SendVote(List<Vote> result, ulong usrId);
    }
}

