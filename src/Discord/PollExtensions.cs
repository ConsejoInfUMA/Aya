using System;
using System.Linq;
using Discord;
using Aya.Polls;

namespace Aya.Discord
{
    public static class PollExtensions
    {
        public static int CountVotes(this Poll poll, ulong candidateId)
            => poll.Votes.Where(v => v.Candidate.Id == candidateId).Sum(v => v.Value);

        public static string FormatCandidates(this Poll poll, bool includeVotes = false)
        {
            var str = poll.Candidates.Select(c => $"<@{c.Id}>");
            return String.Join('\n', str);
        }

        public static string FormatVotes(this Poll poll)
        {
            var str = poll.Votes
                .GroupBy(v => v.Candidate)
                .Select(g => (g.Sum(v => v.Value), g.Key))
                .OrderByDescending(g => g.Item1)
                .Select(v => $"<@{v.Key.Id}>: {v.Item1}")
                .ToList();
            return String.Join('\n', str);
        }

        public static Embed ToEmbed(this Poll poll)
        {

            var status = poll.State switch
            {
                PollState.Waiting => "En espera",
                PollState.Registering => "Registro de candidatos",
                PollState.SendingMessages => "Enviando mensajes",
                PollState.Voting => "Votación en curso.",
                PollState.ProcessingResults => "Procesando resultados",
                PollState.Finished => "Votación finalizada.",
                _ => "Estado desconocido"
            };

            var embed = new EmbedBuilder()
                .WithTitle(poll.Title)
                .WithFooter($"Estado: {status}")
                .WithColor(Color.Blue);
            try
            {

                if (poll.State == PollState.Registering)
                {
                    var scandidates = poll.FormatCandidates();
                    scandidates = String.IsNullOrEmpty(scandidates) ? "Esperando candidatos..." : scandidates;
                    embed.AddField("Candidatos", scandidates);
                }
                else if (poll.State > PollState.Registering)
                {
                    var numVoters = poll.Voters.Count();
                    var numVotes = poll.Voters.Count(v => v.Value);
                    embed.AddField("Votantes", $"{numVotes}/{numVoters}");
                }

                if (poll.State == PollState.Finished)
                {
                    var sResult = poll.FormatVotes();
                    embed.AddField("result", sResult);
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
            }
            return embed.Build();
        }
    }
}

