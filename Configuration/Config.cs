using System;
using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;
using SuicidePro.API;
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
		
		[Description("Configuration for the Disintegrate effect.")]
		public Disintegrate DisintegrateEffect { get; set; } = new Disintegrate
		{
			Config = new EffectConfig
			{
				Aliases = new[] {"raygun"}, Name = "disintegrate",
				Description = "Destroy your body!", Response = "Disintegrated",
				IgnoreDamageHandlerConfigs = true
			}
		};

		[Description("Enables debug messages in the console.")]
		public bool Debug { get; set; }

		[Description("Whether you will still be able to run disabled effects that are force registered by its developer.")]
		public bool AllowRunningDisabledForceRegistered { get; set; }

		[Description("Configuration for the Content Gun, summoned using .contentgun and requires cg.give permission")]
		public ContentGunConfigClass ContentGunConfig { get; set; } = new ContentGunConfigClass();

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
			/// A <see cref="List{T}"/> of <see cref="RoleType"/>, that when the player is a role that is contained within the list, prevents execution of the command.
			/// </summary>
			[Description("RoleTypes that are not allowed to use this command.")]
            public List<RoleType> BannedRoles { get; set; } = new List<RoleType>();

			/// <summary>
			/// <see cref="float"/> in seconds to wait before the <see cref="Exiled.API.Features.Player"/>'s death is applied.
			/// </summary>
			[Description("Number of seconds to wait before player's death is applied.")]
            public float Delay { get; set; }
		}

		public class CustomHandlerCommandConfig : BaseCommandConfig
		{
			[Description("A handler for mostly things after the death.")]
			public CustomDamageHandler DamageHandler { get; set; } = new CustomDamageHandler();
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
			[Description("The RoleType of the created ragdoll.")]
			public RoleType RagdollRoleType { get; set; } = RoleType.Scientist;
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
