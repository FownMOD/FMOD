using PlayerRoles;
using PlayerRoles.PlayableScps.Scp939;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.Roles
{
    public class Scp939 : Role
    {
        public Scp939(PlayerRoleBase roleBase) : base(roleBase)
        {
            this.Base = roleBase as Scp939Role;
        }
        public new Scp939Role Base;

    }
}
