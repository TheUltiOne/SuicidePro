using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerStatsSystem;

namespace SuicidePro.Handlers.CustomEffectHandlers.Effects
{
    public class Disintegrate : API.CustomEffect
    {
        public override string Id { get; } = "disintegrate";

        public override void Use(Player player)
        {
            player.ReferenceHub.playerStats.KillPlayer(new DisruptorDamageHandler(player.Footprint, -1));
        }
    }
}