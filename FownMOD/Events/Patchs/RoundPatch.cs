using FMOD.API;
using GameCore;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.Patchs
{
    public class RoundPatch
    {
        [HarmonyPatch(typeof(RoundStart), "Awake")]
        public class RoundStartFix
        {
            [HarmonyPostfix]
            public void Postfix()
            {
                Handlers.Round.RoundStartInvoke(new EventArgs.Round.RoundStartArgs(Player.List.Count));
            }
        }
        [HarmonyPatch(typeof(RoundSummary), "Start")]
        public class RoundEndFix
        {
            [HarmonyPostfix]
            public void Postfix()
            {
                Handlers.Round.RoundEndInvoke(new EventArgs.Round.RoundEndArgs());
            }
        }
    }
}
