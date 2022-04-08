using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;
using InventorySystem.Items;
using MEC;
using PlayerStatsSystem;

namespace SuicidePro.ContentGun
{
    public class Handler
    {
        public static readonly List<ItemBase> ContentGuns = new List<ItemBase>();
        public static readonly List<ContentGunCooldown> Cooldowns = new List<ContentGunCooldown>();
        public List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();

        public void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (ContentGuns.Contains(ev.Item.Base))
                ev.IsAllowed = false;
        }

        public void OnDying(DyingEventArgs ev)
        {
            if (ev.ItemsToDrop.IsEmpty() || ev.ItemsToDrop == null)
                return;

            var tempItems = ev.ItemsToDrop;
            foreach (Item item in ev.ItemsToDrop)
                if (ContentGuns.Contains(item.Base))
                    tempItems.Remove(item);
            ev.ItemsToDrop = tempItems;
        }

        public void OnChangingItem(ChangingItemEventArgs ev)
        {
            if (ev.NewItem == null)
                return;

            if (ContentGuns.Contains(ev.NewItem.Base))
                ev.Player.ShowHint($"You are <b>now selecting</b> your <b><color=red>Content Gun</color></b>\nYou have <b><color=red>{Cooldowns.Find(x => x.UserId == ev.Player.UserId).UsesLeft}</color> uses left</b>.");
        }

        public void OnShooting(ShootingEventArgs ev)
        {
            try
            {
                var item = ev.Shooter.CurrentItem;
                if (!ContentGuns.Contains(item.Base))
                    return;
                
                ev.IsAllowed = false;
                var cooldown = Cooldowns.First(x => x.UserId == ev.Shooter.UserId);
                cooldown.UsesLeft--;

                var handler = new CustomReasonDamageHandler(Plugin.Instance.Config.ContentGunConfig.DeathCause, float.MaxValue);
                Plugin.Instance.VelocityInfo.SetValue(handler, Plugin.Instance.Config.ContentGunConfig.Velocity.ToVector3(ev.Shooter.CameraTransform));

                var ragdoll = new Exiled.API.Features.Ragdoll(new RagdollInfo(Server.Host.ReferenceHub, handler, Plugin.Instance.Config.ContentGunConfig.RagdollRoleType, ev.Shooter.Position, ev.Shooter.CameraTransform.rotation, Plugin.Instance.Config.ContentGunConfig.RagdollName, 1.0))
                    {
                        Scale = Plugin.Instance.Config.ContentGunConfig.Scale
                    };

                ragdoll.Spawn();
                Coroutines.Add(Timing.RunCoroutine(CleanupRagdoll(ragdoll)));
                if (cooldown.UsesLeft <= 0)
                {
                    ev.Shooter.ShowHint("Your <b>Content Gun</b> has <b><color=red>no more uses</color></b>.\nPlease wait <b>3m45s</b> before using <b><color=red>the command again</color></b>.");
                    cooldown.DeletedAt = DateTime.Now;
                    cooldown.UsesLeft = Plugin.Instance.Config.ContentGunConfig.Uses;
                    ContentGuns.Remove(item.Base);
                    ev.Shooter.RemoveHeldItem();
                }
                else
                {
                    ev.Shooter.ShowHint($"You have <b><color=red>{cooldown.UsesLeft}</color> uses remaining</b> on your <b><color=red>Content Gun</color></b>");
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public IEnumerator<float> CleanupRagdoll(Exiled.API.Features.Ragdoll ragdoll)
        {
            yield return Timing.WaitForSeconds(Plugin.Instance.Config.ContentGunConfig.CleanupTime);
            ragdoll.UnSpawn();
        }

        public void OnUnloadingWeapon(UnloadingWeaponEventArgs ev)
        {
            if (ContentGuns.Contains(ev.Firearm.Base))
                ev.IsAllowed = false;
        }

        public void OnWaitingForPlayers()
        {
            Cooldowns.Clear();
            ContentGuns.Clear();
            Timing.KillCoroutines(Coroutines.ToArray());
            Coroutines.Clear();
        }
    }

    public class ContentGunCooldown
    {
        public string UserId { get; set; } // UserId to prevent disconnecting to get uses back.
        public int UsesLeft { get; set; } = Plugin.Instance.Config.ContentGunConfig.Uses;
        public DateTime DeletedAt { get; set; }

        public ContentGunCooldown(string userId)
        {
            UserId = userId;
            DeletedAt = DateTime.Now;
        }
    }
}