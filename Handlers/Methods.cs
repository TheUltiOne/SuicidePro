using System.Linq;
using Exiled.API.Features;
using MEC;
using PlayerStatsSystem;
using SuicidePro.Configuration;
using SuicidePro.Handlers.CustomEffect;

namespace SuicidePro.Handlers
{
    public static class Methods
    {
        public static bool Run(this Config.BaseCommandConfig config, Player player)
        {
            switch (config)
            {
                case EffectConfig eCfg:
                    return eCfg.Run(player);
                case Config.CustomHandlerCommandConfig cCfg:
                    cCfg.Run(player);
                    return true;
            }

            return false;
        }

        public static bool Run(this EffectConfig config, Player player)
        {
            if (!config.Enabled)
                return false;

            CustomEffect.CustomEffect.Effects.First(x => x.Id == config.AssociatedId).Use(player);
            (config as Config.CustomHandlerCommandConfig).Run(player);
            return true;
        }

        public static void Run(this Config.CustomHandlerCommandConfig config, Player player)
        {
            Timing.CallDelayed(config.Delay, () =>
            {
                var handler = new CustomReasonDamageHandler(config.DamageHandler.Reason, float.MaxValue, config.DamageHandler.CassieIfScp);
                player.Hurt(handler);
                var velocity = player.ReferenceHub.playerMovementSync.PlayerVelocity + config.DamageHandler.Velocity.ToVector3(player.CameraTransform);
                Plugin.Instance.VelocityInfo.SetValue(handler, velocity);
            });
        }
    }
}