using Discord.Commands;

namespace EvolvaBot.Modules
{
    internal abstract class Command
    {

        protected Module module { get; }

        protected Command(Module module)
        {
            this.module = module;
        }

        internal abstract void Init(CommandGroupBuilder cgb);
    }
}