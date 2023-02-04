using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using MEC;
using UnityEngine;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;
using SuicidePro.Handlers.ContentGun;

namespace SuicidePro
{
    public class Plugin : Plugin<Config>
    {
        public override string Author { get; } = "warden161";
        public override string Name { get; } = "Suicide - Pro Edition";
        public override Version Version { get; } = new Version(3, 1, 0);
        public override Version RequiredExiledVersion { get; } = new Version(6, 0, 0);

        public static Plugin Instance;
        public List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
        private Handler _cgEventHandlers;

        public override void OnEnabled()
        {
            Instance = this;

            Server.RoundEnded += OnRoundEnded;

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

            Server.RoundEnded -= OnRoundEnded;

            Instance = null;
            base.OnDisabled();
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            Timing.KillCoroutines(Coroutines.ToArray());
            Coroutines.Clear();
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
