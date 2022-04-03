using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Permissions.Extensions;
using Mirror;

namespace SuicidePro.ContentGun
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ContentGunCommand : ICommand
    {
        public string Command { get; } = "contentgun";
        public string[] Aliases { get; } = {"cg", "givecontentgun", "givecontent", "cgun", "givecg", "givecgun"};
        public string Description { get; } = "Gives you the content gun.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Plugin.Instance.Config.ContentGunConfig.Enabled)
            {
                response = "The Content Gun is not enabled.";
                return false;
            }

            var player = Player.Get(sender);
            if (player == null)
            {
                response = "You cannot use this command as the server.";
                return false;
            }

            if (!player.CheckPermission("cg.give"))
            {
                response = "You do not have the permission to use the content gun.";
                return false;
            }

            if (TryFind(Handler.Cooldowns, x => x.UserId == player.UserId, out var cooldown))
            {
                var value = (DateTime.Now - cooldown.DeletedAt).TotalSeconds;
                if (value <= Plugin.Instance.Config.ContentGunConfig.Cooldown && cooldown.UsesLeft <= 0)
                {
                    player.ShowHint($"There are <b><color=red>{value}</color>s</b> before you can use the <b><color=red>Content Gun</color> again</b>.");
                    response = $"You must wait {value}s before being able to get the content gun again.";
                    return false;
                }
            }

            foreach (var item in player.Items)
            {
                if (Handler.ContentGuns.Contains(item.Base))
                {
                    player.ShowHint("You <b>already</b> have a <b><color=red>Content Gun</color></b>.");
                    response = "You already have a content gun.";
                    return false;
                }
            }

            if (cooldown == null)
                Handler.Cooldowns.Add(new ContentGunCooldown(player.UserId));

            var cg = player.AddItem(Plugin.Instance.Config.ContentGunConfig.GunItemType) as Firearm;
            Handler.ContentGuns.Add(cg.Base);
            cg.Ammo = (byte)Plugin.Instance.Config.ContentGunConfig.Uses;

            player.ShowHint("You <b>now</b> have a <b><color=red>Content Gun</color></b>.");
            response = "Gave you a content gun!";
            return true;
        }

        public bool TryFind<T>(IEnumerable<T> enumerable, Func<T, bool> func, out T item)
        {
            item = enumerable.FirstOrDefault(func);
            if (item == null)
                return false;

            return true;
        }
    }
}