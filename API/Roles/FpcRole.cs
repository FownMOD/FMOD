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
        public FpcRole(Player owner, PlayerRoleBase roleBase) : base(owner, roleBase)
        {
            Player player = Player.Get(this.Base.gameObject);
            player = owner;
            this.Base = (FpcStandardRoleBase)roleBase;
        }
        public new FpcStandardRoleBase Base {  get; set; }
        public VisibilityController VisibilityController => Base.VisibilityController;

    }
}
