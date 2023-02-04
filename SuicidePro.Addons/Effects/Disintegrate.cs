using System;
using System.ComponentModel;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using PlayerStatsSystem;

namespace SuicidePro.Addons.Effects
{
    public class Disintegrate : API.Features.CustomEffect
    {
        public override string Id { get; } = "disintegrate";

        /// <inheritdoc/>
        public override void Use(Player player, string[] args)
        {
            player.ReferenceHub.playerStats.KillPlayer(new DisruptorDamageHandler(Server.Host.Footprint, -1));
        }
    }
}