using PlayerRoles;
using PlayerRoles.PlayableScps.HUDs;
using PlayerRoles.PlayableScps.HumeShield;
using PlayerRoles.PlayableScps.Scp049;
using PlayerRoles.Subroutines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.Roles
{
    public class Scp049Role : Role
    {
        public Scp049Role(PlayerRoles.PlayableScps.Scp049.Scp049Role roleBase) : base(roleBase)
        {
            this.Base = roleBase;
        }
        public new PlayerRoles.PlayableScps.Scp049.Scp049Role Base { get; set; }
        public HumeShieldModuleBase ShieldModuleBase => Base.HumeShieldModule;
        public SubroutineManagerModule Subroutine => Base.SubroutineModule;
        public ScpHudBase ScpHudUI => Base.HudPrefab;
    }
}
