using FMOD.API;
using FMOD.Events.Interfaces;
using InventorySystem.Items.Pickups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.EventArgs.Pickup
{
    public class Create:IFMODEvent
    {
        public Create(ItemPickupBase pickupBase)
        {
            this.Pickup = API.Pickup.Get(pickupBase);
        }
        public API.Pickup Pickup { get; set; }
    }
}
