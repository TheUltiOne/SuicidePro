using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using SuicidePro.Handlers;
using SuicidePro.Handlers.CustomEffect;
using YamlDotNet.Serialization;

namespace SuicidePro.API
{
    public class CustomEffect
    {
        /// <summary>
        /// A list of all <see cref="CustomEffect"/>.
        /// </summary>
        /// <remarks>This does not exactly contain all <see cref="CustomEffect"/>. It will only do so if developers who add their <see cref="CustomEffect"/> use <see cref="Register"/> on their effect.</remarks>
        public static readonly List<CustomEffect> Effects = new List<CustomEffect>();

        /// <summary>
        /// The ID of the CustomEffect. This is absolutely required to change.
        /// </summary>
        [YamlIgnore]
        public virtual string Id { get; } = "default";

        /// <summary>
        /// A property used to see the <see cref="CustomEffect"/> config.
        /// </summary>
        /// <remarks>This will not be automatically added to any config file.
        /// To add it to a config, just create an instance inside of your config and use the <see cref="Register"/> method in your <see cref="Plugin{TConfig}.OnEnabled"/>.
        /// </remarks>
        public EffectConfig Config { get; set; } = new EffectConfig();

        /// <summary>
        /// The method that will be ran once this kill command is used.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> that ran the command.</param>
        /// <remarks>The command by itself already handles checking if the command matches requirements set in config. You do not have to check for anything.</remarks>
        public virtual void Use(Player player)
        {
        }

        /// <summary>
        /// Runs this <see cref="CustomEffect"/> on a specific <see cref="Player"/> (<paramref name="player"/>).
        /// </summary>
        /// <param name="player">The <see cref="Player"/> that the this <see cref="CustomEffect"/> will be run on.</param>
        /// <returns>A <see cref="bool"/> that indicates whether this <see cref="CustomEffect"/> should be allowed to run.</returns>
        public bool Run(Player player)
        {
            if (!Config.Enabled)
                return false;

            Use(player);
            Config.Run(player);
            return true;
        }

        /// <summary>
        /// A method used to properly add a <see cref="CustomEffect"/> with checks for it to be usable with with the kill command.
        /// </summary>
        public void Register()
        {
            if (Effects.Contains(this))
            {
                Log.Warn($"{this} attempted to register but failed: already registered.");
                return;
            }

            if (Effects.Any(x => x.Id == Id))
            {
                Log.Warn($"{this} attempted to register but failed: another effect already uses this ID.");
                return;
            }

            Effects.Add(this);
            Log.Debug($"{this} registered successfully!", Plugin.Instance.Config.Debug);
        }

        public static CustomEffect Get(string id)
            => Effects.First(x => x.Id == id);

        public override string ToString()
            => $"{Config.Name} (ID {Id})";

        public CustomEffect()
        {
        }
    }
}