using Exiled.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using SuicidePro.Handlers;

namespace SuicidePro.API
{
    public abstract class BaseEffect
    {
        /// <summary>
        /// Name of the command in .kill
        /// </summary>
        [Description("The name that will be used, for example .kill test -- if this is default, then running .kill will run this command.")]
        public string Name { get; set; } = "default";

        /// <summary>
        /// Sets if the kill effect can be used.
        /// </summary>
        [Description("Should the kill effect be usable?")]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// The description of the command in .kill list
        /// </summary>
        [Description("The command's description in the kill list")]
        public string Description { get; set; } = "The default kill command. Simply kills you.";

        /// <summary>
        /// The response in the console.
        /// </summary>
        [Description("Response in console after using the command")]
        public string Response { get; set; } = "You died.";

        /// <summary>
        /// The EXILED permission required for this command.
        /// </summary>
        /// <remarks>If this is none, then none is needed.
        /// If this is default, then the permission will be kl.<see cref="Name"/>
        /// If otherwise, then the permission will be that written.</remarks>
        [Description("The Exiled permission required to use this command. If this is none, then none is needed. If this is default, then it will be automatically kl.command_name. If none of these, then it will take whatever is written as permission (e.g. scpstats.hats)")]
        public string Permission { get; set; } = "none";

        /// <summary>
        /// Other <see cref="string"/>s usable for the command.
        /// </summary>
        [Description("Other names that will still run the command")]
        public string[] Aliases { get; set; } = Array.Empty<string>();

        /// <summary>
        /// A <see cref="List{T}"/> of <see cref="RoleTypeId"/>, that when the player is a role that is contained within the list, prevents execution of the command.
        /// </summary>
        [Description("RoleTypes that are not allowed to use this command.")]
        public List<RoleTypeId> BannedRoles { get; set; } = new List<RoleTypeId>();

        /// <summary>
        /// <see cref="float"/> in seconds to wait before the <see cref="Exiled.API.Features.Player"/>'s death is applied.
        /// </summary>
        [Description("Number of seconds to wait before player's death is applied.")]
        public float Delay { get; set; }

        /// <summary>
        /// A <see cref="List{T}"/> of commands that should be ran upon usage of the command.
        /// </summary>
        [Description("Commands to run.\n# Accepted variables: %playerid%")]
        public List<string> Commands { get; set; } = new List<string>();

        /// <summary>
        /// A <see cref="List{T}"/> of commands that should be ran upon usage of the command, before the <see cref="Delay"/>.
        /// </summary>
        [Description("These commands will be run before the delay is applied.")]
        public List<string> PreDelayCommands { get; set; } = new List<string>();

        /// <summary>
        /// Runs this <see cref="BaseEffect"/> on a specific <see cref="Player"/> (<paramref name="player"/>).
        /// </summary>
        /// <param name="player">The <see cref="Player"/> that the this <see cref="BaseEffect"/> will be run on.</param>
        /// <param name="args">The args specified in the console whilst running this command.</param>
        /// <returns>A <see cref="bool"/> that indicates whether this <see cref="BaseEffect"/> should be allowed to run.</returns>
        public abstract void Execute(Player player, string[] args);

        /// <summary>
        /// Runs the <see cref="BaseEffect"/> on a specific <see cref="Player"/> (<paramref name="player"/>) properly, by running commands and applying delay.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> that the this <see cref="BaseEffect"/> will be run on.</param>
        /// <param name="args">The args specified in the console whilst running this command.</param>
        /// <returns>A <see cref="bool"/> that indicates whether this <see cref="BaseEffect"/> should be allowed to run.</returns>
        public bool Run(Player player, string[] args)
        {
            if (!Enabled)
                return false;

            Methods.Run(this, player, args);
            return true;
        }
    }
}
