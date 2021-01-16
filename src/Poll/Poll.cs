using System;
using System.Collections.Generic;

namespace Aya.Polls
{
    public class Poll
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public ulong CreatedBy { get; set; }
        public List<Candidate> Candidates { get; } = new List<Candidate>();
        public List<Vote> Votes { get; } = new List<Vote>();
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

        public override string ToString()
            => $"Poll -> Id: {Id}, Title: {Title}, MessageId: {MessageId}, State: {State}";

    }

    public enum PollState
    {
        Waiting = 0,
        Registering = 1,
        SendingMessages = 2,
        Voting = 3,
        ProcessingResults = 4,
        Finished = 5
    }
}

