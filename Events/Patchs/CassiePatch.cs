using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cassie;
using HarmonyLib;

namespace FMOD.Events.Patchs
{
    public class CassiePatch
    {
        [HarmonyPatch(typeof(CassieTtsPayload), "PlaySubtitleMessage")]
        public class CPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(CassieTtsPayload __instance)
            {
                Events.EventArgs.CASSIE.StartingArgs args = new Events.EventArgs.CASSIE.StartingArgs(__instance.Content);
                Events.Handlers.CASSIE.StartingInvoke(args);
                if(args.IsAllow == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
