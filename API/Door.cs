using FMOD.Enums;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using LabApi.Features.Wrappers;
using MapGeneration;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMOD.API
{
    public class Door
    {
        public static List<Door> Doors = new List<Door>();
        public static Door RandomDoor()
        {
            return Doors.RandomItem();
        }
        public static Door Get(DoorName doorName)
        {
            return Doors.FirstOrDefault(x => x.DoorName == doorName);
        }
        public static Door Get(Vector3 pos)
        {
            return Doors.FirstOrDefault(x => x.Position == pos);
        }
        public static Door Get(FacilityZone facilityZone)
        {
            return Doors.FirstOrDefault(x => x.Zone == facilityZone);
        }
        public Vector3 Position => GameObject.transform.position;
        public Quaternion Rotation
        {
            get
            {
                return this.GameObject.transform.rotation;
            }
            set
            {
                this.GameObject.transform.rotation = value;
                NetworkServer.Spawn(this.GameObject);
            }
        }
        public DoorVariant Base { get; }
        public GameObject GameObject => Base.gameObject;
        public Transform Transform => Base.transform;
        public Vector3 Scale
        {
            get
            {
                return this.GameObject.transform.localScale;
            }
            set
            {
                this.GameObject.transform.localScale = value;
                NetworkServer.Spawn(this.GameObject);
            }
        }
        public KeycardPermissions KeycardPermissions
        {
            get
            {
                return (KeycardPermissions)this.RequiredPermissions;
            }
            set
            {
                this.RequiredPermissions = (DoorPermissionFlags)value;
            }
        }
        public DoorPermissionFlags RequiredPermissions
        {
            get
            {
                return this.Base.RequiredPermissions.RequiredPermissions;
            }
            set
            {
                this.Base.RequiredPermissions.RequiredPermissions = value;
            }
        }

        public string Name => Base.name;
        public DoorName DoorName { get; }
        public MapGeneration.FacilityZone Zone
        {
            get
            {
                Room room = Room.GetRoom(GameObject.transform.position);
                return room.Zone;
            }
        }
        public bool AllowsScp106
        {
            get
            {
                IScp106PassableDoor scp106PassableDoor = this.Base as IScp106PassableDoor;
                return scp106PassableDoor != null && scp106PassableDoor.IsScp106Passable;
            }
            set
            {
                IScp106PassableDoor scp106PassableDoor = this.Base as IScp106PassableDoor;
                if (scp106PassableDoor != null)
                {
                    scp106PassableDoor.IsScp106Passable = value;
                }
            }
        }

    }
}
