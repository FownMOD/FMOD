using AdminToys;
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

namespace FMOD.API.Doors
{
    public class Door
    {
        public static List<Door> Doors = new List<Door>();
        public Door(DoorVariant doorVariant)
        {
            this.Base = doorVariant;
        }
        public static Door RandomDoor()
        {
            return Doors.RandomItem();
        }
        public static Door Create(Vector3 Position)
        {
            GameObject door = FPrefabsManger.Spawn(Position, PrefabType.EZBreakableDoor);
            DoorVariant doorVariant = door.gameObject.AddComponent<DoorVariant>();
            UnityEngine.Object.Instantiate(door);
            Doors.Add(new Door(doorVariant));
            return new Door(doorVariant);
        }
        public static Door Get(DoorVariant doorVariant)
        {
            if (Doors.Any(x => x.Base == doorVariant))
            {
                return Doors.First(x => x.Base == doorVariant);
            }
            return new Door(doorVariant);
        }
        public static Door Get(DoorName doorName)
        {
            if (Doors.Any(x => x.DoorName == doorName))
            {
                return Doors.First(x => x.DoorName == doorName);
            }
            return null;
        }
        public static Door Get(GameObject door)
        {
            if (door.TryGetComponent<DoorVariant>(out DoorVariant dv))
            {
                return Get(dv);
            }
            return null;
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
        public DoorPermissionsPolicy PermissionsPolicy
        {
            get
            {
                return this.Base.PermissionsPolicy;
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
        public DoorName DoorName
        {
            get
            {
                if (Enum.TryParse(Name, out DoorName doorName))
                {
                    return doorName;
                }
                return DoorName.UnknownDoor;
            }
        }
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
