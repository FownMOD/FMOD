using FMOD.Enums;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Door = FMOD.API.Doors.Door;

namespace FMOD.API.SpawnHelper
{
    public class SpawnDoorStaticPosition: StaticSpawnPosition
    {
        public SpawnDoorStaticPosition(Door door)
        {
            Door = door;
        }
        public SpawnDoorStaticPosition(DoorName doorName)
        {
            Door = Door.Get(doorName);
        }
        public Door Door { get; set; }
        public DoorName DoorName
        {
            get => Door.DoorName;
        }
        public string BaseName
        {
            get => Door.Name;
        }
        public Vector3 Position
        {
            get
            {
                return Door.Position + Vector3.up * 1.5f + Door.Transform.forward * 0.5f;
            }
        }
        public Vector3 Rotation;
    }
}
