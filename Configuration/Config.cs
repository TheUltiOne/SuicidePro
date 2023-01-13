﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Interfaces;
using PlayerRoles;
using SuicidePro.Handlers.CustomEffectHandlers;
using SuicidePro.Handlers.CustomEffectHandlers.Effects;
using UnityEngine;

namespace SuicidePro.Configuration
{
	public sealed class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		[Description("Name of kill command, for example .kill or you can change it to for example bruh, then the command will be .bruh")]
		public string CommandPrefix { get; set; } = "kill";

		[Description("If one of these is used, it will still summon the kill command.")]
		public string[] CommandAliases { get; set; } = 
		{
			"die",
			"suicide"
		};

		[Description("If .<command prefix/alias> <any of these> is used, it will show a helpful message showing all variations usable.")]
		public string[] HelpCommandAliases { get; set; } =
		{
			"help",
			"all",
			"list"
		};

		[Description("Default kill configs that are not special but just use velocity. You can add your own by copying and pasting one")]
		public List<CustomHandlerCommandConfig> KillConfigs { get; set; } = new List<CustomHandlerCommandConfig>
		{
			new CustomHandlerCommandConfig(),
			new CustomHandlerCommandConfig {Name = "fling",Aliases = new[] {"wee"},Description = "Weeeeeeeeeeeeee",Permission = "default", Response = "tripping", DamageHandler = new CustomDamageHandler {Reason = "Tripped!", Velocity = new Velocity(15, 1, 0)}},
			new CustomHandlerCommandConfig {Name = "ascend", Aliases = new[] {"fly"}, Description = "Fly high up in the air.", Permission = "default", Response = "Sent to the next dimension", DamageHandler = new CustomDamageHandler {Reason = "Ascended", Velocity = new Velocity(0, 10, 0)}},
			new CustomHandlerCommandConfig {Name = "flip", Description = "Do a flip!", Permission = "default", Response = "Epic tricks", DamageHandler = new CustomDamageHandler {Reason = "Did a flip", Velocity = new Velocity(1f, 5, 0)}},
			new CustomHandlerCommandConfig {Name = "backflip", Description = "Do a backflip!", Permission = "default", Response = "Epic back tricks", DamageHandler = new CustomDamageHandler {Reason = "Did a backflip", Velocity = new Velocity(-1f, 5, 0)}},
			new CustomHandlerCommandConfig {Name = "???", Description = "I don't even know what this will do.", Permission = "default", Response = "bruh", DamageHandler = new CustomDamageHandler {Reason = "???", Velocity = new Velocity(70, 70, 70)}}
		};

		[Description("The same as above, except it uses DamageTypes instead of custom messages.")]
		public List<DamageHandlerCommandConfig> DamageTypeKillConfigs { get; set; } =
			new List<DamageHandlerCommandConfig>
			{
				new DamageHandlerCommandConfig { Name = "disintegrate", Aliases = new[] {"railgun", "disruptor"}, Description = "Disintegrate yourself!", Permission = "default", Response = "Vanished", DamageType = DamageType.ParticleDisruptor }
			};

		[Description("Configuration for the Explode effect.")]
		public Explode ExplodeEffect { get; set; } = new Explode
		{
			Config = new EffectConfig
			{
				Aliases = new[] {"boom"}, Name = "explode", Delay = 0.3f,
				Description = "Explode! (Does not deal damage or break doors)", Response = "Boom!",
				DamageHandler = new CustomDamageHandler {Reason = "Boom!", Velocity = new Velocity(2, 0, 0)}
			}
		};

		[Description("Enables debug messages in the console.")]
		public bool Debug { get; set; }

		[Description("Whether you will still be able to run disabled effects that are force registered by its developer.")]
		public bool AllowRunningDisabledForceRegistered { get; set; }

		[Description("Configuration for the Content Gun, summoned using .contentgun and requires cg.give permission")]
		public ContentGunConfigClass ContentGunConfig { get; set; } = new ContentGunConfigClass();

		/*[Description("!!!! ---- Experimental ---- !!!!\n# Configs below may cause bugs and such, please report them in GitHub issues or #plugin-bug-reports.\n# Enables .selectkill command, which allows you to have an effect upon your death.")]
		public bool AllowSelectingDeathEffect { get; set; }*/

		public class BaseCommandConfig
		{
			/// <summary>
			/// Name of the command in .kill
			/// </summary>
			[Description("The name that will be used, for example .kill test -- if this is default, then running .kill will run this command.")]
			public string Name { get; set; } = "default";

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

			[Description("Commands to run.\n# Accepted variables: %playerid%")]
			public List<string> Commands { get; set; } = new List<string>();

			[Description("These commands will be run before the delay is applied.")]
			public List<string> PreDelayCommands { get; set; } = new List<string>();
		}

		public class CustomHandlerCommandConfig : BaseCommandConfig
		{
			[Description("A handler for mostly things after the death.")]
			public CustomDamageHandler DamageHandler { get; set; } = new CustomDamageHandler();
		}

		public class DamageHandlerCommandConfig : BaseCommandConfig
		{
			[Description("Damage type used for the kill effect.")]
			public DamageType DamageType { get; set; } = DamageType.Unknown;
		}

		public class CustomDamageHandler
		{
			/*public CustomDamageHandler(int forwardVelocity, int rightVelocity, int upVelocity)
			{
				ForwardVelocity = forwardVelocity;
				RightVelocity = rightVelocity;
				UpVelocity = upVelocity;
			}*/

			[Description("Death reason in ragdoll info.")]
			public string Reason { get; set; } = "Suicided.";

			[Description("C.A.S.S.I.E. announcement if the player is an SCP.")]
			public string CassieIfScp { get; set; } = String.Empty;
			[Description("Velocity (strength, sort of) of the body when killed.")]
			public Velocity Velocity { get; set; } = new Velocity();
		}

		public class ContentGunConfigClass
		{
			[Description("If .contentgun command is enabled.")]
			public bool Enabled { get; set; }
			[Description("Velocity (strength, sort of) of the body that will be created upon shooting.")]
			public Velocity Velocity { get; set; } = new Velocity(10, 1, 0);
			[Description("The ragdoll's name in ragdoll info.")]
			public string RagdollName { get; set; } = "Nerd";
			[Description("Death reason in ragdoll info.")]
			public string DeathCause { get; set; } = "Spawned using the Content Gun!";
			[Description("The RoleTypeId of the created ragdoll.")]
			public RoleTypeId RagdollRoleType { get; set; } = RoleTypeId.Scientist;
			[Description("The size of the created ragdoll.")]
			public Vector3 Scale { get; set; } = Vector3.one;
			[Description("Cooldown before being able to use .contentgun again.")]
			public int Cooldown { get; set; } = 180;
			[Description("Times you can shoot with the content gun.")]
			public int Uses { get; set; } = 10;
			[Description("The ItemType for the gun given after using .contentgun")]
			public ItemType GunItemType { get; set; } = ItemType.GunCOM15;
			[Description("Time (in seconds) before ragdolls created by content gun are deleted.")]
			public int CleanupTime { get; set; } = 30;
		}
	}
}
