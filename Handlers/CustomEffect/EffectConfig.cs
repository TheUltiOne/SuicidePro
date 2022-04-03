using System.Collections.Generic;
using System.Linq;
using SuicidePro.Configuration;
using YamlDotNet.Serialization;

namespace SuicidePro.Handlers.CustomEffect
{
    public class EffectConfig : Config.CustomHandlerCommandConfig
    {
        [YamlIgnore]
        public string AssociatedId => Plugin.Instance.Config.CustomEffects.First(x => x.Value == this).Key;
        public bool Enabled { get; } = true;
    }
}