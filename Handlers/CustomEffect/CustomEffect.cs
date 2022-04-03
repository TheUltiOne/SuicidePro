using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Exiled.API.Features;
using SuicidePro.Configuration;

namespace SuicidePro.Handlers.CustomEffect
{
    public abstract class CustomEffect
    {
        public static List<Type> GetAllEffects()
            => Plugin.Instance.Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(CustomEffect)) && x != typeof(CustomEffect)).ToList();

        public static void EffectInstanceFactory()
        {
            foreach (Type type in GetAllEffects())
                Effects.Add(Activator.CreateInstance(type) as CustomEffect);
        }

        public static List<CustomEffect> Effects = new List<CustomEffect>();

        public abstract string Id { get; set; }
        public EffectConfig Config => Plugin.Instance.Config.CustomEffects[Id];

        public abstract void Use(Player player);
    }
}