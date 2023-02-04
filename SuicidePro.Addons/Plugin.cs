using System;
using SuicidePro.API.Enums;

namespace SuicidePro.Addons
{
    public class Plugin : Exiled.API.Features.Plugin<Config>
    {
        public override string Name { get; } = "SuicidePro.Addons";
        public override string Author { get; } = "warden161";
        public override Version Version { get; } = new Version(0, 1, 0);
        public override Version RequiredExiledVersion { get; } = new Version(6, 0, 0);

        public static Plugin Instance { get; private set; }

        public override void OnEnabled()
        {
            /*foreach (var script in Config.ScriptedEventEffects)
                script.Register(IgnoreRequirementType.IdDuplicates | IgnoreRequirementType.Duplicates);
            */
            Instance = this;

            Config.GunSuicideEffect.Register();
            Config.ExplodeEffect.Register();
            Config.DisintegrateEffect.Register();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            base.OnDisabled();
        }
    }
}
