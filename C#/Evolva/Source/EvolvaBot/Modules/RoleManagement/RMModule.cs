using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.Modules;

using EvolvaBot.Classes;
using EvolvaBot.Classes.Permissions;

namespace EvolvaBot.Modules.RoleManagement
{
    internal class RMModule : Module
    {

        public RMModule()
        {
                
        }

        public override void Install(ModuleManager manager)
        {
            var client = EvolvaBot.Client;

            manager.CreateCommands("!rm", cgb =>
            {
                cgb.CreateCommand("addRole")
                    .Alias("ar")
                    .Description("Adds a specific role to the specified user.\n")
                    .Parameter("user", ParameterType.Required)
                    .Parameter("role", ParameterType.Required)
                    .AddCheck(SimpleCheckers.CanManageRoles)
                    .Do(async e =>
                    {
                        var getUser = e.GetArg("user");
                        var getRole = e.GetArg("role");
                        var user = e.Server.FindUsers(getUser).FirstOrDefault();
                        var role = e.Server.FindRoles(getRole).FirstOrDefault();
                        if (getUser == null)
                        {
                            await e.Channel.SendMessage("Invalid parameters: username. Error: PARAMETER_EQUALS_NULL").ConfigureAwait(false);
                            EvolvaBot.PrintError("Failed to supply paramaters: PARAMETER_EQUALS_NULL");
                            return;
                        }
                        if (getRole == null)
                        {
                            await e.Channel.SendMessage("Invalid parameters: role. Error: PARAMETER_EQUALS_NULL").ConfigureAwait(false);
                            EvolvaBot.PrintError("Failed to supply paramaters: PARAMETER_EQUALS_NULL");
                            return;
                        }
                        EvolvaBot.PrintInfo(e.User.Name + " executed command [!rm addRole] from module [RoleManagement] which returned:");
                        try
                        {
                            await user.AddRoles(role).ConfigureAwait(false);
                            EvolvaBot.PrintInfo("Added role " + role + " to " + user);
                            await e.Channel.SendMessage($"**{e.User.Name}**: Successfully added role `{role}` to `{user}`");
                        }
                        catch
                        {
                            await e.Channel.SendMessage("Couldn't add role");
                            EvolvaBot.PrintInfo("Failed to add role " + role + " to " + user);
                            return;
                        }
                    });

                cgb.CreateCommand("removeRole")
                    .Alias("rr")
                    .Description("Removes a specific role from the specified user.\n")
                    .Parameter("user", ParameterType.Required)
                    .Parameter("role", ParameterType.Required)
                    .AddCheck(SimpleCheckers.CanManageRoles)
                    .Do(async e =>
                    {
                        var getUser = e.GetArg("user");
                        var getRole = e.GetArg("role");
                        var user = e.Server.FindUsers(getUser).FirstOrDefault();
                        var role = e.Server.FindRoles(getRole).FirstOrDefault();
                        if (getUser == null)
                        {
                            await e.Channel.SendMessage("Invalid parameters: username. Error: PARAMETER_EQUALS_NULL").ConfigureAwait(false);
                            EvolvaBot.PrintError("Failed to supply paramaters: PARAMETER_EQUALS_NULL");
                            return;
                        }
                        if (getRole == null)
                        {
                            await e.Channel.SendMessage("Invalid parameters: role. Error: PARAMETER_EQUALS_NULL").ConfigureAwait(false);
                            EvolvaBot.PrintError("Failed to supply paramaters: PARAMETER_EQUALS_NULL");
                            return;
                        }
                        EvolvaBot.PrintInfo(e.User.Name + " executed command [!rm removeRole] from module [RoleManagement] which returned:");
                        try
                        {
                            await user.RemoveRoles(role).ConfigureAwait(false);
                            EvolvaBot.PrintInfo("Removed role " + role + " from " + user);
                            await e.Channel.SendMessage($"**{e.User.Name}**: Successfully removed role `{role}` from `{user}`");
                        }
                        catch
                        {
                            await e.Channel.SendMessage("Couldn't remove role");
                            EvolvaBot.PrintInfo("Failed to remove role " + role + " from " + user);
                            return;
                        }
                    });

                cgb.CreateCommand("createRole")
                    .Alias("cr")
                    .Description("Creates a new role.\n")
                    .Parameter("name", ParameterType.Required)
                    .AddCheck(SimpleCheckers.CanManageRoles)
                    .Do(async e =>
                    {
                        var roleName = e.GetArg("name");
                        if (e.GetArg("name") == null)
                        {
                            await e.Channel.SendMessage("Invalid parameters: username. Error: PARAMETER_EQUALS_NULL").ConfigureAwait(false);
                            EvolvaBot.PrintError("Failed to supply paramaters: PARAMETER_EQUALS_NULL");
                            return;
                        }
                        EvolvaBot.PrintInfo(e.User.Name + " executed command [!rm createRole] from module [RoleManagement] which returned:");
                        try
                        {
                            await e.Server.CreateRole(roleName);
                            EvolvaBot.PrintInfo($"Created new role [{roleName}]");
                            await e.Channel.SendMessage($"**{e.User.Name}**: Successfully created a new role called `{roleName}`");
                        }
                        catch
                        {
                            await e.Channel.SendMessage("Couldn't create new role");
                            EvolvaBot.PrintError($"Failed to create role: {roleName}");
                            return;
                        }
                    });
                
            });
        }
    }
}