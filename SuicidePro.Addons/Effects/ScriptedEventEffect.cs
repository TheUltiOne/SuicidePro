using Exiled.API.Features;
using SuicidePro.API.Features;
using System.Collections.Generic;
using ScriptedEvents.API.Helpers;
using System.ComponentModel;

namespace SuicidePro.Addons.Effects
{
    public class ScriptedEventEffect : CustomEffect
    {
        /// <inheritdoc/>
        public override string Id { get; } = nameof(ScriptedEventEffect).ToLower();

        [Description("List of scripts to run. Inside script, you can use %playerid% to replace with suicided player id.")]
        public List<string> Scripts { get; set; } = new();

        /// <inheritdoc/>
        public override void Use(Player player, string[] args)
        {
            foreach (var scriptText in Scripts)
            {
                var script = ScriptHelper.ReadScript(scriptText);
                foreach (var action in script.Actions)
                {
                    foreach (var argument in action.Arguments)
                        argument.Replace("%playerid%", player.Id.ToString());
                }

                ScriptHelper.RunScript(script);
            }
        }
    }
}
