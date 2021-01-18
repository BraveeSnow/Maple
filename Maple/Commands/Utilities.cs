using System.Threading.Tasks;
using Discord.Commands;

namespace Maple.Commands
{
    public class Utilities : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("Pong!");
        }
    }
}