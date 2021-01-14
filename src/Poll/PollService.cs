using System.Collections.Generic;
using System.Linq;
using Aya.Storage;

namespace Aya.Polls
{
    public class PollService : IPollService
    {
        private Poll _activePoll;
        private IStorage<Poll> _polls;

        public PollService(IStorage<Poll> polls)
        {
            _polls = polls;
            // TODO: load poll and check if there is an active poll
        }

        public void OnPollUpdated(Poll poll)
        {
            // TODO: create event
        }

        public void GetStatus()
        {
            throw new System.NotImplementedException();
        }

        public Poll StartPoll(string title, ulong messageId, ulong id, IEnumerable<ulong> voters)
        {
            var poll = new Poll(voters.ToDictionary((u) => u, (k) => false))
            {
                MessageId = messageId,
                Title = title,
                CreatedBy = id
            };

            _activePoll = poll;
            _polls.Save(poll.Id, poll);
            return _activePoll;
        }

        public bool TryGetActivePoll(out Poll poll)
        {
            poll = _activePoll;

            if (_activePoll is null)
            {
                return false;
            }

            return true;
        }

        public void NextState()
        {
            _activePoll.State = _activePoll.State switch
            {
                PollState.Waiting => PollState.Registering,
                PollState.Registering => PollState.Voting,
                PollState.Voting => PollState.Finished,
                PollState.Finished => PollState.Finished,
                _ => PollState.Waiting
            };
        }

        public void ChangeMessageId(ulong id)
        {
            if (TryGetActivePoll(out Poll poll))
            {
                poll.MessageId = id;
            }
        }

        public void ShowResults()
        {
            throw new System.NotImplementedException();
        }

        public void AddCandidate(ulong id, string name)
        {
            if (_activePoll.State != PollState.Registering)
            {
                return;
            }

            _activePoll.Candidates.Add(new Candidate { Id = id, Name = name });
        }
    }
}

