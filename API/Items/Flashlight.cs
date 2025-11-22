using InventorySystem.Items;
using InventorySystem.Items.ToggleableLights.Flashlight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.Items
{
    public class Flashlight:Item
    {
        public Flashlight(FlashlightItem flashlightItem)
        {
            this.Base = flashlightItem;
        }
        public new FlashlightItem Base;
        public ItemVariantsModule ItemVariantsModule
        {
            get => Base.VariantsModule;
        }
    }
}
