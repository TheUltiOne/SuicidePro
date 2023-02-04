using Exiled.API.Interfaces;
using SuicidePro.Addons.Effects;
using SuicidePro.Handlers.CustomEffectHandlers.Effects;
using System.ComponentModel;

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

        [Description("Configuration for the builtin Explode effect.")]
        public Explode ExplodeEffect { get; set; } = new Explode
        {
            Aliases = new[] { "boom" },
            Name = "explode",
            Description = "Explode! (Does not deal damage or break doors)",
            Response = "Boom!",
        };

        [Description("Configuration for the builtin Disintegrate effect.")]
        public Disintegrate DisintegrateEffect { get; set; } = new Disintegrate
        {
            Name = "disintegrate",
            Description = "Disintegrate yourself.",
            Response = "death"
        };


        public GunSuicideEffect GunSuicideEffect { get; set; } = new()
        {
            Name = "gun",
            Description = "Kills you with a little gun animation",
            Response = "Getready",
        };
    }
}
