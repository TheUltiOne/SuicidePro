using System.Collections.Generic;
using Exiled.API.Features;
using MEC;
using PlayerStatsSystem;
using SuicidePro.Configuration;
using SuicidePro.Handlers.CustomEffectHandlers;

namespace SuicidePro.Handlers
{
    public static class Methods
    {
        /// <summary>
        /// Runs a <see cref="Config.BaseCommandConfig"/> on a specific <see cref="Player"/>.
        /// </summary>
        /// <param name="config">The <see cref="Config.BaseCommandConfig"/> associated with the <see cref="API.CustomEffect"/> or default config to run.</param>
        /// <param name="player">The <see cref="Player"/> that the <paramref name="config"/> will be run on.</param>
        public static void Run(this Config.BaseCommandConfig config, Player player)
        {
            Plugin.Instance.Coroutines.Add(Timing.RunCoroutine(_runConfig(config, player)));
        }

        /// <summary>
        /// Runs a series of commands as the server.
        /// </summary>
        /// <param name="commands">The commands to run.</param>
        /// <param name="player">The player to use (for variables)</param>
        public static void RunCommands(this List<string> commands, Player player = null)
        {
            foreach (var cmd in commands)
                GameCore.Console.singleton.TypeCommand(cmd.Replace("%playerid%", player.Id.ToString()));
        }

        /// <summary>
        /// Internal coroutine that <see cref="Run"/> wraps.
        /// </summary>
        private static IEnumerator<float> _runConfig(Config.BaseCommandConfig config, Player player)
        {
            config.PreDelayCommands.RunCommands(player);
            yield return Timing.WaitForSeconds(config.Delay);

            switch (config)
            {
                case EffectConfig eCfg when eCfg.IgnoreDamageHandlerConfigs:
                    yield break;
                case Config.DamageHandlerCommandConfig dCfg:
                    player.Hurt(-1, dCfg.DamageType);
                    break;
                case Config.CustomHandlerCommandConfig cCfg:
                    var handler = new CustomReasonDamageHandler(cCfg.DamageHandler.Reason, float.MaxValue, cCfg.DamageHandler.CassieIfScp);
                    player.Hurt(handler);
                    handler.StartVelocity = player.Velocity + cCfg.DamageHandler.Velocity.ToVector3(player.CameraTransform);
                    break;
            }

            config.Commands.RunCommands(player);
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