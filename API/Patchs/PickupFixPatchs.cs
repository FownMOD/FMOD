using HarmonyLib;
using InventorySystem.Items.Pickups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.Patchs
{
    public class PickupFixPatchs
    {
        [HarmonyPatch(typeof(ItemPickupBase), "Start")]
        public class PickupCreate
        {
            [HarmonyPostfix]
            public static void Postfix(ItemPickupBase resula)
            {
                Pickup.List.Add(new Pickup(resula));
            }
        }
        [HarmonyPatch(typeof(ItemPickupBase), "OnDestroy")]
        public class PickupDestroy
        {
            [HarmonyPostfix]
            public static void Postfix(ItemPickupBase resula)
            {
                Pickup.List.Remove(new Pickup(resula));
            }
        }
    }
}
