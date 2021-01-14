namespace Aya.Polls
{
    public class Vote
    {
        public ulong Id { get; set; }
        public Candidate Candidate { get; set; }
        public uint Points { get; set; } = 0;
    }
}

