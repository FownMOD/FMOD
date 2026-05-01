using HarmonyLib;
using MapGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.Patchs
{
    public class RoomFixPatchs
    {
        [HarmonyPatch(typeof(RoomIdentifier), "Start")]
        public class RoomCrete
        {
            [HarmonyPostfix]
            public static void Postfix(RoomIdentifier room)
            {
                Room.List.Add(new Room(room));
            }
        }
        [HarmonyPatch(typeof(RoomIdentifier), "OnDestroy")]
        public class RoomDestroy
        {
            [HarmonyPostfix]
            public static void Postfix(RoomIdentifier room)
            {
                Room.List.Remove(new Room(room));
            }
        }
    }
}
