using Exiled.API.Features;
using Exiled.API.Features.Items;

namespace SuicidePro.Handlers.CustomEffect.Effect
{
    public class Explode : CustomEffect
    {
        public override string Id { get; set; } = "explode";

        public override void Use(Player player)
            => new ExplosiveGrenade(ItemType.GrenadeHE, player) {FuseTime = 0.3f, MaxRadius = 0}.SpawnActive(player.Position, player);
    }
}