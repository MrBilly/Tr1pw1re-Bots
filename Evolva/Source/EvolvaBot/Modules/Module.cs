using System;
using System.Collections.Generic;
using Discord.Modules;

namespace EvolvaBot.Modules
{

    internal abstract class Module : IModule
    {

        protected readonly HashSet<Command> commands = new HashSet<Command>();

        public abstract void Install(ModuleManager manager);
        
    }
}