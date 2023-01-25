using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Interfaces;
using PlayerRoles;
using SuicidePro.Handlers.CustomEffectHandlers.Effects;
using UnityEngine;
using SuicidePro.Handlers.Effects;

namespace SuicidePro
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
		public List<DamageHandlerEffect> KillConfigs { get; set; } = new()
		{
			new(),
			new() {Name = "fling",Aliases = new[] {"wee"},Description = "Weeeeeeeeeeeeee", Response = "tripping", Reason = "Tripped!", Velocity = new Velocity(15, 1, 0)},
			new() {Name = "ascend", Aliases = new[] {"fly"}, Description = "Fly high up in the air.", Response = "Sent to the next dimension", Reason = "Ascended", Velocity = new Velocity(0, 10, 0)},
			new() {Name = "flip", Description = "Do a flip!", Response = "Epic tricks",Reason = "Did a flip", Velocity = new Velocity(1f, 5, 0)},
			new() {Name = "backflip", Description = "Do a backflip!", Response = "Epic back tricks", Reason = "Did a backflip", Velocity = new Velocity(-1f, 5, 0)},
			new() {Name = "???", Description = "I don't even know what this will do.", Response = "bruh", Reason = "???", Velocity = new Velocity(70, 70, 70)},
			new() {Name = "disintegrate", Description = "Disintegrate yourself.", Response = "death", DamageType = DamageType.ParticleDisruptor},
		};

		[Description("Configuration for the builtin Explode effect.")]
		public Explode ExplodeEffect { get; set; } = new Explode
		{
			Aliases = new[] {"boom"}, Name = "explode",
			Description = "Explode! (Does not deal damage or break doors)", Response = "Boom!",
		};

		[Description("Enables debug messages in the console.")]
		public bool Debug { get; set; }

		[Description("Whether you will still be able to run disabled effects that are force registered by its developer.")]
		public bool AllowRunningDisabled { get; set; }

		[Description("Configuration for the Content Gun, summoned using .contentgun and requires cg.give permission")]
		public ContentGunConfigClass ContentGunConfig { get; set; } = new ContentGunConfigClass();

		// TODO
		/*[Description("!!!! ---- Experimental ---- !!!!\n# Configs below may cause bugs and such, please report them in GitHub issues or #plugin-bug-reports.\n# Enables .selectkill command, which allows you to have an effect upon your death.")]
		public bool AllowSelectingDeathEffect { get; set; }*/

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
