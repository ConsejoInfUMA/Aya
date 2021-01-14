using System;
using System.Linq;
using Discord;
using Aya.Polls;

namespace Aya.Discord
{
    public static class PollExtensions
    {
        public static Embed ToEmbed(this Poll poll)
        {
            var footer = poll.State switch
            {
                PollState.Waiting => "En espera",
                PollState.Registering => "Registro de candidatos",
                PollState.Voting => "Votación en curso.",
                PollState.Finished => "Votación finalizada.",
                _ => "Desconocido"
            };

            var embed = new EmbedBuilder()
                .WithTitle(poll.Title)
                .WithFooter(footer)
                .WithColor(Color.Blue);

            if (poll.State != PollState.Waiting)
            {
                var candidates = poll.Candidates.Select(c => $"<@{c.Id}>");
                var scandidates = String.Join("\n", candidates);
                scandidates = String.IsNullOrEmpty(scandidates) ? "Esperando candidatos..." : scandidates;
                embed.AddField("Candidatos", scandidates);
            }

            if (poll.State > PollState.Registering)
            {
                var numVoters = poll.Voters.Count();
                var numVotes = poll.Voters.Count(v => v.Value);
                embed.AddField("Votantes", $"{numVotes}/{numVoters}");
            }

            return embed.Build();
        }
    }
}

