using System.ComponentModel;
using SuicidePro.Configuration;

namespace SuicidePro.Handlers.CustomEffectHandlers
{
    public class EffectConfig : Config.CustomHandlerCommandConfig
    {
        /// <summary>
        ///  A config item that sets whether the Kill command can be used or not.
        /// </summary>
        [Description("Sets if this kill command is usable or not.")]
        public bool Enabled { get; set; } = true;
    }
}