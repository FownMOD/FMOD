using FMOD.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.EventArgs.Player
{
    public class PlayerLeftArgs : System.EventArgs, Interfaces.IFMODPlayerEvent
    {
        public PlayerLeftArgs(ReferenceHub player)
        {
            Player = API.Player.Get(player);
            IsAllowed = true;
        }
        public API.Player Player {  get; set; }
        public bool IsAllowed {  get; set; }
    }
}
