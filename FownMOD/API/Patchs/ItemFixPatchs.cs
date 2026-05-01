using FMOD.API.Items;
using HarmonyLib;
using InventorySystem.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.Patchs
{
    public class ItemFixPatchs
    {
        [HarmonyPatch(typeof(ItemBase), "Start")]
        public class ItemCreate
        {
            [HarmonyPostfix]
            public void Postfix(ItemBase @base) 
            {
                Item.List.Add(new Item(@base));
            }
        }
        [HarmonyPatch(typeof(ItemBase), "Destroy")]
        public class ItemDestroy
        {
            [HarmonyPostfix]
            public void Postfix(ItemBase @base)
            {
                Item.List.Remove(new Item(@base));
            }
        }
    }
}
