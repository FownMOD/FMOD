using HarmonyLib;
using Interactables.Interobjects.DoorUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.Patchs
{
    public class DoorFixPatch
    {
        [HarmonyPatch(typeof(DoorVariant), "Start")]
        public class DoorCreate
        {
            [HarmonyPostfix]
            public static void Postfix(DoorVariant doorVariant)
            {
                Door.Doors.Add(new Door(doorVariant));
            }
        }
        [HarmonyPatch(typeof(DoorVariant), "OnDestroy")]
        public class DoorDestroyed
        {
            [HarmonyPostfix]
            public static void Postfix(DoorVariant doorVariant)
            {
                Door.Doors.Remove(new Door(doorVariant));
            }
        }
    }
}
