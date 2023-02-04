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
        public float VerticalOffset { get; set; } = 1;
        public float ForwardOffset { get; set; } = 1;
        public float TimeBeforeDeath { get; set; } = 3;

        /// <inheritdoc/>
        public override void Use(Player player, string[] args)
        {
            player.EnableEffect<Ensnared>();
            var pickup = Pickup.Create(GunItemType);

            pickup.IsLocked = true;
            pickup.Position = player.Position + (player.Transform.forward * ForwardOffset) + Vector3.up * VerticalOffset;
            pickup.Rotation = Quaternion.Euler(-player.Transform.rotation.eulerAngles);
            pickup.GameObject.GetComponent<Rigidbody>().isKinematic = true;
            pickup.Spawn();

            Timing.CallDelayed(TimeBeforeDeath, () => {
                PlayGunSound(player);
                player.Kill(DeathReason);
                pickup.UnSpawn();
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
