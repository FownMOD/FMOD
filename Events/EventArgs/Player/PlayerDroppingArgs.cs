using FMOD.API;
using FMOD.API.Items;
using FMOD.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.EventArgs.Player
{
    public class PlayerDroppingArgs: System.EventArgs, IFMODPlayerEvent
    {
        public PlayerDroppingArgs(ReferenceHub player, Item item)
        {
            Item = item;
            Player = API.Player.Get(player);
            IsAllowed = true;
        }
        public Item Item { get; set; }
        public API.Player Player { get; set; }
        public bool IsAllowed { get; set; }
    }
}
