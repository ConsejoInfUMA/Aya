namespace Aya.Polls
{
    public class Vote
    {
        public ulong Id { get; set; }
        public Candidate Candidate { get; set; }
        public int Value { get; set; } = 0;
    }
}

