using FMOD.API.CustHint;
using HarmonyLib;
using Hints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.Patchs
{
    [HarmonyPatch(typeof(HintDisplay))]
    public static class HintDisplayPatch
    {
        private static readonly HashSet<uint> ABHintIDs = new HashSet<uint>();
        public static void RegisterABHintID(uint id)
        {
            ABHintIDs.Add(id);
        }

        public static void UnregisterABHintID(uint id)
        {
            ABHintIDs.Remove(id);
        }

        public static bool IsABHint(Hint hint)
        {
            if (hint is TextHint textHint)
            {
                try
                {
                    var parameters = AccessTools.Field(typeof(Hint), "Parameters")?.GetValue(hint) as HintParameter[];
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            if (param is PositionHintParameter)
                            {
                                return true;
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            return false;
        }
        [HarmonyPatch("Show")]
        [HarmonyPrefix]
        public static bool Show(Hint hint)
        {
            if (IsABHint(hint))
            {
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(HintMessage), MethodType.Constructor, new Type[] { typeof(Hint) })]
        [HarmonyPostfix]
        public static void OnHintMessageCreated(HintMessage __instance)
        {
            if (IsABHint(__instance.Content))
            {
                return;
            }
        }
    }

}
