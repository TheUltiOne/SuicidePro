using System.ComponentModel;
using System.Linq;
using SuicidePro.Configuration;
using YamlDotNet.Serialization;

namespace SuicidePro.Handlers.CustomEffect
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