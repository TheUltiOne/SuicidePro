using System;
using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;
using SuicidePro.ContentGun;
using SuicidePro.Handlers.CustomEffect;
using UnityEngine;

namespace SuicidePro.Configuration
{
	public sealed class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;
		
		public string CommandPrefix { get; set; } = "kill";

		public string[] CommandAliases { get; set; } = 
		{
			"die",
			"suicide"
		};

		public string[] HelpCommandAliases { get; set; } =
		{
			"help",
			"all",
			"list"
		};

		[Description("If the permission is set to \"default\", it will just be vip.commandname")]
		public List<CustomHandlerCommandConfig> KillConfigs { get; set; } = new List<CustomHandlerCommandConfig>
		{
			new CustomHandlerCommandConfig(),
			new CustomHandlerCommandConfig {Name = "fling",Aliases = new[] {"wee"},Description = "Weeeeeeeeeeeeee",Permission = "default", Response = "tripping", DamageHandler = new CustomDamageHandler {Reason = "Tripped!", Velocity = new Velocity(15, 1, 0)}},
			new CustomHandlerCommandConfig {Name = "ascend", Aliases = new[] {"fly"}, Description = "Fly high up in the air.", Permission = "default", Response = "Sent to the next dimension", DamageHandler = new CustomDamageHandler {Reason = "Ascended", Velocity = new Velocity(0, 10, 0)}},
			new CustomHandlerCommandConfig {Name = "flip", Description = "Do a flip!", Permission = "default", Response = "Epic tricks", DamageHandler = new CustomDamageHandler {Reason = "Did a flip", Velocity = new Velocity(0.5f, 2, 0)}},
			new CustomHandlerCommandConfig {Name = "backflip", Description = "Do a backflip!", Permission = "default", Response = "Epic back tricks", DamageHandler = new CustomDamageHandler {Reason = "Did a backflip", Velocity = new Velocity(-0.5f, 2, 0)}},
			new CustomHandlerCommandConfig {Name = "???", Description = "I don't even know what this will do.", Permission = "default", Response = "bruh", DamageHandler = new CustomDamageHandler {Reason = "???", Velocity = new Velocity(70, 70, 70)}}
	};

		public Dictionary<string, EffectConfig> CustomEffects { get; set; } =
			new Dictionary<string, EffectConfig>
			{
				{"explode", new EffectConfig {Name = "explode", Aliases = new[] {"explosion","blowup","boom"}, Description = "Go out with a blast! (explosion does not deal damage)", Delay = 0.3f, Response = "Boom", DamageHandler = new CustomDamageHandler {Reason = "Boom!"}}}
			};

		public bool Debug { get; set; }

		public ContentGunConfigClass ContentGunConfig { get; set; } = new ContentGunConfigClass();

		public class BaseCommandConfig
		{
			public string Name { get; set; } = "default";

            public string Description { get; set; } = "The default kill command. Simply kills you.";

            public string Response { get; set; } = "You died.";

            public string Permission { get; set; } = "none";

            public string[] Aliases { get; set; } = Array.Empty<string>();

            public List<RoleType> BannedRoles { get; set; } = new List<RoleType>();

            public float Delay { get; set; }
		}

		public class CustomHandlerCommandConfig : BaseCommandConfig
		{
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

			public string Reason { get; set; } = "Suicided.";
			public string CassieIfScp { get; set; } = String.Empty;
			public Velocity Velocity { get; set; } = new Velocity();
		}

		public class ContentGunConfigClass
		{
			public bool Enabled { get; set; }
			public Velocity Velocity { get; set; } = new Velocity(10, 1, 0);
			public string RagdollName { get; set; } = "Nerd";
			public string DeathCause { get; set; } = "Spawned using the Content Gun!";
			public RoleType RagdollRoleType { get; set; } = RoleType.Scientist;
			public Vector3 Scale { get; set; } = Vector3.one;
			public int Cooldown { get; set; } = 180;
			public int Uses { get; set; } = 10;
			public ItemType GunItemType { get; set; } = ItemType.GunCOM15;
			public int CleanupTime { get; set; } = 30;
		}
	}
}
