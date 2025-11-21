using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.Visibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.Roles
{
    public class FpcRole : Role
    {
        public FpcRole(PlayerRoleBase roleBase) : base(roleBase)
        {
            this.Base = (FpcStandardRoleBase)roleBase;
        }
        public new FpcStandardRoleBase Base {  get; set; }
        public VisibilityController VisibilityController => Base.VisibilityController;

    }
}
