using System.Collections.Generic;
using Exiled.API.Features;
using MEC;
using PlayerStatsSystem;
using SuicidePro.Handlers.CustomEffectHandlers;
using SuicidePro;
using SuicidePro.API;

namespace SuicidePro
{
    public static class Methods
    {
        /// <summary>
        /// Runs a <see cref="BaseEffect"/> on a specific <see cref="Player"/>.
        /// </summary>
        /// <param name="effect">The <see cref="BaseEffect"/> to run.</param>
        /// <param name="player">The <see cref="Player"/> that the <paramref name="effect"/> will be run on.</param>
        public static void Run(this BaseEffect effect, Player player, string[] args)
        {
            Plugin.Instance.Coroutines.Add(Timing.RunCoroutine(_run(effect, player, args)));
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
        private static IEnumerator<float> _run(BaseEffect effect, Player player, string[] args)
        {
            effect.PreDelayCommands.RunCommands(player);
            yield return Timing.WaitForSeconds(effect.Delay);
            effect.Execute(player, args);
            effect.Commands.RunCommands(player);
        }
    }
}
