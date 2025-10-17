using FMOD.Events.Interfaces;
using InventorySystem.Items.Pickups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.EventArgs.Pickup
{
    public class Desroy:IFMODEvent
    {
        public Desroy(ItemPickupBase pickupBase)
        {
            this.Pickup = API.Pickup.Get(pickupBase);
        }
        public API.Pickup Pickup { get; set; }

    }
}
