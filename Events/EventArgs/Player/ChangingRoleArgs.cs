using FMOD.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.EventArgs.Player
{
    public class ChangingRoleArgs: System.EventArgs, IFMODPlayerEvent
    {
        public ChangingRoleArgs(ReferenceHub player)
        {
            Player = API.Player.Get(player);
            Reason = PlayerRoles.RoleChangeReason.None;
            IsAllowed = true;
        }
        public PlayerRoles.RoleChangeReason Reason { get; set; }
        public API.Player Player { get; set; }
        public bool IsAllowed { get; set; }
    }
}
