using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using InventorySystem.Items.Firearms;
using MEC;
using SuicidePro.API.Features;
using System.ComponentModel;
using UnityEngine;
using Utils.Networking;

namespace SuicidePro.Addons.Effects
{
    public class GunSuicideEffect : CustomEffect
    {
        /// <inheritdoc/>
        public override string Id { get; } = nameof(GunSuicideEffect).ToLower();

        [Description("What should be the gun's item type?")]
        public ItemType GunItemType { get; set; } = ItemType.GunCOM18;
        public string DeathReason { get; set; } = "Suicided (in a really cool manner)";
        public Vector3 PositionOffset { get; set; } = new(0, 2, 0);

        /// <inheritdoc/>
        public override void Use(Player player, string[] args)
        {
            player.EnableEffect<Ensnared>();
            var pickup = Pickup.Create(GunItemType);
            var pos = (player.Transform.forward * 2f) + PositionOffset + player.Position;
            Quaternion rot = Quaternion.Euler(player.Transform.rotation * player.Transform.forward + new Vector3(0, -2, 0));

            pickup.Position = pos;
            pickup.Rotation = rot;
            Timing.CallDelayed(3, () => {
                PlayGunSound(player);
                player.Kill(DeathReason);
            });
        }

        public void PlayGunSound(Player player)
        {
            new GunAudioMessage()
            {
                Weapon = GunItemType,
                ShooterHub = player.ReferenceHub,
                ShooterPosition = new(player.Position),
                AudioClipId = 0,
                MaxDistance = 5,
            }.SendToAuthenticated(0);
        }
    }
}
