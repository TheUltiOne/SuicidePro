using System.Linq;
using Exiled.API.Features;
using JetBrains.Annotations;
using MEC;
using PlayerStatsSystem;
using SuicidePro.Configuration;
using SuicidePro.Handlers.CustomEffectHandlers;

namespace SuicidePro.Handlers
{
    public static class Methods
    {
        /// <summary>
        /// Runs a <see cref="Config.CustomHandlerCommandConfig"/> on a specific <see cref="Player"/>.
        /// </summary>
        /// <param name="config">The <see cref="Config.CustomHandlerCommandConfig"/> associated with the <see cref="CustomEffect"/> or default config to run.</param>
        /// <param name="player">The <see cref="Player"/> that the <paramref name="config"/> will be run on.</param>
        public static void Run(this Config.CustomHandlerCommandConfig config, Player player)
        {
            Timing.CallDelayed(config.Delay, () =>
            {
                if (config is EffectConfig eCfg && eCfg.IgnoreDamageHandlerConfigs)
                    return;

                var handler = new CustomReasonDamageHandler(config.DamageHandler.Reason, float.MaxValue, config.DamageHandler.CassieIfScp);
                player.Hurt(handler);
                var velocity = player.ReferenceHub.playerMovementSync.PlayerVelocity + config.DamageHandler.Velocity.ToVector3(player.CameraTransform);
                Plugin.Instance.VelocityInfo.SetValue(handler, velocity);
            });
        }

        
        // Archived methods
        /*
        /// <summary>
        /// Runs an <see cref="Config.BaseCommandConfig"/> (more specifically <see cref="EffectConfig"/> or <see cref="Config.CustomHandlerCommandConfig"/>) on a specific <see cref="Player"/>.
        /// </summary>
        /// <param name="config">The <see cref="Config.BaseCommandConfig"/> associated with the <see cref="CustomEffect"/> or default effect to run.</param>
        /// <param name="player">The <see cref="Player"/> that the <paramref name="config"/> will be run on.</param>
        /// <returns>A <see cref="bool"/> that indicates whether the <see cref="CustomEffect"/> should be allowed to run.</returns>
        /// <remarks>This method will always return true if <paramref name="config"/> is not an <see cref="EffectConfig"/>.</remarks>
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
        }*/
    }
}