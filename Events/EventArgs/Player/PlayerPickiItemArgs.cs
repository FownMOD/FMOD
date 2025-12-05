using FMOD.API;
using FMOD.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.EventArgs.Player
{
    public class PlayerPickiItemArgs : System.EventArgs, IFMODPlayerEvent
    {
        public PlayerPickiItemArgs(ReferenceHub player, API.Pickup Pickup)
        {
            Player = API.Player.Get(player);
            this.Pickup = Pickup;
            IsAllowed = true;
        }
        public API.Pickup Pickup { get; }
        public API.Player Player {  get;}
        public bool IsAllowed { get; set; }
    }
}
