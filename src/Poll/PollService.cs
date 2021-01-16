using System;
using System.Collections.Generic;
using System.Linq;
using Aya.Storage;

namespace Aya.Polls
{
    public class PollService : IPollService
    {
        private Poll _activePoll;
        private IStorage<Poll> _polls;

        public Poll ActivePoll => _activePoll;

        public event IPollService.OnPollUpdatedDelegate OnPollUpdated;
        public event IPollService.OnPollUpdatedDelegate OnPollStateUpdated;

        public PollService(IStorage<Poll> polls)
        {
            _polls = polls;
            // TODO: load poll and check if there is an active poll
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

            PollUpdated(poll);

            return _activePoll;
        }

        public void StartVoting()
        {
            RequireActivePoll();
            RequireState(PollState.Registering);
            PollUpdated(_activePoll);
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

        public void AddCandidate(ulong id, string name)
        {
            RequireState(PollState.Registering);
            _activePoll.Candidates.Add(new Candidate { Id = id, Name = name });
            PollUpdated(_activePoll);
        }

        private void PollUpdated(Poll poll, bool save = true)
        {
            if (_activePoll != null && save)
            {
                _polls.Save(_activePoll.Id, _activePoll);
            }

            OnPollUpdated?.Invoke(poll);
        }

        public void NextState()
        {
            RequireActivePoll();
            _activePoll.State = _activePoll.State switch
            {
                PollState.Waiting => PollState.Registering,
                PollState.Registering => PollState.SendingMessages,
                PollState.SendingMessages => PollState.Voting,
                PollState.Voting => PollState.ProcessingResults,
                PollState.ProcessingResults => PollState.Finished,
                PollState.Finished => PollState.Finished,
                _ => PollState.Waiting
            };

            OnPollStateUpdated?.Invoke(_activePoll);
            PollUpdated(_activePoll);
        }

        private void RequireActivePoll()
        {
            if (_activePoll == null)
            {
                throw new Exception("Active poll required");
            }
        }

        private void RequireState(PollState state)
        {
            if (_activePoll.State != state)
            {
                throw new Exception($"State assertion failed. {state} state required. Current state: {_activePoll.State}");
            }
        }

        public void SendVote(List<Vote> votes, ulong usrId)
        {
            RequireActivePoll();

            var voted = _activePoll.Voters[usrId];
            if (voted)
            {
                return;
            }
            // Ensure votes has different values
            var comparer = new VoteValueComparer();
            var distinct = votes.Distinct(comparer);

            foreach (var vote in distinct)
            {
                _activePoll.Votes.Add(vote);
            }

            _activePoll.Voters[usrId] = true;
            PollUpdated(_activePoll, false);
        }

        public bool TryGetCandidate(ulong id, out Candidate candidate)
        {
            RequireActivePoll();
            candidate = _activePoll.Candidates.FirstOrDefault(c => c.Id == id);
            if (candidate == null)
            {
                return false;
            }

            return true;
        }

        public bool RemoveCandidate(ulong id)
        {
            if (TryGetCandidate(id, out var candidate))
            {
                if (_activePoll.Candidates.Remove(candidate))
                {
                    PollUpdated(_activePoll);
                    return true;
                }

                return false;
            }

            return false;
        }

    }
}

