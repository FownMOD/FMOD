using FMOD.API;
using FMOD.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.EventArgs.Player
{
    public class PlayerPickingArgs : System.EventArgs, IFMODPlayerEvent
    {
        public PlayerPickingArgs(ReferenceHub player, API.Pickup pickup)
        {
            Pickup = pickup;
            Player = API.Player.Get(player);
            IsAllowed = true;
        }
        public API.Pickup Pickup { get; set; }
        public API.Player Player {  get; set; }
        public bool IsAllowed { get; set; }
    }
}
