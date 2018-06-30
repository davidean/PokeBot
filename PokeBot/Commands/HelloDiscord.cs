using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace PokeBot.Commands
{
    public class HelloDiscord : ModuleBase<SocketCommandContext>
    {
        [Command("Hello"), Alias("hd"), Summary("A command that displys hello discord")]
        public async Task Hello()
        {
           await Context.Channel.SendMessageAsync("Hello discord server");
        }
    } 
}
