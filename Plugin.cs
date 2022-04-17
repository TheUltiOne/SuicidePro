using System;
using System.Reflection;
using Exiled.API.Features;
using PlayerStatsSystem;
using SuicidePro.ContentGun;
using UnityEngine;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace SuicidePro
{
    public class Plugin : Plugin<Configuration.Config>
    {
        public override string Author { get; } = "TheUltiOne";
        public override string Name { get; } = "Suicide - Pro Edition";
        public override Version Version { get; } = new Version(2, 1, 1);
        public override Version RequiredExiledVersion { get; } = new Version(5, 1, 0);

        public static Plugin Instance;
        private static Handler _cgEventHandlers;
        public FieldInfo VelocityInfo;

        public override void OnEnabled()
        {
            Instance = this;
            Config.ExplodeEffect.Register();
            Config.DisintegrateEffect.Register();

            VelocityInfo = typeof(CustomReasonDamageHandler).GetField("StartVelocity", BindingFlags.NonPublic | BindingFlags.Instance);

            if (Config.ContentGunConfig.Enabled)
            {
                _cgEventHandlers = new Handler();
                Player.Dying += _cgEventHandlers.OnDying;
                Player.DroppingItem += _cgEventHandlers.OnDroppingItem;
                Player.ChangingItem += _cgEventHandlers.OnChangingItem;
                Player.Shooting += _cgEventHandlers.OnShooting;
                Player.UnloadingWeapon += _cgEventHandlers.OnUnloadingWeapon;
                Server.WaitingForPlayers += _cgEventHandlers.OnWaitingForPlayers;
            }

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            if (_cgEventHandlers != null)
            {
                Player.Dying -= _cgEventHandlers.OnDying;
                Player.DroppingItem -= _cgEventHandlers.OnDroppingItem;
                Player.ChangingItem -= _cgEventHandlers.OnChangingItem;
                Player.Shooting -= _cgEventHandlers.OnShooting;
                Player.UnloadingWeapon -= _cgEventHandlers.OnUnloadingWeapon;
                Server.WaitingForPlayers -= _cgEventHandlers.OnWaitingForPlayers;
                _cgEventHandlers = null;
            }

            Instance = null;
            base.OnDisabled();
        }
    }

    public class Velocity
    {
        public float ForwardVelocity { get; set; }
        public float UpwardsVelocity { get; set; }
        public float RightVelocity { get; set; }

        public Velocity()
        {
            ForwardVelocity = 1;
            UpwardsVelocity = 1;
            RightVelocity = 1;
        }

        public Velocity(float fwd, float upw, float rgt)// Just for creating an instance to look better.
        {
            ForwardVelocity = fwd;
            UpwardsVelocity = upw;
            RightVelocity = rgt;
        } 

        public Vector3 ToVector3(Transform transform)
            => transform.forward * ForwardVelocity + transform.up * UpwardsVelocity + transform.right * RightVelocity;
    }
}
