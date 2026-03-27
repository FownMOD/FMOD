using PlayerRoles;
using PlayerRoles.FirstPersonControl.Spawnpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMOD.API.SpawnHelper
{
    public class SpawnRoleStaticPosition: StaticSpawnPosition
    {
        public SpawnRoleStaticPosition(RoleTypeId roleTypeId)
        {
            RoleType = roleTypeId;
        }
        public RoleTypeId RoleType { get; set; }
        public Vector3 Position
        {
            get
            {
                RoleSpawnpointManager.TryGetSpawnpointForRole(RoleType, out ISpawnpointHandler spawnpoint);
                spawnpoint.TryGetSpawnpoint(out Vector3 position, out float h);
                return position;
            }
        }
    }
}
