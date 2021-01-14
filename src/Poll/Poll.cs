using System;
using System.Collections.Generic;

namespace Aya.Polls
{
    public class Poll
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public ulong CreatedBy { get; set; }
        public HashSet<Candidate> Candidates { get; } = new HashSet<Candidate>();
        public HashSet<Vote> Votes { get; } = new HashSet<Vote>();
        public Dictionary<ulong, bool> Voters { get; private set; }
        public PollState State { get; set; } = PollState.Waiting;
        public DateTime CreatedOn { get; } = DateTime.Now;
        public DateTime FinishedOn { get; set; }
        public ulong MessageId { get; set; }

        public Poll(Dictionary<ulong, bool> voters)
        {
            Voters = voters;
            Id = System.Guid.NewGuid().ToString();
        }
    }

    public enum PollState
    {
        Waiting,
        Registering,
        Voting,
        Finished
    }
}

