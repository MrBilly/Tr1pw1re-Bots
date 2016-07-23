using Discord.Modules;
using System.Collections.Generic;
using Tr1pw1re.Classes;

namespace Tr1pw1re.Modules
{
    public abstract class DiscordModule : IModule
    {
        protected readonly HashSet<DiscordCommand> commands = new HashSet<DiscordCommand>();

        public abstract string Prefix { get; }

        public abstract void Install(ModuleManager manager);
    }
}