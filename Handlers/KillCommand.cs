using System;
using System.Linq;
using System.Text;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using RemoteAdmin;
using SuicidePro.Configuration;
using Log = Exiled.API.Features.Log;

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

			if (Plugin.Instance.Config.HelpCommandAliases.Contains(arg))
			{
				var build = new StringBuilder("Here are all the kill commands you can use:\n\n");
				foreach (var commandConfig in Plugin.Instance.Config.KillConfigs)
				{
					if (commandConfig.Permission == "none" || player.CheckPermission(FormatPermission(commandConfig)))
						build.Append($"<b><color=white>.{Plugin.Instance.Config.CommandPrefix}</color> <color=yellow>{commandConfig.Name}</color></b> {(commandConfig.Aliases.Any() ? $"<color=#3C3C3C>({String.Join(", ", commandConfig.Aliases)})</color>" : String.Empty)}\n<color=white>{commandConfig.Description}</color>\n\n");
				}

				foreach (var effect in API.CustomEffect.Effects)
				{
					if (effect.Config.Enabled && (effect.Config.Permission == "none" || player.CheckPermission(FormatPermission(effect.Config))))
						build.Append($"<b><color=white>.{Plugin.Instance.Config.CommandPrefix}</color> <color=yellow>{effect.Config.Name}</color></b> {(effect.Config.Aliases.Any() ? $"<color=#3C3C3C>({String.Join(", ", effect.Config.Aliases)})</color>" : String.Empty)}\n<color=white>{effect.Config.Description}</color>\n\n");
				}

				response = build.ToString();
				return true;
			}

			if (arg == null)
				arg = "default";

			Config.BaseCommandConfig config = Plugin.Instance.Config.KillConfigs.FirstOrDefault(x => x.Name == arg || x.Aliases.Contains(arg));
			var customConfig = API.CustomEffect.Effects.FirstOrDefault(x => x.Config.Name == arg || x.Config.Aliases.Contains(arg));

			if (config == null && customConfig == null)
			{
				response = $"Could not find any kill command with the name or alias {arg}.";
				return false;
			}

			if (customConfig != null)
				config = customConfig.Config;

			if (config.Permission != "none" && !player.CheckPermission(FormatPermission(config)))
			{
				response = "You do not have the required permissions for this command.";
				return false;
			}
	
			if (!Round.IsStarted && !RoundSummary.singleton.RoundEnded)
			{
				response = "The round has not started yet.";
				return false;
			}

			if (config.BannedRoles.Contains(player.Role) || player.IsDead)
			{
				response = "You cannot run this kill variation as your role.";
				return false;
			}

			if (customConfig == null)
			{
				((Config.CustomHandlerCommandConfig) config).Run(player);
			}
			else
			{
				var ans = customConfig.Run(player);
				if (!ans)
				{
					response = "This effect is disabled.";
					return false;
				}
			}

			response = config.Response;
			return true;
		}

		public string FormatPermission(Config.BaseCommandConfig config)
		{
			if (config.Permission == "default")
			{
				Log.Debug("Permission name is 'default', returning kl." + config.Name, Plugin.Instance.Config.Debug);
				return $"kl.{config.Name}";
			}
			Log.Debug("Permission name is not 'default', returning " + config.Permission, Plugin.Instance.Config.Debug);
			return config.Permission;
		}
	}
}
