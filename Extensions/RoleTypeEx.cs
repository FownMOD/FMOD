using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Extensions
{
    public static class RoleTypeEx
    {
        public static PlayerRoleBase GetBase(this RoleTypeId role)
        {
            PlayerRoleLoader.TryGetRoleTemplate(role, out PlayerRoleBase result);
            return result;
        }

    }
}
