using FMOD.API;
using FMOD.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.EventArgs.Player
{
    public class ChangingItemArgs : System.EventArgs, IFMODPlayerEvent
    {
        public ChangingItemArgs(ReferenceHub player, Item item)
        {
            NewItem = item;
            Player = API.Player.Get(player);
            IsAllowed = true;
        }
        public Item NewItem { get; set; }
        public API.Player Player {  get; set; }
        public bool IsAllowed {  get; set; }
    }
}
