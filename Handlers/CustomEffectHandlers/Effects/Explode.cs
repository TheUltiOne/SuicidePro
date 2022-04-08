using Exiled.API.Features;
using Exiled.API.Features.Items;

namespace SuicidePro.Handlers.CustomEffect.Effect
{
    public class Explode : API.CustomEffect
    {
        public override string Id { get; } = "explode";

        public override void Use(Player player)
        {
            new ExplosiveGrenade(ItemType.GrenadeHE) {MaxRadius = 0, FuseTime = 0.3f}.SpawnActive(player.Position, player);
        }
    }
}