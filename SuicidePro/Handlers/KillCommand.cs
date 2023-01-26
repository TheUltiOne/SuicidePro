using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using RemoteAdmin;
using Log = Exiled.API.Features.Log;
using SuicidePro;
using SuicidePro.API;
using SuicidePro.API.Features;

namespace SuicidePro.Handlers
{
	[CommandHandler(typeof(ClientCommandHandler))]
	public class KillCommand : ICommand
	{
		public string Command { get; } = Plugin.Instance.Config.CommandPrefix;
		public string[] Aliases { get; } = Plugin.Instance.Config.CommandAliases;
		public string Description { get; } = "A kill command with more features.";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			
			PlayerCommandSender playerCommandSender = sender as PlayerCommandSender;
			if (playerCommandSender == null)
			{
				response = "You must run this command as a client and not server.";
				return false;
			}

			string arg = arguments.FirstOrDefault();
			Player player = Player.Get(playerCommandSender);


			IEnumerable<BaseEffect> effects = Plugin.Instance.Config.KillConfigs.Cast<BaseEffect>().Concat(CustomEffect.Effects.Cast<BaseEffect>());
            if (Plugin.Instance.Config.HelpCommandAliases.Contains(arg))
			{
				var build = new StringBuilder("Here are all the kill commands you can use:\n\n");
				foreach (var commandConfig in effects)
				{
					if (commandConfig.Permission == "none" || player.CheckPermission(FormatPermission(commandConfig)))
						build.Append($"<b><color=white>.{Plugin.Instance.Config.CommandPrefix}</color> <color=yellow>{commandConfig.Name}</color></b> {(commandConfig.Aliases.Any() ? $"<color=#3C3C3C>({String.Join(", ", commandConfig.Aliases)})</color>" : String.Empty)}\n<color=white>{commandConfig.Description}</color>\n\n");
				}

				response = build.ToString();
				return true;
			}

			arg ??= "default";

			var effect = effects.FirstOrDefault(x => x.Name == arg || x.Aliases.Contains(arg));

			if (effect == null)
			{
				response = $"Could not find any kill command with the name or alias {arg}.";
				return false;
			}

			if (effect.Permission != "none" && !player.CheckPermission(FormatPermission(effect)))
			{
				response = "You do not have the required permissions for this command.";
				return false;
			}
	
			if (!Round.IsStarted)
			{
				response = "The round has not started yet.";
				return false;
			}

			if (effect.BannedRoles.Contains(player.Role) || player.IsDead)
			{
				response = "You cannot run this kill variation as your role.";
				return false;
			}

			var ans = effect.Run(player, GetArgs(arguments));
			if (!ans && !Plugin.Instance.Config.AllowRunningDisabled)
			{
				response = "This effect is disabled.";
				return false;
			}

			response = effect.Response;
			return true;
		}

		// So much linq
		// Todo: optimize heavily
		public string[] GetArgs(ArraySegment<string> args)
		{
			if (args.IsEmpty())
				return args.ToArray();

			return args.ToList().GetRange(1, args.Count).ToArray();
		}

		public string FormatPermission(BaseEffect effect)
		{
			if (effect.Permission == "default")
			{
				Log.Debug("Permission name is 'default', returning kl." + effect.Name);
				return $"kl.{effect.Name}";
			}
			Log.Debug("Permission name is not 'default', returning " + effect.Permission);
			return effect.Permission;
		}
	}
}
