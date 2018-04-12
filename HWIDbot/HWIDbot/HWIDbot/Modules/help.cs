using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWIDbot.Modules
{

    public class help : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task helpAsync()
        {
            EmbedBuilder e = new EmbedBuilder();

            e.WithTitle("HARDWARE ID HELP").WithDescription("Use the command  **!hwid [your hwid]** \nGet your hwid in the boomer login screen.").WithColor(Color.Blue);

            await ReplyAsync("", false, e.Build());
        }
    }
}
