using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.Roles
{
    public class DestroyedRole : Role
    {
        public DestroyedRole(PlayerRoles.DestroyedRole roleBase) : base(roleBase)
        {
            this.Base = roleBase;
        }
        public new PlayerRoles.DestroyedRole Base;
        public bool IsHidden => Base.IsHidden;

    }
}
