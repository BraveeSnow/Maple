using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Maple.NHentai;
using Maple.NHentai.Models;

namespace Maple.Commands
{
    [RequireNsfw]
    public class AnimeNsfw : ModuleBase<ShardedCommandContext>
    {
        private const int NHentaiColor = 0xF15478;
        
        private readonly TextInfo _ti;
        private readonly NHentaiClient _nhc;

        public AnimeNsfw(NHentaiClient nhc)
        {
            _nhc = nhc;
            _ti = new CultureInfo("en-US", false).TextInfo;
        }

        [Command("nhentai")]
        public async Task NHentai()
        {
            await ReplyAsync(embed: CreateDoujinEmbed(await _nhc.GetRandomDoujin()));
        }

        [Command("nhentai")]
        public async Task NHentai(int id)
        {
            await ReplyAsync(embed: CreateDoujinEmbed(await _nhc.GetDoujinById(id)));
        }

        private Embed CreateDoujinEmbed(Doujin d)
        {
            var e = new EmbedBuilder
            {
                Title = d.GetTitle(),
                Description = d.GetNativeTitle()
            };
            
            e.WithUrl(d.GetUrl());
            e.WithAuthor("Provided by NHentai");
            e.WithThumbnailUrl(d.GetCoverArtUrl());
            e.Color = new Color(NHentaiColor);

            e.AddField("ID", d.GetId(), true);
            e.AddField(
                "Characters",
                _ti.ToTitleCase(d.GetCharacters().DefaultIfEmpty("None")
                    .Aggregate((a, b) => $"{a}, {b}")),
                true
            );
            e.AddField(
                "Parodies",
                _ti.ToTitleCase(d.GetParodies().DefaultIfEmpty("None")
                    .Aggregate((a, b) => $"{a}, {b}")),
                true
            );
            e.AddField("Pages", d.GetPageCount(), true);
            e.AddField("Language", _ti.ToTitleCase(d.GetLanguage()), true);
            e.AddField(
                "Publish Date",
                d.GetDatePublished().Date.ToLongDateString(),
                true
            );
            e.WithFooter(_ti.ToTitleCase(d.GetTagList().DefaultIfEmpty("No tags available.")
                .Aggregate((a, b) => $"{a} | {b}")));
            
            return e.Build();
        }
    }
}