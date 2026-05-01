using PlayerRoles;
using PlayerRoles.PlayableScps.HUDs;
using PlayerRoles.PlayableScps.Scp173;
using PlayerRoles.Subroutines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.Roles
{
    public class Scp173Role : Role
    {
        public Scp173Role(PlayerRoles.PlayableScps.Scp173.Scp173Role roleBase) : base(roleBase)
        {
            this.Base = roleBase;
        }
        public new PlayerRoles.PlayableScps.Scp173.Scp173Role Base { get; }
        public SubroutineManagerModule SubroutineModule
        {
            get =>Base.SubroutineModule;
        }
    }
}
