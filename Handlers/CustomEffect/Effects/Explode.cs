﻿using Exiled.API.Features;
using Exiled.API.Features.Items;

namespace SuicidePro.Handlers.CustomEffect.Effect
{
    public class Explode : CustomEffect
    {
        public override string Id { get; } = "explode";

        public override void Use(Player player)
        {
            var grenade = Item.Create(ItemType.GrenadeHE) as ExplosiveGrenade;
            grenade.FuseTime = 0.3f;
            grenade.MaxRadius = 0;

            grenade.SpawnActive(player.Position, player);
        }
    }
}