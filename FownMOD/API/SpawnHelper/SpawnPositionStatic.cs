using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMOD.API.SpawnHelper
{
    public class SpawnPositionStatic: StaticSpawnPosition
    {
        public SpawnPositionStatic(Vector3 position)
        {
            Position = position;
        }
        public Vector3 Position { get; set; }
    }
}
