using Exiled.API.Enums;
using Exiled.API.Features;

namespace SuicidePro.Handlers.CustomEffectHandlers.Effects
{
    public class Disintegrate : API.CustomEffect
    {
        public override string Id { get; } = "disintegrate";

        public override void Use(Player player)
        {
            player.Kill(DamageType.ParticleDisruptor);
        }
    }
}