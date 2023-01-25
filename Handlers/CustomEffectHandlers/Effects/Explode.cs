using System;
using System.ComponentModel;
using Exiled.API.Features;
using Exiled.API.Features.Items;

namespace SuicidePro.Handlers.CustomEffectHandlers.Effects
{
    public class Explode : SuicidePro.API.Features.CustomEffect
    {
        public override string Id { get; } = "explode";

        [Description(
            "It is recommended not to change this. Radius of grenade explosion calculation (door damage, player damage). If this is -1, the value will not be modified, therefore, this will act as a normal grenade.")]
        public int Radius { get; set; } = 0;

        [Description(
            "Fuse time of the grenade. I don't know what the minimum is before the grenade acts all wacky, but 0.3 is a safe value.")]
        public float Fuse { get; set; } = 0.3f;

        /// <inheritdoc/>
        public override void Use(Player player, string[] args)
        {
            var grenade = Item.Create(ItemType.GrenadeHE) as ExplosiveGrenade;
            grenade.FuseTime = Fuse;

            if (Radius != -1)
                grenade.MaxRadius = Radius;

            grenade.SpawnActive(player.Position, player);
        }
    }
}