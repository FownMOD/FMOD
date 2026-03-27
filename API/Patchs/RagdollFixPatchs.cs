using HarmonyLib;
using PlayerRoles.Ragdolls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.Patchs
{
    public class RagdollFixPatchs
    {
        [HarmonyPatch(typeof(BasicRagdoll), "Start")]
        public class RagdollStart
        {
            [HarmonyPostfix]
            public static void Postfix(BasicRagdoll basicRagdoll)
            {
                Ragdoll.Ragdolls.Add(new Ragdoll(basicRagdoll));
            }
        }
        [HarmonyPatch(typeof(BasicRagdoll), "Destroy")]
        public class RagdollDestroy
        {
            [HarmonyPostfix]
            public static void Postfix(BasicRagdoll basicRagdoll)
            {
                Ragdoll.Ragdolls.Remove(new Ragdoll(basicRagdoll));
            }
        }
    }
}
