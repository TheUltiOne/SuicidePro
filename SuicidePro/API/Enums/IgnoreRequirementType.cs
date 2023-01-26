using System;

namespace SuicidePro.API.Enums
{
    [Flags]
    public enum IgnoreRequirementType
    {
        /// <summary>
        /// Does not force the <see cref="CustomEffect"/> to be registered and makes sure it meets all requirements.
        /// </summary>
        None = 0,

        /// <summary>
        /// Ignores <see cref="Handlers.CustomEffectHandlers.EffectConfig.Enabled"/> property.
        /// </summary>
        Enabled = 1,

        /// <summary>
        /// Ignores duplicate IDs.
        /// </summary>
        IdDuplicates = 2,

        /// <summary>
        /// Ignores already registered <see cref="CustomEffect"/>s.
        /// </summary>
        Duplicates = 4
    }
}