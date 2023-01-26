using Exiled.API.Features;
using SuicidePro.API.Enums;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Serialization;

namespace SuicidePro.API.Features
{
    public abstract class CustomEffect : BaseEffect
    {
        /// <summary>
        /// A <see cref="HashSet{T}"/> of all <see cref="CustomEffect"/>.
        /// </summary>
        /// <remarks>This does not exactly contain all <see cref="CustomEffect"/>. It will only do so if developers who add their <see cref="CustomEffect"/> use <see cref="Register"/> on their effect.</remarks>
        public static readonly HashSet<CustomEffect> Effects = new();

        /// <summary>
        /// The ID of the CustomEffect.
        /// </summary>
        [YamlIgnore]
        public abstract string Id { get; }

        /// <summary>
        /// The method that will be ran once this kill command is used.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> that ran the command.</param>
        /// <param name="args">The arguments used when the Kill Command was used.</param>
        /// <remarks>The command by itself already handles checking if the command matches requirements set in config. You do not have to check for anything.</remarks>
        public abstract void Use(Player player, string[] args);

        /// <inheritdoc/>
        public override void Execute(Player player, string[] args)
            => Use(player, args);

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

            if (!Enabled && !force.HasFlag(IgnoreRequirementType.Enabled))
            {
                Log.Warn($"{this} attempted to register but failed: already registered.");
                return;
            }

            Effects.Add(this);
            Log.Debug($"{this} registered successfully!");
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

        /// <inheritdoc/>
        public override string ToString()
            => $"{Name} (ID {Id})";

        // For config to not throw an exception. (requires a default constructor with no parameters)
        public CustomEffect()
        {
        }
    }
}
