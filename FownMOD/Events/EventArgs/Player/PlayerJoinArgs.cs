using FMOD.API;
using FMOD.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.EventArgs.Player
{
    public class PlayerJoinArgs: System.EventArgs, IPlayerEvent
    {
        public PlayerJoinArgs(ReferenceHub player) 
        {
            Player = API.Player.Get(player);
        }
        public API.Player Player {  get; set; }
    }
}
