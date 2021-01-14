namespace Aya.Polls
{
    public class Candidate
    {
        public ulong Id { get; set; }
        public string Name { get; set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object y)
        {
            var c = y as Candidate;
            return c != null && Id == c.Id;
        }
    }
}

