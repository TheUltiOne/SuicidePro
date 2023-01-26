using Exiled.API.Interfaces;
using SuicidePro.Addons.Effects;
using System.Collections.Generic;

namespace SuicidePro.Addons
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; }
        /*public List<ScriptedEventEffect> ScriptedEventEffects { get; set; } = new()
        {
            new() { Scripts = new() {"DemoScript"} }
        };*/
    }
}
