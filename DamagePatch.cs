/*using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using NorthwoodLib.Pools;
using PlayerStatsSystem;

namespace SuicidePro
{
    [HarmonyPatch(typeof(CustomReasonDamageHandler), MethodType.Constructor, new[] { typeof(string), typeof(float), typeof(string) })]
    internal static class CustomReasonDamageHandlerFix
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            int index = newInstructions.FindLastIndex(instruction => instruction.opcode == OpCodes.Ldarg_0);

            newInstructions.Insert(index, new CodeInstruction(OpCodes.Ret));

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}*/