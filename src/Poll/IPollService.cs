using System.Collections.Generic;

namespace Aya.Polls
{
    public interface IPollService
    {
        Poll StartPoll(string title, ulong messageId, ulong id, IEnumerable<ulong> voters);
        bool TryGetActivePoll(out Poll poll);
        void AddCandidate(ulong id, string name);
        void ChangeMessageId(ulong id);
        void NextState();
        void ShowResults();
    }
}

