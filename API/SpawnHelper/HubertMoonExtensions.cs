using HarmonyLib;
using MapGeneration.Holidays;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.SpawnHelper
{
    public static class HubertMoonExtensions
    {
        private static readonly MethodInfo ServerExplodeFacilityMethod =
        AccessTools.Method(typeof(HubertMoon), "ServerExplodeFacility");

        private static readonly MethodInfo ServerExplodePlayersMethod =
            AccessTools.Method(typeof(HubertMoon), "ServerExplodePlayers");

        private static readonly FieldInfo EndingSequenceTimerField =
            AccessTools.Field(typeof(HubertMoon), "_endingSequenceTimer");

        private static readonly FieldInfo ElapsedMovementTimeField =
            AccessTools.Field(typeof(HubertMoon), "_elapsedMovementTime");

        private static readonly FieldInfo CachedTransformField =
            AccessTools.Field(typeof(HubertMoon), "_cachedTransform");

        public static void TriggerFullExplosion()
        {
            if (!NetworkServer.active)
            {
                return;
            }
            HubertMoon moon = UnityEngine.Object.FindAnyObjectByType<HubertMoon>();
            ServerExplodeFacilityMethod?.Invoke(moon,null);
        }
    }
}
