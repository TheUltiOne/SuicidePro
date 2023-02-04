using Exiled.API.Features;
using SuicidePro.API.Features;
using PlayerStatsSystem;

namespace SuicidePro.Addons.Effects
{
    public class Disintegrate : CustomEffect
    {
        public override string Id { get; } = "disintegrate";

        /// <inheritdoc/>
        public override void Use(Player player, string[] args)
        {
            player.ReferenceHub.playerStats.KillPlayer(new DisruptorDamageHandler(Server.Host.Footprint, -1));
        }
    }
}