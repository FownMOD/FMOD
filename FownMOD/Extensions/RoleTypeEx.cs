using FMOD.API;
using FMOD.API.Roles;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMOD.Extensions
{
    public static class RoleTypeEx
    {
        public static PlayerRoleBase GetBase(this RoleTypeId role)
        {
            PlayerRoleLoader.TryGetRoleTemplate(role, out PlayerRoleBase result);
            return result;
        }
        public static Vector3 GetSpawnPosition(this RoleTypeId role)
        {
            return GetFMODRole(role).GetSpawnPosition();
        }
        public static bool HasAny(this RoleTypeId role)
        {
            return Player.List.Any(x => x.Role.RoleTypeId == role);
        }
        public static int GetCount(this RoleTypeId role)
        {
            return Player.List.Where(x => x.Role.RoleTypeId == role).Count();
        }
        public static Role GetFMODRole(this RoleTypeId role)
        {
            PlayerRoleBase playerRoleBase = GetBase(role);
            return new Role(playerRoleBase);
        }
    }
}
