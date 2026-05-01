using FMOD.API.Roles;
using HarmonyLib;
using NorthwoodLib.Pools;
using PlayerRoles.FirstPersonControl.NetworkMessages;
using PlayerRoles.Visibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.Patchs
{
    [HarmonyPatch(typeof(FpcServerPositionDistributor), nameof(FpcServerPositionDistributor.WriteAll))]
    internal class PlayerVisibilityPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            const int offset = 6;
            int index = newInstructions.FindIndex(
                instruction => instruction.Calls(AccessTools.Method(typeof(VisibilityController), nameof(VisibilityController.ValidateVisibility)))) + offset;

            newInstructions.InsertRange(
                index,
                new CodeInstruction[]
                {
                    // referenceHub (target)
                    new CodeInstruction(OpCodes.Ldloc_S, 5),

                    // flag2 (isInvisible reference)
                    new CodeInstruction(OpCodes.Ldloca_S, 7),

                    // HandleGlobalInvisibility(ReferenceHub, ref bool)
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PlayerVisibilityPatch), nameof(HandleGlobalInvisibility), new[] { typeof(ReferenceHub), typeof(bool).MakeByRefType() })),
                });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
        private static void HandleGlobalInvisibility(ReferenceHub hubTarget, ref bool isInvisible)
        {
            if (isInvisible)
                return;

            if (!(Player.Get(hubTarget) is Player target))
                return;
            if (target.Invisible ==true||FpcRole.VisibilityList.Contains(target))
            {
                isInvisible = true;
            }
        }
    }
}

