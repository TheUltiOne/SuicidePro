using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using PlayerStatsSystem;
using SuicidePro.API;
using System;
using System.ComponentModel;
using System.Linq;

namespace SuicidePro.Handlers.Effects
{
    public class DamageHandlerEffect : BaseEffect
    {
        [Description("Death reason in ragdoll info.")]
        public string Reason { get; set; } = "Suicided.";

        [Description("C.A.S.S.I.E. announcement if the player is an SCP.")]
        public string CassieIfScp { get; set; } = string.Empty;

        [Description("Velocity (strength, sort of) of the body when killed.")]
        public Velocity Velocity { get; set; } = new Velocity();

        [Description("This will override Reason if it's changed.")]
        public DamageType DamageType { get; set; } = DamageType.Unknown;

        /// <inheritdoc/>
        public override void Execute(Player player, string[] args)
        {
            var reason = DamageType == DamageType.Unknown ? Reason : DamageTypeExtensions.TranslationConversion.FirstOrDefault(k => k.Value == DamageType).Key.LogLabel;

            var damageHandler = new CustomReasonDamageHandler(reason, -1, CassieIfScp);
            damageHandler.StartVelocity = Velocity.ToVector3(player.Transform);
            player.ReferenceHub.playerStats.KillPlayer(damageHandler);
        }
    }
}
