using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using YamlDotNet.Serialization;

namespace SuicidePro.Handlers.CustomEffect
{
    public abstract class CustomEffect
    {
        public static List<Type> GetAllEffects()
            => Plugin.Instance.Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(CustomEffect)) && x != typeof(CustomEffect)).ToList();

        public static void CreateEffectInstances()
        {
            foreach (Type type in GetAllEffects())
                Effects.Add(Activator.CreateInstance(type) as CustomEffect);
        }

        public static List<CustomEffect> Effects = new List<CustomEffect>();

        [YamlIgnore] // i plan to remove effectconfig from the config.cs and just create instance directly in config in a list which is why I have these YamlIgnores
        public abstract string Id { get; }
        [YamlIgnore]
        public EffectConfig Config => Plugin.Instance.Config.CustomEffects[Id];

        public abstract void Use(Player player);
    }
}