using FMOD.Events.Interfaces;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.EventArgs.Player
{
    public class PlayerEscapeingArgs : System.EventArgs, IFMODPlayerEvent
    {
        public PlayerEscapeingArgs(ReferenceHub player, RoleTypeId newRole, Escape.EscapeScenarioType escapeScenario)
        {
            Player = API.Player.Get(player);
            IsAllowed = escapeScenario != Escape.EscapeScenarioType.None;
            NewRole = newRole;
            EscapeScenarioType = escapeScenario;
        }
        public API.Player Player { get; set; }
        public bool IsAllowed { get; set; }
        public RoleTypeId NewRole { get; set; }
        public Escape.EscapeScenarioType EscapeScenarioType { get; set; }
        public Escape.EscapeMessage EscapeMessage { get; set; }
    }
}
