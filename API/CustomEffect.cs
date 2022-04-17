using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using SuicidePro.API.Enums;
using SuicidePro.Handlers;
using SuicidePro.Handlers.CustomEffectHandlers;
using YamlDotNet.Serialization;

namespace SuicidePro.API
{
    public class CustomEffect
    {
        /// <summary>
        /// A <see cref="HashSet{T}"/> of all <see cref="CustomEffect"/>.
        /// </summary>
        /// <remarks>This does not exactly contain all <see cref="CustomEffect"/>. It will only do so if developers who add their <see cref="CustomEffect"/> use <see cref="Register"/> on their effect.</remarks>
        public static readonly HashSet<CustomEffect> Effects = new HashSet<CustomEffect>();

        /// <summary>
        /// The ID of the CustomEffect. This is absolutely required to override.
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
        /// <param name="force">A <see cref="IgnoreRequirementType"/> flag that dictates what requirements will be ignored. Defaults to None.</param>
        public void Register(IgnoreRequirementType force = IgnoreRequirementType.None)
        {
            if (Effects.Contains(this) && !force.HasFlag(IgnoreRequirementType.Duplicates))
            {
                Log.Warn($"{this} attempted to register but failed: already registered.");
                return;
            }

            if (Effects.Any(x => x.Id == Id) && !force.HasFlag(IgnoreRequirementType.IdDuplicates))
            {
                Log.Warn($"{this} attempted to register but failed: another effect already uses this ID.");
                return;
            }

            if (!Config.Enabled && !force.HasFlag(IgnoreRequirementType.Enabled))
            {
                Log.Warn($"{this} attempted to register but failed: already registered.");
                return;
            }

            Effects.Add(this);
            Log.Debug($"{this} registered successfully!", Plugin.Instance.Config.Debug);
        }

        /// <summary>
        /// Gets a <see cref="Register"/>ed <see cref="CustomEffect"/> from it's <see cref="Id"/>.
        /// </summary>
        /// <param name="id">The <see cref="Id"/> of the <see cref="CustomEffect"/> to register.</param>
        /// <returns>The obtained <see cref="CustomEffect"/>. May be null if none found.</returns>
        public static CustomEffect Get(string id)
            => Effects.FirstOrDefault(x => x.Id == id);

        /// <summary>
        /// Gets a <see cref="Register"/>ed <see cref="CustomEffect"/> from it's <see cref="Id"/>.
        /// </summary>
        /// <param name="id">The <see cref="Id"/> of the <see cref="CustomEffect"/> to register.</param>
        /// <param name="effect">The value of the <see cref="CustomEffect"/> obtained. May be null if none found.</param>
        /// <returns>A <see cref="bool"/> dictating whether the <see cref="CustomEffect"/> was found.</returns>
        public static bool TryGet(string id, out CustomEffect effect)
        {
            effect = Get(id);
            return effect != null;
        }

        public override string ToString()
            => $"{Config.Name} (ID {Id})";

        // For config.
        public CustomEffect()
        {
        }
    }
}