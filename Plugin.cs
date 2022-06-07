using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.Loader;
using MEC;
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
        public override Version Version { get; } = new Version(2, 2, 1);
        public override Version RequiredExiledVersion { get; } = new Version(5, 2, 1);

        public const string LiteDBAssemblyName = "LiteDB";
        public static Plugin Instance;
        public List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
        private Handler _cgEventHandlers;

        public override void OnEnabled()
        {
            /*if (Config.AllowSelectingDeathEffect)
            {
                if (Loader.Dependencies.All(x => x.GetName().Name != LiteDBAssemblyName))
                {
                    throw new DllNotFoundException(
                        "Could not find LiteDB dependency installed!\nTo prevent this error from occuring, please either install LiteDB from the GitHub release OR disable Selecting Kill Effect!");
                }
            }*/

            Instance = this;
            Config.ExplodeEffect.Register();

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
